using Microsoft.AspNetCore.Mvc;
using ProjetoAuvo.Web.Models;
using System.Text;
using System.Text.Json;

namespace ProjetoAuvo.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync("http://localhost:5091/api/auth/login", model);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError("", "Login inválido.");
                return View(model);
            }

            var jwt = await response.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("JWT", jwt);

            return RedirectToAction("Index", "Home");
        }
    }
}

