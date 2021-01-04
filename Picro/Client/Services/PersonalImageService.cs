using System;
using Microsoft.AspNetCore.Components.Forms;
using Picro.Client.Communication.Interface;
using Picro.Client.Services.Interface;
using Picro.Client.Utils;
using Picro.Common.Web.Extensions;
using Picro.Module.Image.DataTypes.Response;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Picro.Module.Image.DataTypes.Enums;

namespace Picro.Client.Services
{
    internal class PersonalImageService : IPersonalImageService
    {
        private readonly HttpClient _httpClient;

        private readonly IRequestMessageFactory _requestMessageFactory;

        public PersonalImageService(
            IHttpClientFactory httpClientFactory,
            IRequestMessageFactory requestMessageFactory)
        {
            _requestMessageFactory = requestMessageFactory;
            _httpClient = httpClientFactory.CreateClient(HttpClients.PicroBackend);
        }

        public async Task<IEnumerable<ImageInfo>> GetImages()
        {
            var requestMessage = _requestMessageFactory.Create(HttpMethod.Get, "/Image/GetImages");

            var response = await _httpClient.SendAsync(requestMessage);

            var jsonResponse = await response.GetJsonResponse<IEnumerable<ImageInfo>>();

            if (response.IsSuccessStatusCode && jsonResponse.Success)
            {
                return jsonResponse.Data!;
            }

            return new List<ImageInfo>();
        }

        public async Task<ImageInfo?> UploadImage(IBrowserFile browserFile)
        {
            var requestMessage = _requestMessageFactory.Create(HttpMethod.Post, "/Image/Upload");

            using var sc = new StreamContent(browserFile.OpenReadStream(4096000));

            var formData = new MultipartFormDataContent
            {
                { sc, "file", browserFile.Name }
            };

            requestMessage.Content = formData;

            var response = await _httpClient.SendAsync(requestMessage);

            var jsonResponse = await response.GetJsonResponse<ImageUploadInfoResponse>();

            if (response.IsSuccessStatusCode && jsonResponse.Success)
            {
                return jsonResponse.Data!.ImageInfo;
            }

            return null;
        }

        public async Task<ImageDeletionErrorCode> DeleteImage(Guid imageId)
        {
            var requestMessage = _requestMessageFactory.Create(HttpMethod.Delete, $"/Image/DeleteImage?{nameof(imageId)}={imageId}");

            var response = await _httpClient.SendAsync(requestMessage);
            var jsonResponse = await response.GetJsonResponse<ImageDeletionErrorCode>();

            return jsonResponse.Data;
        }
    }
}