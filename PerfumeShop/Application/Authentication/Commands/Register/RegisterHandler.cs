using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PerfumeShop.Domain.Models;
using PerfumeShop.Infrastructure.Data;
using System.Data;

namespace PerfumeShop.Application.Auth.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, bool>
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public RegisterHandler(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }
        public async Task<bool> Handle(RegisterCommand request, CancellationToken ct)
        {
            var dto = request.Dto;
            dto.Email = dto.Email.ToLower();
            if (await _db.Users.AnyAsync(x => x.Email == dto.Email))
                return false;
            var user = _mapper.Map<User>(dto);
            //{
            //    FullName = dto.FullName,
            //    Email = dto.Email,
            //    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            //    Role = "User"
            //}
            //;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.Role = "User";
            _db.Users.Add(user);

            await _db.SaveChangesAsync(ct);
            return true;
        }
    }
}
