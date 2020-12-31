using System;
using System.Net.Http;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using Picro.Client.Communication.Interface;

namespace Picro.Client.Communication
{
    /// <summary>
    /// Small factory to create a HttpRequestMessage which needs some special handling for debug
    /// </summary>
    public class RequestMessageFactory : IRequestMessageFactory
    {
        public HttpRequestMessage Create()
        {
            var requestMessage = new HttpRequestMessage();
#if DEBUG
            requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
#endif
            return requestMessage;
        }

        public HttpRequestMessage Create(HttpMethod httpMethod, Uri? uri)
        {
            var requestMessage = new HttpRequestMessage(httpMethod, uri);
#if DEBUG
            requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
#endif
            return requestMessage;
        }

        public HttpRequestMessage Create(HttpMethod httpMethod, string? uri)
        {
            var requestMessage = new HttpRequestMessage(httpMethod, uri);
#if DEBUG
            requestMessage.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
#endif
            return requestMessage;
        }
    }
}