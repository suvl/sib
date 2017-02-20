using System.ComponentModel.DataAnnotations;

namespace Sib.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "e-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "A {0} tem de ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "nova password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "confirmar nova password")]
        [Compare("Password", ErrorMessage = "as passwords não estão iguais.")]
        public string ConfirmPassword { get; set; }
    }
}
