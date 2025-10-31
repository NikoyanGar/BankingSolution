using BankingService.Clients;
using BankingService.Models.Responses;
using FluentResults;

namespace BankingService.Services
{
    public class IBankingService
    {
        private readonly IClientLoanClient? _clientLoanClient;
        private readonly IScoringClient? _scoringClient;

        public IBankingService(IClientLoanClient clientLoanClient, IScoringClient scoringClient)
        {
            _clientLoanClient = clientLoanClient;
            _scoringClient = scoringClient;
        }

        public async Task<Result<LoanDecisionResponse>> EvaluateLoanAsync(string clientId, decimal amount)
        {
            var score = await _scoringClient!.GetScoreAsync(clientId);

            if (score.Value == null)
            {
                return Result.Ok<LoanDecisionResponse>(new LoanDecisionResponse
                {
                    ClientId = clientId,
                    RequestedAmount = amount,
                    Decision = "Declined",
                    Reason = "Score not found"
                });
            }

            var loanHistory = await _clientLoanClient!.GetClientLoanHistoryAsync(clientId);
            if (loanHistory.History!.Count(l => !l.IsRepaid) > 1)
            {
                return Result.Ok<LoanDecisionResponse>(new LoanDecisionResponse
                {
                    ClientId = clientId,
                    RequestedAmount = amount,
                    Decision = "Declined",
                    Reason = "Too many unpaid loans"
                });
            }

            if (score.Value.Score < 600)
            {
                return Result.Ok<LoanDecisionResponse>(new LoanDecisionResponse
                {
                    ClientId = clientId,
                    RequestedAmount = amount,
                    Decision = "Declined",
                    Reason = "Low credit score"
                });
            }

            return new LoanDecisionResponse
            {
                ClientId = clientId,
                RequestedAmount = amount,
                Decision = "Approved",
                Reason = "Good credit history and score"
            };
        }
    }
}
