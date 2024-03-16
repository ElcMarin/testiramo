using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using maturitetna.Models;
using maturitetna.Data;
using maturitetna.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace maturitetna.Controllers;
[ServiceFilter(typeof(SessionCheckFilterUser))]
public class UserController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly ApplicationDbContext _db;

    public UserController(ILogger<AdminController> logger, ApplicationDbContext db)
    {
        _db = db;
        _logger = logger;
    }

    public IActionResult Index()
    {
        var my_appointments = _db.appointment.Where(a => a.id_user == int.Parse(HttpContext.Session.GetString("id")) && a.appointmentTime >= DateTime.Now && a.reschedulingId == 0).Include(a => a.hairdresser).Include(a => a.haircut).OrderBy(a => a.appointmentTime);

        var my_recurring_appointments_query = _db.appointment.Where(a => a.id_user == int.Parse(HttpContext.Session.GetString("id")) && a.appointmentTime >= DateTime.Now && a.reschedulingId != 0)
            .OrderBy(a => a.appointmentTime)
            .Include(a => a.hairdresser)
            .Include(a => a.haircut);

        var appointments_to_update = my_recurring_appointments_query.Where(a => a.rescheduleIn14Days == true && a.appointmentTime < DateTime.Now.AddDays(7)).ToList();

        foreach (var appointment in appointments_to_update)
        {
            _db.appointment.Add(new appointmentEntity
            {
                id_user = appointment.id_user,
                id_hairdresser = appointment.id_hairdresser,
                id_haircut = appointment.id_haircut,
                appointmentTime = appointment.appointmentTime.AddDays(14),
                created = DateTime.UtcNow,
                reschedulingId = appointment.reschedulingId,
                rescheduleIn14Days = true,
            });

            appointment.rescheduleIn14Days = false;
        }

        _db.SaveChanges();


        var my_recurring_appointments = my_recurring_appointments_query
            .AsEnumerable()
            .DistinctBy(a => a.reschedulingId);

        if (my_appointments.Count() == 0 && my_recurring_appointments.Count() == 0) return View(null);
        return View(new { my_appointments, my_recurring_appointments });
    }


      public IActionResult Profile()
    {
        if (HttpContext.Session.GetString("id") != null)
        {
            int i = int.Parse(HttpContext.Session.GetString("id"));
            var user = _db.user.Find(i);
           

            ViewBag.File = FileHelper.GetProfilePicture(i, 'u');

            return View(user);

        }
        else return RedirectToAction("Index", "Home");

            
    }
    
    [HttpPost]
    public IActionResult Profile(userEntity user, IFormFile fileName)
    {
        user.id_user = int.Parse(HttpContext.Session.GetString("id"));
        Console.WriteLine(user.id_user + " " + user.name + " " + user.lastname + " "  + 'u');
        var variable = _db.user.Find(user.id_user);
        variable.name = user.name;
        variable.lastname = user.lastname;
        variable.email = user.email;
        if(user.password != null) variable.password = PasswordHelper.HashPassword(user.password);
        _db.SaveChanges();
    
        if (fileName != null)
        {
            // Sanitize the file name to remove invalid characters
            string sanitizedFileName = string.Join("_", user.id_user.ToString(), 'u') + ".png";
            sanitizedFileName = new string(sanitizedFileName.Where(c => !Path.GetInvalidFileNameChars().Contains(c)).ToArray());

// Combine the sanitized file name with the path
            string filePath = Path.Combine("wwwroot/Storage/ProfilePics", sanitizedFileName);

// Use the sanitized file path in FileStream
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                fileName.CopyTo(stream);
            }

            
            
            // using (var stream = new FileStream("wwwroot/Storage/ProfilePics/" + admin.rights.ToString() + admin.id_admin.ToString() + ".png", FileMode.Create))
            // {
            //     fileName.CopyTo(stream);
            // }
        }
        ViewBag.File = FileHelper.GetProfilePicture(user.id_user, 'u');
        return View(user);
    }



    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear all session data
        return RedirectToAction("Index", "Home");
    }
    

    public IActionResult Style()
    {
        var haircuts = _db.haircut.ToList();

        return View(haircuts);
    }

    public IActionResult Calendar(int id_haircut)
    {
        var haircut = _db.haircut.Find(id_haircut);


        return View(haircut);
    }

    // Helper method to check if a given datetime is within working hours
    private bool IsWithinWorkingHours(DateTime appointmentTime, TimeSpan workingStartTime, TimeSpan workingEndTime)
    {
        if (DateTime.UtcNow.AddDays(14) < appointmentTime) return false;

        TimeSpan appointmentStartTime = appointmentTime.TimeOfDay;

        return appointmentStartTime >= workingStartTime && appointmentStartTime <= workingEndTime;
    }

    // Helper method to check if there are overlapping appointments for the chosen hairdresser at the specified date and time
    private bool IsAppointmentOverlap(int hairdresserId, DateTime appointmentTime, int haircutDuration)
    {
        var overlappingAppointments = _db.appointment
            .Where(a => a.id_hairdresser == hairdresserId && a.appointmentTime < appointmentTime.AddMinutes(haircutDuration) && a.appointmentTime.AddMinutes(a.haircut.duration) > appointmentTime)
            .Any();

        return overlappingAppointments;
    }

    public IActionResult MakeAppointment(int id_hairdresser, int id_haircut, int day, int month, int year, TimeSpan time, bool rescheduleIn2Weeks)
    {
        var hairdresser = _db.hairdresser.Find(id_hairdresser);
        if (hairdresser == null)
        {
            // Hairdresser does not exist
            return RedirectToAction("Calendar", new { id_haircut = id_haircut });
        }
        if (hairdresser.is_working == false)
        {
            // Hairdresser is not working
            return RedirectToAction("Calendar", new { id_haircut = id_haircut });
        }

        var haircut = _db.haircut.Find(id_haircut);
        if (haircut == null)
        {
            // Haircut does not exist
            return RedirectToAction("Calendar", new { id_haircut = id_haircut });
        }

        var hairdresser_haircut = _db.hairdresserHaircut.Any(hh => hh.id_haircut == id_haircut && hh.id_hairdresser == id_hairdresser);
        if (!hairdresser_haircut)
        {
            // Hairdresser does not offer this haircut
            return RedirectToAction("Calendar", new { id_haircut = id_haircut });
        }

        var haircutDuration = haircut.duration;
        
        var appointmentTime = new DateTime(year, month, day, time.Hours, time.Minutes, 0);

        // Check if the selected date and time are within the working hours of the hairdresser
        if (!IsWithinWorkingHours(appointmentTime, hairdresser.startTime, hairdresser.endTime))
        {
            // Selected time is outside working hours
            return RedirectToAction("Calendar", new { id_haircut = id_haircut });
        }

        // Check if there are overlapping appointments for the chosen hairdresser at the specified date and time
        if (IsAppointmentOverlap(id_hairdresser, appointmentTime, haircutDuration))
        {
            // Overlapping appointment
            return RedirectToAction("Calendar", new { id_haircut = id_haircut });
        }

        var user = _db.user.Find(int.Parse(HttpContext.Session.GetString("id")));

        // All checks passed, create a new appointment
        var newAppointment = new appointmentEntity
        {
            id_user = user.id_user,
            id_hairdresser = id_hairdresser,
            id_haircut = id_haircut,
            appointmentTime = appointmentTime,
            created = DateTime.UtcNow,
            rescheduleIn14Days = false,
            reschedulingId = rescheduleIn2Weeks ? appointmentTime.Ticks.GetHashCode() : 0
        };

        // Save the new appointment to the database
        _db.appointment.Add(newAppointment);

        if (rescheduleIn2Weeks) {
            _db.appointment.Add(new appointmentEntity
            {
                id_user = user.id_user,
                id_hairdresser = id_hairdresser,
                id_haircut = id_haircut,
                appointmentTime = appointmentTime.AddDays(14),
                created = DateTime.UtcNow,
                reschedulingId = newAppointment.reschedulingId,
                rescheduleIn14Days = true,
            });
        }

        _db.SaveChanges();

        MailService.SendConfirmAppointment(newAppointment);

        return View(newAppointment);
    }

    public IActionResult AvailableOnDay(int id_haircut, int year, int month, int day)
    {
        var haircut = _db.haircut.Find(id_haircut);
        if (haircut == null)
        {
            // Haircut does not exist
            return RedirectToAction("Calendar", new { id_haircut = id_haircut });
        }

        var selectedDate = new DateTime(year, month, day);
        var currentDate = DateTime.Now;

        if (selectedDate < currentDate.Date || selectedDate > currentDate.AddDays(14))
        {
            // Selected date is in the past or more than 14 days in the future
            return RedirectToAction("Calendar", new { id_haircut = id_haircut });
        }
        var hairdressersIds = _db.hairdresserHaircut.Where(hh => hh.id_haircut == id_haircut).Select(hh => hh.id_hairdresser).ToList();

        var hairdressers = _db.hairdresser.Where(h => hairdressersIds.Contains(h.id_hairdresser) && h.is_working).ToList();

        if (hairdressers.Count == 0)
        {
            return RedirectToAction("Calendar", new { id_haircut = id_haircut });
        }

        var minStartTime = hairdressers.Min(h => h.startTime);
        var maxEndTime = hairdressers.Max(h => h.endTime);

        var availableTimesWithHairdresser = new Dictionary<hairdresserEntity, List<TimeSpan>>();

        foreach (var currentHairdresser in hairdressers)
        {
            var existingAppointments = _db.appointment
                .Where(a => a.id_hairdresser == currentHairdresser.id_hairdresser && a.appointmentTime.Date == selectedDate.Date)
                .Include(a => a.haircut)
                .ToList();

            var availableTimes = new List<TimeSpan>();

            for (var i = minStartTime; i < maxEndTime; i = i.Add(new TimeSpan(0, 15, 0)))
            {
                if(currentDate.Date == selectedDate.Date && i <= currentDate.TimeOfDay) continue;

                var endTime = i.Add(new TimeSpan(0, haircut.duration, 0));

                // Check if the time slot is within the hairdresser's working hours
                if (i >= currentHairdresser.startTime && endTime <= currentHairdresser.endTime) // 3.15 < 
                {
                    // Check if the time slot is not occupied by an existing appointment
                    if (!existingAppointments.Any(a => a.appointmentTime.TimeOfDay < endTime && a.appointmentTime.AddMinutes(a.haircut.duration).TimeOfDay > i))
                    {
                        availableTimes.Add(i);
                    }
                }
            }

            if (availableTimes.Count > 0)
            {
                availableTimesWithHairdresser.Add(currentHairdresser, availableTimes);
            }
        }

        return View(availableTimesWithHairdresser);
    }

    [HttpPost]
    public IActionResult CancelAppointment(int appointment_id)
    {
        var user_id = int.Parse(HttpContext.Session.GetString("id"));
        var appointment = _db.appointment.Find(appointment_id);
        if (appointment == null || appointment.id_user != user_id)
        {
            return RedirectToAction("Index");
        }
        if (appointment.reschedulingId != 0)
        {
            _db.appointment.RemoveRange(_db.appointment.Where(a => a.reschedulingId == appointment.reschedulingId && appointment.id_user == user_id));
        } else
        {
            _db.appointment.Remove(appointment);
        }
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}
    
public class CalendarAvailableDatesModel
{
    public int id_haircut { get; set; }
    public int year { get; set; }
    public int day { get; set; }
    public int month { get; set; }
}
