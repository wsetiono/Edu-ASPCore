using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Education.Areas.Identity.Models
{
    public class AppUser : IdentityUser
    {
        [PersonalData]
        [MaxLength(25)]
        public string Name { get; set; }

    }
}
