using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMANews.Areas.Identity.Data
{
    public class CourseCategory
    {
        //many-many
        //jedan kolegij ima vise kategorija, a jedna kategorija je u vise kolegija, 
        //npr. kolegij PMA ima kategorije:1kolokvij, 2kolokvij, 1ispitnirok
        //npr. kolegij PROMA ima kategorije:1kolokvij, 2kolokvij, 1ispitnirok
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }  
        public int CourseId { get; set; }
        public int CategoryId { get; set; }

        public virtual Course Course { get; set; }
        public virtual Category Category { get; set; }

        public ICollection<PostImage> PostImages { get; set; }
    }
}
