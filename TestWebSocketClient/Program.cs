using ProtoBuf;
using System.Net.WebSockets;
using TestWebSocketClient;

var ws = new ClientWebSocket();
await ws.ConnectAsync(new Uri("ws://localhost:6969/ws"), CancellationToken.None);
Console.WriteLine("Connected!");

var received = Task.Run(async() =>
{
    while (true)
    {
        var buffer = new ArraySegment<byte>(new byte[1024]);
        var result = await ws.ReceiveAsync(buffer, CancellationToken.None);
        if (result.MessageType == WebSocketMessageType.Close)
            break;
        var inputStream = new System.IO.MemoryStream(buffer.Array, 0, result.Count);
        var currencyPair = Serializer.Deserialize<CurrencyPair>(inputStream);
        Console.WriteLine(currencyPair);
    }
});
await received;