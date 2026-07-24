namespace iTender.Application.DTOs
{
    public class CDPViewModel
    {
        public Guid EmployerId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSise { get; set; } = 10;
    }
}
