using iTender.Application.DTOs;

namespace iTender.Application.Commands.Tender
{
    public class UpdateFullTenderCommand
    {
        public UpdateTenderModel Model { get; }

        public UpdateFullTenderCommand(UpdateTenderModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }
    }
}
