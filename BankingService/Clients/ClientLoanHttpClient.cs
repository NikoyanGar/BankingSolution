using BankingService.DTOs;
using System.Text;
using System.Text.Json;

namespace BankingService.Clients
{
    public class ClientLoanHttpClient : IClientLoanClient
    {
        private readonly HttpClient _httpClient;

        public ClientLoanHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoanResponseDto> GetClientLoanHistoryAsync(string clientId)
        {
            //https://localhost:5002/api/Loan/getLoanHistory

            var requestBody = new { clientId = clientId };
            //HttpContent content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            //https://localhost:5002/api/Loan/C1
            var response = await _httpClient.GetAsync($"/api/Loan/{clientId}");
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<LoanResponseDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return data ?? new LoanResponseDto();
        }
    }
}
