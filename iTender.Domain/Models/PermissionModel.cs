namespace iTender.Domain.Models
{
    public class PermissionModel
    {
        public Guid PermissionId { get; set; }
        public Guid? PermissionContactId { get; set; }
        public string? PermissionName { get; set; }
        public Guid IndividualId { get; set; }
        public bool hasPermission { get; set; }
    }
}
