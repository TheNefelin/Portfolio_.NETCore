using ClassLibrary_DTOs;
using ClassLibrary_DTOs.Auth;
using Newtonsoft.Json;
using System.Text;

namespace MauiAppAdmin.Services
{
    public class ApiAuthService
    {
        private readonly HttpClient _httpClient;

        public ApiAuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultApiDTO<LoggedinDTO>> Login(string email, string password)
        {
            try
            {
                const string endpoint = "/api/auth/login";

                LoginDTO loginDTO = new LoginDTO() { Email = email, Password = password };

                // se agrega un elemento en la cabecera
                if (!_httpClient.DefaultRequestHeaders.Contains("ApiKey"))
                    _httpClient.DefaultRequestHeaders.Add("ApiKey", "ESMERILEMELO");

                var json = JsonConvert.SerializeObject(loginDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(endpoint, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ResponseApiDTO<LoggedinDTO>>(jsonResponse);

                if (result == null) {
                    return new ResultApiDTO<LoggedinDTO>()
                    {
                        Success = false,
                        StatusCode = 500,
                        Message = "Error en la solicitud."
                    };
                }

                ResultApiDTO<LoggedinDTO> resultApi = new()
                {
                    StatusCode = result.StatusCode,
                    Message = result.Message,
                    Data = result.Data,
                };

                if (response.IsSuccessStatusCode)
                {
                    resultApi.Success = true;

                    // Almacenar los tokens de manera segura
                    await SecureStorage.SetAsync("id", result.Data.Id_User);
                    await SecureStorage.SetAsync("user", email);
                    await SecureStorage.SetAsync("role", result.Data.Role);
                    await SecureStorage.SetAsync("expire_min", result.Data.ExpireMin);
                    await SecureStorage.SetAsync("jwt_token", result.Data.ApiToken);
                    await SecureStorage.SetAsync("sql_token", result.Data.Sql_Token);
                    await SecureStorage.SetAsync("date_login", DateTime.Now.ToString("O"));
                }

                return resultApi;
            }
            catch (Exception ex)
            {
                return new ResultApiDTO<LoggedinDTO>()
                {
                   Success = false,
                   StatusCode = 500,
                   Message = ex.Message,
                };
            }
        }

        public async static Task<LoggedinDTO> GetLoggedUser()
        {
            return new LoggedinDTO
            {
                Id_User = await SecureStorage.GetAsync("id"),
                Role = await SecureStorage.GetAsync("role"),
                ExpireMin = await SecureStorage.GetAsync("expire_min"),
                ApiToken = await SecureStorage.GetAsync("jwt_token"),
                Sql_Token = await SecureStorage.GetAsync("sql_token"),
            };
        }

        public async static Task<string> GetUser()
        {
            string user = await SecureStorage.GetAsync("user");
            return user;
        }

        public static void RemoveUser()
        {
            //SecureStorage.RemoveAll();
            SecureStorage.Remove("id");
            SecureStorage.Remove("role");
            SecureStorage.Remove("expire_min");
            SecureStorage.Remove("jwt_token");
            SecureStorage.Remove("sql_token");
        }
    }
}
