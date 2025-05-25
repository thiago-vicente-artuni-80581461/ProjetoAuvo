using Microsoft.EntityFrameworkCore;
using ProjetoAuvo.Repository.Models;


namespace ProjetoAuvo.Repository
{
    public class AlvoContext : DbContext
    {
        public AlvoContext() { }
        public AlvoContext(DbContextOptions<AlvoContext> options) : base(options) { }
        public DbSet<PaisFavorito> PaisFavorito { get; set; }
        public DbSet<CidadeFavorita> CidadeFavorita { get; set; }

       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}