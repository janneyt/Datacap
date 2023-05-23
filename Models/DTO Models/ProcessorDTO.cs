namespace Datacap.Models
{
    public class ProcessorDTO
    {
        public string ProcessorName { get; set; }
        public List<TransactionDTO> Transactions { get; set; }
    }
}
