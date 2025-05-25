using System.ComponentModel.DataAnnotations;

namespace ProjetoAuvo.Web.Models
{
    public class Login
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Senha { get; set; }
    }
}
