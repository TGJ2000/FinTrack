using Dapper;
using FinTrack.DTOs;
using FinTrack.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FinTrack.Repositories
{
    public class TransactionRepository(SqlConnection connection)
    {
        public async Task<List<Models.Transaction>> GetTransactions(int user, DateOnly? startDate, DateOnly? endDate, string? type, int? page, int? pageSize)
        {
            DateTime? start = startDate.HasValue ? startDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            DateTime? end = endDate.HasValue ? endDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            int pageNumber = page ?? 1;
            int pageLength = pageSize ?? 20;
            string sql = "sp_GetUserTransactions";
            IEnumerable<Models.Transaction>? userTransactions = await connection.QueryAsync<Models.Transaction>(sql: sql, param: new { UserId = user, StartDate = start, EndDate = end, Type = type, Page = pageNumber, PageSize = pageLength }, commandType: CommandType.StoredProcedure);
            return userTransactions.ToList();
        }

        public async Task<List<Models.Transaction>> ExportTransactions(int user, DateOnly? startDate, DateOnly? endDate, string? type)
        {
            DateTime? start = startDate.HasValue ? startDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            DateTime? end = endDate.HasValue ? endDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            string sql = "sp_ExportTransactions";
            IEnumerable<Models.Transaction>? userTransactions = await connection.QueryAsync<Models.Transaction>(sql: sql, param: new { UserId = user, StartDate = start, EndDate = end, Type = type }, commandType: CommandType.StoredProcedure);
            return userTransactions.ToList();
        }

        public async Task<List<MonthlyReport>> GetMonthlyReport(int user, int year)
        {
            string sql = "sp_GetMonthlyReport";
            IEnumerable<MonthlyReport>? userTransactions = await connection.QueryAsync<MonthlyReport>(sql: sql, param: new { UserId = user, Year = year }, commandType: CommandType.StoredProcedure);
            return userTransactions.ToList();
        }

        public async Task CreateTransaction(int userId, CreateTransactionDto transaction)
        {
            DateTime transDate = transaction.TransactionDate.HasValue ? transaction.TransactionDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime.Now);
            DateTime createdAt = DateTime.Now;
            string sql = "INSERT INTO Transactions (UserId, CategoryId, Amount, Type, Description, TransactionDate, CreatedAt) VALUES (@UserId, @CategoryId, @Amount, @Type, @Description, @TransactionDate, @CreatedAt)";
            await connection.ExecuteAsync(sql: sql, param: new { UserId = userId, transaction.CategoryId, transaction.Amount, transaction.Type, transaction.Description, TransactionDate = transDate, CreatedAt = createdAt });
        }

        public async Task UpdateTransaction(int userId, int transactionId, UpdateTransactionDto transaction)
        {
            string sql = "UPDATE Transactions SET " +
                "CategoryId = COALESCE(@CategoryId, CategoryId)," +
                "Amount = COALESCE(@Amount, Amount)," +
                "Type = COALESCE(@Type, Type)," +
                "Description = COALESCE(@Description, Description)," +
                "TransactionDate = COALESCE(@TransactionDate, TransactionDate)" +
                " WHERE TransactionId = @TransactionId AND UserId = @UserId";

            DateTime? transDate = transaction.TransactionDate.HasValue ? transaction.TransactionDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            await connection.ExecuteAsync(sql: sql, param: new { UserId = userId, TransactionId = transactionId, transaction.CategoryId, transaction.Amount, transaction.Type, transaction.Description, TransactionDate = transDate });
        }

        public async Task DeleteTransaction(int userId, int transactionId)
        {
            string sql = "DELETE FROM Transactions WHERE TransactionId = @TransactionId AND UserId = @UserId";
            await connection.ExecuteAsync(sql: sql, param: new { TransactionId = transactionId, UserId = userId });
        }

        public async Task<List<TransactionSummary>> GetTransactionSummary(int user, DateOnly? startDate, DateOnly? endDate)
        {
            DateTime? start = startDate.HasValue ? startDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            DateTime? end = endDate.HasValue ? endDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            IEnumerable<TransactionSummary>? transactionSummary = await connection.QueryAsync<TransactionSummary>(sql: "sp_GetTransactionSummary", param: new { UserId = user, StartDate = start, EndDate = end }, commandType: CommandType.StoredProcedure);
            return transactionSummary.ToList();
        }
    }
}
