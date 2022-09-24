using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

namespace MKasperczyk.Chat.Api.Sockets
{
    public abstract class ConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _connection = new ConcurrentDictionary<string, WebSocket>();
        public WebSocket GetSocket(string id)
        {
            return _connection.FirstOrDefault(con => con.Key == id).Value;
        }

        public async Task RemoveSocket(WebSocket socket)
        {
            string key = _connection.FirstOrDefault(con => con.Value == socket).Key;
            await RemoveSocket(key);
        }
        public string GetId(WebSocket socket)
        {
            string key = _connection.FirstOrDefault(con => con.Value == socket).Key;
            return key;
        }

        public async Task RemoveSocket(string id)
        {
            _connection.TryRemove(id, out var socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, $"closed connection for {id}", CancellationToken.None);
        }
        public void AddSocket(WebSocket socket)
        {
            _connection.TryAdd(Guid.NewGuid().ToString(), socket);
        }

        public async Task SendMessage(WebSocket socket, string content)
        {
            if (socket.State != WebSocketState.Open) return;
            await socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.GetBytes(content), 0, content.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public async Task SendMessage(string id, string content)
        {
            await SendMessage(GetSocket(id), content);
        }

        public abstract Task Receive(WebSocket socket, WebSocketReceiveResult result, byte[] buffor);
    }
}
