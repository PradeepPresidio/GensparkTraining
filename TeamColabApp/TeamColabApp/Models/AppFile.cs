using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TeamColabApp.Models
{
    public class AppFile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long FileId { get; set; }
        public required string FileName { get; set; }
        public  byte[]? Content { get; set; }
        public required string FileType { get; set; }
        public DateTime UploadedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public long? UserId { get; set; }
        [ForeignKey("UserId")]
        public User? User { get; set; }
        public long? ProjectTaskId { get; set; }
        [ForeignKey("ProjectTaskId")]
        public ProjectTask? ProjectTask { get; set; }
        public long? ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public Project? Project { get; set; }
    }
}