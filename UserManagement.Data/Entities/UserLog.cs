using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagement.Models;

public class UserLog
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    public long UserId { get; set; }

    public string UserDisplayName { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string Details { get; set; } = string.Empty;

    public DateTime TimestampUtc { get; set; }
}
