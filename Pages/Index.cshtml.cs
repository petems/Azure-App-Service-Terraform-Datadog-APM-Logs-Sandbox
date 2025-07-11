using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AzureAppServiceSample.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Example of using log scopes for correlation
            using (_logger.BeginScope("UserSession:{UserId}", "anonymous"))
            using (_logger.BeginScope("RequestId:{RequestId}", HttpContext.TraceIdentifier))
            {
                _logger.LogInformation("Home page accessed");
                
                // Any logs within this scope will include the scope properties
                _logger.LogDebug("Processing home page request");
            }
        }
    }
} 