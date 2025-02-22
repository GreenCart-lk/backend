using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using Lib.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace AuthenticationApi.Infrastructure.Repositories
{
    internal class UserRepository(AuthenticationDbContext context, IConfiguration config) : IUser
    {
        private async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u=>u.Email == email);
            return user is null? null! : user;
        }
        public async Task<GetUserDTO> GetUser(int userId)
        {
            var user = await context.Users.FindAsync(userId);
            return user is not null ? new GetUserDTO(user.Id,
                user.Name!,
                user.TelephoneNumber!,
                user.Address!,
                user.Email!,
                user.Role!) : null!;
        }

        public Task<Response> Login(LoginDTO loginDTO)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> Register(AppUserDTO appUserDTO)
        {
            var getUser = await GetUserByEmail(appUserDTO.Email);
            if (getUser is null)
                return new Response(false, $"you cannot use this email for registration");

            var result = context.Users.Add(new AppUser()
            {
                Name = appUserDTO.Name,
                Email = appUserDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password),
                TelephoneNumber = appUserDTO.TelephoneNumber,
                Address = appUserDTO.Address,
                Role = appUserDTO.Role
            });

            await context.SaveChangesAsync();
            return result.Entity.Id > 0 ? new Response(true, "User registered successfully") :
                new Response(false, "Invalid data provided");
        }
    }
}
