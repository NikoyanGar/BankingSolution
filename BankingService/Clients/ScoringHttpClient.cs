using BankingService.Models.Responses;
using FluentResults;
using System.Text.Json;

namespace BankingService.Clients
{
    public class ScoringHttpClient : IScoringClient
    {
        private readonly HttpClient _httpClient;

        public ScoringHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Result<ScoringResponse>> GetScoreAsync(string clientId)
        {
            //[FromQuery] GetScoreModel scoreModel
            ///https://localhost:5001/api/Scoring/C1
            var response = await _httpClient.GetAsync($"/api/Scoring/{clientId}");
            
            if (!response.IsSuccessStatusCode) return Result.Fail<ScoringResponse>("Failed to get score");

            var scoring = await response.Content.ReadFromJsonAsync<ScoringResponse>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (scoring == null)
                return Result.Fail<ScoringResponse>("Empty or invalid response");

            return Result.Ok(scoring);
        }
    }
}
