using System.Collections.Generic;
using System.Threading.Tasks;
using UserManagement.Models;

namespace UserManagement.Services.Domain.Interfaces;

public interface IUserService 
{
    Task<List<User>> FilterByActiveAsync(bool isActive);

    Task<List<User>> GetAllAsync();

    Task<User?> GetByIdAsync(long id);

    Task CreateAsync(User user);

    Task UpdateAsync(User user);

    Task DeleteAsync(User user);
}
