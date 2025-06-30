using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System;

public class SmsService
{
    private readonly IConfiguration _config;

    public SmsService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendOtp(string phoneNumber, string otp)
    {
        var apiKey = _config["SmsSettings:ApiKey"];

        // Clean and format the number
        phoneNumber = phoneNumber.Replace("+", "");
        if (!phoneNumber.StartsWith("91"))
        {
            phoneNumber = "91" + phoneNumber;
        }

        var payload = new
        {
            route = "otp",
            variables_values = otp,
            numbers = phoneNumber,
            flash = "0"
        };

        var jsonPayload = JsonSerializer.Serialize(payload);
        var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("authorization", apiKey);

        var response = await client.PostAsync("https://www.fast2sms.com/dev/bulkV2", content);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"SMS API Failed: {response.StatusCode} - {responseBody}");
        }
    }
}
