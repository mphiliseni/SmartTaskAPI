using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SmartTaskAPI.DTOs.Dashboard;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SmartTaskAPI.Pages
{
    [Authorize(Roles = "Admin")]
    public class DashboardModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public DashboardModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public AdminDashboardResponse DashboardData { get; set; } = new();

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var token = HttpContext.Request.Cookies["access_token"]; //Get token from cookie (or session)

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:5028/api/dashboard/admin");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                DashboardData = JsonSerializer.Deserialize<AdminDashboardResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })!;
            }
        }
    }
}
