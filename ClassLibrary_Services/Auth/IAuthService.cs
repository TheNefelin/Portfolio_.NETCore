using ClassLibrary_DTOs;
using ClassLibrary_DTOs.Auth;

namespace ClassLibrary_Services.Auth
{
    public interface IAuthService
    {
        Task<ResponseApiDTO<object>> RegisterAsync(RegisterDTO registerDTO, CancellationToken cancellationToken);
        Task<ResponseApiDTO<LoggedinDTO>> LoginAsync(LoginDTO loginDTO, JwtConfigDTO jwtConfigDTO, CancellationToken cancellationToken);
    }
}
