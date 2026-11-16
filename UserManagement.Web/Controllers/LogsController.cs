using System;
using System.Linq;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Models.Logs;

namespace UserManagement.Web.Controllers;

[Route("logs")]
public class LogsController : Controller
{
    private readonly IUserLogService _userLogService;

    public LogsController(IUserLogService userLogService)
    {
        _userLogService = userLogService;
    }

    [HttpGet]
    public ViewResult List(int page = 1)
    {
        const int pageSize = 10;

        var logs = _userLogService
            .GetAll()
            .OrderByDescending(log => log.TimestampUtc);

        var totalLogs = logs.Count();

        var items = logs
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(log => new LogListItemViewModel
            {
                Id = log.Id,
                UserId = log.UserId,
                UserDisplayName = log.UserDisplayName,
                Action = log.Action,
                Details = log.Details,
                TimestampUtc = log.TimestampUtc
            })
            .ToList();

        var model = new LogListViewModel
        {
            Items = items,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling(totalLogs / (double)pageSize)
        };

        return View(model);
    }

    [HttpGet("view/{id}")]
    public IActionResult View(long id)
    {
        var log = _userLogService.GetById(id);

        if (log == null)
        {
            return NotFound();
        }

        var model = new LogDetailsViewModel
        {
            Id = log.Id,
            UserId = log.UserId,
            UserDisplayName = log.UserDisplayName,
            Action = log.Action,
            Details = log.Details,
            TimestampUtc = log.TimestampUtc
        };

        return View(model);
    }
}
