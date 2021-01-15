using Picro.Client.Communication.Interface;
using Picro.Client.Utils;
using System.Net.Http;
using System.Threading.Tasks;

namespace Picro.Client.Communication
{
	public class SessionService : ISessionService
	{
		private readonly IKeepAliveService _keepAliveService;

		private readonly IRequestMessageFactory _requestMessageFactory;

		private readonly HttpClient _httpClient;

		public SessionService(
			IKeepAliveService keepAliveService,
			IHttpClientFactory httpClientFactory,
			IRequestMessageFactory requestMessageFactory)
		{
			_keepAliveService = keepAliveService;
			_requestMessageFactory = requestMessageFactory;
			_httpClient = httpClientFactory.CreateClient(HttpClients.PicroBackend);
		}

		public async Task InitializeSession()
		{
			var requestMessage = _requestMessageFactory.Create(HttpMethod.Get, "Identity/BeginSession");

			var networkResponse = await _httpClient.SendAsync(requestMessage);

			if (networkResponse.IsSuccessStatusCode)
			{
				await _keepAliveService.InitializeConnection();
			}
		}
	}
}