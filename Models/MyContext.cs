#pragma warning disable CS8618
using Microsoft.EntityFrameworkCore;

namespace musicShare.Models;
public class MyContext : DbContext 
{ 
    public MyContext(DbContextOptions options) : base(options) { }
    public DbSet<User> Users { get; set; } 
    public DbSet<Song> Songs {get;set;}
    public DbSet<Like> Likes {get;set;}
    public DbSet<Dislike> Dislikes {get;set;}
}