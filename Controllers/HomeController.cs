using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using musicShare.Models;

namespace musicShare.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        // Session
        ViewBag.NotLoggedIn = true;
        return View();
    }

    // User Registration form handling
    [HttpPost("user/register")]
    public IActionResult Register(User newUser)
    {
        ViewBag.NotLoggedIn = true;
        // Check Model from registration form
        if (ModelState.IsValid)
        {
            // Verify if the email is unique from database entries
            if (_context.Users.Any(u => u.Email == newUser.Email))
            {
                ModelState.AddModelError("Email", "Email in use.");
                return View("Index");
            }
            // Email is unique, proceed to hash and store credentials
            PasswordHasher<User> Hasher = new PasswordHasher<User>();
            newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
            _context.Add(newUser);
            _context.SaveChanges();
            // Create newUser's session and log them in
            HttpContext.Session.SetInt32("UserId", newUser.UserId);
            return RedirectToAction("Dashboard");
        }
        else
        {
            // Incorrect Model from form
            return View("Index");
        }
    }

    // User Login form handling
    [HttpPost("user/login")]
    public IActionResult Login(LogUser loginUser)
    {
        ViewBag.NotLoggedIn = true;
        // Check model from login form
        if (ModelState.IsValid)
        {
            // Verify email exists in database
            User? userInDb = _context.Users.FirstOrDefault(u => u.Email == loginUser.LogEmail);
            // Email isn't in database
            if (userInDb == null)
            {
                ModelState.AddModelError("LogEmail", "Invalid login attempt");
                return View("Index");
            }
            // Verify Password matches
            PasswordHasher<LogUser> hasher = new PasswordHasher<LogUser>();
            var result = hasher.VerifyHashedPassword(loginUser, userInDb.Password, loginUser.LogPassword);
            // Must be incorrect from what is stored
            if (result == 0)
            {
                ModelState.AddModelError("LogEmail", "Invalid login attempt");
                return View("Index");
            }
            // Create session for successful User logged in
            HttpContext.Session.SetInt32("UserId", userInDb.UserId);
            return RedirectToAction("Dashboard");
        }
        else
        {
            // Incorrect model from login form
            return View("Index");
        }
    }

    [HttpGet("Dashboard")]
    public IActionResult Dashboard()
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Index");
        }
        ViewBag.NotLoggedIn = false;
        User? userInDb = _context.Users.Include(a => a.SongsSubmitted).FirstOrDefault(a => a.UserId == HttpContext.Session.GetInt32("UserId"));
        // Store User Submitted songs with likes
        ViewBag.UserLikedSongs = _context.Songs.Include(a => a.Submitter).Include(b => b.UsersWhoLiked).ToList();
        ViewBag.UserDislikedSongs = _context.Songs.Include(a => a.Submitter).Include(b => b.UsersWhoDisliked).ToList();
        ViewBag.LoggedIn = userInDb;
        return View("Dashboard");
    }

    // Clear Session redirect to Login/Reg
    [HttpGet("Logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
