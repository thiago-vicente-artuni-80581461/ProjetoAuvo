using ProjetoAuvo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProjetoAuvo.Services.Interface
{
    public interface IWeatherService
    {
        void ExcluirCidadeClima(int id);
        Task<IEnumerable<CidadeFavorita>> GetCidadeFavoritos();
        Task<Clima?> ObterClimaAsync(string city);
        void SalvarCidade(CidadeFavorita cidade);
        void SalvarPais(PaisFavorito pais);
    }
}
