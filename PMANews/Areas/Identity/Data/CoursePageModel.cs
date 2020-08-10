using PMANews.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PMFNotes.Areas.Identity.Data
{
    public class CoursePageModel
    {
        public List<Post> Posts { get; set; }
        public List<PostImage> PostImages { get; set; }
        public List<PostFile> PostFiles { get; set; }
    }
}
