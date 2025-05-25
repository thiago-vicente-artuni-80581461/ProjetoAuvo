namespace ProjetoAuvo.Repository.Models
{
    public class CidadeFavorita
    {
        public int Id { get; set; }
        public string Cidade { get; set; }
        public string Condicao { get; set; }
        public string Descricao { get; set; }
        public decimal Temperatura { get; set; }
        public decimal SensacaoTermica { get; set; }
    }
}
