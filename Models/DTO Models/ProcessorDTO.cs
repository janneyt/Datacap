using Datacap.Models.DTO_Models;

namespace Datacap.Models
{
    public class ProcessorDTO
    {
        public string ProcessorName { get; set; }
        public List<TransactionDTO> Transactions { get; set; }
        public decimal TotalFee { get; set; }
        public int Rank { get; set; }
        public FeeRuleDTO FeeRule { get; set; } // Link to the fee rules
    }
}
