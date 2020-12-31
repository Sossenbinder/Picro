using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Picro.Common.Web.DataTypes;

namespace Picro.Common.Web.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static Task<JsonResponsePayload> GetJsonResponse(this HttpResponseMessage httpResponseMessage)
            => httpResponseMessage.GetContentDeserialized<JsonResponsePayload>();

        public static Task<JsonResponsePayload<T>> GetJsonResponse<T>(this HttpResponseMessage httpResponseMessage)
            => httpResponseMessage.GetContentDeserialized<JsonResponsePayload<T>>();

        public static async Task<T> GetContentDeserialized<T>(this HttpResponseMessage httpResponseMessage)
        {
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            var deserializedContent = JsonConvert.DeserializeObject<T>(responseContent);

            return deserializedContent;
        }
    }
}