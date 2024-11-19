using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using ClassLibrary_Services.Portfolio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/language")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class LanguageController : ControllerBase
    {
        private readonly IBaseCRUDService<LanguageDTO> _service;

        public LanguageController(IBaseCRUDService<LanguageDTO> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseApiDTO<IEnumerable<LanguageDTO>>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _service.GetAllAsync(cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseApiDTO<LanguageDTO>>> GetById(int id, CancellationToken cancellationToken)
        {
            var response = await _service.GetByIdAsync(id, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseApiDTO<LanguageDTO>>> Insert(LanguageDTO dto, CancellationToken cancellationToken)
        {
            var response = await _service.InsertAsync(dto, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseApiDTO<LanguageDTO>>> Update(LanguageDTO dto, CancellationToken cancellationToken)
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
