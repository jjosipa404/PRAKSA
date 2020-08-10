using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMANews.Areas.Identity.Data
{
    public class Course
    {
        //KOLEGIJ  

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  //id

        [Display(Name = "Naziv")]
        public string Name { get; set; } //naziv

        [Display(Name = "Kratica")]
        public string ShortName { get; set; } //kratica

        [Display(Name = "Odjel")]
        public int DepartmentId { get; set; }  //odjel
        public virtual Department Department { get; set; }

        public ICollection<CourseCategory> Categories { get; set; } //kategorije
        public ICollection<Post> Posts { get; set; }  //objave
        public ICollection<PostImage> PostImages { get; set; }  //objave biljeski
        public ICollection<PostFile> PostFiles { get; set; }  
        public ICollection<CourseApplicationUser> ApplicationUsers { get; set; } //studenti

    }

   
}
