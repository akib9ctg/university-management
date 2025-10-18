﻿using MediatR;

namespace UniversityManagement.Application.Auth.Commands.Login
{
    public sealed record LoginCommand
    (
        string Email,
        string Password
    ) : IRequest<LoginResponse>;

}
