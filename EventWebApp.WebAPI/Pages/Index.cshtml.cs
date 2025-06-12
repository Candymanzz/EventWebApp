using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventWebApp.WebAPI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            logger = logger;
        }

        public void OnGet()
        {

        }
    }
}
