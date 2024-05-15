using System.Net;
using System.Text;
using System.Text.Json;

namespace Some.RandomGenerator
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();

            app.UseHttpsRedirection();
            app.UseWebSockets();
            app.Map("/ws", async context =>
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    using var ws = await context.WebSockets.AcceptWebSocketAsync();
                    while (true)
                    {
                        var message = new { Time = DateTime.UtcNow.ToLongTimeString(), Value = new Random().Next(0, 100) };
                        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));
                        var arraySegment = new ArraySegment<byte>(bytes, 0, bytes.Length);
                        if (ws.State == System.Net.WebSockets.WebSocketState.Open)
                        {
                            await ws.SendAsync(arraySegment, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        else if (ws.State == System.Net.WebSockets.WebSocketState.Closed || ws.State == System.Net.WebSockets.WebSocketState.Aborted)
                        {
                            break;
                        }
                        Thread.Sleep(500);
                    }
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                }
            });

            await app.RunAsync();
        }
    }
}
