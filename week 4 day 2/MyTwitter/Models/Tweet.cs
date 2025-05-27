using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MyTwitter.Models
{

    public class Tweet
    {
        [key]
        public int TweetId { get; set; }

        [Required]
        public int UserId { get; set; }

        [foreignKey("UserId")]
        public virtual User User {  get; set; }

        [Required, StringLength(100)
        public string Title { get; set; }

        [Required, StringLength(500)]
        public string Body { get; set; }

        [Required]
        public int  Likes { get; set; 
        public List<string> Tags { get; set; } = new List<string>();
    }

}