using OpenPol.WebAPI.Models.Request;
using OpenPol.WebAPI.Models.Response;

namespace OpenPol.WebAPI.Services
{
    public interface IUserService
    {

        UserResponse Auth(AuthRequest model);

    }
}
