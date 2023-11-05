#pragma warning disable CS8629, CS8634
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
        Song? songToShow = _context.Songs.Include(s => s.Submitter).Include(d => d.UsersWhoLiked).Include(b => b.UsersWhoDisliked).FirstOrDefault(a => a.SongId == songId);
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
        ViewBag.UserDislikedSongs = _context.Songs.Include(a => a.Submitter).Include(b => b.UsersWhoDisliked).ToList();
        return View();
    }

    // Add Like to song; LikeId, UserId, SongId
    [HttpGet("song/like/{songId}")]
    public IActionResult LikeSong(int songId)
    {
        if(HttpContext.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Index");
        }
        Like newLike = new Like()
        {
            UserId = (int)HttpContext.Session.GetInt32("UserId"),
            SongId = songId
        };
        _context.Add(newLike);
        _context.SaveChanges();
        return Redirect($"/song/{songId}");
    }

    // Remove Like from database
    [HttpGet("song/unlike/{songId}")]
    public IActionResult UnLikeSong(int songId)
    {
        if(HttpContext.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Index");
        }
        // User must be logged and someone who liked the post
        int loggedUserId = (int)HttpContext.Session.GetInt32("UserId");
        Like? likeToDelete = _context.Likes.SingleOrDefault(a => a.UserId == loggedUserId && a.SongId == songId);
        _context.Remove(likeToDelete);
        _context.SaveChanges();
        return Redirect($"/song/{songId}");
    }

    // Add Dislike to song; DislikeId, UserId, SongId
    [HttpGet("song/dislike/{songId}")]
    public IActionResult DislikeSong(int songId)
    {
        if(HttpContext.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Index");
        }
        Dislike newDislike = new Dislike()
        {
            UserId = (int)HttpContext.Session.GetInt32("UserId"),
            SongId = songId
        };
        _context.Add(newDislike);
        _context.SaveChanges();
        return Redirect($"/song/{songId}");
    }

    // Remove Disike from database
    [HttpGet("song/undislike/{songId}")]
    public IActionResult UnDislikeSong(int songId)
    {
        if(HttpContext.Session.GetInt32("UserId") == null)
        {
            return RedirectToAction("Index");
        }
        // User must be logged and someone who liked the post
        int loggedUserId = (int)HttpContext.Session.GetInt32("UserId");
        Dislike? dislikeToDelete = _context.Dislikes.SingleOrDefault(a => a.UserId == loggedUserId && a.SongId == songId);
        _context.Remove(dislikeToDelete);
        _context.SaveChanges();
        return Redirect($"/song/{songId}");
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