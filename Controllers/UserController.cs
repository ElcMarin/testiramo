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
        return View();
    }

    public IActionResult Logout()
    {
        return View();
    }
    
}
    

