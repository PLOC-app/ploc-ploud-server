using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public interface INotificationService
    {
        Task<NotificationResponse> NotifyAsync(NotificationRequest notificationRequest);
    }
}
