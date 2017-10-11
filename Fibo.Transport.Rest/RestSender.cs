using System.Threading.Tasks;
using RestSharp;

namespace Fibo.Transport.Rest
{
    public class RestSender<T> : ISender<T>
        where T : class
    {
        private readonly IRestClient _client;
        private readonly string _resource;

        public RestSender(string baseUrl, string resource)
        {
            _client = new RestClient(baseUrl);
            _resource = resource;
        }

        public void Dispose()
        {
        }

        public async Task<Response> SendAsync(T message, string sessionId)
        {
            var request = new RestRequest(_resource, Method.POST)
            {
                JsonSerializer = BigIntegerJsonSerializer.Default
            };
            request.AddJsonBody(message);
            request.AddHeader(Constants.SessionIdHeader, sessionId);
            var result = await _client.ExecuteTaskAsync(request);
            return new Response { StatusCode = (int)result.StatusCode, Message = result.Content };
        }
    }
}
