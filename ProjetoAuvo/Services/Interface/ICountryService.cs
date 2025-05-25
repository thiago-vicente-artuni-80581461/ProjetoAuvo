using ProjetoAuvo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAuvo.Services.Interface
{
    public interface ICountryService
    {
        Task<List<Pais>> BuscarPaisesAsync();
        void ExcluirPais(int id);
        Task<IEnumerable<PaisFavorito>> GetPaisFavoritos();

    }
}
