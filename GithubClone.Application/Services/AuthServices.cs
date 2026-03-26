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
        private readonly IEmailService _emailService;

        public AuthServices(
            IUserRepository repo,
            IConfiguration config,
            IMapper mapper,
            IEmailService emailService)
        {
            _repo = repo;
            _config = config;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<UserDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = await _repo.GetByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new Exception("User already exists");

            var user = _mapper.Map<User>(dto);

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            user.CreatedAt = DateTime.UtcNow;

            // email verificaiton
            user.IsEmailVerified = false;
            user.EmailVerificationToken = Guid.NewGuid().ToString();
            user.EmailVerificationTokenExpiry = DateTime.UtcNow.AddHours(24);

            await _repo.CreateAsync(user);

            //   link
            var verificationLink =
                $"{_config["App:BaseUrl"]}/api/auth/verify-email?email={Uri.EscapeDataString(user.Email)}&token={Uri.EscapeDataString(user.EmailVerificationToken)}";

            //  email
            var body = $@"
        <h2>Email Verification</h2>
        <p>Click the button below to verify your account:</p>
        <a href='{verificationLink}' 
           style='padding:10px 20px;background-color:#007bff;color:white;text-decoration:none;border-radius:5px;'>
           Verify Email
        </a>
        <p>This link will expire in 24 hours.</p>
    ";

            await _emailService.SendEmailAsync(
                user.Email,
                "Verify your email",
                body
            );

            return _mapper.Map<UserDto>(user);
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            //  Block login
            if (!user.IsEmailVerified)
                throw new Exception("Please verify your email first");

            var claims = new[]
            {
            new Claim("id", user.Id.ToString()),
            new Claim("username", user.Username),
            new Claim("email", user.Email)
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials:
                    new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Verify email
        public async Task<bool> VerifyEmailAsync(string token)
        {
            var user = await _repo.GetByVerificationTokenAsync(token);

            if (user == null)
                throw new Exception("Invalid token");

            if (user.EmailVerificationTokenExpiry < DateTime.UtcNow)
                throw new Exception("Token expired");

            user.IsEmailVerified = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpiry = null;

            await _repo.UpdateAsync(user);

            return true;
        }
    }


}

