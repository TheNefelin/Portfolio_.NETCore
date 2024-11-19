using ClassLibrary_DTOs;
using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_Services.Portfolio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/technology")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class TechnologyController : ControllerBase
    {
        private readonly IBaseCRUDService<TechnologyDTO> _service;

        public TechnologyController(IBaseCRUDService<TechnologyDTO> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseApiDTO<IEnumerable<TechnologyDTO>>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _service.GetAllAsync(cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseApiDTO<TechnologyDTO>>> GetById(int id, CancellationToken cancellationToken)
        {
            var response = await _service.GetByIdAsync(id, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseApiDTO<TechnologyDTO>>> Insert(TechnologyDTO dto, CancellationToken cancellationToken)
        {
            var response = await _service.InsertAsync(dto, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseApiDTO<TechnologyDTO>>> Update(TechnologyDTO dto, CancellationToken cancellationToken)
        {
            var response = await _service.UpdateAsync(dto, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("id")]
        public async Task<ActionResult<ResponseApiDTO<object>>> Delete(int id, CancellationToken cancellationToken)
        {
            var response = await _service.DeleteAsync(id, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }
    }
}
