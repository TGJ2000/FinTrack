using System.ComponentModel.DataAnnotations;

namespace FinTrack.DTOs
{
    public class CreateTransactionDto
    {
        public int CategoryId { get; set; }
        [Range(0.01, 999999999.99)]
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateOnly? TransactionDate { get; set; }
    }
}
