using HR_Project.Common.Models.DTOs;
using HR_Project.Presentation.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace HR_Project.Presentation.APIService
{
	public class APIService : IAPIService
	{
		private readonly HttpClient _httpClient;

		public APIService(HttpClient httpClient)
		{
			_httpClient = httpClient;
		}

		public async Task<T> GetAsync<T>(string endpoint, string token)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var response = await _httpClient.GetAsync(endpoint);

			if (!response.IsSuccessStatusCode)
			{

				throw new Exception($"API isteği başarısız: {response.StatusCode}");
			}

			var content = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(content);
		}

		public async Task<TResponse> PostAsync<TRequest, TResponse>(string endpoint, TRequest data, string token)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
			var jsonData = JsonConvert.SerializeObject(data);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

			
			try
			{
                var response = await _httpClient.PostAsync(endpoint, content);
				if (!response.IsSuccessStatusCode)
				{
					throw new Exception($"API isteği başarısız: {response.StatusCode}");
				}
				var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
			catch (Exception message)
			{

				throw message;
			}
			

			
		}

		//getbyid
		public async Task<T> GetByIdAsync<T>(string endpoint, string id, string token)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _httpClient.GetAsync($"{endpoint}/{id}");

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"API isteği başarısız: {response.StatusCode}");
			}

			var content = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(content);
		}

		//delete
		public async Task<T> DeleteAsync<T>(string endpoint, int id, string token)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _httpClient.DeleteAsync($"{endpoint}/{id}");

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"API isteği başarısız: {response.StatusCode}");
			}

			var content = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(content);
		}

		//update
		public async Task<T> UpdateAsync<T>(string endpoint, T data, string token)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var jsonData = JsonConvert.SerializeObject(data);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

			var response = await _httpClient.PutAsync(endpoint, content);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"API isteği başarısız: {response.StatusCode}");
			}

			var responseContent = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(responseContent);
		}

		//get created model
		public async Task<T> GetCreateModelAsync<T>(string endpoint, string token)
		{
			_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

			var response = await _httpClient.GetAsync($"{endpoint}");

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"API isteği başarısız: {response.StatusCode}");
			}
			var responseContent = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<T>(responseContent);
		}

		// login
		public async Task<TokenResponse> LoginAsync(LoginDTO loginModel)
		{
			var jsonData = JsonConvert.SerializeObject(loginModel);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("Account/Login", content);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"API isteği başarısız: {response.StatusCode}");
			}

			var responseContent = await response.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<TokenResponse>(responseContent);
		}

		//register
		public async Task<RegisterResponse> RegisterAsync(RegisterDTO registerModel)
		{
			var jsonData = JsonConvert.SerializeObject(registerModel);
			var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

			var response = await _httpClient.PostAsync("https://localhost:7258/api/Account/register", content);

			if (!response.IsSuccessStatusCode)
			{
				throw new Exception($"API isteği başarısız: {response.StatusCode}");
			}
            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<RegisterResponse>(responseContent);
        }


		public async Task<T> GetAsyncWoToken<T>(string endpoint)
        {

            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {

                throw new Exception($"API isteği başarısız: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }

        public async Task ConfirmAsync(string endpoint, MailConfirmDTO data)
        {
            var jsonData = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");


            try
            {
                var response = await _httpClient.PostAsync(endpoint, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"API isteği başarısız: {response.StatusCode}");
                }
            }
            catch (Exception message)
            {

                throw message;
            }



        }
    }

    
}
