using System.ComponentModel.DataAnnotations;

namespace ECommerce.Identidade.API.Models
{
    public class UsuarioLogin
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está com o formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(10, ErrorMessage = "O campo {0} precisa ter mais que {2} e menos que {1} caracteres", MinimumLength = 6)]
        public string Senha { get; set; }
    }
}
