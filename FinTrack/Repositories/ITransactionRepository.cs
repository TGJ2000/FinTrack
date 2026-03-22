using FinTrack.DTOs;
using FinTrack.Models;

namespace FinTrack.Repositories
{
    public interface ITransactionRepository
    {
        Task<List<Models.Transaction>> GetTransactions(int user, DateOnly? startDate, DateOnly? endDate, string? type, int? page, int? pageSize);
        Task<List<Models.Transaction>> ExportTransactions(int user, DateOnly? startDate, DateOnly? endDate, string? type);
        Task<List<MonthlyReport>> GetMonthlyReport(int user, int year);
        Task CreateTransaction(int userId, CreateTransactionDto transaction);
        Task UpdateTransaction(int userId, int transactionId, UpdateTransactionDto transaction);
        Task DeleteTransaction(int userId, int transactionId);
        Task<List<TransactionSummary>> GetTransactionSummary(int user, DateOnly? startDate, DateOnly? endDate);
    }
}
