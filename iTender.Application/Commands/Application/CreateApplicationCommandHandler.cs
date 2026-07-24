using iTender.Application.Interfaces;
using iTender.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using static iTender.Domain.Constants.CrmFieldNames;

namespace iTender.Application.Commands.Application
{
    public class CreateApplicationCommandHandler : ICreateApplicationCommandHandler
    {
        private readonly IApplicationRepository _repository;

        public CreateApplicationCommandHandler(IApplicationRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateApplicationCommand command, CancellationToken ct)
        {
            var model = new ApplicationModel
            {
                Id = Guid.Empty,
                ApplicationNumber = command.ApplicationNumber,
                Type = command.Type,
                ContractorId = command.ContractorId,
                CreatedOn = DateTime.UtcNow,
                ActivationDate = DateTime.UtcNow,
                StatusCode = ApplicationFields.StatusCodeCaptureInProgress,
                StateCode = 0,
                ContractorPotentiallyEmerging = false
            };

            return await _repository.CreateAsync(model, ct);
        }
    }
}
