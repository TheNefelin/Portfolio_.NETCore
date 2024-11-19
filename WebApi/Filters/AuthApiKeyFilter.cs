using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary_Services.Auth;

namespace WebApi.Filters
{
    public class AuthApiKeyFilter : IAsyncActionFilter
    {
        private const string ApiKeyHeaderName = "ApiKey";
        private readonly AuthApiKeyService _authApiKeyService;

        public AuthApiKeyFilter(AuthApiKeyService authApiKeyService)
        {
            _authApiKeyService = authApiKeyService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Intentar obtener el valor de ApiKey del header
            if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var apiKey) || string.IsNullOrEmpty(apiKey))
            {
                // Si ApiKey no está presente o es nula, retornar un 401
                context.Result = new UnauthorizedObjectResult(new { StatusCode = 401, Message = "ApiKey es Requerida." });
                return;
            }

            // Llama al servicio para validar ApiKey en la base de datos
            var validationResponse = await _authApiKeyService.ValidateApiKey(apiKey);

            if (validationResponse.StatusCode != 200)
            {
                context.Result = new ObjectResult(new { validationResponse.StatusCode, validationResponse.Message })
                {
                    StatusCode = validationResponse.StatusCode
                };
                return;
            }

            // Llamar a la siguiente acción si la ApiKey está presente
            await next();
        }
    }
}
