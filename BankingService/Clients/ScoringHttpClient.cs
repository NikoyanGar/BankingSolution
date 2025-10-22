using BankingService.DTOs;

namespace BankingService.Clients
{
    public class ScoringHttpClient : IScoringClient
    {
        private readonly HttpClient _httpClient;

        public ScoringHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ScoringDto> GetScoreAsync(string clientId)
        {
            //[FromQuery] GetScoreModel scoreModel
            //https://localhost:5001/api/Scoring/GetByClientId?ClientId=C001
            var response = await _httpClient.GetAsync($"/api/Scoring/GetByClientId?ClientId={clientId}");
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<ScoringDto>();
        }
    }
}
