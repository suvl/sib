using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Sib.Models.ManageViewModels
{
    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Telemóvel"), RegularExpression(@"93\d{7}|91\d{7}|92\d{7}|96\d{7}", ErrorMessage = "Telemóvel inválido")]
        public string PhoneNumber { get; set; }
    }
}
