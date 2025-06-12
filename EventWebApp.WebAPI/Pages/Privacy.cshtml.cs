using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EventWebApp.WebAPI.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            logger = logger;
        }

        public void OnGet()
        {
        }
    }

}
