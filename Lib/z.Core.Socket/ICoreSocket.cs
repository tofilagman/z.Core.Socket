using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace z.Core.Socket
{
    public interface ICoreSocket
    {
        ICoreSocketConnection CoreSocketConnection { get; set; }
        Task OnConnected(WebSocket socket);
        Task OnDisconnected(WebSocket socket);
        Task SendMessageAsync(WebSocket socket, string message);
        Task SendMessageAsync(string socketId, string message);
        Task SendMessageToAllAsync(string message);
        Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);
    }

    public interface ICoreSocketConnection
    {
        WebSocket GetSocketById(string id);
        ConcurrentDictionary<string, WebSocket> GetAll();
        string GetId(WebSocket socket);
        void AddSocket(WebSocket socket);
        Task RemoveSocket(string id);
    }
}
