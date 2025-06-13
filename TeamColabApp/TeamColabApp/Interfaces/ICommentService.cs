using TeamColabApp.Models.DTOs;
namespace TeamColabApp.Interfaces
{
    public interface ICommentService
    {
        public virtual Task<CommentResponseDto> AddCommentAsync(CommentRequestDto commentRequest)
        {
            throw new NotImplementedException();
        }
        public virtual Task<CommentResponseDto> UpdateCommentAsync(long commentId, CommentRequestDto commentRequest)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> DeleteCommentAsync(long commintId)
        {
            throw new NotImplementedException();
        }
        // public virtual Task<bool> SoftDeleteCommentAsync(long commentId)
        // {
        //     throw new NotImplementedException();
        // }
        // public virtual Task<bool> HardDeleteCommentAsync(long commentId)
        // {
        //     throw new NotImplementedException();
        // }
        // public virtual Task<CommentResponseDto> RetrieveSoftDeleteCommentAsync(long commentId)
        // {
        //     throw new NotImplementedException();
        // }
        public virtual Task<IEnumerable<CommentResponseDto>> GetCommentsByProjectTaskAsync(long id)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<CommentResponseDto>> GetCommentsByProjectNameAsync(string Name)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<CommentResponseDto>> GetCommentsByUserNameAsync(string Name)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<CommentResponseDto>> GetAllCommentsAsync()
        {
            throw new NotImplementedException();
        }
    }
}