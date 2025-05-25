using DocumentFormat.OpenXml.Drawing.Charts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjetoAuvo.Repository.Models;
using ProjetoAuvo.Services;
using ProjetoAuvo.Services.Interface;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

namespace ProjetoAuvo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaisController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ICountryService _countryService;

        public PaisController(IWeatherService weatherService, ICountryService countryService)
        {
            _weatherService = weatherService;
            _countryService = countryService;
        }

        /// <summary>
        /// Retorna todos os pa�ses.
        /// </summary>
        [HttpGet("paises")]
        public async Task<IActionResult> GetPaises()
        {
            var paises = await _countryService.BuscarPaisesAsync();
           
            return Ok(paises);
        }

        /// <summary>
        /// Salva o Pa�s Favorito
        /// </summary>
        [Authorize]
        [HttpPost("SalvarPais")]
        public void SalvarPais([FromBody] PaisFavorito pais)
        {
             _weatherService.SalvarPais(pais);
        }

        /// <summary>
        /// Busca todos os pa�ses favoritos
        /// </summary>
        [Authorize]
        [HttpGet("GetPaisFavoritos")]
        public async Task<IActionResult> GetPaisFavoritos()
        {
            IEnumerable<PaisFavorito> favoritos = await _countryService.GetPaisFavoritos();

            return Ok(favoritos); 
        }

        /// <summary>
        /// Exclui os Pa�s Favoridos
        /// </summary>
        [Authorize]
        [HttpDelete("ExcluirPais/{Id}")]
        public void ExcluirPais(int Id)
        {
             _countryService.ExcluirPais(Id);
        }
    }
}
