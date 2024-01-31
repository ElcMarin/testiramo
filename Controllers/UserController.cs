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
        return View();
    }


      public IActionResult Profile()
    {
        if (HttpContext.Session.GetString("id") != null)
        {
            int i = int.Parse(HttpContext.Session.GetString("id"));
            var user = _db.user.Find(i);
           

            ViewBag.File = FileHelper.GetProfilePicture(i, user.rights);

            return View(user);

        }
        else return RedirectToAction("Index", "Home");

            
    }
    
    [HttpPost]
    public IActionResult Profile(userEntity user, IFormFile fileName)
    {
        user.id_user = int.Parse(HttpContext.Session.GetString("id"));
        Console.WriteLine(user.id_user + " " + user.name + " " + user.lastname + " "  + user.rights);
        var variable = _db.user.Find(user.id_user);
        variable.name = user.name;
        variable.lastname = user.lastname;
        variable.email = user.email;
        if(user.password != null) variable.password = PasswordHelper.HashPassword(user.password);
        _db.SaveChanges();
    
        if (fileName != null)
        {
            // Sanitize the file name to remove invalid characters
            string sanitizedFileName = string.Join("_", user.id_user.ToString(), user.rights.ToString()) + ".png";
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
        ViewBag.File = FileHelper.GetProfilePicture(user.id_user, user.rights);
        return View(user);
    }

    


    public IActionResult Logout()
    {
        HttpContext.Session.Clear(); // Clear all session data
        return RedirectToAction("Index", "Home");
    }

    public IActionResult BookAppointment()
    {
        return View();
    }
    
}
    

