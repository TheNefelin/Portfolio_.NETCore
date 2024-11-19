using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using ClassLibrary_Services.Portfolio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/project-language")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class ProjectLanguageController : ControllerBase
    {
        private readonly ISimpleCRUDService<ProjectLanguageDTO> _service;

        public ProjectLanguageController(ISimpleCRUDService<ProjectLanguageDTO> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseApiDTO<IEnumerable<ProjectLanguageDTO>>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _service.GetAllAsync(cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseApiDTO<ProjectLanguageDTO>>> Insert(ProjectLanguageDTO dto, CancellationToken cancellationToken)
        {
            var response = await _service.InsertAsync(dto, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<ActionResult<ResponseApiDTO<object>>> Delete(ProjectLanguageDTO dto, CancellationToken cancellationToken)
        {
            var response = await _service.DeleteAsync(dto, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }
    }
}
