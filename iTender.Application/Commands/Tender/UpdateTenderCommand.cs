using iTender.Domain.Models;

namespace iTender.Application.Commands.Tender
{
    public class UpdateTenderCommand
    {
        public TenderModel Model { get; }

        public UpdateTenderCommand(TenderModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }
    }
}
