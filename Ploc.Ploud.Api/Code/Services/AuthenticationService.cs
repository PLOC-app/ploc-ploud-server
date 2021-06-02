using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public class AuthenticationService  : IAuthenticationService
    {
        private static readonly String ServiceUrl = String.Concat(Config.ApiUrl, "Authorization/Ploud/");

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest)
        {
            AuthenticationResponse authenticationResponse = null;
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ServiceUrl))
                {
                    String json = JsonSerializer.Serialize(authenticationRequest);
                    using (StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;
                        using (HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                        {
                            authenticationResponse = await response.Content.ReadFromJsonAsync<AuthenticationResponse>();
                            if(authenticationResponse == null)
                            {
                                authenticationResponse = new AuthenticationResponse();
                                authenticationResponse.IsAuthenticated = false;
                            }
                        }
                    }
                }
            }
            return authenticationResponse;
        }
    }
}
