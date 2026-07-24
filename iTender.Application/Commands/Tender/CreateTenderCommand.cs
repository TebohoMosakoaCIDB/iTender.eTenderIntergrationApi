using iTender.Application.DTOs;

namespace iTender.Application.Commands.Tender
{
    public class CreateTenderCommand
    {
        public CreateTenderModel Model { get; }

        public CreateTenderCommand(CreateTenderModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }
    }
}
