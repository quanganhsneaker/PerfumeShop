using MediatR;
using PerfumeShop.DTOs;

namespace PerfumeShop.Auth.Commands.Register
{
    public record RegisterCommand(RegisterDto Dto) : IRequest<bool>;
    
}
