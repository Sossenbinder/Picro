using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Picro.Client.Communication.Interface;
using Picro.Client.Services.Interface;
using Picro.Client.Utils;
using Picro.Common.Web.DataTypes;
using Picro.Common.Web.Extensions;

namespace Picro.Client.Services
{
    internal class ImageService : IImageService
    {
        private readonly HttpClient _httpClient;

        private readonly IRequestMessageFactory _requestMessageFactory;

        public ImageService(
            IHttpClientFactory httpClientFactory,
            IRequestMessageFactory requestMessageFactory)
        {
            _requestMessageFactory = requestMessageFactory;
            _httpClient = httpClientFactory.CreateClient(HttpClients.PicroBackend);
        }

        public async Task UploadImage(IBrowserFile browserFile)
        {
            var requestMessage = _requestMessageFactory.Create(HttpMethod.Post, "/Image/Upload");

            using var sc = new StreamContent(browserFile.OpenReadStream(4096000));

            var formData = new MultipartFormDataContent
            {
                { sc, "file", browserFile.Name }
            };

            requestMessage.Content = formData;

            var response = await _httpClient.SendAsync(requestMessage);

            var jsonResponse = await response.GetJsonResponse<string>();

            if (response.IsSuccessStatusCode)
            {
            }
        }
    }
}