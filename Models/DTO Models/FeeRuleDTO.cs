namespace Datacap.Models.DTO_Models
{
    public class FeeRuleDTO
    {
        public string ProcessorName { get; set; }
        public decimal LowerBound { get; set; }
        public decimal UpperBound { get; set; }
        public RateDTO RateDetails { get; set; }
    }
}
