using ClassLibrary_DTOs;
using Dapper;
using System.Data;

namespace ClassLibrary_Services.Auth
{
    public class AuthApiKeyService
    {
        private readonly IDbConnection _connection;

        public AuthApiKeyService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ResponseApiDTO<object>> ValidateApiKey(string apiKey)
        {
            try
            {
                var result = await _connection.QueryFirstOrDefaultAsync<string>(
                    "SELECT ApiKey FROM Mae_Config WHERE Id = @Id",
                    new { Id = 1 });

                if (result == null)
                    return new ResponseApiDTO<object>
                    {
                        StatusCode = 404,
                        Message = "Registro No Encontrado",
                    };

                if (!apiKey.Equals(result))
                    return new ResponseApiDTO<object>
                    {
                        StatusCode = 401,
                        Message = "No Estas Autorizado",
                    };

                return new ResponseApiDTO<object>
                {
                    StatusCode = 200,
                    Message = "",
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<object>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }
    }
}
