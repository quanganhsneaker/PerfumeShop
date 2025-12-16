using AutoMapper;
using MediatR;
using PerfumeShop.Domain.Interfaces;
using PerfumeShop.Domain.Models;

namespace PerfumeShop.Application.Auth.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly IUserRepository _userRepo;
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public RegisterHandler(
            IUserRepository userRepo,
            IUnitOfWork uow,
            IMapper mapper)
        {
            _userRepo = userRepo;
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<bool> Handle(RegisterCommand request, CancellationToken ct)
        {
            var dto = request.Dto;
            dto.Email = dto.Email.ToLower();

            if (await _userRepo.ExistsByEmailAsync(dto.Email))
                return false;

            var user = _mapper.Map<User>(dto);
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Role = "User";

            await _userRepo.AddAsync(user);
            await _uow.SaveChangesAsync(ct);

            return true;
        }
    }
}
