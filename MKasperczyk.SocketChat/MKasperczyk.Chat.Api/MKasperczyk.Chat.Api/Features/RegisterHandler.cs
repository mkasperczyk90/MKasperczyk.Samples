using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Models;
using MKasperczyk.Chat.Api.Repositories;
using System.ComponentModel.DataAnnotations;

namespace MKasperczyk.Chat.Api.Features
{
    public class RegisterHandler
    {
        public async static Task<IResult> Handle(IUnitOfWork unitOfWork, IValidator<RegisterRequest> validator, RegisterRequest registerInfo)
        {
            var validationResult = validator.Validate(registerInfo);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest();
            }

            PasswordHasher<string> passwordHasher = new PasswordHasher<string>();
            var hashedPassword = passwordHasher.HashPassword(registerInfo.UserName, registerInfo.Password);

            await unitOfWork.UserRepository.AddUserAsync(registerInfo.UserName, hashedPassword);
            unitOfWork.Save();

            return Results.Ok("User Created");
        }
    }
}
