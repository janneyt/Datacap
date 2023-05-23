namespace Datacap.Models.DTO_Models
{
    public class ResultDTO
    {
        public List<ProcessorTotalDTO> ProcessorTotals { get; set; }
        public List<RankingDTO> Rankings { get; set; }
        public string ErrorMessage { get; set; }
    }
}
