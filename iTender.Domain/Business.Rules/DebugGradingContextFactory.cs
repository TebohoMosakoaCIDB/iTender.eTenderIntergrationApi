using System.Text;

namespace iTender.Domain.Business.Rules
{
    public class DebugGradingContextFactory : IGradingContextFactory
    {
        private readonly IGradingContextFactory _innerFactory;

        public DebugContextReader ContextReader { get; }

        public DebugGradingContextFactory(IGradingContextFactory innerFactory)
        {
            _innerFactory = innerFactory;
            ContextReader = new DebugContextReader();
        }

        public async Task<GradingContext> CreateContext(Guid classOfWorkId)
        {
            var context = await _innerFactory.CreateContext(classOfWorkId); // ✅ FIX

            ContextReader.SetContext(context);

            return context;
        }
    }

    public class DebugContextReader
    {
        GradingContext _context;

        public DebugContextReader()
        {

        }

        private string ReturnIfContextNotNull(Func<string> func)
        {
            if (_context != null)
                return func();

            return "Value not available";
        }

        public string Sponsorships
        {
            get
            {
                return ReturnIfContextNotNull(() =>
                {
                    StringBuilder sponsorships = new StringBuilder();
                    foreach (var sponsorship in _context.Sponsorships)
                        sponsorships.AppendLine(sponsorship.ToString());

                    return sponsorships.ToString();
                });
            }
        }

        public string NetAssetValue
        {
            get
            {
                return ReturnIfContextNotNull(() => _context.NetAssetValue.ToString());
            }
        }

        public string GradeAppliedFor
        {
            get
            {
                return ReturnIfContextNotNull(() => _context.GradeAppliedFor.ToString());
            }
        }

        public string ClassOfWorkAppliedFor
        {
            get
            {
                return ReturnIfContextNotNull(() => _context.ClassOfWorkAppliedFor.ToString());
            }
        }

        public string RegisteredProfessionalsCount
        {
            get
            {
                return ReturnIfContextNotNull(() => _context.RegisteredProfessionalsCount.ToString());
            }
        }

        public string LargestContract
        {
            get
            {
                return ReturnIfContextNotNull(() => _context.LargestContract.ToString());
            }
        }

        public string LargestContractForCow
        {
            get
            {
                return ReturnIfContextNotNull(() => _context.LargestContractForCow.ToString());
            }
        }

        public string BestAnnualTurnover
        {
            get
            {
                return ReturnIfContextNotNull(() => _context.BestAnnualTurnover.ToString());
            }
        }

        public DateTime ApplicationReceivedDate
        {
            get
            {
                return _context.ApplicationReceivedDate;
            }
        }

        internal void SetContext(GradingContext context)
        {
            _context = context;
        }
    }
}
