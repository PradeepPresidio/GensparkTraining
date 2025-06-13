using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using TeamColabApp.Contexts;
using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Repositories;
using TeamColabApp.Services;
using Xunit;

namespace UnitTests.NotificationServiceTests
{
    public class NotificationServiceTests
    {
        private TeamColabAppContext GetContext()
        {
            var options = new DbContextOptionsBuilder<TeamColabAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new TeamColabAppContext(options);
        }

        private IHubContext<NotificationHub> GetFakeHubContext()
        {
            var mockClients = new Moq.Mock<IHubClients>();
            var mockClientProxy = new Moq.Mock<IClientProxy>();

            mockClients.Setup(clients => clients.User(Moq.It.IsAny<string>()))
                       .Returns(mockClientProxy.Object);

            var mockHubContext = new Moq.Mock<IHubContext<NotificationHub>>();
            mockHubContext.Setup(c => c.Clients).Returns(mockClients.Object);

            return mockHubContext.Object;
        }

        [Fact]
        public async Task AddNotificationAsync_Success()
        {
            var context = GetContext();
            var notificationRepo = new NotificationRepository(context);
            var userRepo = new UserRepository(context);
            var projectRepo = new ProjectRepository(context);
            var taskRepo = new ProjectTaskRepository(context);
            var hubContext = GetFakeHubContext();

            var user = new User { Name = "Alice", Role = "member" };
            var project = new Project { Title = "Demo", Description = "desc", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(7) };
            var task = new ProjectTask
            {
                Title = "Fix bug",
                Status = "Open",
                Priority = "High",
                Description = "Fix the UI issue",
                Project = project
            };

            context.Users.Add(user);
            context.Projects.Add(project);
            context.ProjectTasks.Add(task);
            await context.SaveChangesAsync();

            var dto = new NotificationRequestDto
            {
                Message = "Task assigned",
                Type = "info",
                UserId = user.UserId,
                ProjectId = project.ProjectId,
                ProjectTaskId = task.ProjectTaskId
            };

            var service = new NotificationService(notificationRepo, context, taskRepo, userRepo, projectRepo, hubContext);
            var result = await service.AddNotificationAsync(dto);

            Assert.NotNull(result);
            Assert.Equal("Task assigned", result.Message);
            Assert.Equal("info", result.Type);
        }

        [Fact]
        public async Task DeleteNotificationAsync_Success()
        {
            var context = GetContext();
            var notificationRepo = new NotificationRepository(context);
            var userRepo = new UserRepository(context);
            var projectRepo = new ProjectRepository(context);
            var taskRepo = new ProjectTaskRepository(context);
            var hubContext = GetFakeHubContext();

            var user = new User { Name = "Bob", Role = "member" };
            var notification = new Notification
            {
                Message = "Remove me",
                Type = "warn",
                User = user
            };

            context.Users.Add(user);
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            var service = new NotificationService(notificationRepo, context, taskRepo, userRepo, projectRepo, hubContext);
            var result = await service.DeleteNotificationAsync(notification.NotificationId);

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateNotificationAsync_Throws_IfNotFound()
        {
            var context = GetContext();
            var notificationRepo = new NotificationRepository(context);
            var userRepo = new UserRepository(context);
            var projectRepo = new ProjectRepository(context);
            var taskRepo = new ProjectTaskRepository(context);
            var hubContext = GetFakeHubContext();

            var service = new NotificationService(notificationRepo, context, taskRepo, userRepo, projectRepo, hubContext);
            var dto = new NotificationRequestDto { Message = "Update", Type = "info", UserId = 1 };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.UpdateNotificationAsync(999, dto));
        }

        [Fact]
        public async Task UpdateNotificationAsync_Success()
        {
            var context = GetContext();
            var notificationRepo = new NotificationRepository(context);
            var userRepo = new UserRepository(context);
            var projectRepo = new ProjectRepository(context);
            var taskRepo = new ProjectTaskRepository(context);
            var hubContext = GetFakeHubContext();

            var user = new User { Name = "Carol", Role = "member" };
            var notification = new Notification
            {
                Message = "Old message",
                Type = "old",
                User = user
            };

            context.Users.Add(user);
            context.Notifications.Add(notification);
            await context.SaveChangesAsync();

            var dto = new NotificationRequestDto
            {
                Message = "Updated message",
                Type = "info",
                UserId = user.UserId
            };

            var service = new NotificationService(notificationRepo, context, taskRepo, userRepo, projectRepo, hubContext);
            var result = await service.UpdateNotificationAsync(notification.NotificationId, dto);

            Assert.Equal("Updated message", result.Message);
            Assert.Equal("info", result.Type);
        }
    }
}
