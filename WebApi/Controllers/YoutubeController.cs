using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using ClassLibrary_Services.Portfolio;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/youtube")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]

    public class YoutubeController : ControllerBase
    {
        private readonly IBaseCRUDService<YoutubeDTO> _service;

        public YoutubeController(IBaseCRUDService<YoutubeDTO> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseApiDTO<IEnumerable<YoutubeDTO>>>> GetAll(CancellationToken cancellationToken)
        {
            var response = await _service.GetAllAsync(cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ResponseApiDTO<YoutubeDTO>>> GetById(int id, CancellationToken cancellationToken)
        {
            var response = await _service.GetByIdAsync(id, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]
        public async Task<ActionResult<ResponseApiDTO<YoutubeDTO>>> Insert(YoutubeDTO dto, CancellationToken cancellationToken)
        {
            var response = await _service.InsertAsync(dto, cancellationToken);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut]
        public async Task<ActionResult<ResponseApiDTO<YoutubeDTO>>> Update(YoutubeDTO dto, CancellationToken cancellationToken)
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
