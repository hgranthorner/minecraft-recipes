using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Recipes.Tests
{
    public static class Helpers
    {
        public static async Task<T> NetworkRequestAsync<T>(
            Task<HttpResponseMessage> http)
        {
            var response = await http;

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(content);
        }
    }
}