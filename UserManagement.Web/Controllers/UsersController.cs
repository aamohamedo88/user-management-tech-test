using System;
using System.Linq;
using System.Threading.Tasks;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Users;

namespace UserManagement.Web.Controllers;
[Route("users")]
public class UsersController : Controller
{
    private readonly IUserService _userService;
    private readonly IUserLogService _userLogService;

    public UsersController(IUserService userService, IUserLogService userLogService)
    {
        _userService = userService;
        _userLogService = userLogService;
    }

    [HttpGet]
    public async Task<IActionResult> List(bool? isActive = null)
    {
        var users = isActive.HasValue
            ? await _userService.FilterByActiveAsync(isActive.Value)
            : await _userService.GetAllAsync();

        var items = users.Select(p => new UserListItemViewModel
        {
            Id = p.Id,
            Forename = p.Forename,
            Surname = p.Surname,
            Email = p.Email,
            DateOfBirth = p.DateOfBirth,
            IsActive = p.IsActive
        });

        var model = new UserListViewModel
        {
            Items = items.ToList()
        };

        return View(model);
    }

    [HttpGet("add")]
    public ViewResult Add()
    {
        return View(new AddUserViewModel());
    }

    [HttpPost("add")]
    public async Task<IActionResult> Add(AddUserViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = new User
        {
            Forename = model.Forename,
            Surname = model.Surname,
            Email = model.Email,
            DateOfBirth = model.DateOfBirth!.Value,
            IsActive = model.IsActive
        };

        await _userService.CreateAsync(user);

        await _userLogService.CreateAsync(new UserLog
        {
            UserId = user.Id,
            UserDisplayName = $"{user.Forename} {user.Surname}",
            Action = "Created",
            Details = $"User {user.Email} was created.",
            TimestampUtc = DateTime.UtcNow
        });

        return RedirectToAction(nameof(List));
    }

    [HttpGet("view/{id}")]
    public async Task<IActionResult> View(long id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var logs = await _userLogService.GetByUserIdAsync(user.Id);

        var model = new ViewUserViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive,
            Logs = logs
         .OrderByDescending(log => log.TimestampUtc)
         .ToList()
        };

        return View(model);
    }

    [HttpGet("edit/{id}")]
    public async Task<IActionResult> Edit(long id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var model = new EditUserViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive
        };

        return View(model);
    }

    [HttpPost("edit/{id}")]
    public async Task<IActionResult> Edit(long id, EditUserViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        user.Forename = model.Forename;
        user.Surname = model.Surname;
        user.Email = model.Email;
        user.DateOfBirth = model.DateOfBirth!.Value;
        user.IsActive = model.IsActive;

        await _userService.UpdateAsync(user);

        await _userLogService.CreateAsync(new UserLog
        {
            UserId = user.Id,
            UserDisplayName = $"{user.Forename} {user.Surname}",
            Action = "Updated",
            Details = $"User {user.Email} was updated.",
            TimestampUtc = DateTime.UtcNow
        });

        return RedirectToAction(nameof(List));
    }

    [HttpGet("delete/{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        var model = new DeleteUserViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email
        };

        return View(model);
    }

    [HttpPost("delete/{id}")]
    public async Task<IActionResult> Delete(long id, DeleteUserViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        var user = await _userService.GetByIdAsync(id);

        if (user == null)
        {
            return NotFound();
        }

        await _userLogService.CreateAsync(new UserLog
        {
            UserId = user.Id,
            UserDisplayName = $"{user.Forename} {user.Surname}",
            Action = "Deleted",
            Details = $"User {user.Email} was deleted.",
            TimestampUtc = DateTime.UtcNow
        });

        await _userService.DeleteAsync(user);

        return RedirectToAction(nameof(List));
    }
}
