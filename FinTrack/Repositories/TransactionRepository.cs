using Dapper;
using FinTrack.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Transactions;

namespace FinTrack.Repositories
{
    public class TransactionRepository
    {
        private SqlConnection _connection;

        public TransactionRepository(SqlConnection connection)
        {
            _connection = connection;
        }

        public async Task<List<Models.Transaction>> GetTransactions(int user, DateOnly ?startDate, DateOnly ?endDate, string ?type, int ?page, int ?pageSize)
        {
            DateTime? start = startDate.HasValue ? startDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            DateTime? end = endDate.HasValue ? endDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            int pageNumber = page ?? 1;
            int pageLength = pageSize ?? 20;
            string sql = "sp_GetUserTransactions";
            IEnumerable<Models.Transaction>? userTransactions = await _connection.QueryAsync<Models.Transaction>(sql: sql, param: new {UserId = user, StartDate = start, EndDate = end, Type = type, Page = pageNumber, PageSize = pageLength}, commandType: CommandType.StoredProcedure);
            return userTransactions.ToList();
        }

        public async Task CreateTransaction(int user, int categoryId, decimal amount, string type, string description, DateOnly? transactionDate)
        {
            DateTime transDate = transactionDate.HasValue ? transactionDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime.Now);
            DateTime createdAt = DateTime.Now;
            string sql = "INSERT INTO Transactions (UserId, CategoryId, Amount, Type, Description, TransactionDate, CreatedAt) VALUES (@UserId, @CategoryId, @Amount, @Type, @Description, @TransactionDate, @CreatedAt)";
            await _connection.ExecuteAsync(sql: sql, param: new { UserId = user, CategoryId = categoryId, Amount = amount, Type = type, Description = description, TransactionDate = transDate, CreatedAt = createdAt });
        }

        public async Task<List<TransactionSummary>> GetTransactionSummary(int user, DateOnly ?startDate, DateOnly ?endDate)
        {
            DateTime? start = startDate.HasValue ? startDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            DateTime? end = endDate.HasValue ? endDate.Value.ToDateTime(TimeOnly.MinValue) : (DateTime?)null;
            IEnumerable<TransactionSummary>? transactionSummary = await _connection.QueryAsync<TransactionSummary>(sql: "sp_GetTransactionSummary", param: new { UserId = user, StartDate = start, EndDate = end }, commandType: CommandType.StoredProcedure);
            return transactionSummary.ToList();
        }
    }
}
