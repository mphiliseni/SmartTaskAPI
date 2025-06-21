using Microsoft.AspNetCore.Authorization;
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
        public AdminDashboardResponse DashboardData { get; set; } = new();

        public DashboardModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var token = Request.Cookies["access_token"];
            if (string.IsNullOrWhiteSpace(token))
            {
                DashboardData = new AdminDashboardResponse();
                return;
            }

            var client = _httpClientFactory.CreateClient();
            client.BaseAddress = new Uri("https://localhost:5028"); 
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/dashboard/admin");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                DashboardData = JsonSerializer.Deserialize<AdminDashboardResponse>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new AdminDashboardResponse();
            }
        }
    }
}
