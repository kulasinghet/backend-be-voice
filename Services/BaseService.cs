using Be_My_Voice_Backend.Models;
using Be_My_Voice_Backend.Services.IServices;
using Newtonsoft.Json;
using System.Text;

namespace Be_My_Voice_Backend.Services
{
    public class BaseService : IBaseService
    {
        public APIResponse responseModel { get; set; }
        public IHttpClientFactory httpClient { get; set; }

        public BaseService(IHttpClientFactory httpClientFactory)
        {
            this.responseModel = new();
            this.httpClient = httpClientFactory;
        }

        public async Task<T> SendAsync<T>(APIRequest apiRequest)
        {
            try
            {
                var client = httpClient.CreateClient("MagicAPI");
                HttpRequestMessage message = new HttpRequestMessage();
                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(apiRequest.url);

                if (apiRequest.data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(apiRequest.data), Encoding.UTF8, "applications/json");
                }

                switch (apiRequest.method)
                {
                    case "POST":
                        message.Method = HttpMethod.Post;
                        break;
                    case "PUT":
                        message.Method = HttpMethod.Put;
                        break;
                    case "DELETE":
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }

                HttpResponseMessage apiResponse = null;

                apiResponse = await client.SendAsync(message);

                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                var APIResponse = JsonConvert.DeserializeObject<T>(apiContent);

                return APIResponse;

            }
            catch (Exception ex)
            {
                var dto = new APIResponse
                {
                    ErrorMessage = new List<string> { Convert.ToString(ex.Message) },
                    Success = false
                };

                var res = JsonConvert.SerializeObject(dto);
                var APIResponse = JsonConvert.DeserializeObject<T>(res);

                return APIResponse;
            }
        }
    }
}
