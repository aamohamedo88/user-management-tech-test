using System.Collections.Generic;
using System.Linq;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Implementations;

public class UserLogService : IUserLogService
{
    private readonly IDataContext _dataAccess;

    public UserLogService(IDataContext dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public void Create(UserLog log)
    {
        _dataAccess.Create(log);
    }

    public IEnumerable<UserLog> GetAll()
    {
        return _dataAccess.GetAll<UserLog>();
    }

    public IEnumerable<UserLog> GetByUserId(long userId)
    {
        return _dataAccess
            .GetAll<UserLog>()
            .Where(log => log.UserId == userId);
    }

    public UserLog? GetById(long id)
    {
        return _dataAccess
            .GetAll<UserLog>()
            .SingleOrDefault(log => log.Id == id);
    }
}
