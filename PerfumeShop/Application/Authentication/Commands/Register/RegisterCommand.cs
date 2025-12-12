using MediatR;
using PerfumeShop.Application.DTOs;

namespace PerfumeShop.Application.Auth.Commands.Register
{
    public record RegisterCommand(RegisterDto Dto) : IRequest<bool>;
    
}
