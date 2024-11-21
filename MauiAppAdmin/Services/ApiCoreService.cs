using ClassLibrary_DTOs.Auth;
using ClassLibrary_DTOs.PasswordManager;
using ClassLibrary_DTOs;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MauiAppAdmin.Services
{
    public class ApiCoreService
    {
        private readonly HttpClient _httpClient;

        public ApiCoreService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ResultApiDTO<CoreIVDTO>> Login(string password)
        {
            return await RequestApiQuery<CoreIVDTO>($"/api/core/login", null, password);
        }

        public async Task<ResultApiDTO<List<CoreDTO>>> GetAll()
        {
            return await RequestApiQuery<List<CoreDTO>>($"/api/core/get-all", null, "");
        }

        public async Task<ResultApiDTO<CoreDTO>> Create(CoreDTO coreDTO)
        {
            coreDTO.Id = 0;
            return await RequestApiQuery<CoreDTO>($"/api/core/insert", coreDTO, "");
        }

        public async Task<ResultApiDTO<CoreDTO>> Edit(CoreDTO coreDTO)
        {
            return await RequestApiQuery<CoreDTO>($"/api/core/update", coreDTO, "");
        }

        public async Task<ResultApiDTO<CoreDTO>> Delete(int id)
        {
            CoreDTO coreDTO = new() { Id = id, Data01 = "na", Data02 = "na", Data03 = "na", };

            return await RequestApiQuery<CoreDTO>($"/api/core/delete", coreDTO, "");
        }

        private async Task<ResultApiDTO<T>> RequestApiQuery<T>(string uri, CoreDTO coreDTO, string password)
        {
            try
            {
                LoggedinDTO loggedinDTO = await ApiAuthService.GetLoggedUser();

                if (coreDTO != null)
                    coreDTO.Id_User = loggedinDTO.Id_User;

                CoreRequestDTO<dynamic> coreRequestDTO = new()
                {
                    Sql_Token = loggedinDTO.Sql_Token,
                    Id_User = loggedinDTO.Id_User,
                    Password = password,
                    CoreData = coreDTO,
                };

                if (!string.IsNullOrEmpty(loggedinDTO.ApiToken))
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loggedinDTO.ApiToken);

                if (!_httpClient.DefaultRequestHeaders.Contains("ApiKey"))
                    _httpClient.DefaultRequestHeaders.Add("ApiKey", "ESMERILEMELO");

                var json = JsonConvert.SerializeObject(coreRequestDTO);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PatchAsync(uri, content);
                var jsonResponse = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<ResponseApiDTO<T>>(jsonResponse);

                ResultApiDTO<T> resultApi = new()
                {
                    Success = response.IsSuccessStatusCode,
                    StatusCode = result.StatusCode,
                    Message = result.Message,
                    Data = result.Data,
                };

                if (result.StatusCode == 401)
                {
                    var shell = Application.Current.MainPage as AppShell;
                    shell?.PerformLogout(); // Invoca el logout desde AppShell
                }

                return resultApi;
            }
            catch (Exception ex)
            {
                return new ResultApiDTO<T>()
                {
                    Success = false,
                    StatusCode = 500,
                    Message = $"Error: {ex.Message}",
                };
            }
        }
    }
}
