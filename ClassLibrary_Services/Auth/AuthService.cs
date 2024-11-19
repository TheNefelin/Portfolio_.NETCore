using ClassLibrary_DTOs.Auth;
using ClassLibrary_DTOs.Portfolio;
using ClassLibrary_DTOs;
using System.Data;
using System.Security.Claims;
using System.Text;
using Dapper;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;

namespace ClassLibrary_Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IDbConnection _connection;
        private readonly IPasswordService _authPassword;

        public AuthService(IDbConnection connection, IPasswordService authPassword)
        {
            _connection = connection;
            _authPassword = authPassword;
        }

        public async Task<ResponseApiDTO<object>> RegisterAsync(RegisterDTO registerDTO, CancellationToken cancellationToken)
        {
            if (registerDTO.Password1 != registerDTO.Password2)
                return new ResponseApiDTO<object>
                {
                    StatusCode = 400,
                    Message = "Las contraseñas no Coinciden.",
                };

            var (hash, salt) = _authPassword.HashPassword(registerDTO.Password1);

            var newUser = new UserDTO
            {
                Id = Guid.NewGuid().ToString(),
                Email = registerDTO.Email,
                Hash1 = hash,
                Salt1 = salt,
            };

            try
            {
                var result = await _connection.QueryFirstAsync<ResponseSqlDTO>(
                    new CommandDefinition(
                         $"Auth_Register",
                        new { newUser.Id, newUser.Email, newUser.Hash1, newUser.Salt1 },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                return new ResponseApiDTO<object>
                {
                    StatusCode = result.StatusCode,
                    Message = result.Msge,
                    Data = result.StatusCode == 201 ? new { UserId = newUser.Id } : null
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

        public async Task<ResponseApiDTO<LoggedinDTO>> LoginAsync(LoginDTO loginDTO, JwtConfigDTO jwtConfigDTO, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _connection.QueryFirstOrDefaultAsync<UserDTO>(
                    new CommandDefinition(
                        $"Auth_Login",
                        new { loginDTO.Email },
                        commandType: CommandType.StoredProcedure,
                        transaction: default,
                        cancellationToken: cancellationToken
                ));

                if (result == null)
                    return new ResponseApiDTO<LoggedinDTO>
                    {
                        StatusCode = 401,
                        Message = "Usuario o Contraseña Incorrecta."
                    };

                bool passwordCorrect = _authPassword.VerifyPassword(loginDTO.Password, result.Hash1, result.Salt1);

                if (!passwordCorrect)
                    return new ResponseApiDTO<LoggedinDTO>
                    {
                        StatusCode = 401,
                        Message = "Usuario o Contraseña Incorrecta."
                    };

                UserDTO userDTO = MapToDTO(result);

                var token = GenerateJwtToken(userDTO, jwtConfigDTO);

                LoggedinDTO loggedinDTO = new LoggedinDTO()
                {
                    Id_User = result.Id,
                    Sql_Token = result.SqlToken,
                    Role = result.Role,
                    ExpireMin = jwtConfigDTO.ExpireMin,
                    ApiToken = token
                };

                return new ResponseApiDTO<LoggedinDTO>
                {
                    StatusCode = 200,
                    Message = "Autenticación Exitosa.",
                    Data = loggedinDTO
                };
            }
            catch (Exception ex)
            {
                return new ResponseApiDTO<LoggedinDTO>
                {
                    StatusCode = 500,
                    Message = "Error en la operación de base de datos: " + ex.Message
                };
            }
        }

        private UserDTO MapToDTO(UserDTO userDTO)
        {
            return new UserDTO
            {
                Id = userDTO.Id,
                Email = userDTO.Email,
                SqlToken = userDTO.SqlToken,
                Role = userDTO.Role,
            };
        }

        private string GenerateJwtToken(UserDTO user, JwtConfigDTO jwtConfig)
        {
            // Define los claims (información contenida en el token)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // Genera una clave simétrica a partir del secret en appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Configuración del token: audiencia, emisor, expiración y firma
            var token = new JwtSecurityToken(
                issuer: jwtConfig.Issuer,
                audience: jwtConfig.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(jwtConfig.ExpireMin)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
