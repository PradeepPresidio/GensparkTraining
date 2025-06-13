using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TeamColabApp.Models
{

    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public long ProjectId { get; set; }

        [StringLength(200, MinimumLength = 3)]
        public required string Title { get; set; }
        [StringLength(500)]
        public required string Description { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; } = "Ongoing";
        public string Priority { get; set; } = "Medium";
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public long? TeamLeaderId { get; set; }
        [ForeignKey("TeamLeaderId")]
        public User? TeamLeader { get; set; }
        public ICollection<User>? TeamMembers { get; set; }
        public ICollection<ProjectTask>? ProjectTasks { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<AppFile>? ProjectFiles { get; set; }
    }
}