using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Controllers;

namespace UserManagement.Data.Tests;

public class LogsControllerTests
{
    private readonly Mock<IUserLogService> _userLogService = new();

    private LogsController CreateController() =>
        new(_userLogService.Object);

    [Fact]
    public async Task List_CallsGetAllAsync()
    {
        // Arrange
        _userLogService
            .Setup(service => service.GetAllAsync())
            .ReturnsAsync(new List<UserLog>());

        var controller = CreateController();

        // Act
        await controller.List();

        // Assert
        _userLogService.Verify(
            service => service.GetAllAsync(),
            Times.Once);
    }

    [Fact]
    public async Task View_WhenLogExists_ReturnsView()
    {
        // Arrange
        var log = new UserLog
        {
            Id = 1,
            UserId = 1,
            UserDisplayName = "Test User",
            Action = "Created",
            Details = "User was created.",
            TimestampUtc = DateTime.UtcNow
        };

        _userLogService
            .Setup(service => service.GetByIdAsync(1))
            .ReturnsAsync(log);

        var controller = CreateController();

        // Act
        var result = await controller.View(1);

        // Assert
        result.Should().BeOfType<ViewResult>();
    }

    [Fact]
    public async Task View_WhenLogDoesNotExist_ReturnsNotFound()
    {
        // Arrange
        _userLogService
            .Setup(service => service.GetByIdAsync(999))
            .ReturnsAsync((UserLog?)null);

        var controller = CreateController();

        // Act
        var result = await controller.View(999);

        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}
