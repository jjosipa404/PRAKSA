using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PMANews.Areas.Identity.Data
{
    public class CourseApplicationUser
    {
        //many-many relationship
        //jedan student ima vise kolegija, a na jendom kolegiju je vise studenata
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        public int CourseId { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Course Course { get; set; }
    }
}
