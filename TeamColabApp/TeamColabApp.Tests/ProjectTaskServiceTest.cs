using Xunit;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Contexts;
using TeamColabApp.Repositories;
using TeamColabApp.Services;
using TeamColabApp.Mappers;

namespace TeamColabApp.Tests.Services
{
    public class ProjectTaskServiceTests
    {
        private TeamColabAppContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<TeamColabAppContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new TeamColabAppContext(options);

            // Seed test data
            var user1 = new User { Name = "Alice" ,Password = "pass" };
            var user2 = new User { Name = "Bob", Password = "pass" };
            var project = new Project { Title = "TestProject", Description = "Desc", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(10) };

            context.Users.AddRange(user1, user2);
            context.Projects.Add(project);
            context.SaveChanges();

            return context;
        }

        private ProjectTaskService GetService(TeamColabAppContext context)
        {
            var taskRepo = new ProjectTaskRepository(context);
            var projectRepo = new ProjectRepository(context);
            var userRepo = new UserRepository(context);
            return new ProjectTaskService(taskRepo, projectRepo, userRepo, context);
        }

        [Fact]
        public async Task AddAsync_ShouldAddProjectTask()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var dto = new ProjectTaskRequestDto
            {
                Title = "Task1",
                Description = "Some description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5),
                Status = "Open",
                Priority = "High",
                ProjectTitle = "TestProject",
                AssignedUserNames = new[] { "Alice" }
            };

            var result = await service.AddAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Task1", result.Title);
        }

        [Fact]
        public async Task AddAsync_ShouldThrow_WhenProjectNotFound()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var dto = new ProjectTaskRequestDto
            {
                Title = "Invalid",
                ProjectTitle = "Nonexistent",
                AssignedUserNames = []
            };

            await Assert.ThrowsAsync<Exception>(() => service.AddAsync(dto));
        }

        [Fact]
        public async Task SoftDeleteAsync_ShouldMarkInactive()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var added = await service.AddAsync(new ProjectTaskRequestDto
            {
                Title = "ToDelete",
                Description = "desc",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Status = "New",
                Priority = "Medium",
                ProjectTitle = "TestProject"
            });

            var success = await service.SoftDeleteAsync(added.ProjectTaskId);
            Assert.True(success);

            var task = await context.ProjectTasks.FindAsync(added.ProjectTaskId);
            Assert.False(task!.IsActive);
        }

        [Fact]
        public async Task HardDeleteAsync_ShouldRemoveTask()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var added = await service.AddAsync(new ProjectTaskRequestDto
            {
                Title = "ToHardDelete",
                Description = "desc",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Status = "New",
                Priority = "Medium",
                ProjectTitle = "TestProject"
            });

            var deleted = await service.HardDeleteAsync(added.ProjectTaskId);
            Assert.True(deleted);

            var task = await context.ProjectTasks.FindAsync(added.ProjectTaskId);
            Assert.Null(task);
        }

        [Fact]
        public async Task RetrieveSoftDeleteAsync_ShouldReactivateTask()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var added = await service.AddAsync(new ProjectTaskRequestDto
            {
                Title = "SoftDeleted",
                Description = "desc",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Status = "New",
                Priority = "Medium",
                ProjectTitle = "TestProject"
            });

            await service.SoftDeleteAsync(added.ProjectTaskId);
            var restored = await service.RetrieveSoftDeleteAsync(added.ProjectTaskId);

            Assert.True(restored.IsActive);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnActiveTasks()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            await service.AddAsync(new ProjectTaskRequestDto
            {
                Title = "ActiveTask",
                Description = "desc",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Status = "Open",
                Priority = "High",
                ProjectTitle = "TestProject"
            });

            var tasks = await service.GetAllAsync();
            Assert.Single(tasks);
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyTask()
        {
            var context = GetInMemoryDbContext();
            var service = GetService(context);

            var added = await service.AddAsync(new ProjectTaskRequestDto
            {
                Title = "ToUpdate",
                Description = "desc",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                Status = "Open",
                Priority = "High",
                ProjectTitle = "TestProject"
            });

            var updated = await service.UpdateAsync(added.ProjectTaskId, new ProjectTaskRequestDto
            {
                Title = "Updated",
                Description = "new desc",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(3),
                Status = "InProgress",
                Priority = "Low",
                ProjectTitle = "TestProject"
            });

            Assert.Equal("Updated", updated.Title);
            Assert.Equal("InProgress", updated.Status);
        }
    }
}
