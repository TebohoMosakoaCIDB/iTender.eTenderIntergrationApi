namespace iTender.Domain.Models
{
    public class BatchModel
    {
        public Guid Id { get; set; }
        public string BatchNumber { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
