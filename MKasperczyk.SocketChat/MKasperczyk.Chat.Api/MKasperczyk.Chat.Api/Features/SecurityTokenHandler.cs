using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.IdentityModel.Tokens;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MKasperczyk.Chat.Api.Features
{
    public class SecurityTokenHandler
    {
        public async static Task<IResult> Handle(IDbContextFactory<ChatContext> dbContextFactory, IConfiguration configuration, IValidator<TokenRequest> validator, TokenRequest tokenRequest)
        {
            var validationResult = validator.Validate(tokenRequest);
            if (!validationResult.IsValid)
            {
                return Results.Json(new
                {
                    success = false,
                    message = validationResult?.Errors?.FirstOrDefault()?.ErrorMessage
                });
            }

            using var dbContext = dbContextFactory.CreateDbContext();

            var loggedInUser = await dbContext.Users.FirstOrDefaultAsync(user => user.Username == tokenRequest.UserName);
            if (loggedInUser == null)
            {
                return Results.Unauthorized();
            }

            bool isCorrect = VerifyPassword(tokenRequest, loggedInUser.Password);
            if (isCorrect)
            {
                return Results.Unauthorized();
            }

            string token = GetToken(configuration, tokenRequest.UserName);

            return Results.Json(new
            {
                id = loggedInUser.Id,
                token = token,
                user = loggedInUser.Username,
                success = true
            });
        }

        private static bool VerifyPassword(TokenRequest tokenRequest, string passwordFromDb)
        {
            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            var passwordVerification = passwordHasher.VerifyHashedPassword(tokenRequest.UserName, passwordFromDb, tokenRequest.Password);
            return passwordVerification != PasswordVerificationResult.Success;
        }

        private static string GetToken(IConfiguration configuration, string userName)
        {
            var issuer = configuration["Jwt:Issuer"];
            var audience = configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, userName),
                    new Claim(JwtRegisteredClaimNames.Name, userName),
                    new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
