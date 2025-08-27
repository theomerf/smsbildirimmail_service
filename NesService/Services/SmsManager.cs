using Newtonsoft.Json;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class SmsManager : ISmsService
    {
        private readonly IRepositoryManager _manager;

        public SmsManager(IRepositoryManager manager)
        {
            _manager = manager;
        }

        public async Task SendSmsAsync(Guid id)
        {
            try
            {
                var sms = await _manager.Sms.GetSmsById(id);

                string smsApiUrl = "https://api.smsprovider.com/send";

                var payload = new
                {
                    from = sms.From,
                    to = sms.To,
                    message = sms.Message,
                };

                string jsonPayload = JsonConvert.SerializeObject(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsync(smsApiUrl, content);

                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"Sms gönderme hatası - ID: {id}, StatusCode: {response.StatusCode}, {responseContent}");
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