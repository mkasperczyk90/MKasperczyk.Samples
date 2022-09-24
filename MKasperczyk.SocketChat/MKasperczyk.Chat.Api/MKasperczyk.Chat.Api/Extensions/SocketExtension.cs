using MKasperczyk.Chat.Api.Sockets;

namespace MKasperczyk.Chat.Api.Extensions
{
    public static class SocketExtension
    {
        public static IServiceCollection AddWebSocket(this IServiceCollection services)
        {
            services.AddTransient<ConnectionManager>();
            services.AddSingleton<ChatHandler>();
            return services;
        }
    }
}
