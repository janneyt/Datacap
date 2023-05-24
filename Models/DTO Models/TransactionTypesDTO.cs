namespace Datacap.Models
{
    public class TransactionTypeDTO
    {
        public int TransactionTypeID { get; set; }
        public string TypeName { get; set; }

        // All of the below is for the fake database/File IO classes to be able to convert between strings and this data model
        public override string ToString()
        {
            return $"{TransactionTypeID}:{TypeName}";
        }

        public static TransactionTypeDTO Parse(string transactionTypeString)
        {
            var parts = transactionTypeString.Split(":");

            return new TransactionTypeDTO
            {
                TransactionTypeID = int.Parse(parts[0]),
                TypeName = parts[1]
            };
        }
    }
}

