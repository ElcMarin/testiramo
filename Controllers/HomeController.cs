using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using maturitetna.Models;
using maturitetna.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace maturitetna.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _db;

    
    
    
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
    {
        _db = db;
        _logger = logger;
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("id");
        HttpContext.Session.Remove("rights");
        return RedirectToAction("Index", "Home");
    }
    
    public IActionResult Index()
    {
       return View();
    }

    public IActionResult Register()
    {
        return View();
    }
    
    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult Login()
    {
        return View();
    }
 
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            var hairdresser = await _db.hairdresser.FirstOrDefaultAsync(h => h.email == model.email && h.password == model.password);
            if (hairdresser != null)
            {
                HttpContext.Session.SetString("id", hairdresser.id_hairdresser.ToString());
                HttpContext.Session.SetString("rights", "h"); // 'h' for Hairdresser

                return RedirectToAction("Index", "Hairdresser");
            }

            var admin = await _db.admin.FirstOrDefaultAsync(a => a.email == model.email && a.password == model.password);
            if (admin != null)
            {
                HttpContext.Session.SetString("id", admin.id_admin.ToString());
                HttpContext.Session.SetString("rights", "a"); // 'a' for Admin

                return RedirectToAction("Index", "Admin");
            }

            var user = await _db.user.FirstOrDefaultAsync(u => u.email == model.email && u.password == model.password);
            if (user != null)
            {
                HttpContext.Session.SetString("id", user.id_user.ToString());
                HttpContext.Session.SetString("rights", "u"); // 'u' for User

                return RedirectToAction("Index", "User");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt");
        }

        return View(model);
    }
    
    // LOGIN POST
    /*[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        if (ModelState.IsValid)
        {
            // Check if the user exists in the Hairdresser table
            var hairdresser = await _db.hairdresser.FirstOrDefaultAsync(h => h.email == model.email && h.password == model.password);
            if (hairdresser != null)
            {
                
                return RedirectToAction("Index","Hairdresser");
            }

            // Check if the user exists in the Admin table
            var admin = await _db.admin.FirstOrDefaultAsync(a => a.email == model.email && a.password == model.password);
            if (admin != null)
            {
                // Redirect to Admin dashboard or specific action
                
                return RedirectToAction("Index","Admin");
            }

            // Check if the user exists in the User table
            var user = await _db.user.FirstOrDefaultAsync(u => u.email == model.email && u.password == model.password);
            if (user != null)
            {
                // Redirect to User dashboard or specific action
                return RedirectToAction("Index","User");
            }

            // If the user is not found in any role, show an error
            ModelState.AddModelError(string.Empty, "Invalid login attempt");
        }

        // If the ModelState is not valid or login fails, return to the login view
        return View(model);
    }
    

    
    */
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}

