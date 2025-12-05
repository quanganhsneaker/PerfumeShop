using MediatR;
using PerfumeShop.DTOs;
using PerfumeShop.Models;

namespace PerfumeShop.Auth.Commands.Login
{
    public record LoginCommand(LoginDto Dto) : IRequest<LoginResult>;
   
}
