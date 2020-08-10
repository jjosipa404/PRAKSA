using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMANews.Areas.Identity.Data
{
    public class Category
    {
        //KATEGORIJA 

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Naziv")]
        public string Name { get; set; }  //naziv

       
        public ICollection<CourseCategory> Courses { get; set; } //kolegiji
        public ICollection<PostImage> PostImages { get; set; } 
        public ICollection<PostFile> PostFiles { get; set; }

    }
}
