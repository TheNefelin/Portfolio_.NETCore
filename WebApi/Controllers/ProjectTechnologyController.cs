using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using ClassLibrary_Services.Portfolio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/project-technology")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class ProjectTechnologyController : ControllerBase
    {
        private readonly ISimpleCRUDService<ProjectTechnologyDTO> _service;

        public ProjectTechnologyController(ISimpleCRUDService<ProjectTechnologyDTO> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseApiDTO<IEnumerable<ProjectTechnologyDTO>>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _service.GetAllAsync(cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseApiDTO<ProjectTechnologyDTO>>> Insert(ProjectTechnologyDTO dto, CancellationToken cancellationToken)
        {
            var response = await _service.InsertAsync(dto, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete]
        public async Task<ActionResult<ResponseApiDTO<object>>> Delete(ProjectTechnologyDTO dto, CancellationToken cancellationToken)
        {
            var response = await _service.DeleteAsync(dto, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }
    }
}
