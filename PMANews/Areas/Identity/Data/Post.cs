using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMANews.Areas.Identity.Data
{
    public class Post
    {
        //---------------------------------------------------- ID
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        //---------------------------------------------------- NASLOV
        [Display(Name = "Naslov")]
        public string Title { get; set; }
        //---------------------------------------------------- SADRZAJ
        [Display(Name = "Sadržaj")]
        public string Content { get; set; }   
        //---------------------------------------------------- DATUM UREDJIVANJA
        [DataType(DataType.Date)]
        [Display(Name = "Objavljeno")]
        public DateTime DateCreated { get; set; }
        //---------------------------------------------------- KATEGORIJA
        [Display(Name = "Kolegij")]
        public int CourseId { get; set; }         
        public virtual Course Course { get; set; }
        //---------------------------------------------------- AUTOR
        [Display(Name = "Autor")]
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }
        //---------------------------------------------------- KOMENTARI
        public ICollection<Comment> Comments { get; set; }
    }
}
