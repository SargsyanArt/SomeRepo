using Microsoft.AspNetCore.Mvc.RazorPages;
using Some.Importer;

namespace Some.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async void OnGet()
        {
            //await DataHandler.GetFromPolygon();
            await DataHandler.GetFromPolygonContinuously();
        }
    }
}
