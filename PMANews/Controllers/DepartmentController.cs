using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using PMANews.Areas.Identity.Data;
using PMANews.Data;

namespace PMANews.Controllers
{
    [Authorize]
    public class DepartmentController : Controller
    {
        private readonly PMFNotesContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DepartmentController(PMFNotesContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: /Department/IndexCourses/1
        public async Task<IActionResult> Courses(int id) //id odjela(DepartmentId)
        {           
            ViewBag.course = _context.Course.Where(c => c.Id == id).FirstOrDefault();
            var listCourses = _context.Course.Where(c => c.DepartmentId == id).OrderBy(c => c.Name);
            ViewBag.departmentName = _context.Department.Where(d => d.Id == id).FirstOrDefault().Name;
            ViewBag.depid = id;
            if(listCourses == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return View(await listCourses.ToListAsync());
        }

     



    }
}
