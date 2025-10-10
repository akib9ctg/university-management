using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityManagement.Application.Auth.Commands.SignUp
{
    public sealed record SignUpResponse
    (
        Guid Id,
        string Email
    );
}
