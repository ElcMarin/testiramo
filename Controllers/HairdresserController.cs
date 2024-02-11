using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using maturitetna.Models;
using maturitetna.Data;
using maturitetna.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace maturitetna.Controllers;
[ServiceFilter(typeof(SessionCheckFilterHairdresser))]
public class HairdresserController : Controller
{
    private readonly ILogger<AdminController> _logger;
    private readonly ApplicationDbContext _db;

    public HairdresserController(ILogger<AdminController> logger, ApplicationDbContext db)
    {
        _db = db;
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Profile()
    {
        if (HttpContext.Session.GetString("id") != null)
        {
            int i = int.Parse(HttpContext.Session.GetString("id"));
            var hairdresser = _db.hairdresser.Find(i);
           

            ViewBag.File = FileHelper.GetProfilePicture(i, 'h');

            return View(hairdresser);
        }
        else return RedirectToAction("Index", "Home");
    }
    
    [HttpPost]
    public IActionResult Profile(hairdresserEntity hairdresser, IFormFile fileName)
    {
        hairdresser.id_hairdresser = int.Parse(HttpContext.Session.GetString("id"));
        Console.WriteLine( hairdresser.id_hairdresser + " " +  hairdresser.name + " " +  hairdresser.lastname + " "  + 'h');
        var user = _db. hairdresser.Find( hairdresser.id_hairdresser);
        user.name =  hairdresser.name;
        user.lastname =  hairdresser.lastname;
        user.email =  hairdresser.email;

        user.startTime = hairdresser.startTime;
        user.endTime = hairdresser.endTime;
        user.is_working = hairdresser.is_working;

        // if( hairdresser.password != null) user.password = PasswordHelper.HashPassword( hairdresser.password);
        _db.SaveChanges();
    
        if (fileName != null)
        {
            // Sanitize the file name to remove invalid characters
            string sanitizedFileName = string.Join("_",  hairdresser.id_hairdresser.ToString(), 'h') + ".png";
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
        ViewBag.File = FileHelper.GetProfilePicture( hairdresser.id_hairdresser, 'h');
        return View( hairdresser);
    }

    public async Task<IActionResult> ToggleMyHaircut(int id_haircut)
    {
        if (await _db.hairdresserHaircut.FirstOrDefaultAsync(hh => hh.id_haircut == id_haircut && hh.id_hairdresser == int.Parse(HttpContext.Session.GetString("id"))) == null)
        {
            var hh = new hairdresserHaircutEntity
            {
                id_haircut = id_haircut,
                id_hairdresser = int.Parse(HttpContext.Session.GetString("id"))
            };
            await _db.hairdresserHaircut.AddAsync(hh);
            await _db.SaveChangesAsync();
        }
        else
        {
            var hh = await _db.hairdresserHaircut.FirstOrDefaultAsync(hh => hh.id_haircut == id_haircut && hh.id_hairdresser == int.Parse(HttpContext.Session.GetString("id")));
            _db.hairdresserHaircut.Remove(hh);
            await _db.SaveChangesAsync();
        }

        return RedirectToAction("MyHaircuts");
    }

    public async Task<IActionResult> MyHaircuts()
    {
        var allHaircuts = await _db.haircut.ToListAsync();

        var myHaircuts = await _db.hairdresserHaircut.Where(hh => hh.id_hairdresser == int.Parse(HttpContext.Session.GetString("id"))).ToListAsync();

        return View(new {
            myHaircuts,
            allHaircuts
        });
    }

    public IActionResult DaysOff()
    {
        return View();
    }
    
    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear all session data
        return RedirectToAction("Index", "Home");
    }
}
    

