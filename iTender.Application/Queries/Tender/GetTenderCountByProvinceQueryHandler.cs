using iTender.Application.DTOs;
using iTender.Application.Interfaces;

namespace iTender.Application.Queries.Tender
{
    public class GetTenderCountByProvinceQueryHandler
    {
        private readonly ITenderRepository _repository;

        public GetTenderCountByProvinceQueryHandler(ITenderRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ProvinceStatViewModel>> Handle(CancellationToken ct)
        {
            return _repository.GetTenderCountByProvince(ct);
        }
    }
}
