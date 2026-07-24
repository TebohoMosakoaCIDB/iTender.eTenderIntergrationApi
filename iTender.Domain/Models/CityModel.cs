namespace iTender.Domain.Models
{
    public class CityModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid? ProvinceId { get; set; }
    }
}
