using musicShare.Models;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace musicShare.Controllers;
public class SongController : Controller
{
    private readonly ILogger<SongController> _logger;
    private MyContext _context;

    public SongController(ILogger<SongController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    // Song Routing

    
    [HttpGet("song/create")]
    public IActionResult AddSong()
    {
        if(HttpContext.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Index");
        }
        ViewBag.NotLoggedIn = false;
        return View();
    }


}