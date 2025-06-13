using TeamColabApp.Models.DTOs;
namespace TeamColabApp.Interfaces
{
    public interface INotificationService
    {
        public virtual Task<NotificationResponseDto> AddNotificationAsync(NotificationRequestDto notificationRequest)
        {
            throw new NotImplementedException();
        }
        public virtual Task<NotificationResponseDto> UpdateNotificationAsync(long notificationId, NotificationRequestDto notificationRequest)
        {
            throw new NotImplementedException();
        }
        public virtual Task<bool> DeleteNotificationAsync(long notificationId)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<NotificationResponseDto>> GetNotificationsByUserNameAsync(string Name)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<NotificationResponseDto>> GetNotificationsByProjectNameAsync(string Name)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<NotificationResponseDto>> GetNotificationsByProjectTaskAsync(long projectTask)
        {
            throw new NotImplementedException();
        }
        public virtual Task<IEnumerable<NotificationResponseDto>> GetAllNotificationsAsync()
        {
            throw new NotImplementedException();
        }
    }
}