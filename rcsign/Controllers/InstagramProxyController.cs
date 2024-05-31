using Microsoft.AspNetCore.Mvc;

namespace rcsign.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InstagramProxyController : Controller
    {
        private readonly HttpClient _httpClient;

        public InstagramProxyController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("access-token")]
        public async Task<IActionResult> GetAccessToken(string clientId, string clientSecret, string redirectUri, string code)
        {
            var response = await _httpClient.PostAsync(
                "https://api.instagram.com/oauth/access_token",
                new FormUrlEncodedContent([
                    new("client_id", clientId),
                    new("client_secret", clientSecret),
                    new("grant_type", "authorization_code"),
                    new("redirect_uri", redirectUri),
                    new("code", code)
                ])
            );

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode);
            }

            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }
    }
}
