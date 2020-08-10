using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMANews.Areas.Identity.Data
{
    public class Department
    {
        //ODJEL

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } //id

        [Display(Name = "Naziv")] 
        public string Name { get; set; }  //naziv

        public ICollection<Course> Courses { get; set; }   //kolegiji

        public ICollection<ApplicationUser> Students { get; set; }  //studenti
    }
}
