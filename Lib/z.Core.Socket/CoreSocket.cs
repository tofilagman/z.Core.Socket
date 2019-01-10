using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace z.Core.Socket
{
    public abstract class CoreSocket : ICoreSocket
    {
        public ICoreSocketConnection CoreSocketConnection { get; set; }

        public CoreSocket(ICoreSocketConnection coreSocketConnection)
        {
            this.CoreSocketConnection = coreSocketConnection;
        }

#pragma warning disable CS1998
        public virtual async Task OnConnected(WebSocket socket)
        {
            this.CoreSocketConnection.AddSocket(socket);
        }
#pragma warning restore CS1998

        public virtual async Task OnDisconnected(WebSocket socket)
        {
            await CoreSocketConnection.RemoveSocket(CoreSocketConnection.GetId(socket));
        }

        public abstract Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
         
        public async Task SendMessageAsync(WebSocket socket, string message)
        {
            if (socket.State != WebSocketState.Open)
                return;

            await socket.SendAsync(buffer: new ArraySegment<byte>(array: Encoding.ASCII.GetBytes(message),
                                                                  offset: 0,
                                                                  count: message.Length),
                                   messageType: WebSocketMessageType.Text,
                                   endOfMessage: true,
                                   cancellationToken: CancellationToken.None);
        }

        public async Task SendMessageAsync(string socketId, string message)
        {
            await SendMessageAsync(CoreSocketConnection.GetSocketById(socketId), message);
        }

        public async Task SendMessageToAllAsync(string message)
        {
            foreach (var pair in CoreSocketConnection.GetAll())
            {
                if (pair.Value.State == WebSocketState.Open)
                    await SendMessageAsync(pair.Value, message);
            }
        }
         
    }
}
