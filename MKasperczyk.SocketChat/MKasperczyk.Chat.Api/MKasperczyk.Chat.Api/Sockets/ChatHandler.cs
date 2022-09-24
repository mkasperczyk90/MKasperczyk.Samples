using System.ComponentModel;
using System.Net.WebSockets;

namespace MKasperczyk.Chat.Api.Sockets
{
    public class ChatHandler : SocketHandler
    {
        public ChatHandler(ConnectionManager connectionManager): base(connectionManager)
        {
        }

        //public override async Task OnConnected(WebSocket socket)
        //{
        //    await base.OnConnected(socket);
        //    var socketId = ConnectionManager.GetId(socket);
        //}
        public override Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffor)
        {
            var socketId = ConnectionManager.GetId(socket);
            throw new NotImplementedException();
        }
    }
}
