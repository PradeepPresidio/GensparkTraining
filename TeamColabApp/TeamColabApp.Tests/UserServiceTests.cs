using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Interfaces;
using TeamColabApp.Services;
using TeamColabApp.Contexts;
using TeamColabApp.Misc;

public class UserServiceTests
{
    private readonly Mock<IRepository<long, User>> _mockRepo = new();
    private readonly Mock<IEncryptionService> _mockEncryption = new();
    private readonly TeamColabAppContext _context;

    public UserServiceTests()
    {
        var options = new DbContextOptionsBuilder<TeamColabAppContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new TeamColabAppContext(options);
    }

    private UserService CreateService() => new(_mockRepo.Object, _mockEncryption.Object, _context);

    [Fact]
    public async Task GetUserByName_ReturnsUser()
    {
        var user = new User { Name = "John", IsActive = true };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User?> { user });

        var service = CreateService();
        var result = await service.GetUserByName("John");

        Assert.Equal("John", result.Name);
    }

    [Fact]
    public async Task GetUserByName_ThrowsIfNotFound()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User?>());
        var service = CreateService();
        await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetUserByName("Unknown"));
    }

    [Fact]
    public async Task GetAllUsersAsync_ReturnsActiveUsers()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User?> {
            new() { Name = "Active", IsActive = true },
            new() { Name = "Inactive", IsActive = false }
        });

        var result = await CreateService().GetAllUsersAsync();

        Assert.Single(result);
        Assert.Equal("Active", result.First().Name);
    }

    [Fact]
    public async Task AddUserAsync_Success()
    {
        var dto = new UserRequestDto { Name = "NewUser", Password = "pass" };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User?>());
        _mockEncryption.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
            .ReturnsAsync(new EncryptModel { EncryptedString = "enc" });
        _mockRepo.Setup(r => r.AddAsync(It.IsAny<User>()))
            .ReturnsAsync(new User { Name = "NewUser", Password = "enc", IsActive = true });

        var result = await CreateService().AddUserAsync(dto);

        Assert.Equal("NewUser", result.Name);
    }

    [Fact]
    public async Task AddUserAsync_ThrowsOnDuplicate()
    {
        var dto = new UserRequestDto { Name = "User", Password = "pass" };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User?> { new() { Name = "User" } });

        await Assert.ThrowsAsync<Exception>(() => CreateService().AddUserAsync(dto));
    }

    [Fact]
    public async Task UpdateUserAsync_Success()
    {
        var existing = new User { UserId = 1, Name = "OldName", IsActive = true };
        await _context.Users.AddAsync(existing);
        await _context.SaveChangesAsync();

        var dto = new UserRequestDto { Name = "NewName", Password = "123", Role = "User" };

        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User?>());
        _mockRepo.Setup(r => r.GetByIdAsync(existing.UserId)).ReturnsAsync(existing);
        _mockRepo.Setup(r => r.UpdateAsync(existing.UserId, It.IsAny<User>())).ReturnsAsync(existing);
        _mockEncryption.Setup(e => e.EncryptData(It.IsAny<EncryptModel>()))
            .ReturnsAsync(new EncryptModel { EncryptedString = "enc" });

        var result = await CreateService().UpdateUserAsync(dto, "OldName");

        Assert.Equal("NewName", result.Name);
    }

    [Fact]
    public async Task UpdateUserAsync_ThrowsIfNotFound()
    {
        var dto = new UserRequestDto { Name = "Updated", Password = "pass" };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User?>());
        await Assert.ThrowsAsync<KeyNotFoundException>(() => CreateService().UpdateUserAsync(dto, "Missing"));
    }

    [Fact]
    public async Task HardDeleteUserAsync_Success()
    {
        var user = new User { UserId = 1, Name = "ToDelete", IsActive = true };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        _mockRepo.Setup(r => r.GetByIdAsync(user.UserId)).ReturnsAsync(user);
        _mockRepo.Setup(r => r.DeleteAsync(user.UserId)).ReturnsAsync(true);

        var result = await CreateService().HardDeleteUserAsync("ToDelete");

        Assert.True(result);
    }

    [Fact]
    public async Task HardDeleteUserAsync_ThrowsIfNotFound()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() => CreateService().HardDeleteUserAsync("Invalid"));
    }

    [Fact]
    public async Task SoftDeleteUserAsync_Success()
    {
        var user = new User { UserId = 2, Name = "Softy", IsActive = true };
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User?> { user });
        _mockRepo.Setup(r => r.UpdateAsync(user.UserId, user)).ReturnsAsync(user);

        var result = await CreateService().SoftDeleteUserAsync("Softy");

        Assert.True(result);
    }

    [Fact]
    public async Task SoftDeleteUserAsync_ReturnsFalseIfNotFound()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User?>());
        var result = await CreateService().SoftDeleteUserAsync("Missing");
        Assert.False(result);
    }

    [Fact]
    public async Task RetrieveSoftDeleteUserAsync_Success()
    {
        var user = new User { UserId = 3, Name = "Recover", IsActive = false };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        _mockRepo.Setup(r => r.GetByIdAsync(user.UserId)).ReturnsAsync(user);
        _mockRepo.Setup(r => r.UpdateAsync(user.UserId, user)).ReturnsAsync(user);

        var result = await CreateService().RetrieveSoftDeleteUserAsync("Recover");

        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task RetrieveSoftDeleteUserAsync_ThrowsIfNotFound()
    {
        await Assert.ThrowsAsync<Exception>(() => CreateService().RetrieveSoftDeleteUserAsync("NoOne"));
    }

    [Fact]
    public async Task GetUsersByRoleAsync_ReturnsMatchingUsers()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User?>
        {
            new() { Name = "User1", Role = "Admin", IsActive = true },
            new() { Name = "User2", Role = "User", IsActive = true }
        });

        var result = await CreateService().GetUsersByRoleAsync("Admin");

        Assert.Single(result);
        Assert.Equal("User1", result.First().Name);
    }

    [Fact]
    public async Task GetUsersUnderProjectAsync_ReturnsMatchingUsers()
    {
        _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<User?>
        {
            new() {
                Name = "Dev",
                IsActive = true,
                Projects = new List<Project> { new Project { Title = "Alpha",Description = "Test project" } }
            }
        });

        var result = await CreateService().GetUsersUnderProjectAsync("Alpha");

        Assert.Single(result);
        Assert.Equal("Dev", result.First().Name);
    }
}
