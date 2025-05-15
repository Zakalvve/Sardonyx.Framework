using Microsoft.AspNetCore.Mvc;
using Sardonyx.Framework.Core.CQRS.Application;
using Sardonyx.Framework.Testing.WebApp.TestClient;

namespace Sardonyx.Framework.Testing.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly IExecutor executor;
        private readonly ITestClient client;

        public DemoController(IExecutor executor, ITestClient client)
        {
            this.executor = executor;
            this.client = client;
        }

        [HttpGet("command")]
        public async Task<IActionResult> GetMessage()
        {
            var result = await executor.ExecuteCommandAsync(new TestCommand.TestCommand("Hello, world!"));

            return Ok(result);
        }

        [HttpGet("query")]
        public async Task<IActionResult> GetResult()
        {
            var result = await executor.ExecuteQueryAsync(new TestQuery.TestQuery("Hello, world!"));

            return Ok(result);
        }

        [HttpGet("client")]
        public async Task<IActionResult> GetClient()
        {
            var result = await client.Examples.Get();

            return Ok(result);
        }
    }
}
