using Microsoft.AspNetCore.Mvc;
using Some.Web.Models;
using Some.Web.Services;
using System.Text.Json;

namespace Some.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class DataController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Send([FromBody] Measurement data)
        {
            var producer = new KafkaProducer("localhost:9092");

            Tuple<bool, string>? result = await producer.ProduceAsync("demo-messages", JsonSerializer.Serialize(data));
            return Ok(new ApiResponse(result.Item1, result.Item2));
        }
    }
}
