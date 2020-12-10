using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace UniversityApp.BL.Services.Implements
{
    public class RestClientSingleton
    {
        private static HttpClient client = null;

        private RestClientSingleton() { }

        public static HttpClient Instance()
        {
            if (client == null)
            {
                client = new HttpClient
                {
                    BaseAddress = new Uri("http://university-api.azurewebsites.net/")
                };

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            return client;
        }
    }
}
