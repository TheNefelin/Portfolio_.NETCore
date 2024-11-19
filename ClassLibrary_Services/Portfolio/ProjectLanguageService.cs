using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using Dapper;
using System.Data;

namespace ClassLibrary_Services.Portfolio
{
    public class ProjectLanguageService : ISimpleCRUDService<ProjectLanguageDTO>
    {
        private readonly IDbConnection _connection;

        public ProjectLanguageService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ResponseApiDTO<IEnumerable<ProjectLanguageDTO>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryAsync<ProjectLanguageDTO>(
                    new CommandDefinition(
                        $"PF_ProLeng_Get",
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<IEnumerable<ProjectLanguageDTO>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<IEnumerable<ProjectLanguageDTO>>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<ProjectLanguageDTO>> InsertAsync(ProjectLanguageDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_ProLeng_Insert",
                        new { dto.Id_Project, dto.Id_Language },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<ProjectLanguageDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<ProjectLanguageDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<ProjectLanguageDTO>> DeleteAsync(ProjectLanguageDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_ProLeng_Delete",
                        new { dto.Id_Project, dto.Id_Language },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<ProjectLanguageDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<ProjectLanguageDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }
    }
}
