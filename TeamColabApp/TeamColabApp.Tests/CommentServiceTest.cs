using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TeamColabApp.Contexts;
using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Repositories;
using TeamColabApp.Services;
using TeamColabApp.Mappers;

namespace TeamColabApp.Tests.Services
{
    public class CommentServiceTests
    {
        private TeamColabAppContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<TeamColabAppContext>()
                .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
                .Options;
            var context = new TeamColabAppContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [Fact]
        public async Task AddCommentAsync_ShouldAddAndReturnComment()
        {
            var context = GetInMemoryContext();

            var user = new User { UserId = 1, Name = "Alice", Role = "member", IsActive = true };
            var project = new Project { ProjectId = 1, Title = "Demo Project", Description = "Demo Description" };
            context.Users.Add(user);
            context.Projects.Add(project);
            context.SaveChanges();

            var commentRepo = new CommentRepository(context);
            var taskRepo = new ProjectTaskRepository(context);
            var service = new CommentService(commentRepo, taskRepo, context);

            var request = new CommentRequestDto
            {
                Content = "Initial comment",
                UserName = "Alice",
                ProjectTitle = "Demo Project",
                ProjectTaskId = 0
            };

            var result = await service.AddCommentAsync(request);

            Assert.NotNull(result);
            Assert.Equal("Initial comment", result.Content);
        }

        [Fact]
        public async Task AddCommentAsync_ShouldThrow_WhenUserNotFound()
        {
            var context = GetInMemoryContext();
            var commentRepo = new CommentRepository(context);
            var taskRepo = new ProjectTaskRepository(context);
            var service = new CommentService(commentRepo, taskRepo, context);

            var request = new CommentRequestDto
            {
                Content = "Failing comment",
                UserName = "Ghost",
                ProjectTitle = "Demo Project"
            };

            await Assert.ThrowsAsync<Exception>(() => service.AddCommentAsync(request));
        }

        [Fact]
        public async Task UpdateCommentAsync_ShouldUpdateAndReturnComment()
        {
            var context = GetInMemoryContext();

            var user = new User { UserId = 1, Name = "Bob", Role = "member", IsActive = true };
            var project = new Project { ProjectId = 1, Title = "Update Project", Description = "Details" };
            var comment = new Comment { CommentId = 1, Content = "Old", UserId = 1, ProjectId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

            context.Users.Add(user);
            context.Projects.Add(project);
            context.Comments.Add(comment);
            context.SaveChanges();

            var commentRepo = new CommentRepository(context);
            var taskRepo = new ProjectTaskRepository(context);
            var service = new CommentService(commentRepo, taskRepo, context);

            var request = new CommentRequestDto
            {
                Content = "Updated content",
                UserName = "Bob",
                ProjectTitle = "Update Project",
                ProjectTaskId = 0
            };

            var result = await service.UpdateCommentAsync(1, request);

            Assert.NotNull(result);
            Assert.Equal("Updated content", result.Content);
        }

        [Fact]
        public async Task DeleteCommentAsync_ShouldReturnTrue_WhenFound()
        {
            var context = GetInMemoryContext();
            var comment = new Comment { CommentId = 1, Content = "Delete me", UserId = 1, ProjectId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

            context.Comments.Add(comment);
            context.SaveChanges();

            var commentRepo = new CommentRepository(context);
            var taskRepo = new ProjectTaskRepository(context);
            var service = new CommentService(commentRepo, taskRepo, context);

            var result = await service.DeleteCommentAsync(1);

            Assert.True(result);
        }

        [Fact]
        public async Task GetCommentsByUserNameAsync_ShouldReturnComments()
        {
            var context = GetInMemoryContext();

            var user = new User { UserId = 2, Name = "Commenter", Role = "member", IsActive = true };
            var comment = new Comment { Content = "User comment", UserId = 2, ProjectId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow };

            context.Users.Add(user);
            context.Comments.Add(comment);
            context.SaveChanges();

            var commentRepo = new CommentRepository(context);
            var taskRepo = new ProjectTaskRepository(context);
            var service = new CommentService(commentRepo, taskRepo, context);

            var result = await service.GetCommentsByUserNameAsync("Commenter");

            Assert.Single(result);
        }

        [Fact]
        public async Task GetAllCommentsAsync_ShouldReturnAll()
        {
            var context = GetInMemoryContext();
            context.Comments.AddRange(
                new Comment { Content = "A", UserId = 1, ProjectId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
                new Comment { Content = "B", UserId = 1, ProjectId = 1, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
            );
            context.SaveChanges();

            var commentRepo = new CommentRepository(context);
            var taskRepo = new ProjectTaskRepository(context);
            var service = new CommentService(commentRepo, taskRepo, context);

            var result = await service.GetAllCommentsAsync();

            Assert.Equal(2, result.Count());
        }
    }
}
