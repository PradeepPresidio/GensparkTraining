using TeamColabApp.Models;
using TeamColabApp.Models.DTOs;
using TeamColabApp.Contexts;
namespace TeamColabApp.Misc
{
    
    public static class EntityNameToId
    {
        public static long GetUserId(string name, TeamColabAppContext context)
        {
            return context.Users.FirstOrDefault(u => u.Name == name)?.UserId ?? 0;
        }

        public static long GetProjectId(string name, TeamColabAppContext context)
        {
            return context.Projects.FirstOrDefault(p => p.Title == name)?.ProjectId ?? 0;
        }

        public static long GetProjectTaskId(string projectName,string ProjectTaskName, TeamColabAppContext context)
        {
            return context.ProjectTasks
                .FirstOrDefault(pt => pt.Project.Title == projectName && pt.Title == ProjectTaskName)?.ProjectTaskId ?? 0;
        }
    }
}