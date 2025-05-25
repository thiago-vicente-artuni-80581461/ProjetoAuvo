using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoAuvo.Models;
using ProjetoAuvo.Web.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text.Json;


namespace ProjetoAuvo.Web.Controllers
{
    public class CadastroController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _chaveApiPais = "http://localhost:5091/api/pais/paises";
        private readonly string _chaveApiCidadesClima = "http://localhost:5091/api/cidadeclima";
        private readonly string _chaveBuscarFavoritosPais = "http://localhost:5091/api/pais/GetPaisFavoritos";
        private readonly string _chaveSalvarFavoritosPais = "http://localhost:5091/api/pais/SalvarPais";
        private readonly string _chaveExcluirFavoritosPais = "http://localhost:5091/api/pais/ExcluirPais";
        private readonly string _chaveBuscarFavoritosCidade = "http://localhost:5091/api/cidadeclima/GetCidadeFavoritos";
        private readonly string _chaveSalvarFavoritosCidades = "http://localhost:5091/api/cidadeclima/SalvarCidadeClima";
        private readonly string _chaveExcluirFavoritosCidades = "http://localhost:5091/api/cidadeclima/ExcluirCidadeClima";

        public CadastroController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Pais()
        {
            List<PaisFavorito> lista = new List<PaisFavorito>();

            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Obter dados da API de País
            var paisResponse = await client.GetAsync(_chaveBuscarFavoritosPais);

            if (!paisResponse.IsSuccessStatusCode)
            {
                // erro no carregamento
                return View("ErroApi");
            }
            var json = await paisResponse.Content.ReadAsStringAsync();

            var paises = JsonSerializer.Deserialize<List<PaisFavorito>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(paises);
        }

        [HttpGet]
        public async Task<IActionResult> FormularioPais()
        {
            List<Pais> lista = new List<Pais>();

            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();

            // Obter dados da API de País
            var paisResponse = await client.GetAsync(_chaveApiPais);

            if (!paisResponse.IsSuccessStatusCode)
            {
                // erro no carregamento
                return View("ErroApi");
            }
            var json = await paisResponse.Content.ReadAsStringAsync();

            var paises = JsonSerializer.Deserialize<List<Pais>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            GloboClima viewModel = new GloboClima
            {
                Paises = paises
            };

            var conf = viewModel.Paises.OrderBy(th => th.Nome).ToList();

            List<SelectListItem> listaPaises = new List<SelectListItem>();

            SelectListItem l = new SelectListItem
            {
                Text = "--Selecione--",
                Value = null,
                Selected = true
            };
            listaPaises.Add(l);

            foreach (var item in conf)
            {
                l = new SelectListItem
                {
                    Text = item.Nome,
                    Value = item.Codigo.ToString()
                };
                listaPaises.Add(l);
            }

            viewModel.PaisesSelect = listaPaises;

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SalvarFavorito(string paisId, string paisNome)
        {
            try
            {
                var favorito = new PaisFavorito
                {
                    Codigo = paisId,
                    Nome = paisNome
                };

                var token = HttpContext.Session.GetString("JWT");

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Auth");
                }

                var client = _httpClientFactory.CreateClient();

                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsJsonAsync(_chaveSalvarFavoritosPais, favorito);

                if (!response.IsSuccessStatusCode)
                    return View("ErroApi");

                return RedirectToAction("Pais", "Cadastro");
            }
            catch (ValidationException)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoverPais(int Id)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWT");

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Auth");
                }

                var client = _httpClientFactory.CreateClient();

                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync(_chaveExcluirFavoritosPais + "/"+Id);

                if (!response.IsSuccessStatusCode)
                    return View("ErroApi");

                return Json(new { sucesso = true});
            }

            catch (ValidationException)
            {
                return RedirectToAction("Pais", "Cadastro");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Clima()
        {
            List<CidadeFavorita> lista = new List<CidadeFavorita>();

            var token = HttpContext.Session.GetString("JWT");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            var client = _httpClientFactory.CreateClient();

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Obter dados da API de País
            var paisResponse = await client.GetAsync(_chaveBuscarFavoritosCidade);

            if (!paisResponse.IsSuccessStatusCode)
            {
                // erro no carregamento
                return View("ErroApi");
            }
            var json = await paisResponse.Content.ReadAsStringAsync();

            var cidades = JsonSerializer.Deserialize<List<CidadeFavorita>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return View(cidades);
        }

        [HttpGet]
        public async Task<IActionResult> FormularioClima()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BuscarCidade(string cidade)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWT");

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Auth");
                }

                var client = _httpClientFactory.CreateClient();

                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(_chaveApiCidadesClima + "/" + Uri.EscapeDataString(cidade));

                if (!response.IsSuccessStatusCode)
                    return View("ErroApi");

                var json = await response.Content.ReadAsStringAsync();

                var informacoesCidade = JsonSerializer.Deserialize<CidadeFavorita>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Json(new { resposta = informacoesCidade });
            }

            catch (ValidationException)
            {
                return RedirectToAction("Clima", "Cadastro");
            }
        }

        [HttpPost]
        public async Task<IActionResult> SalvarFavoritoCidade(CidadeFavorita cidadeFavorita)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWT");

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Auth");
                }

                var client = _httpClientFactory.CreateClient();

                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.PostAsJsonAsync(_chaveSalvarFavoritosCidades, cidadeFavorita);

                if (!response.IsSuccessStatusCode)
                    return View("ErroApi");

                return RedirectToAction("Clima", "Cadastro");
            }
            catch (ValidationException)
            {

                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> RemoverCidadeClima(int Id)
        {
            try
            {
                var token = HttpContext.Session.GetString("JWT");

                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Auth");
                }

                var client = _httpClientFactory.CreateClient();

                if (!string.IsNullOrEmpty(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.DeleteAsync(_chaveExcluirFavoritosCidades + "/" + Id);

                if (!response.IsSuccessStatusCode)
                    return View("ErroApi");

                return Json(new { sucesso = true });
            }

            catch (ValidationException)
            {
                return RedirectToAction("Pais", "Cadastro");
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
