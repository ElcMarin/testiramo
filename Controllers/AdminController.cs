using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using maturitetna.Models;
using maturitetna.Data;
using maturitetna.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace maturitetna.Controllers;
[ServiceFilter(typeof(SessionCheckFilterAdmin))]
public class AdminController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly ApplicationDbContext _db;

    public AdminController(ILogger<AdminController> logger, ApplicationDbContext db)
    {
        _db = db;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var appointment = await _db.appointment.Include(a => a.hairdresser).Include(a => a.haircut).Include(a => a.user).ToListAsync();
        return View(appointment);
    }
    public async Task<IActionResult> Hairdressers()
    {
        var hairdresser = await _db.hairdresser.ToListAsync();
        return View(hairdresser);
    }
    
    public IActionResult CreateHairdresser()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateHairdresser(hairdresserEntity hairdresser)
    {
        ModelState.Remove("appointements");
        ModelState.Remove("haircuts");
        if (ModelState.IsValid)
        {
          
            hairdresser.password = PasswordHelper.HashPassword(hairdresser.password);

           
            _db.hairdresser.Add(hairdresser);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Hairdressers));
        }

        
        return View(hairdresser);
    }
    
    
    // UREDI ADMINA
    public async Task<IActionResult> EditHairdresser(int id)
    {
        var hairdresser = await _db.hairdresser.FindAsync(id);
        if (hairdresser == null)
        {
            return NotFound();
        }
        return View(hairdresser);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditHairdresser(int id, hairdresserEntity hairdresser)
    {
        if (id != hairdresser.id_hairdresser)
        {
            return NotFound();
        }
        var found_hairdresser = _db.hairdresser.Find(id);

        found_hairdresser.name = hairdresser.name;
        found_hairdresser.lastname = hairdresser.lastname;
        found_hairdresser.email = hairdresser.email;

        found_hairdresser.gender = hairdresser.gender;
        found_hairdresser.is_working = hairdresser.is_working;
        found_hairdresser.startTime = hairdresser.startTime;
        found_hairdresser.endTime = hairdresser.endTime;

        _db.SaveChanges();
        return View(found_hairdresser);
    }
    
    
    
    
    public async Task<IActionResult> Admins()
    {
        // Fetch all admin records from the database asynchronously
        var admins = await _db.admin.ToListAsync(); // Replace AdminEntities with your adminEntity DbSet name

        // Pass the retrieved admins to the view
        return View(admins);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var admin = await _db.admin.FindAsync(id);
        if (admin == null)
        {
            return NotFound();
        }

        _db.admin.Remove(admin);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var appointment = await _db.appointment.FindAsync(id);
        if (appointment == null)
        {
            return NotFound();
        }

        _db.appointment.Remove(appointment);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _db.user.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _db.user.Remove(user);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    
    
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // Clear all session data
            return RedirectToAction("Index", "Home");
        }
    
    
    public async Task<IActionResult> Users()
    {
        var user = await _db.user.ToListAsync();
        return View(user);
    }


    public async Task<IActionResult> EditUser(int id)
    {
        var user = await _db.user.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EditUser(int id, [Bind("id_user,name,lastname,email,password")] userEntity user)
    {
        if (id != user.id_user)
        {
            return NotFound();
        }

        var found_user = _db.user.Find(id);

        found_user.name = user.name;
        found_user.email = user.email;
        found_user.lastname = user.lastname;

        _db.SaveChanges();
        return RedirectToAction(nameof(Users)); // Redirect to admin list or another page
    }
    
    public IActionResult CreateUser()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> CreateUser(userEntity user)
    {
        if (ModelState.IsValid)
        {
            // Hash the password
            user.password = PasswordHelper.HashPassword(user.password);

            // Add the user with hashed password to the database
            _db.user.Add(user);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Users));
        }

        // If ModelState is not valid, return to the Create view with validation errors
        return View(user);
    }
    
    public async Task<IActionResult> DeleteHairdresser(int id)
    {
        var hairdresser = await _db.hairdresser.FindAsync(id);
        if (hairdresser == null)
        {
            return NotFound();
        }

        _db.hairdresser.Remove(hairdresser);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Users));
    }
    
    public IActionResult Profile()
    {
        if (HttpContext.Session.GetString("id") != null)
        {
            int i = int.Parse(HttpContext.Session.GetString("id"));
            var admin = _db.admin.Find(i);
           

            ViewBag.File = FileHelper.GetProfilePicture(i, 'a');

            return View(admin);

        }
        else return RedirectToAction("Index", "Home");

            
    }
    
    [HttpPost]
    public IActionResult Profile(adminEntity admin, IFormFile fileName)
    {
        admin.id_admin = int.Parse(HttpContext.Session.GetString("id"));
        Console.WriteLine(admin.id_admin + " " + admin.name + " " + admin.lastname + " "  + 'a');
        var user = _db.admin.Find(admin.id_admin);
        user.name = admin.name;
        user.lastname = admin.lastname;
        user.email = admin.email;
        if(admin.password != null) user.password = PasswordHelper.HashPassword(admin.password);
        _db.SaveChanges();
    
        if (fileName != null)
        {
            // Sanitize the file name to remove invalid characters
            string sanitizedFileName = string.Join("_", admin.id_admin.ToString(), 'a') + ".png";
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
        ViewBag.File = FileHelper.GetProfilePicture(admin.id_admin, 'a');
        return View(user);
    }

    
    //USTVARI ADMINA
    public IActionResult Create()
    {
        return View();
    }
  [HttpPost]
public async Task<IActionResult> Create(adminEntity admin)
{
    if (ModelState.IsValid)
    {
        // Hash the password
        admin.password = PasswordHelper.HashPassword(admin.password);

        // Add the admin with hashed password to the database
        _db.admin.Add(admin);
        await _db.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    // If ModelState is not valid, return to the Create view with validation errors
    return View(admin);
}
    
    
    // UREDI ADMINA
    public async Task<IActionResult> Edit(int id)
    {
        var admin = await _db.admin.FindAsync(id);
        if (admin == null)
        {
            return NotFound();
        }
        return View(admin);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("id_admin,name,lastname,email,password")] adminEntity admin)
    {
        if (id != admin.id_admin)
        {
            return NotFound();
        }


        var found_admin = _db.admin.Find(id);

        found_admin.name = admin.name;
        found_admin.lastname = admin.lastname;
        found_admin.email = admin.email;

        _db.SaveChanges();
        return RedirectToAction(nameof(Admins));
    }

    // APPOINTMENS

   
    public IActionResult AddAppointments()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> AddAppointments(appointmentEntity appointment)
    {
        if (ModelState.IsValid)
        {
            _db.appointment.Add(appointment);
            await _db.SaveChangesAsync();
        }
        return View();
    }

    public IActionResult Styles(int hairdresser_id)
    {
        var hairdresser = _db.hairdresser.Find(hairdresser_id);
        if(hairdresser == null) { return NotFound(); }

        var allHaircuts = _db.haircut.ToList();

        var myHaircuts = _db.hairdresserHaircut.Where(hh => hh.id_hairdresser == hairdresser_id).ToList();

        return View(new
        {
            hairdresser,
            myHaircuts,
            allHaircuts
        });
    }
    

    public async Task<IActionResult> ToggleHaircut(int hairdresser_id, int haircut_id)
    {
        if (await _db.hairdresserHaircut.FirstOrDefaultAsync(hh => hh.id_haircut == haircut_id && hh.id_hairdresser == hairdresser_id) == null)
        {
            var hh = new hairdresserHaircutEntity
            {
                id_haircut = haircut_id,
                id_hairdresser = hairdresser_id,
            };
            await _db.hairdresserHaircut.AddAsync(hh);
            await _db.SaveChangesAsync();
        }
        else
        {
            var hh = await _db.hairdresserHaircut.FirstOrDefaultAsync(hh => hh.id_haircut == haircut_id && hh.id_hairdresser == hairdresser_id);
            _db.hairdresserHaircut.Remove(hh);
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Styles", new { hairdresser_id = hairdresser_id });
    }
}

