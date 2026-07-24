namespace iTender.Application.Commands.Tender
{
    public class DeleteTenderCommand
    {
        public Guid Id { get; }

        public DeleteTenderCommand(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentException("Tender Id is required.");

            Id = id;
        }
    }
}
