using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Configuration;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class NotificationManager : INotificationService
    {
        private readonly IRepositoryManager _manager;
        private readonly String _serviceAccountJsonPath;
        private const string FirebaseScope = "https://www.googleapis.com/auth/firebase.messaging";
        private const string FcmEndpoint = "https://fcm.googleapis.com/v1/projects/nesservice-6687d/messages:send";
        private readonly GoogleCredential _googleCredential;

        public NotificationManager(IRepositoryManager manager)
        {
            _manager = manager;

            _serviceAccountJsonPath = ConfigurationManager.AppSettings["ServiceAccountJsonPath"];

            if (string.IsNullOrEmpty(_serviceAccountJsonPath))
            {
                throw new Exception("ServiceAccountJsonPath girilmedi");
            }
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _serviceAccountJsonPath);

            _googleCredential = GoogleCredential.FromFile(fullPath)
                .CreateScoped(FirebaseScope);
        }

        private async Task<string> GetAccessTokenAsync()
        {
            var token = await _googleCredential.UnderlyingCredential.GetAccessTokenForRequestAsync();
            return token;
        }

        public async Task SendNotificationAsync(Guid id)
        {
            try
            {
                var notification = await _manager.Notification.GetNotificationById(id);

                var accessToken = await GetAccessTokenAsync();

                var message = new
                {
                    message = new
                    {
                        token = notification.To,
                        notification = new
                        {
                            title = notification.Title,
                            body = notification.Body
                        }
                    }
                };

                var jsonMessage = JsonConvert.SerializeObject(message);
                using (var httpclient = new HttpClient())
                {
                    httpclient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                    var content = new StringContent(jsonMessage, Encoding.UTF8, "application/json");
                    var response = await httpclient.PostAsync(FcmEndpoint, content);
                    string responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"FCM Gönderme hatası {response.StatusCode} - {responseString}");
                    }
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
