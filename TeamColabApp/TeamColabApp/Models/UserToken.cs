using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Sockets;
namespace TeamColabApp.Models 
{

   public enum TokenType
{
    Refresh,
    Blacklist
}

public class UserToken
{
    [Required]
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long TokenId { get; set; }

    public string? TokenValue { get; set; }

    public DateTime ExpiresAt { get; set; }

    public TokenType TokenType { get; set; }  
}

}