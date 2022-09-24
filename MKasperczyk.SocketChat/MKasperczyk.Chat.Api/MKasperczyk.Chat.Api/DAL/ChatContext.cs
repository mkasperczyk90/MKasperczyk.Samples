
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Reflection.Metadata;

namespace MKasperczyk.Chat.Api.DAL
{
    public class ChatContext: DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Message> Messages => Set<Message>(); 
        public DbSet<ChatChanel> Chanels => Set<ChatChanel>(); 
        public DbSet<ChanelRecipients> ChanelUsers => Set<ChanelRecipients>();


        public ChatContext(DbContextOptions<ChatContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatChanel>()
                .HasMany(chanel => chanel.Messages)
                .WithOne(msg => msg.Chanel)
                .HasForeignKey(chanel => chanel.ChanelId);

            modelBuilder.Entity<ChatChanel>()
                .HasMany(chanel => chanel.Recipients);

            //modelBuilder.Entity<Message>()
            //    .HasOne(m => m.Chanel)
            //    .WithMany(m => m.Messages)
            //    .HasForeignKey(m => m.)

            /*modelBuilder.Entity<Message>()
                .HasOne<User>(s => s.Sender);

            modelBuilder.Entity<Message>()
                .HasMany(message => message.Recipients)
                .WithOne();*/
        }
    }
}
