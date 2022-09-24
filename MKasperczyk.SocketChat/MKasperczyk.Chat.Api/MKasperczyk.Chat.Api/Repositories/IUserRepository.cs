using MKasperczyk.Chat.Api.DAL;

namespace MKasperczyk.Chat.Api.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User?> GetUserAsync(int id);
        Task AddUserAsync(string userName, string password);
    }
}
