using ProjetoAuvo.Repository.Interface;
using ProjetoAuvo.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAuvo.Repository
{
    public class PaisRepository : IPaisRepository
    {
        private readonly AlvoContext _context;

        public PaisRepository(AlvoContext context)
        {
            _context = context;
        }

        public void SalvarPais(PaisFavorito pais)
        {
            try
            {
                _context.PaisFavorito.Add(pais);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void SalvarCidade(CidadeFavorita cidade)
        {
            try
            {
                _context.CidadeFavorita.Add(cidade);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<PaisFavorito> RecuperarRegistroPaisFavorito()
        {
            try
            {
                return _context.PaisFavorito.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void ExcluirPais(int id)
        {
            try
            {
                var pais = _context.PaisFavorito.ToList().Where(th => th.Id == id).FirstOrDefault();
                _context.PaisFavorito.Remove(pais);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<CidadeFavorita> GetCidadeFavoritos()
        {
            try
            {
               return _context.CidadeFavorita.ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void ExcluirCidadeClima(int id)
        {
            try
            {
                var cidade = _context.CidadeFavorita.ToList().Where(th => th.Id == id).FirstOrDefault();
                _context.CidadeFavorita.Remove(cidade);
                _context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
