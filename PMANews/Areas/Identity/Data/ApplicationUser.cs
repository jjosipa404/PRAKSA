using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace PMANews.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        //STUDENT

        [Display(Name = "Ime")]
        public string FirstName { get; set; }

        [Display(Name = "Prezime")]
        public string LastName { get; set; }

        [Display(Name = "Odjel")]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [Display(Name = "Uloga")]
        public int RoleId { get; set; }
        public virtual ApplicationRole Role { get; set; }

        public ICollection<CourseApplicationUser> Courses { get; set; } //kolegiji
        public ICollection<Post> Posts { get; set; } //objave
        public ICollection<PostImage> PostImages { get; set; } //objave biljeski
        public ICollection<PostFile> PostFiles { get; set; } //objave fileova
        public ICollection<Comment> Comments { get; set; } //komentari

     


    }
}
