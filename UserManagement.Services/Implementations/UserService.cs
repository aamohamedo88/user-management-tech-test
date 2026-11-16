using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;

namespace UserManagement.Services.Domain.Implementations;

public class UserService : IUserService
{
    private readonly IDataContext _dataAccess;
    public UserService(IDataContext dataAccess) => _dataAccess = dataAccess;

    /// <summary>
    /// Return users by active state
    /// </summary>
    /// <param name="isActive"></param>
    /// <returns></returns>
    public async Task<List<User>> FilterByActiveAsync(bool isActive)
    {
        return await _dataAccess
            .GetAll<User>()
            .Where(user => user.IsActive == isActive)
            .ToListAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _dataAccess
            .GetAll<User>()
            .ToListAsync();
    }

    public async Task<User?> GetByIdAsync(long id)
    {
        return await _dataAccess
            .GetAll<User>()
            .SingleOrDefaultAsync(user => user.Id == id);
    }

    public async Task CreateAsync(User user)
    {
        await _dataAccess.CreateAsync(user);
    }

    public async Task UpdateAsync(User user)
    {
        await _dataAccess.UpdateAsync(user);
    }

    public async Task DeleteAsync(User user)
    {
        await _dataAccess.DeleteAsync(user);
    }
}
