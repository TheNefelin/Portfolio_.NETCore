using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using Dapper;
using System.Data;

namespace ClassLibrary_Services.Portfolio
{
    public class LanguageService : IBaseCRUDService<LanguageDTO>
    {
        private readonly IDbConnection _connection;

        public LanguageService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ResponseApiDTO<IEnumerable<LanguageDTO>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryAsync<LanguageDTO>(
                    new CommandDefinition(
                        $"PF_Lenguaje_Get",
                        new { Id = 0 },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<IEnumerable<LanguageDTO>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<IEnumerable<LanguageDTO>>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<LanguageDTO>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstOrDefaultAsync<LanguageDTO>(
                    new CommandDefinition(
                        $"PF_Lenguaje_Get",
                        new { Id = id },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                if (result == null)
                    return new ResponseApiDTO<LanguageDTO>
                    {
                        StatusCode = 404,
                        Message = "Registro No Encontrado."
                    };

                return new ResponseApiDTO<LanguageDTO>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<LanguageDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<LanguageDTO>> InsertAsync(LanguageDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_Lenguaje_Insert",
                        new { dto.Name, dto.ImgUrl },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                dto.Id = Convert.ToInt32(result.Id);

                return new ResponseApiDTO<LanguageDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<LanguageDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<LanguageDTO>> UpdateAsync(LanguageDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_Lenguaje_Update",
                        new { dto.Id, dto.Name, dto.ImgUrl },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<LanguageDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<LanguageDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<LanguageDTO>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_Lenguaje_Delete",
                        new { Id = id },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<LanguageDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<LanguageDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }
    }
}
