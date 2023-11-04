#pragma warning disable CS8618
using System.ComponentModel.DataAnnotations;
namespace musicShare.Models;
public class Dislike
{
    [Key]
    public int DislikeId {get;set;}
    public int UserId {get;set;}
    public User? UserWhoDisliked {get;set;}
    public int SongId {get;set;}
    public Song? SongDisliked {get;set;}
}