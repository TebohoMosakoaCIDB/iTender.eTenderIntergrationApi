namespace iTender.Application.Commands.Application
{
    public interface ICreateApplicationCommandHandler
    {
        Task<Guid> Handle(CreateApplicationCommand command, CancellationToken ct);
    }
}
