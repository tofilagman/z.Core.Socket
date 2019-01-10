using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace SampleWebApp.Sockets
{
    [CoreSocket("/Chat")]
    public class ChatRoomSocket : CoreSocket
    {
        public ChatRoomSocket(ICoreSocketConnection coreSocketConnection) : base(coreSocketConnection) { }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            var gg = Encoding.UTF8.GetString(buffer);
            await SendMessageToAllAsync(gg);

            while (true)
            {
                await SendMessageAsync(socket, Guid.NewGuid().ToString());
                await Task.Delay(1000);
            }
        }
    }
}
