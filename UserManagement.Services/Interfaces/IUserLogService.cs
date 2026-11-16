using System.Collections.Generic;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserLogService
{
    void Create(UserLog log);

    IEnumerable<UserLog> GetAll();

    IEnumerable<UserLog> GetByUserId(long userId);

    UserLog? GetById(long id);
}
