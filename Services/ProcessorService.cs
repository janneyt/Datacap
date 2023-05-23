using System.Collections.Generic;
using System.Linq;
using Datacap.Models.DTO_Models;

namespace Datacap.Services
{
    public class ProcessorService
    {
        private List<FeeRuleDTO> FeeRules;

        public ProcessorService(List<FeeRuleDTO> defaultFeeRules)
        {
            FeeRules = defaultFeeRules;
        }

        public void AddProcessor(FeeRuleDTO newFeeRule)
        {
            FeeRules.Add(newFeeRule);
        }

        public FeeRuleDTO GetProcessor(string processorName)
        {
            return FeeRules.FirstOrDefault(r => r.ProcessorName == processorName);
        }
    }
}

