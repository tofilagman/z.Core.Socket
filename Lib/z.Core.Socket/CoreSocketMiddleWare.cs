using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace z.Core.Socket
{
    public class CoreSocketMiddleWare
    {
        private readonly RequestDelegate Next;
        private ICoreSocket CoreSocket { get; set; }

        public CoreSocketMiddleWare(RequestDelegate next, ICoreSocket coreSocket)
        {
            this.Next = next;
            this.CoreSocket = coreSocket;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                await Next.Invoke(context);
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();

            await CoreSocket.OnConnected(socket);

            await Receive(socket, async (result, buffer) =>
            {
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await CoreSocket.ReceiveAsync(socket, result, buffer);
                    return;
                }

                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await CoreSocket.OnDisconnected(socket);
                    return;
                }
            });
        }

        private async Task Receive(WebSocket socket, Action<WebSocketReceiveResult, byte[]> handleMessage)
        {
            try
            {
                var buffer = new byte[1024 * 4];

                while (socket.State == WebSocketState.Open)
                {
                    var result = await socket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer),
                                                           cancellationToken: CancellationToken.None);

                    handleMessage(result, buffer);
                }
            }
            catch  
            {

            } 
        }
    }
}
