﻿using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace MarketPlace.OrderAPI.Extensions
{
  
    public class BackendApiAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public BackendApiAuthenticationHttpClientHandler(IHttpContextAccessor accessor)
        {
            _contextAccessor = accessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = await _contextAccessor.HttpContext.GetTokenAsync("access_token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);


            return await base.SendAsync(request, cancellationToken);
        }
    }
    
}
