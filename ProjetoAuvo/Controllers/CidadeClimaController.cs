using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjetoAuvo.Repository.Models;
using ProjetoAuvo.Services;
using ProjetoAuvo.Services.Interface;

namespace ProjetoAuvo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CidadeClimaController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public CidadeClimaController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        /// <summary>
        /// Retorna o clima atual de uma cidade.
        /// </summary>
        /// <param name="cidade">Nome da cidade (ex: Rio de Janeiro)</param>
        [HttpGet("{cidade}")]
        public async Task<IActionResult> ObterClima(string cidade)
        {
            var clima = await _weatherService.ObterClimaAsync(cidade);

            if (clima == null)
                return NotFound("Cidade não encontrada ou erro na API externa.");

            return Ok(clima);
        }

        /// <summary>
        /// Salva o clima da cidade
        /// </summary>
        [Authorize]
        [HttpPost("SalvarCidadeClima")]
        public void SalvarCidadeClima([FromBody] CidadeFavorita cidade)
        {
            _weatherService.SalvarCidade(cidade);
        }

        /// <summary>
        /// Busca as cidades favoritas
        /// </summary>
        [Authorize]
        [HttpGet("GetCidadeFavoritos")]
        public async Task<IActionResult> GetCidadeFavoritos()
        {
            IEnumerable<CidadeFavorita> favoritos = await _weatherService.GetCidadeFavoritos();

            return Ok(favoritos);
        }

        /// <summary>
        /// Exclui a cidade do favorito
        /// </summary>
        [Authorize]
        [HttpDelete("ExcluirCidadeClima/{Id}")]
        public void ExcluirCidadeClima(int Id)
        {
            _weatherService.ExcluirCidadeClima(Id);
        }
    }
}
