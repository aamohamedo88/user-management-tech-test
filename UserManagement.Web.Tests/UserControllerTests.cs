using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Models;
using UserManagement.Services.Domain.Interfaces;
using UserManagement.Web.Controllers;
using UserManagement.Web.Models.Users;

namespace UserManagement.Data.Tests;

public class UserControllerTests
{
    [Fact]
    public async Task List_WhenServiceReturnsUsers_ModelMustContainUsers()
    {
        // Arrange: Initializes objects and sets the value of the data that is passed to the method under test.
        var controller = CreateController();
        var users = SetupUsers();

        // Act: Invokes the method under test with the arranged parameters.
        var result = await controller.List(null);

        // Assert: Verifies that the action of the method under test behaves as expected.
        var viewResult = result.Should().BeOfType<ViewResult>().Subject;
        var model = viewResult.Model.Should().BeOfType<UserListViewModel>().Subject;

        model.Items.Should().HaveCount(users.Length);
    }

    private User[] SetupUsers(string forename = "Johnny", string surname = "User", string email = "juser@example.com", bool isActive = true)
    {
        var users = new[]
        {
            new User
            {
                Forename = forename,
                Surname = surname,
                Email = email,
                IsActive = isActive
            }
        };

        _userService
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(users.ToList());

        return users;
    }

    private readonly Mock<IUserService> _userService = new();
    private readonly Mock<IUserLogService> _userLogService = new();
    private UsersController CreateController() => new(_userService.Object, _userLogService.Object);

    [Fact]
    public async Task Add_WhenModelIsValid_CreatesUser()
    {
        // Arrange
        var controller = CreateController();

        var model = new AddUserViewModel
        {
            Forename = "Test",
            Surname = "User",
            Email = "test@example.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            IsActive = true
        };

        // Act
        await controller.Add(model);

        // Assert
        _userService.Verify(
            service => service.CreateAsync(
                It.Is<User>(user =>
                    user.Forename == "Test" &&
                    user.Surname == "User" &&
                    user.Email == "test@example.com" &&
                    user.IsActive)),
            Times.Once);
    }

    [Fact]
    public async Task Add_WhenModelIsValid_CreatesLog()
    {
        // Arrange
        var controller = CreateController();

        var model = new AddUserViewModel
        {
            Forename = "Test",
            Surname = "User",
            Email = "test@example.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            IsActive = true
        };

        // Act
        await controller.Add(model);

        // Assert
        _userLogService.Verify(
            service => service.CreateAsync(
                It.Is<UserLog>(log =>
                    log.Action == "Created" &&
                    log.UserDisplayName == "Test User")),
            Times.Once);
    }

    [Fact]
    public async Task Edit_WhenModelIsValid_UpdatesUser()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Forename = "Old",
            Surname = "User",
            Email = "old@example.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            IsActive = true
        };

        _userService
            .Setup(service => service.GetByIdAsync(1))
            .ReturnsAsync(user);

        var controller = CreateController();

        var model = new EditUserViewModel
        {
            Id = 1,
            Forename = "Updated",
            Surname = "User",
            Email = "updated@example.com",
            DateOfBirth = new DateTime(2000, 1, 1),
            IsActive = true
        };

        // Act
        await controller.Edit(1, model);

        // Assert
        _userService.Verify(
            service => service.UpdateAsync(
                It.Is<User>(updatedUser =>
                    updatedUser.Forename == "Updated" &&
                    updatedUser.Email == "updated@example.com")),
            Times.Once);
    }

    [Fact]
    public async Task Delete_WhenUserExists_DeletesUser()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Forename = "Test",
            Surname = "User",
            Email = "test@example.com"
        };

        _userService
            .Setup(service => service.GetByIdAsync(1))
            .ReturnsAsync(user);

        var controller = CreateController();

        var model = new DeleteUserViewModel
        {
            Id = 1,
            Forename = "Test",
            Surname = "User",
            Email = "test@example.com"
        };

        // Act
        await controller.Delete(1, model);

        // Assert
        _userService.Verify(
            service => service.DeleteAsync(user),
            Times.Once);
    }
}
