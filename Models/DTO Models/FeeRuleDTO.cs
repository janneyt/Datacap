namespace Datacap.Models.DTO_Models
{
    public class FeeRuleDTO
    {
        public string ProcessorName { get; set; }
        public RateDTO SmallTransactionRate { get; set; }
        public decimal SmallTransactionFlatFee { get; set; }
        public RateDTO LargeTransactionRate { get; set; }
        public decimal LargeTransactionFlatFee { get; set; }
    }
}

