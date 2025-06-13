using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeamColabApp.Models
{
    public class User
    {
        [Required]
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long UserId { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public required string Name { get; set; }

        [StringLength(100, MinimumLength = 8)]
        public string? Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Role { get; set; } = "member";
        public bool IsActive { get; set; } = true;
        public ICollection<Project>? TeamLedProjects { get; set; }
        public ICollection<Project>? Projects { get; set; }
        public ICollection<ProjectTask>? ProjectTasks { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
        public ICollection<AppFile>? UserFiles { get; set; }
}
}