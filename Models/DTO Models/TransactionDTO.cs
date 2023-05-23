namespace Datacap.Models
{
    public class TransactionDTO
    {
        public int TransactionID { get; set; }
        public decimal Amount { get; set; }
        public string ProcessorName { get; set; }
        public TransactionTypeDTO TransactionType { get; set; }
        public decimal Fee { get; set; }
        public int Rank { get; set; }
    }
}

