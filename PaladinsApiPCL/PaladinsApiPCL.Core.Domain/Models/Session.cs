namespace PaladinsApiPCL.Core.Domain.Models
{
    public class Session : PaladinsResponse
    {
        public string session_id { get; set; }
        public string timestamp { get; set; }
    }
}
