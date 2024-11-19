using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using Dapper;
using System.Data;

namespace ClassLibrary_Services.Portfolio
{
    public class UrlService : IBaseCRUDService<UrlDTO>
    {
        private readonly IDbConnection _connection;

        public UrlService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ResponseApiDTO<IEnumerable<UrlDTO>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryAsync<UrlDTO>(
                    new CommandDefinition(
                        $"PF_Enlace_Get",
                        new { Id = 0 },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<IEnumerable<UrlDTO>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<IEnumerable<UrlDTO>>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<UrlDTO>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstOrDefaultAsync<UrlDTO>(
                    new CommandDefinition(
                        $"PF_Enlace_Get",
                        new { Id = id },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                if (result == null)
                    return new ResponseApiDTO<UrlDTO>
                    {
                        StatusCode = 404,
                        Message = "Registro No Encontrado."
                    };

                return new ResponseApiDTO<UrlDTO>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<UrlDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<UrlDTO>> InsertAsync(UrlDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_Enlace_Insert",
                        new { dto.Name, dto.Url, dto.Status, dto.Id_UrlGrp },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                dto.Id = Convert.ToInt32(result.Id);

                return new ResponseApiDTO<UrlDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<UrlDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<UrlDTO>> UpdateAsync(UrlDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_Enlace_Update",
                        new { dto.Id, dto.Name, dto.Url, dto.Status, dto.Id_UrlGrp },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<UrlDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<UrlDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<UrlDTO>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_Enlace_Delete",
                        new { Id = id },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<UrlDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<UrlDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }
    }
}
