using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using Dapper;
using System.Data;

namespace ClassLibrary_Services.Portfolio
{
    public class ProjectTechnologyService : ISimpleCRUDService<ProjectTechnologyDTO>
    {
        private readonly IDbConnection _connection;

        public ProjectTechnologyService(IDbConnection connection)
        {
            _connection = connection;
        }

        public async Task<ResponseApiDTO<IEnumerable<ProjectTechnologyDTO>>> GetAllAsync(CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryAsync<ProjectTechnologyDTO>(
                    new CommandDefinition(
                        $"PF_ProTec_Get",
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<IEnumerable<ProjectTechnologyDTO>>
                {
                    StatusCode = 200,
                    Message = "",
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<IEnumerable<ProjectTechnologyDTO>>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<ProjectTechnologyDTO>> InsertAsync(ProjectTechnologyDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_ProTec_Insert",
                        new { dto.Id_Project, dto.Id_Technology },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<ProjectTechnologyDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<ProjectTechnologyDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        public async Task<ResponseApiDTO<ProjectTechnologyDTO>> DeleteAsync(ProjectTechnologyDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                        $"PF_ProTec_Delete",
                        new { dto.Id_Project, dto.Id_Technology },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<ProjectTechnologyDTO>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<ProjectTechnologyDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }
    }
}
