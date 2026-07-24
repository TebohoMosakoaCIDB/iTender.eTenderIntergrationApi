using iTender.Domain.Models;

namespace iTender.Application.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateJwtToken(string username, List<PermissionModel> permissions);
    }
}
