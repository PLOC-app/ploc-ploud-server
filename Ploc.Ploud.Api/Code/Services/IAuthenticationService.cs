using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest authenticationRequest);
    }
}
