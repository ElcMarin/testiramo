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

    public IActionResult Index()
    {
        return View();
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


    public IActionResult EditUser()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> EditUser(int id)
    {
        var user = await _db.user.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
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

            return RedirectToAction(nameof(Index));
        }

        // If ModelState is not valid, return to the Create view with validation errors
        return View(user);
    }
    
    public IActionResult Profile()
    {
        if (HttpContext.Session.GetString("id") != null)
        {
            int i = int.Parse(HttpContext.Session.GetString("id"));
            var admin = _db.admin.Find(i);
           

            ViewBag.File = FileHelper.GetProfilePicture(i, admin.rights);

            return View(admin);

        }
        else return RedirectToAction("Index", "Home");

            
    }

    [HttpPost]
    public IActionResult Profile(adminEntity admin, IFormFile fileName)
    {
        admin.id_admin = int.Parse(HttpContext.Session.GetString("id"));
        Console.WriteLine(admin.id_admin + " " + admin.name + " " + admin.lastname + " "  + admin.rights);
        var user = _db.admin.Find(admin.id_admin);
        user.name = admin.name;
        user.lastname = admin.lastname;
        user.email = admin.email;
        if(admin.password != null) user.password = PasswordHelper.HashPassword(admin.password);
        _db.SaveChanges();

        if (fileName != null)
        {
            using (var stream = new FileStream("wwwroot/Storage/ProfilePics/" + admin.id_admin + ".png", FileMode.Create))
            {
                fileName.CopyTo(stream);
            }
        }
        return View(admin);
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

        if (ModelState.IsValid)
        {
            try
            {
                _db.Update(admin);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Handle exception
            }

            return RedirectToAction(nameof(Index)); // Redirect to admin list or another page
        }

        return View(admin);

    }


    
    /*public IActionResult Logout()
    {
        HttpContext.Session.Remove("email");
        HttpContext.Session.Remove("password");

            
        return RedirectToAction("Index", "Home");

    }
    */
}

