using System;
using System.Collections.Generic;
using System.Text;

namespace iTender.Application.Commands.Contact
{
    public class DeleteContactCommand
    {
        public Guid Id { get; set; }

        public DeleteContactCommand(Guid id)
        {
            Id = id;
        }
    }
}
