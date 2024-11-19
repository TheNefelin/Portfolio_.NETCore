using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using ClassLibrary_Services.Portfolio;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/public")]
    [ApiController]
    public class PublicController : ControllerBase
    {
        private readonly ILogger<PublicController> _logger;
        private readonly IBaseCRUDService<YoutubeDTO> _youtubeService;
        private readonly IPublicService _publicService;

        public PublicController(
            ILogger<PublicController> logger,
            IBaseCRUDService<YoutubeDTO> youtubeService,
            IPublicService publicService)
        {
            _logger = logger;
            _youtubeService = youtubeService;
            _publicService = publicService;
        }

        [HttpGet]
        [Route("projects")]
        public async Task<ActionResult<IEnumerable<ProjectsDTO>>> GetAllProyecto(CancellationToken cancellationToken)
        {
            var response = await _publicService.GetAllProjectsAsync(cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("urls")]
        public async Task<ActionResult<ResponseApiDTO<IEnumerable<UrlsGrpsDTO>>>> GetAllUrlsGrps(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando la recuperación de grupos de URLs.");
            var response = await _publicService.GetAllUrlsGrpsAsync(cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        [Route("youtube")]
        public async Task<ActionResult<ResponseApiDTO<IEnumerable<YoutubeDTO>>>> GetAllYoutube(CancellationToken cancellationToken)
        {
            var response = await _youtubeService.GetAllAsync(cancellationToken);

            return StatusCode(response.StatusCode, response);
        }
    }
}
