using ClientLoanServiceAPI.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ClientLoanServiceAPI.Repositories
{
    public class LoanHistoryRepository: ILoanHistoryRepository
    {
        private readonly IDistributedCache _distributedCache;
        private const string prefix = "loan_history_";

        public LoanHistoryRepository(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<List<LoanHistory>> GetLoanByClientIdAsync(string clientId)
        {
            var key = $"{prefix}{clientId}";
            var loanInfoJson = await _distributedCache.GetStringAsync(key);
            if (string.IsNullOrEmpty(loanInfoJson)) return new List<LoanHistory>();

            try
            {
                return JsonSerializer.Deserialize<List<LoanHistory>>(loanInfoJson) ?? new List<LoanHistory>();
            }
            catch (JsonException)
            {
                var single = JsonSerializer.Deserialize<LoanHistory>(loanInfoJson);
                return single is null ? new List<LoanHistory>() : new List<LoanHistory> { single };
            }
        }

        public async Task AddLoanAsync(LoanHistory loanHistory)
        {
            var key = $"{prefix}{loanHistory.ClientId}";
            var existingJson = await _distributedCache.GetStringAsync(key);
            List<LoanHistory> historyList;

            if (string.IsNullOrEmpty(existingJson))
            {
                historyList = new List<LoanHistory>();
            }
            else
            {
                try
                {
                    historyList = JsonSerializer.Deserialize<List<LoanHistory>>(existingJson) ?? new List<LoanHistory>();
                }
                catch
                {
                    var single = JsonSerializer.Deserialize<LoanHistory>(existingJson);
                    historyList = single is null ? new List<LoanHistory>() : new List<LoanHistory> { single };
                }
            }

            historyList.Add(loanHistory);
            var json = JsonSerializer.Serialize(historyList);
            await _distributedCache.SetStringAsync(key, json);
        }
    }
}
