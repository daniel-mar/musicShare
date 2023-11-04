#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace musicShare.Models;
public class User
{
    [Key]
    public int UserId {get;set;}
    [Required]
    [MinLength(2)]
    public string Name {get;set;}
    [EmailAddress]
    [Required]
    public string Email {get;set;}
    [Required]
    [DataType(DataType.Password)]
    [MinLength(8)]
    public string Password {get;set;}
    [NotMapped]
    [Required]
    [DataType(DataType.Password)]
    [Compare("Password")]
    public string PassConfirm {get;set;}

    public List<Song> SongsSubmitted {get;set;} = new List<Song>();
    public List<Like> SongsLiked {get;set;} = new List<Like>();
    public List<Dislike> SongsDisiked {get;set;} = new List<Dislike>();

    public DateTime CreatedAt {get;set;} = DateTime.Now;
    public DateTime UpdatedAt {get;set;} = DateTime.Now;
}