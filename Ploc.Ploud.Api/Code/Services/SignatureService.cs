using System;
using System.Diagnostics;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public class SignatureService : ISignatureService
    {
        private static readonly String ServiceUrl = String.Concat(Config.ApiUrl, "Signature/Ploud/Verify");

        public async Task<SignatureResponse> VerifySignatureAsync(SignatureRequest signatureRequest)
        {
            SignatureResponse signatureResponse = new SignatureResponse();
            signatureResponse.IsValid = false;
            using (HttpClient client = new HttpClient())
            {
                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, ServiceUrl))
                {
                    String json = JsonSerializer.Serialize(signatureRequest);
                    using (StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json"))
                    {
                        request.Content = stringContent;
                        using (HttpResponseMessage response = await client.SendAsync(request, HttpCompletionOption.ResponseContentRead).ConfigureAwait(false))
                        {
                            if (!response.IsSuccessStatusCode)
                            {
                                Trace.TraceError(json);
                                return signatureResponse;
                            }
                            signatureResponse.IsValid = true;
                        }
                    }
                }
            }
            return signatureResponse;
        }
    }
}
