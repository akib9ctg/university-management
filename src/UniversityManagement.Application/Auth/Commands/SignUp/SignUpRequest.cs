using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversityManagement.Application.Auth.Commands.SignUp
{
    public sealed record SignUpRequest
    (
        string FirstName,
        string LastName,
        string Email,
        string Password,
        string ConfirmPassword
    );
}
