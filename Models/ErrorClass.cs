namespace Datacap.Models
{
    // Error model is used to catch, configure and return any exceptions or errors
    public class ErrorModel
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }

}
