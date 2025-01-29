namespace MarketPlace.EmailBrokerAPI.Models
{
    public class EmailDto
    {
        public IEnumerable<string> Emails{ get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
