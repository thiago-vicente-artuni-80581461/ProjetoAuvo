using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAuvo.Models
{
    public class Clima
    {
        public string Cidade { get; set; }
        public string Condicao { get; set; }
        public string Descricao { get; set; }
        public double Temperatura { get; set; }
        public double SensacaoTermica { get; set; }
    }
}
