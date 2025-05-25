using ProjetoAuvo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAuvo.Repository.Interface
{
    public interface IPaisRepository
    {
        void ExcluirCidadeClima(int id);
        void ExcluirPais(int id);
        IEnumerable<CidadeFavorita> GetCidadeFavoritos();
        IEnumerable<PaisFavorito> RecuperarRegistroPaisFavorito();
        void SalvarCidade(CidadeFavorita cidade);
        void SalvarPais(PaisFavorito pais);
    }
}
