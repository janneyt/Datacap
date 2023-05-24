namespace Datacap.Models.DTO_Models;
public class ProcessorResponse
{
    public string Name { get; set; }
    public decimal TotalFee { get; set; }
    public int Rank { get; set; }
    public List<TransactionDTO> Transactions { get; set; }
}

