using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json.Nodes;

namespace WebApp6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger, IDiagnosticContext diagnosticContext)
        {
            _logger = logger;
        }

        [HttpPost]
        public void Post(JsonNode node)
        {
            _logger.LogInformation(node.ToJsonString());
        }
    }
}