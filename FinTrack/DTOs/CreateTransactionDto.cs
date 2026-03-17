namespace FinTrack.DTOs
{
    public class CreateTransactionDto
    {
        public int CategoryId { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateOnly? TransactionDate { get; set; }
    }
}
