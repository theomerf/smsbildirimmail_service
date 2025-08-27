using Microsoft.AspNetCore.Mvc;
using NesInterface.Models;
using System.Text.Json;

namespace NesInterface.Components
{
    public class RequestStatsViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            using HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("x-api-key", "yiSpT>lO~BC;BEi}JFgD`)IP^:oTMS");

            var response = await client.GetAsync("https://localhost:44382/Stats/Get");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Api isteği başarısız oldu");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var stats = JsonSerializer.Deserialize<IEnumerable<Stat>>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            });

            return View(stats);
        }
    }
}
