using System.Net.WebSockets;

namespace MKasperczyk.Chat.Api.Sockets
{
    public abstract class SocketHandler
    {
        public ConnectionManager ConnectionManager { get; set; }
        public SocketHandler(ConnectionManager connectionManager)
        {
            ConnectionManager = connectionManager;
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
            await Task.Run(() => ConnectionManager.AddSocket(socket));
        }
        public virtual async Task OnDisconnect(WebSocket socket)
        {
            await ConnectionManager.RemoveSocket(socket);
        }
        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffor);
    }
}
