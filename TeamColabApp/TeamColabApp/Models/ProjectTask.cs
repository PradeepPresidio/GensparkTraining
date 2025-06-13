using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TeamColabApp.Models
{
    public class ProjectTask
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long ProjectTaskId { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ICollection<User>? AssignedUsers { get; set; }
        public string Status { get; set; } = "Pending";
        public string Priority { get; set; } = "Medium";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public long ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public required Project Project { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<AppFile>? ProjectTaskFiles { get; set; }
    }
}