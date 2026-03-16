using AutoMapper;
using GithubClone.Application.DTOs;
using GithubClone.Application.Interfaces.Repository;
using GithubClone.Application.Interfaces.Services;
using GithubClone.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GithubClone.Application.Services
{
    public class AuthServices : IAuthService
    {
        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;


        public AuthServices(IUserRepository repo, IConfiguration config, IMapper mapper)
        {
            _repo = repo;
            _config = config;
            _mapper = mapper;
        }
        public async Task<UserDto> RegisterAsync(RegisterDto dto)
        {
            //Check if user already exists
            var existingUser = await _repo.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("User already exists");

            //Username = dto.Username,
            //Email = dto.Email,

            // Use IMapper instead of these two 

            var user = _mapper.Map<User>(dto);


            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                user.CreatedAt = DateTime.UtcNow;
                await _repo.CreateAsync(user);

            return   _mapper.Map<UserDto>(user);



        }
        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);
            if(user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                throw new Exception("Invalid credintials");
            }

            var claims = new[]
            {

                //Claims are infromation that goes into the token

                new Claim("id",user.Id.ToString()),
                new Claim("username",user.Username),
                new Claim("email",user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));   //Fetches secret key from the appsetting,converts string into bytes and create a security key object for signing with jwt
            var token = new JwtSecurityToken(
                 issuer: _config["Jwt:Issuer"],
                claims: claims, //includes array of claims
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials:
                    new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
                );
            return new JwtSecurityTokenHandler().WriteToken(token);



        }


    }
}
