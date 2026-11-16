using System;
using System.Linq;
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
    public ViewResult List(bool? isActive)
    {
        var users = isActive.HasValue
            ? _userService.FilterByActive(isActive.Value)
            : _userService.GetAll();

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
    public IActionResult Add(AddUserViewModel model)
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

        _userService.Create(user);

        _userLogService.Create(new UserLog
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
    public IActionResult View(long id)
    {
        var user = _userService.GetById(id);

        if (user == null)
        {
            return NotFound();
        }

        var model = new ViewUserViewModel
        {
            Id = user.Id,
            Forename = user.Forename,
            Surname = user.Surname,
            Email = user.Email,
            DateOfBirth = user.DateOfBirth,
            IsActive = user.IsActive,
            Logs = _userLogService
                 .GetByUserId(user.Id)
                 .OrderByDescending(log => log.TimestampUtc)
                 .ToList()
        };

        return View(model);
    }

    [HttpGet("edit/{id}")]
    public IActionResult Edit(long id)
    {
        var user = _userService.GetById(id);

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
    public IActionResult Edit(long id, EditUserViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = _userService.GetById(id);

        if (user == null)
        {
            return NotFound();
        }

        user.Forename = model.Forename;
        user.Surname = model.Surname;
        user.Email = model.Email;
        user.DateOfBirth = model.DateOfBirth!.Value;
        user.IsActive = model.IsActive;

        _userService.Update(user);

        _userLogService.Create(new UserLog
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
    public IActionResult Delete(long id)
    {
        var user = _userService.GetById(id);

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
    public IActionResult Delete(long id, DeleteUserViewModel model)
    {
        if (id != model.Id)
        {
            return BadRequest();
        }

        var user = _userService.GetById(id);

        if (user == null)
        {
            return NotFound();
        }

        _userLogService.Create(new UserLog
        {
            UserId = user.Id,
            UserDisplayName = $"{user.Forename} {user.Surname}",
            Action = "Deleted",
            Details = $"User {user.Email} was deleted.",
            TimestampUtc = DateTime.UtcNow
        });
        _userService.Delete(user);

        return RedirectToAction(nameof(List));
    }
}
