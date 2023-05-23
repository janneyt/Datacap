namespace Datacap.Models
{
    public class ProcessorDTO
    {
        public string ProcessorName { get; set; }
        public List<TransactionDTO> Transactions { get; set; }
        public decimal FeeRate { get; set; } 
        public decimal TotalFee { get; set; }
        public int Rank { get; set; }
    }
}
