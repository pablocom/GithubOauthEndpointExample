using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GithubOauthEndpoint.Controllers
{
    [ApiController]
    [Route("user/signin/callback")]
    public class GithubOauthEndpointController : ControllerBase
    {
        private readonly ILogger<GithubOauthEndpointController> _logger;
        private readonly IHttpClientFactory httpClientFactory;

        public GithubOauthEndpointController(ILogger<GithubOauthEndpointController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<ActionResult<HttpResponseMessage>> Get([FromQuery(Name = "code")] string code, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(code))
                return BadRequest("Code not present in request");
            
            var httpClient = httpClientFactory.CreateClient("GitHub");
            var accessTokenRequestContent = new
            {
                client_id = "e2475c9bf2dc410d31a6", 
                client_secret = "e8f235e039eccaa8dd43665b988b8680d567c7de", 
                code
            };

            var result = await httpClient
                .PostAsync("/login/oauth/access_token", JsonContent.Create(accessTokenRequestContent), cancellationToken)
                .ConfigureAwait(false);

            return Ok(result);
        }
    }
}
