using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMANews.Areas.Identity.Data
{
    public class PostImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Display(Name = "Naziv")]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Objavljeno")]
        public DateTime DateCreated { get; set; }

        public byte[] Image { get; set; }

        [Display(Name = "Kolegij")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Display(Name = "Kategorija kolegija")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Display(Name = "Autor")]
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }


    }

    public class PostImageVM
    {
        [Display(Name = "Naziv")]
        public string Title { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Objavljeno")]
        public DateTime DateCreated { get; set; }

        public byte[] Image { get; set; }

        [Display(Name = "Kolegij")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }

        [Display(Name = "Kategorija kolegija")]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        [Display(Name = "Autor")]
        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }
    }
}
