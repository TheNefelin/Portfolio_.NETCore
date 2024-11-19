using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using ClassLibrary_Services.Portfolio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/url")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class UrlController : ControllerBase
    {
        private readonly IBaseCRUDService<UrlDTO> _service;

        public UrlController(IBaseCRUDService<UrlDTO> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseApiDTO<IEnumerable<UrlDTO>>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _service.GetAllAsync(cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseApiDTO<UrlDTO>>> GetById(int id, CancellationToken cancellationToken)
        {
            var response = await _service.GetByIdAsync(id, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseApiDTO<UrlDTO>>> Insert(UrlDTO dto, CancellationToken cancellationToken)
        {
            var response = await _service.InsertAsync(dto, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseApiDTO<UrlDTO>>> Update(UrlDTO dto, CancellationToken cancellationToken)
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
