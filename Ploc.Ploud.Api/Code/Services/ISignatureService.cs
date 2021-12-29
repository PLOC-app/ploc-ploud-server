
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public interface ISignatureService
    {
        Task<SignatureResponse> VerifySignatureAsync(SignatureRequest request);
    }
}
