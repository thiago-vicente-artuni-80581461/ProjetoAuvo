using Microsoft.AspNetCore.Mvc.Rendering;
using ProjetoAuvo.Models;

namespace ProjetoAuvo.Web.Models
{
    public class GloboClima
    {
        public List<Pais> Paises { get; set; }

        public IEnumerable<SelectListItem> PaisesSelect { get; set; }

    }
}
