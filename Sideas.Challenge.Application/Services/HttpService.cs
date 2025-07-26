using Newtonsoft.Json;

namespace Sideas.Challenge.Application.Services
{
    /// <summary>
    /// Servicio genérico para realizar peticiones HTTP GET y deserializar JSON.
    /// </summary>
    public class HttpService
    {
        private readonly HttpClient _httpClient;

        public HttpService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Realiza una petición GET a la URL especificada y deserializa la respuesta JSON al tipo T.
        /// </summary>
        public async Task<T?> GetAsync<T>(string url)
        {
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                return default;

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
