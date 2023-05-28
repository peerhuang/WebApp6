using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json.Nodes;

namespace WebApp6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IDiagnosticContext _diagnosticContext;
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger, IDiagnosticContext diagnosticContext)
        {
            _logger = logger;
            _diagnosticContext = diagnosticContext;
        }

        [HttpPost]
        public void Post(JsonNode node)
        {
            _logger.LogDebug("111111111111111111111111111111111111111111111111111111111111111");
            _diagnosticContext.Set("11232", 11111);
            _logger.LogInformation(node.ToJsonString());
        }
    }
}