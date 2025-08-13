using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public class NotificationService : INotificationService
    {
        private static readonly string ServiceUrl = string.Concat(Config.ApiUrl, "Notifications/Ploud/SyncComplete");

        public async Task<NotificationResponse> NotifyAsync(NotificationRequest notificationRequest)
        {
            NotificationResponse notificationResponse = new NotificationResponse();
            notificationResponse.IsSent = false;
            
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ServiceUrl))
                {
                    string json = JsonSerializer.Serialize(notificationRequest);
                    
                    using (StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;

                        using (HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                        {
                            if (!response.IsSuccessStatusCode)
                            {
                                return notificationResponse;
                            }
                        
                            notificationResponse.IsSent = true;
                        }
                    }
                }
            }

            return notificationResponse;
        }
    }
}
