using FinTrack.DTOs;
using FinTrack.Models;
using FinTrack.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace FinTrack.Controllers
{
    [Authorize]
    [ApiController]
    [Route(template: "api/transactions")]
    public class TransactionController(ITransactionRepository transactionRepository) : ControllerBase
    {
        private int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        [HttpGet]
        public async Task<IActionResult> GetUserTransactions(DateOnly? startDate, DateOnly? endDate, string? type, int? page, int? pageSize)
        {
            int user = GetUserId();
            List<Transaction> userTransactions = await transactionRepository.GetTransactions(user, startDate, endDate, type, page, pageSize);
            return Ok(userTransactions);
        }
        [HttpGet]
        [Route("export")]
        public async Task<FileResult> ExportUserTransactions(DateOnly? startDate, DateOnly? endDate, string? type)
        {
            int user = GetUserId();
            List<Transaction> userTransactions = await transactionRepository.ExportTransactions(user, startDate, endDate, type);

            StringBuilder csv = new();
            csv.AppendLine("TransactionId, TransactionDate, Amount, Type, Description, Category");

            foreach (Transaction item in userTransactions)
            {
                csv.AppendLine($"{item.TransactionId}, {item.TransactionDate}, {item.Amount}, {item.Type}, \"{item.Description}\", \"{item.Category}\"");
            }

            byte[] bytes = Encoding.UTF8.GetBytes(csv.ToString());

            return File(bytes, "text/csv", "transactions.csv");
        }
        [Route("summary")]
        [HttpGet]
        public async Task<IActionResult> GetTransactionSummary(DateOnly? startDate, DateOnly? endDate)
        {
            int user = GetUserId();
            List<TransactionSummary> transactionSummary = await transactionRepository.GetTransactionSummary(user, startDate, endDate);
            return Ok(transactionSummary);
        }

        [Route("monthlyreport")]
        [HttpGet]
        public async Task<IActionResult> GetMonthlyReport(int year)
        {
            int user = GetUserId();
            List<MonthlyReport> transactionSummary = await transactionRepository.GetMonthlyReport(user, year);
            return Ok(transactionSummary);
        }

        [Route("create")]
        [HttpPost]
        public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionDto transaction)
        {
            int user = GetUserId();
            await transactionRepository.CreateTransaction(user, transaction);
            return StatusCode(201);
        }

        [Route("{transactionId}")]
        [HttpPut]
        public async Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] UpdateTransactionDto transaction)
        {
            int user = GetUserId();
            await transactionRepository.UpdateTransaction(userId: user, transactionId, transaction);
            return Ok();
        }


        [Route("{transactionId}")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTransaction(int transactionId)
        {
            int user = GetUserId();
            await transactionRepository.DeleteTransaction(user, transactionId);
            return Ok();
        }
    }
}
