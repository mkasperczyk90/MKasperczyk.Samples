using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Models;
using MKasperczyk.Chat.Api.Repositories;

namespace MKasperczyk.Chat.Api.Features
{
    public class GetAllUsersHandler
    {
        public async static Task<IResult> Handle(IUnitOfWork unitOfWork)
        {
            var users = await unitOfWork.UserRepository.GetUsersAsync();
            var result = users.Select(u => new UserInfo() { Id = u.Id, UserName = u.Username });

            return Results.Json(result);
        }
    }
}
