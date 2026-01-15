using System;
using System.Collections.Generic;
using System.Text;

namespace Odasoft.XBOL.HttpClients
{
    public class AdminApiClient
    {
        private readonly HttpClient _httpClient;

        public AdminApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


    }
}
