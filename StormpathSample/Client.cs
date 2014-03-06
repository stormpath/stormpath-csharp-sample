using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using Newtonsoft.Json;

namespace StormpathSample
{
    /**
     * This class serves as a proxy to execute requests to the REST API. 
     */
    class Client
    {
        const string BaseUrl = "https://api.stormpath.com/v1";

        readonly string _apiKeyId;
        readonly string _apiKeySecret;

        public Client(string apiKeyId, string apiKeySecret)
        {
            _apiKeyId = apiKeyId;
            _apiKeySecret = apiKeySecret;
        }

        public T Execute<T>(RestRequest request) where T : new()
        {
            return Execute<T>(request, BaseUrl);
        }

        public T Execute<T>(RestRequest request, string href) where T : new()
        {
            var client = new RestClient();
            client.BaseUrl = href;
            // The api key is required for all the requests to Stormpath.
            // In this case, we use Http basic authentication using the id as username
            // and the secret as password.
            client.Authenticator = new HttpBasicAuthenticator(_apiKeyId, _apiKeySecret);
            var response = client.Execute<T>(request);

            int httpStatus = (int) (response.StatusCode);
            if (httpStatus >= 400)
            {
                // Converting the REST API error into an exception with all the properties from the Json body.
                var requestException = new RestException(JsonConvert.DeserializeObject<RestError>(response.Content));
                throw requestException;
            }
            return response.Data;
        }

    }
}
