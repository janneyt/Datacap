namespace Datacap.Models
{
    public class TransactionDTO
    {
        public int TransactionID { get; set; }
        public decimal Amount { get; set; }
        public string ProcessorName { get; set; }
        public TransactionTypeDTO TransactionType { get; set; }
        public decimal Fee { get; set; } // This can be calculated from Amount * Processor's FeeRate
        public int Rank { get; set; } // This can be assigned based on the Processor's ranking
    }
}


