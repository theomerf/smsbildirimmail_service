using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NesInterface.Models;

namespace NesInterface.Controllers
{
    public class HomeController : Controller
    {
        private readonly RepositoryContext _context;

        public HomeController(RepositoryContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var requests = await _context.Requests.ToListAsync();
            return View(requests);
        }

        public IActionResult AddRequest()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddRequest([FromForm]Request request)
        {
            _context.Requests.Add(request);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SendMail([FromQuery]int requestId)
        {
            var request = await _context.Requests.Where(r => r.Id == requestId).FirstOrDefaultAsync();
            if (request != null) 
            {
                if (request.IsBodyHtml)
                {
                    request.Body = $"<!DOCTYPE html>\r\n<html lang=\"tr\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <title>Bilgilendirme</title>\r\n</head>\r\n<body style=\"font-family: Arial, sans-serif; background-color: #f4f4f4; padding: 0; margin: 0;\">\r\n    <table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" width=\"600\" style=\"background-color: #ffffff; border-radius: 8px; overflow: hidden;\">\r\n        <tr>\r\n            <td style=\"background-color: #4CAF50; padding: 20px; text-align: center; color: #ffffff; font-size: 24px; font-weight: bold;\">\r\n                📢 Önemli Bilgilendirme\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td style=\"padding: 30px; color: #333333; font-size: 16px; line-height: 1.6;\">\r\n                <table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" style=\"border-collapse: collapse;\">\r\n                    <tr>\r\n                        <td style=\"text-align:center;padding: 8px; border: 1px solid #dddddd; background-color: #f9f9f9;\"><strong>{request.Body}</strong></td>\r\n                    </tr>\r\n                </table>\r\n            </td>\r\n        </tr>\r\n        <tr>\r\n            <td style=\"background-color: #f4f4f4; padding: 15px; text-align: center; font-size: 12px; color: #888888;\">\r\n                Bu e-posta otomatik olarak gönderilmiştir. Lütfen yanıtlamayın.\r\n            </td>\r\n        </tr>\r\n    </table>\r\n</body>\r\n</html>";
                }
                var mail = new Mail()
                {
                    To = request.To,
                    From = request.From,
                    Subject = request.Subject,
                    Body = request.Body,
                    IsBodyHtml = request.IsBodyHtml,
                    Cc = request.Cc,
                    Bcc = request.Bcc,
                };
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", "yiSpT>lO~BC;BEi}JFgD`)IP^:oTMS");

                var jsonContent = JsonSerializer.Serialize(mail);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44382/Mail/Send", content);

                if (response.IsSuccessStatusCode) 
                {
                    TempData["success"] = "true";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["success"] = "false";
                    return RedirectToAction("Index");
                }


            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendNotification([FromQuery]int requestId)
        {
            var request = await _context.Requests.Where(r => r.Id == requestId).FirstOrDefaultAsync();
            if (request != null)
            {
                var notification = new Notification()
                {
                    To = "dUdJiVh7MqUTR_-HqjefGB:APA91bH8muBouKVURCoU-gP-7kZqBq9XS6lmJK3Ylvlodw3KDJN4j07wVqyGpCy0_zZ58LlpBlc_NVTjrAvWgNM3ZjrTASorDpXOTgPDI6T16NyK_fIoPHA",
                    Title = request.Subject,
                    Body = request.Body,
                };
                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", "yiSpT>lO~BC;BEi}JFgD`)IP^:oTMS");

                var jsonContent = JsonSerializer.Serialize(notification);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44382/Notification/Send", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "true";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["success"] = "false";
                    return RedirectToAction("Index");
                }


            }
            else
            {
                TempData["success"] = "false";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SendSms([FromQuery]int requestId)
        {
            var request = await _context.Requests.Where(r => r.Id == requestId).FirstOrDefaultAsync();
            if (request != null)
            {
                var sms = new Sms()
                {
                    To = request.To,
                    From = request.From,
                    Message = request.Body
                };

                using HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Add("x-api-key", "yiSpT>lO~BC;BEi}JFgD`)IP^:oTMS");

                request.IsBodyHtml = false;
                var jsonContent = JsonSerializer.Serialize(sms);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await client.PostAsync("https://localhost:44382/Sms/Send", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["success"] = "true";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["success"] = "false";
                    return RedirectToAction("Index");
                }


            }
            else
            {
                TempData["success"] = "false";
                return RedirectToAction("Index");
            }
        }

        public IActionResult TestNotification()
        {
            return View();
        }
    }
}
