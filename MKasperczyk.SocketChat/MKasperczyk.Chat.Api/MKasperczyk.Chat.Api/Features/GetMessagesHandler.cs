﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using MKasperczyk.Chat.Api.DAL;
using MKasperczyk.Chat.Api.Models;

namespace MKasperczyk.Chat.Api.Features
{
    public class GetMessagesHandler
    {
        public async static Task<IResult> Handle(IDbContextFactory<ChatContext> dbContextFactory, int senderId, int receiverId)
        {
            if (senderId <= 0 || receiverId <= 0)
            {
                return Results.Json(new
                {
                    success = false,
                    message = "User does not exists."
                });
            }

            using var dbContext = await dbContextFactory.CreateDbContextAsync();
            // TODO: check that sender existis in db
            // TODO: check that senderId it's the same as loged one
            // TODO: check that recipients are exists

            List<GetMessagesResponse> myMessage = await dbContext.Messages
                .Include(msg => msg.Chanel)
                .Include(msg => msg.Chanel.Recipients)
                .Where(msg => msg.Chanel.Recipients.All(u => u.UserId == senderId || u.UserId == receiverId))
                .Select(msg => GetResponseForMessage(msg, senderId))
                .ToListAsync();

            return Results.Json(myMessage);
        }

        public static GetMessagesResponse GetResponseForMessage(Message message, int senderId)
        {
            return new GetMessagesResponse()
            {
                MessageId = message.Id,
                Message = message.Content,
                Type = message.SenderId == senderId ? "sended" : "recieved",
                SendAt = message.SendAt
            };
        }
    }
}
