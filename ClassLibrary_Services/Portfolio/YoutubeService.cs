using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using Dapper;
using System.Data;

namespace ClassLibrary_Services.Portfolio
{
    public class YoutubeService : IBaseCRUDService<YoutubeDTO>
    {
        private readonly IDbConnection _connection;

        public YoutubeService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ResponseApiDTO<IEnumerable<YoutubeDTO>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryAsync<YoutubeDTO>(
                    new CommandDefinition(
                        $"PF_Youtube_Get",
                        new { Id = 0 },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<IEnumerable<YoutubeDTO>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<IEnumerable<YoutubeDTO>>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<YoutubeDTO>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstOrDefaultAsync<YoutubeDTO>(
                    new CommandDefinition(
                        $"PF_Youtube_Get",
                        new { Id = id },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                if (result == null)
                    return new ResponseApiDTO<YoutubeDTO>
                    {
                        StatusCode = 404,
                        Message = "Registro No Encontrado."
                    };

                return new ResponseApiDTO<YoutubeDTO>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<YoutubeDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<YoutubeDTO>> InsertAsync(YoutubeDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_Youtube_Insert",
                        new { dto.Name, dto.EmbedUrl },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                dto.Id = Convert.ToInt32(result.Id);

                return new ResponseApiDTO<YoutubeDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<YoutubeDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<YoutubeDTO>> UpdateAsync(YoutubeDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_Youtube_Update",
                        new { dto.Id, dto.Name, dto.EmbedUrl },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<YoutubeDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<YoutubeDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<YoutubeDTO>> DeleteAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_Youtube_Delete",
                        new { Id = id },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<YoutubeDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<YoutubeDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }
    }
}
