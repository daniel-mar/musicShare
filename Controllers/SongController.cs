#pragma warning disable CS8629
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
        if (HttpContext.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Index");
        }
        ViewBag.NotLoggedIn = false;
        return View();
    }

    // Handle new song creation
    [HttpPost("song/add")]
    public IActionResult CreateSong(Song newSong)
    {
        ViewBag.NotLoggedIn = false;
        if (ModelState.IsValid)
        {
            newSong.UserId = (int)HttpContext.Session.GetInt32("UserId");
            _context.Add(newSong);
            _context.SaveChanges();
            return Redirect($"/song/{newSong.SongId}");
        }
        else
        {
            return View("AddSong");
        }
    }

    // After creation, redirect to view one song
    [HttpGet("song/{songId}")]
    public IActionResult OneSong(int songId)
    {
        ViewBag.NotLoggedIn = false;
        Song? songToShow = _context.Songs.Include(s => s.Submitter).Include(d => d.UsersWhoLiked).FirstOrDefault(a => a.SongId == songId);
        if (songToShow == null)
        {
            return RedirectToAction("~/Views/Home/Dashboard");
        }
        return View(songToShow);
    }

    // View all songs
    [HttpGet("song/all")]
    public IActionResult AllSongs()
    {
        if(HttpContext.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Index");
        }
        ViewBag.NotLoggedIn = false;
        ViewBag.AllSongs = _context.Songs.Include(a => a.Submitter).Include(b => b.UsersWhoLiked).ToList();
        return View("AllSongs");
    }

    // Handle delete song
    [HttpGet("song/delete/{songId}")]
    public IActionResult DeleteSong(int songId)
    {
        Song? songToDelete = _context.Songs.SingleOrDefault(a => a.SongId == songId);
        if (songToDelete == null)
        {
            return RedirectToAction("Dashboard");
        }
        if (songToDelete.UserId != HttpContext.Session.GetInt32("UserId"))
        {
            return RedirectToAction("Logout");
        }
        _context.Songs.Remove(songToDelete);
        _context.SaveChanges();
        return RedirectToAction(nameof(HomeController.Dashboard), "Home");
    }


}