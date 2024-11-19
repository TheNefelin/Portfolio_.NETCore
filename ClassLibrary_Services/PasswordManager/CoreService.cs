using ClassLibrary_DTOs.PasswordManager;
using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using Dapper;
using System.Data;
using ClassLibrary_Services.Auth;

namespace ClassLibrary_Services.PasswordManager
{
    public class CoreService : ICoreService
    {
        private readonly IDbConnection _connection;
        private readonly IPasswordService _authPassword;

        public CoreService(IDbConnection connection, IPasswordService authPassword)
        {
            _connection = connection;
            _authPassword = authPassword;
        }

        public async Task<ResponseApiDTO<CoreIVDTO>> RegisterAsync(CoreRequestDTO<object> request, CancellationToken cancellationToken)
        {
            var valSqlToken = await ValidateSQlToken<CoreIVDTO>(request.Sql_Token, request.Id_User);

            if (valSqlToken.StatusCode != 200)
                return valSqlToken;

            try
            {
                var (hash, salt) = _authPassword.HashPassword(request.Password);

                var result = await _connection.QueryFirstOrDefaultAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PM_Core_Register",
                        new { request.Id_User, hash2 = hash, salt2 = salt },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                if (result == null)
                    return new ResponseApiDTO<CoreIVDTO>
                    {
                        StatusCode = 404,
                        Message = "Registro No Encontrado."
                    };

                return new ResponseApiDTO<CoreIVDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = new CoreIVDTO() { IV = result.Id }
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<CoreIVDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<CoreIVDTO>> LoginAsync(CoreRequestDTO<object> request, CancellationToken cancellationToken)
        {
            var valSqlToken = await ValidateSQlToken<CoreIVDTO>(request.Sql_Token, request.Id_User);

            if (valSqlToken.StatusCode != 200)
                return valSqlToken;

            try
            {
                var result = await _connection.QueryFirstOrDefaultAsync<UserDTO>(
                    new CommandDefinition(
                        $"PM_Core_Login",
                        new { request.Id_User },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                if (result == null)
                    return new ResponseApiDTO<CoreIVDTO>
                    {
                        StatusCode = 401,
                        Message = "Usuario o Contraseña Icorrecta."
                    };

                bool passwordCorrect = _authPassword.VerifyPassword(request.Password, result.Hash2, result.Salt2);

                if (!passwordCorrect)
                    return new ResponseApiDTO<CoreIVDTO>
                    {
                        StatusCode = 401,
                        Message = "Usuario o Contraseña Icorrecta."
                    };

                return new ResponseApiDTO<CoreIVDTO>
                {
                    StatusCode = 200,
                    Message = "Autenticación Exitosa.",
                    Data = new CoreIVDTO() { IV = result.Salt2 }
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<CoreIVDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<IEnumerable<CoreDTO>>> GetAllAsync(CoreRequestDTO<object> request, CancellationToken cancellationToken)
        {
            var valSqlToken = await ValidateSQlToken<IEnumerable<CoreDTO>>(request.Sql_Token, request.Id_User);

            if (valSqlToken.StatusCode != 200)
                return valSqlToken;

            try
            {
                var result = await _connection.QueryAsync<CoreDTO>(
                    new CommandDefinition(
                        $"PM_Core_Get",
                        new { request.Id_User, Id = 0 },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<IEnumerable<CoreDTO>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<IEnumerable<CoreDTO>>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<CoreDTO>> GetByIdAsync(CoreRequestDTO<CoreDTO> request, CancellationToken cancellationToken)
        {
            var valSqlToken = await ValidateSQlToken<CoreDTO>(request.Sql_Token, request.Id_User);

            if (valSqlToken.StatusCode != 200)
                return valSqlToken;

            try
            {
                var result = await _connection.QueryFirstOrDefaultAsync<CoreDTO>(
                    new CommandDefinition(
                        $"PM_Core_Get",
                        new { request.Id_User, request.CoreData.Id },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                if (result == null)
                    return new ResponseApiDTO<CoreDTO>
                    {
                        StatusCode = 404,
                        Message = "Registro No Encontrado."
                    };

                return new ResponseApiDTO<CoreDTO>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<CoreDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<CoreDTO>> InsertAsync(CoreRequestDTO<CoreDTO> request, CancellationToken cancellationToken)
        {
            var valSqlToken = await ValidateSQlToken<CoreDTO>(request.Sql_Token, request.Id_User);

            if (valSqlToken.StatusCode != 200)
                return valSqlToken;

            try
            {
                var result = await _connection.QueryFirstOrDefaultAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PM_Core_Insert",
                        new { request.Id_User, request.CoreData.Data01, request.CoreData.Data02, request.CoreData.Data03 },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                request.CoreData.Id = int.Parse(result.Id);

                return new ResponseApiDTO<CoreDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = request.CoreData,
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<CoreDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<CoreDTO>> UpdateAsync(CoreRequestDTO<CoreDTO> request, CancellationToken cancellationToken)
        {
            var valSqlToken = await ValidateSQlToken<CoreDTO>(request.Sql_Token, request.Id_User);

            if (valSqlToken.StatusCode != 200)
                return valSqlToken;

            try
            {
                var result = await _connection.QueryFirstOrDefaultAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PM_Core_Update",
                        new { request.Id_User, request.CoreData.Id, request.CoreData.Data01, request.CoreData.Data02, request.CoreData.Data03 },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<CoreDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = request.CoreData,
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<CoreDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<object>> DeleteAsync(CoreRequestDTO<CoreDTO> request, CancellationToken cancellationToken)
        {
            var valSqlToken = await ValidateSQlToken<object>(request.Sql_Token, request.Id_User);

            if (valSqlToken.StatusCode != 200)
                return valSqlToken;

            try
            {
                var result = await _connection.QueryFirstOrDefaultAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PM_Core_Delete",
                        new { request.Id_User, request.CoreData.Id },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<object>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = null
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

        private async Task<ResponseApiDTO<T>> ValidateSQlToken<T>(string SqlToken, string Id_User)
        {
            try
            {
                var result = await _connection.QueryFirstOrDefaultAsync<string>(
                    $"SELECT Id FROM Auth_Usuario WHERE SqlToken = @SqlToken AND Id = @Id_User",
                    new { SqlToken, Id_User });

                if (string.IsNullOrEmpty(result))
                    return new ResponseApiDTO<T>
                    {
                        StatusCode = 401,
                        Message = "No Estas Autorizado",
                    };

                return new ResponseApiDTO<T>
                {
                    StatusCode = 200,
                    Message = "",
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<T>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }
    }
}
