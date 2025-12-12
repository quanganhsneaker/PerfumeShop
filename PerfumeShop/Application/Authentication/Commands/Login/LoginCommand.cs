using MediatR;
using PerfumeShop.Domain.Models;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Auth.Commands.Login
{
    public record LoginCommand(LoginDto Dto) : IRequest<LoginResult>;
   
}
