using System.Diagnostics;
using System.Net.WebSockets;

namespace MKasperczyk.Chat.Api.Sockets
{
    public class SocketMiddleware
    {
        private readonly RequestDelegate _next;
        public SocketMiddleware(RequestDelegate next, SocketHandler socketHandler)
        {
            _next = next;
            SocketHandler = socketHandler;
        }

        private SocketHandler SocketHandler { get; set; }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest) return;

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await Receive(socket, async (result, buffor) =>
            {
                if(result.MessageType == WebSocketMessageType.Text)
                {
                    await SocketHandler.Receive(socket, result, buffor);
                } 
                else if(result.MessageType == WebSocketMessageType.Close)
                {
                    await SocketHandler.OnDisconnect(socket);
                }
            });
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> messageHandle)
        {
            var buffer = new byte[1024];
            while(socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                messageHandle(result, buffer);
            }
        }
    }
}
