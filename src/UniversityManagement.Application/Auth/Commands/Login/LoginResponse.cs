using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityManagement.Application.Auth.Commands.Login
{
    public sealed record LoginResponse
    (
        string AccessToken, 
        string RefreshToken
    );
}
