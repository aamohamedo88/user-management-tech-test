using System;
using UserManagement.Models;

namespace UserManagement.Web.Models.Users;

public class ViewUserViewModel
{
    public List<UserLog> Logs { get; set; } = new();
    public long Id { get; set; }

    public string Forename { get; set; } = string.Empty;

    public string Surname { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public bool IsActive { get; set; }
}
