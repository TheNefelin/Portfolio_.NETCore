using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;

namespace ClassLibrary_Services.Portfolio
{
    public interface IPublicService
    {
        Task<ResponseApiDTO<IEnumerable<UrlsGrpsDTO>>> GetAllUrlsGrpsAsync(CancellationToken cancellationToken);
        Task<ResponseApiDTO<IEnumerable<ProjectsDTO>>> GetAllProjectsAsync(CancellationToken cancellationToken);
    }
}
