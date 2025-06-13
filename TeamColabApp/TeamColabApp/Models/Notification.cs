using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TeamColabApp.Models
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long NotificationId { get; set; }
        [StringLength(500, MinimumLength = 1)]
        [Required]
        public required string Message { get; set; }
        public string Type { get; set; } = "General";
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public long? UserId { get; set; }
        [ForeignKey("UserId")]
        public required User? User { get; set; }
        public long? ProjectTaskId { get; set; }
        [ForeignKey("ProjectTaskId")]
        public ProjectTask? ProjectTask { get; set; }
        public long? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}