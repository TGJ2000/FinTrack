using FinTrack.Models;
using FinTrack.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    [ApiController]
    [Route(template: "api/transactions")]
    public class TransactionController : ControllerBase
    {
        private TransactionRepository _repo;

        public TransactionController(TransactionRepository transactionRepository)
        {
            _repo = transactionRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserTransactions(DateOnly? startDate, DateOnly? endDate, string? type, int? page, int? pageSize)
        {
            int user = 1;
            List<Transaction> userTransactions = await _repo.GetTransactions(user, startDate, endDate, type, page, pageSize);
            return Ok(userTransactions);
        }
        [Route("api/summary")]
        [HttpGet]
        public async Task<IActionResult> GetTransactionSummary(DateOnly ?startDate, DateOnly? endDate)
        {
            int user = 1;
            List<TransactionSummary> transactionSummary = await _repo.GetTransactionSummary(user, startDate, endDate);
            return Ok(transactionSummary);
        }
        [Route("api/transactions/create")]
        [HttpPost]
        public async Task<IActionResult> CreateTransaction(int categoryId, decimal amount, string type, string description, DateOnly? transactionDate)
        {
            int user = 1;
            await _repo.CreateTransaction(user, categoryId, amount, type, description, transactionDate);
            return Ok();
        }
    }
}
