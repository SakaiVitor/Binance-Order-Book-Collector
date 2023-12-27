using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class Program
{
    private static readonly Uri WebSocketUri = new Uri("wss://stream.binance.com:9443/ws/bnbbtc@depth");
    private static readonly Uri SnapshotUri = new Uri("https://api.binance.com/api/v3/depth?symbol=BNBBTC&limit=1000");
    private static ClientWebSocket webSocket = new ClientWebSocket();
    private static long lastUpdateId = 0;

    static async Task Main(string[] args)
    {
        Console.WriteLine("Starting Order Book Collector...");
        await GetInitialSnapshot();
        await ConnectWebSocket();
        await ProcessWebSocketMessages();
    }

    private static async Task GetInitialSnapshot()
    {
        Console.WriteLine("Obtaining initial snapshot...");
        using (var client = new HttpClient())
        {
            var response = await client.GetStringAsync(SnapshotUri);
            var snapshot = JObject.Parse(response);
            lastUpdateId = snapshot["lastUpdateId"].Value<long>();

            Console.WriteLine("Initial Snapshot Obtained. LastUpdateId: " + lastUpdateId);
        }
    }

    private static async Task ConnectWebSocket()
    {
        Console.WriteLine("Connecting to WebSocket...");
        await webSocket.ConnectAsync(WebSocketUri, CancellationToken.None);
        Console.WriteLine("Connected to WebSocket.");
    }

    private static async Task ProcessWebSocketMessages()
    {
        Console.WriteLine("Processing WebSocket messages...");
        var buffer = new byte[1024 * 4];

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                ProcessMessage(message);
            }
        }
    }

    private static void ProcessMessage(string message)
    {
        var jsonMessage = JObject.Parse(message);
        var U = jsonMessage["U"].Value<long>();
        var u = jsonMessage["u"].Value<long>();

        Console.WriteLine($"Processing Message: U={U}, u={u}, lastUpdateId={lastUpdateId}");

        // Condição para a primeira mensagem após o snapshot
        if (lastUpdateId == 0 || U <= lastUpdateId + 1 || u >= lastUpdateId + 1)
        {
            if (u > lastUpdateId)
            {
                lastUpdateId = u;
                Console.WriteLine("Received Update: " + jsonMessage.ToString());
            }
        }
        else
        {
            Console.WriteLine("Discarding Out-of-Sequence Message");
        }
    }
}
