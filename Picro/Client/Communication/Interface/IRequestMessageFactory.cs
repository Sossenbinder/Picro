using System;
using System.Net.Http;

namespace Picro.Client.Communication.Interface
{
	public interface IRequestMessageFactory
	{
		HttpRequestMessage Create();

		HttpRequestMessage Create(HttpMethod httpMethod, Uri? uri);

		HttpRequestMessage Create(HttpMethod httpMethod, string? uri);
	}
}