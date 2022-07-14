using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RegistrationAdmin.ViewModels
{
    public class UserViewModels
    {
        [Required]
        [EmailAddress]
        [MinLength(6)]
        [MaxLength(100)]      
        
        public virtual string Email { get; set; }
        [Required]
        [Display(Name = "First name")]
        public virtual string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public virtual string LastName { get; set; }


    }
}
