using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using theupskilzapi.DTOs;

namespace theupskilzapi.Services
{
    public class RazorpayService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public RazorpayService(IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        public async Task<string> CreateOrderAsync(PaymentRequestDto request)
        {
            var client = _httpClientFactory.CreateClient();

            string keyId = _config["Razorpay:Key"];
            string keySecret = _config["Razorpay:Secret"];
            var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{keyId}:{keySecret}"));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authToken);

            var payload = new
            {
                amount = request.Amount,
                currency = request.Currency,
                receipt = request.Receipt,
                payment_capture = 1
            };

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api.razorpay.com/v1/orders", content);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadAsStringAsync();
            return result; // JSON with order_id etc.
        }
    }
}
