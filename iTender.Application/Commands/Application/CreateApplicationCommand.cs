using System;
using System.Collections.Generic;
using System.Text;

namespace iTender.Application.Commands.Application
{
    public class CreateApplicationCommand
    {
        public string ApplicationNumber { get; set; }
        public string Type { get; set; }
        public Guid ContractorId { get; set; }
    }
}
