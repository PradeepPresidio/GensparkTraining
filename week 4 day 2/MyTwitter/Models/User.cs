namespace MyTwitter.Models
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
{
	public class User{
	[key]
	public int UserId { get; set; }

	[Required, StringLength(200)]
	public string Username { get; set }

	[Required, StringLength(200)]
	public string Password { get; set }

	[Required, StringLength(500)]
	public string UserBio { get; set }

	[Required]
	public int FollowersCount { get; set; }

	[Required]
	public int FollowingCount { get; set; }

	public virtual ICollection<Tweet> Tweets { get; set; }
}

}