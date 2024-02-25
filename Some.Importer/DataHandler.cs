using System.Net.WebSockets;
using System.Text;

namespace Some.Importer
{
    public class DataHandler
    {
        private const string apiKey = "8kIphcsqIhpgKsDLhkfiya1h_w2rJB2Y";
        private const string symbol = "XT.X:BTC-USD"; // Replace with the desired stock symbol
        public static async Task GetFromPolygon()
        {
            string apiUrl = $"https://api.polygon.io/v2/aggs/ticker/AAPL/range/1/day/2023-01-09/2023-01-09?apiKey={apiKey}";

            using HttpClient client = new();
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseData);
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
        }

        public static async Task GetFromPolygonContinuously()
        {
            Uri socketUri = new($"wss://socket.polygon.io/crypto");

            using ClientWebSocket webSocket = new();
            try
            {
                await webSocket.ConnectAsync(socketUri, CancellationToken.None);
                string statusMessage = await ReceiveWebSocketMessage(webSocket);
                if (webSocket.State == WebSocketState.Open)
                {
                    string subscribeMessage = $"{{\"action\":\"auth\",\"params\":\"{apiKey}\"}}";
                    await SendWebSocketMessage(webSocket, subscribeMessage);

                    // Wait for the authentication response
                    string authResponse = await ReceiveWebSocketMessage(webSocket);
                    Console.WriteLine(authResponse);

                    // Check if authentication was successful
                    if (!authResponse.Contains("Connected Successfully"))
                    {
                        Console.WriteLine("Authentication failed. Check your API key.");
                        return;
                    }

                    // Subscribe to the trades channel
                    string subscribeTradesMessage = $"{{\"action\":\"subscribe\",\"params\":\"{symbol}\"}}";
                    await SendWebSocketMessage(webSocket, subscribeTradesMessage);

                    while (webSocket.State == WebSocketState.Open)
                    {
                        string message = await ReceiveWebSocketMessage(webSocket);
                        Console.WriteLine(message);
                    }
                }
            }
            catch (WebSocketException ex)
            {
                Console.WriteLine($"WebSocketException: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }
            finally
            {
                if (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseReceived || webSocket.State == WebSocketState.CloseSent)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                }
            }
        }

        static async Task SendWebSocketMessage(ClientWebSocket webSocket, string message)
        {
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        static async Task<string> ReceiveWebSocketMessage(ClientWebSocket webSocket)
        {
            byte[] buffer = new byte[4096];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                return Encoding.UTF8.GetString(buffer, 0, result.Count);
            }
            else return null;
        }
    }
}
