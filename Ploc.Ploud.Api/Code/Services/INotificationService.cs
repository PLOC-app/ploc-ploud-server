using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public interface INotificationService
    {
        Task<NotificationResponse> NotifyAsync(NotificationRequest notificationRequest);
    }
}
