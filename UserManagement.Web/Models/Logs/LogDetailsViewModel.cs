using System;

namespace UserManagement.Web.Models.Logs;

public class LogDetailsViewModel
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public string UserDisplayName { get; set; } = string.Empty;

    public string Action { get; set; } = string.Empty;

    public string Details { get; set; } = string.Empty;

    public DateTime TimestampUtc { get; set; }
}
