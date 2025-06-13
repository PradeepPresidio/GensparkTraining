
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TeamColabApp.Contexts;
using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Repositories;
using TeamColabApp.Services;
using Xunit;

namespace UnitTests.ProjectServiceTests
{
    public class ProjectServiceTests
    {
        private TeamColabAppContext GetContext()
        {
            var options = new DbContextOptionsBuilder<TeamColabAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new TeamColabAppContext(options);
        }

        [Fact]
        public async Task AddProjectAsync_Success()
        {
            var context = GetContext();
            var projectRepo = new ProjectRepository(context);
            var userRepo = new UserRepository(context);
            var service = new ProjectService(projectRepo, userRepo, context);

            var teamLeader = new User { Name = "Leader", Role = "leader", IsActive = true };
            var member = new User { Name = "Member1", Role = "member", IsActive = true };
            context.Users.AddRange(teamLeader, member);
            await context.SaveChangesAsync();

            var dto = new ProjectRequestDto
            {
                Title = "New Project",
                Description = "Desc",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(7),
                TeamLeaderName = "Leader",
                TeamMembersName =[]
            };

            var result = await service.AddProjectAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Title, result.Title);
        }

        [Fact]
        public async Task AddProjectAsync_ThrowsIfDuplicate()
        {
            var context = GetContext();
            var projectRepo = new ProjectRepository(context);
            var userRepo = new UserRepository(context);
            var service = new ProjectService(projectRepo, userRepo, context);

            context.Projects.Add(new Project { Title = "Duplicate Project", Description = "desc",IsActive = true });
            await context.SaveChangesAsync();

            var dto = new ProjectRequestDto
            {
                Title = "Duplicate Project",
                TeamLeaderName = "Leader",
                TeamMembersName = []
            };

            await Assert.ThrowsAsync<Exception>(() => service.AddProjectAsync(dto));
        }

        [Fact]
        public async Task GetAllProjectsAsync_ReturnsActiveOnly()
        {
            var context = GetContext();
            var projectRepo = new ProjectRepository(context);
            var userRepo = new UserRepository(context);
            var service = new ProjectService(projectRepo, userRepo, context);

            context.Projects.AddRange(
                new Project { Title = "Active Project", Description = "des",IsActive = true },
                new Project { Title = "Inactive Project", Description = "des",IsActive = false }
            );
            await context.SaveChangesAsync();

            var projects = await service.GetAllProjectsAsync();
            Assert.Single(projects);
            Assert.Equal("Active Project", projects.First().Title);
        }

        [Fact]
        public async Task SoftDeleteProjectAsync_ReturnsTrue()
        {
            var context = GetContext();
            var projectRepo = new ProjectRepository(context);
            var userRepo = new UserRepository(context);
            var service = new ProjectService(projectRepo, userRepo, context);

            var project = new Project { Title = "Project X", IsActive = true ,Description = "des"};
            context.Projects.Add(project);
            await context.SaveChangesAsync();

            var result = await service.SoftDeleteProjectAsync("Project X");
            Assert.True(result);
        }

        [Fact]
        public async Task GetProjectByTitleAsync_ThrowsIfNotFound()
        {
            var context = GetContext();
            var projectRepo = new ProjectRepository(context);
            var userRepo = new UserRepository(context);
            var service = new ProjectService(projectRepo, userRepo, context);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetProjectByTitleAsync("Missing"));
        }
    }
}
