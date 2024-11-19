using ClassLibrary_DTOs.PasswordManager;
using ClassLibrary_DTOs;
using ClassLibrary_Services.PasswordManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Filters;

namespace WebApi.Controllers
{
    [Route("api/core")]
    [ApiController]
    [Authorize]
    [ServiceFilter(typeof(AuthApiKeyFilter))]
    public class CoreController : ControllerBase
    {
        private readonly ICoreService _coreService;

        public CoreController(ICoreService coreService)
        {
            _coreService = coreService;
        }

        [HttpPatch("register")]
        public async Task<ActionResult<ResponseApiDTO<CoreIVDTO>>> Register(CoreRequestDTO<object> request, CancellationToken cancellationToken)
        {
            var response = await _coreService.RegisterAsync(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("login")]
        public async Task<ActionResult<ResponseApiDTO<CoreIVDTO>>> Login(CoreRequestDTO<object> request, CancellationToken cancellationToken)
        {
            var response = await _coreService.LoginAsync(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("get-all")]
        public async Task<ActionResult<ResponseApiDTO<IEnumerable<CoreDTO>>>> GetAll(CoreRequestDTO<object> request, CancellationToken cancellationToken)
        {
            var response = await _coreService.GetAllAsync(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("get-byid")]
        public async Task<ActionResult<ResponseApiDTO<CoreDTO>>> GetById(CoreRequestDTO<CoreDTO> request, CancellationToken cancellationToken)
        {
            var response = await _coreService.GetByIdAsync(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("insert")]
        public async Task<ActionResult<ResponseApiDTO<CoreDTO>>> Insert(CoreRequestDTO<CoreDTO> request, CancellationToken cancellationToken)
        {
            var response = await _coreService.InsertAsync(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("update")]
        public async Task<ActionResult<ResponseApiDTO<CoreDTO>>> Update(CoreRequestDTO<CoreDTO> request, CancellationToken cancellationToken)
        {
            var response = await _coreService.UpdateAsync(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPatch("delete")]
        public async Task<ActionResult<ResponseApiDTO<object>>> Delete(CoreRequestDTO<CoreDTO> request, CancellationToken cancellationToken)
        {
            var response = await _coreService.DeleteAsync(request, cancellationToken);
            return StatusCode(response.StatusCode, response);
        }
    }
}
