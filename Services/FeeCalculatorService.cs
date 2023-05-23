using System.Collections.Generic;
using System.Linq;
using Datacap.Models;
using Datacap.Models.DTO_Models;

namespace Datacap.Services
{
    public class FeeCalculator
    {
        private List<FeeRuleDTO> FeeRules;

        public FeeCalculator(List<FeeRuleDTO> feeRules)
        {
            this.FeeRules = feeRules;
        }

        public decimal CalculateFee(TransactionDTO transaction)
        {
            var rule = FeeRules.First(r => r.ProcessorName == transaction.ProcessorName);

            var rate = transaction.Amount < rule.UpperBound ? rule.LowerRateDetails : rule.UpperRateDetails;

            return transaction.Amount * rate.PercentageRate + rate.FlatRate;
        }
    }
}

