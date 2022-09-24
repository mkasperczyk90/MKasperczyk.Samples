using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MKasperczyk.Chat.Api.Models;

namespace MKasperczyk.Chat.Api.DAL
{
    public class ChatInitializer
    {
        public async static Task Seed(ChatContext context)
        {
            //context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var adminUser = await context.Users.FirstOrDefaultAsync(u => u.Username == "Admin");

            if (adminUser == null)
            {
                PasswordHasher<string> passwordHasher = new PasswordHasher<string>();

                await context.Users.AddAsync(new User()
                {
                    Username = "Admin",
                    Password = passwordHasher.HashPassword("Admin", "AdminAdmin1")
                });

                await context.Users.AddAsync(new User()
                {
                    Username = "TestUser",
                    Password = passwordHasher.HashPassword("TestUser", "TestUser1")
                });

                await context.Users.AddAsync(new User()
                {
                    Username = "TestUser2",
                    Password = passwordHasher.HashPassword("TestUser2", "TestUser1")
                });

                context.SaveChanges();

                var admin = await context.Users.FirstAsync(user => user.Username == "Admin");
                var testUser = await context.Users.FirstAsync(user => user.Username == "TestUser");

                var chanel = new ChatChanel() { 
                    Name = "FirstChanel",
                };
                await context.Chanels.AddAsync(chanel);

                var message = new Message()
                {
                    Sender = admin,
                    Chanel = chanel,
                    Content = "TestConetnt",
                    SendAt = DateTime.UtcNow
                };
                await context.Messages.AddAsync(message);

                ChanelRecipients chanelUser = new ChanelRecipients()
                {
                    Chanel = chanel,
                    Users = new List<User>()
                    {
                        admin,
                        testUser
                    }
                };
                context.ChanelUsers.Add(chanelUser);
                context.SaveChanges();
            }
        }
    }
}
