using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserLogService
{
    Task CreateAsync(UserLog log);

    Task<List<UserLog>> GetAllAsync();

    Task<List<UserLog>> GetByUserIdAsync(long userId);

    Task<UserLog?> GetByIdAsync(long id);
}
