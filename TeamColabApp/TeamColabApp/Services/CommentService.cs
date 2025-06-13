using TeamColabApp.Models.DTOs;
using TeamColabApp.Mappers;
using TeamColabApp.Models;
using TeamColabApp.Interfaces;
using TeamColabApp.Repositories;
using TeamColabApp.Misc;
using TeamColabApp.Contexts;

namespace TeamColabApp.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<long, Comment> _commentRepository;
        private readonly IRepository<long, ProjectTask> _projectTaskRepository;
        private readonly TeamColabAppContext _context;

        public CommentService(IRepository<long, Comment> commentRepository, IRepository<long, ProjectTask> projectTaskRepository, TeamColabAppContext context)
        {
            _commentRepository = commentRepository;
            _context = context;
            _projectTaskRepository = projectTaskRepository;
        }

        public async Task<CommentResponseDto> AddCommentAsync(CommentRequestDto commentRequest)
        {
            long userId = EntityNameToId.GetUserId(commentRequest.UserName, _context);
            long projectId = EntityNameToId.GetProjectId(commentRequest.ProjectTitle, _context);
            if (commentRequest.ProjectTaskId != 0)
            {
                ProjectTask? projectTask = await _projectTaskRepository.GetByIdAsync(commentRequest.ProjectTaskId);
                if (projectTask == null)
                    throw new Exception("Project Task with given ID not found");               
            }
            if (userId == 0) throw new Exception("User not found.");
            if (projectId == 0) throw new Exception("Project not found.");

            Comment entity = new Comment
            {
                Content = commentRequest.Content,
                UserId = userId,
                ProjectId = projectId,
                ProjectTaskId = commentRequest.ProjectTaskId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            Comment added = await _commentRepository.AddAsync(entity);
            return CommentMapper.EntityToResponseDto(added);
        }

        public async Task<CommentResponseDto> UpdateCommentAsync(long commentId, CommentRequestDto commentRequest)
        {
            Comment? existing = await _commentRepository.GetByIdAsync(commentId);
            if (existing == null)
                throw new KeyNotFoundException($"Comment with ID {commentId} not found.");

            long userId = EntityNameToId.GetUserId(commentRequest.UserName, _context);
            long projectId = EntityNameToId.GetProjectId(commentRequest.ProjectTitle, _context);

            if (userId == 0) throw new Exception("User not found.");
            if (projectId == 0) throw new Exception("Project not found.");

            existing.Content = commentRequest.Content;
            existing.UserId = userId;
            existing.ProjectId = projectId;
            existing.ProjectTaskId = commentRequest.ProjectTaskId;
            existing.UpdatedAt = DateTime.UtcNow;

            Comment updated = await _commentRepository.UpdateAsync(commentId, existing);
            return CommentMapper.EntityToResponseDto(updated);
        }

        public async Task<bool> DeleteCommentAsync(long commentId)
        {
            Comment? existing = await _commentRepository.GetByIdAsync(commentId);
            if (existing == null)
                return false;

            await _commentRepository.DeleteAsync(existing.CommentId);
            return true;
        }

        public async Task<IEnumerable<CommentResponseDto>> GetCommentsByProjectTaskAsync(long projectTaskId)
        {
            IEnumerable<Comment?> comments = await _commentRepository.GetAllAsync();
            return comments
                .Where(c => c != null && c.ProjectTaskId == projectTaskId)
                .Select(c => CommentMapper.EntityToResponseDto(c!));
        }

        public async Task<IEnumerable<CommentResponseDto>> GetCommentsByProjectNameAsync(string projectName)
        {
            long projectId = EntityNameToId.GetProjectId(projectName, _context);
            if (projectId == 0)
                return Enumerable.Empty<CommentResponseDto>();

            IEnumerable<Comment?> comments = await _commentRepository.GetAllAsync();
            return comments
                .Where(c => c != null && c.ProjectId == projectId)
                .Select(c => CommentMapper.EntityToResponseDto(c!));
        }

        public async Task<IEnumerable<CommentResponseDto>> GetCommentsByUserNameAsync(string userName)
        {
            long userId = EntityNameToId.GetUserId(userName, _context);
            if (userId == 0)
                return Enumerable.Empty<CommentResponseDto>();

            IEnumerable<Comment?> comments = await _commentRepository.GetAllAsync();
            return comments
                .Where(c => c != null && c.UserId == userId)
                .Select(c => CommentMapper.EntityToResponseDto(c!));
        }

        public async Task<IEnumerable<CommentResponseDto>> GetAllCommentsAsync()
        {
            IEnumerable<Comment?> comments = await _commentRepository.GetAllAsync();
            return comments
                .Where(c => c != null)
                .Select(c => CommentMapper.EntityToResponseDto(c!));
        }
    }
}
