using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMANews.Areas.Identity.Data
{
    public class ApplicationRole :IdentityRole<int>
    {
        //ULOGA 

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Display(Name = "Naziv")]
        public override string Name { get; set; }  //naziv

        public ICollection<ApplicationUser> Users { get; set; }  //korisnici
    }
}
