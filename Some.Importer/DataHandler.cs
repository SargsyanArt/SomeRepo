namespace Some.Importer
{
    public class DataHandler
    {
        /*
        public static async Task GetFromSocketConnection()
        {
            
            try
            {
                var ws = new ClientWebSocket();
                await ws.ConnectAsync(new Uri("wss://localhost:7002/ws"), CancellationToken.None);
                var receiveTask = Task.Run(async () =>
                {
                    var buffer = new byte[1024];
                    while (true)
                    {
                        var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            break;
                        }
                        var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        Console.WriteLine(message);
                    }
                });
                await receiveTask;
            }
            catch (Exception e)
            {

            }
        }
        */
    }
}
