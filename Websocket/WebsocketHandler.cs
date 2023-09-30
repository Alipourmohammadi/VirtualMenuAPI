using System.Net.WebSockets;
using System.Text;
using VirtualMenuAPI.Data;


// namespace VirtualMenuAPI.Handler;
public class WebSocketHandler
{
    private WebSocket _webSocket ;
    // public WebSocketHandler(WebSocket webSocket)
    // {
    //     _webSocket = webSocket;
    // }
    public void InitializeWebSocket(WebSocket webSocket)
    {
        _webSocket = webSocket;
    }
    public async Task SendDataAsync(string data)
    {
        if (_webSocket.State == WebSocketState.Open)
        {
            var dataBuffer = Encoding.UTF8.GetBytes(data);
            var buffer = new ArraySegment<byte>(dataBuffer);

            await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        else
        {
            // Handle the WebSocket being closed or in an invalid state.
            throw new InvalidOperationException("WebSocket is not in an open state.");
        }
    }

    public async Task ReceiveDataAsync()
    {
        var buffer = new byte[1024];

        while (_webSocket.State == WebSocketState.Open)
        {
            var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var receivedData = Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine($"Received: {receivedData}");
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
        }
    }
}
