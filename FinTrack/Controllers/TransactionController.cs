using FinTrack.DTOs;
using FinTrack.Models;
using FinTrack.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Controllers
{
    [ApiController]
    [Route(template: "api/transactions")]
    public class TransactionController(TransactionRepository transactionRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetUserTransactions(DateOnly? startDate, DateOnly? endDate, string? type, int? page, int? pageSize)
        {
            int user = 1;
            List<Transaction> userTransactions = await transactionRepository.GetTransactions(user, startDate, endDate, type, page, pageSize);
            return Ok(userTransactions);
        }
        [Route("summary")]
        [HttpGet]
        public async Task<IActionResult> GetTransactionSummary(DateOnly? startDate, DateOnly? endDate)
        {
            int user = 1;
            List<TransactionSummary> transactionSummary = await transactionRepository.GetTransactionSummary(user, startDate, endDate);
            return Ok(transactionSummary);
        }
        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto transaction)
        {
            int user = 1;
            await transactionRepository.CreateTransaction(user, transaction);
            return StatusCode(201);
        }

        [Route("{transactionId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] UpdateTransactionDto transaction)
        {
            int user = 1;
            await transactionRepository.UpdateTransaction(userId: user, transactionId, transaction);
            return Ok();
        }


        [Route("{transactionId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            int user = 1;
            await transactionRepository.DeleteTransaction(user, transactionId);
            return Ok();
        }
    }
}
