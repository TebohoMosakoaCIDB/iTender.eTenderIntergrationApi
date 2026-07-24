namespace iTender.Domain.Models
{
    public class AnnotationModel
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string NoteText { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid? RegardingObjectId { get; set; }
    }
}
