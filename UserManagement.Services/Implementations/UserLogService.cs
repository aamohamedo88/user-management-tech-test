using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

    public async Task CreateAsync(UserLog log)
    {
        await _dataAccess.CreateAsync(log);
    }

    public async Task<List<UserLog>> GetAllAsync()
    {
        return await _dataAccess
            .GetAll<UserLog>()
            .ToListAsync();
    }

    public async Task<List<UserLog>> GetByUserIdAsync(long userId)
    {
        return await _dataAccess
            .GetAll<UserLog>()
            .Where(log => log.UserId == userId)
            .ToListAsync();
    }

    public async Task<UserLog?> GetByIdAsync(long id)
    {
        return await _dataAccess
            .GetAll<UserLog>()
            .SingleOrDefaultAsync(log => log.Id == id);
    }
}
