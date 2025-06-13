using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TeamColabApp.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long CommentId { get; set; }

        [StringLength(1000, MinimumLength = 1)]
        public required string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public long? UserId { get; set; }
        [ForeignKey("UserId")]
        public  User? User { get; set; }

        public long? ProjectTaskId { get; set; }
        [ForeignKey("ProjectTaskId")]
        public  ProjectTask? ProjectTask { get; set; }

        public long? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public  Project? Project { get; set; }
    }
}