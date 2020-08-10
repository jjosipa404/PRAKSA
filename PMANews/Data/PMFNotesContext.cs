using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PMANews.Areas.Identity.Data;

namespace PMANews.Data
{
    public class PMFNotesContext : IdentityDbContext
    {
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<ApplicationRole> ApplicationRole { get; set; }
        public DbSet<Post> Post { get; set; }
        public DbSet<PostImage> PostImage { get; set; }
        public DbSet<PostFile> PostFile { get; set; }
        public DbSet<Comment> Comment { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseApplicationUser> CourseApplicationUser { get; set; }
        public DbSet<Department> Department { get; set; }

        public PMFNotesContext(DbContextOptions<PMFNotesContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            //-- one to many relationships ------------------
            modelBuilder.Entity<Post>()
                .HasOne(p => p.Author)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.AuthorId);

            modelBuilder.Entity<Post>()
                .HasOne(p => p.Course)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CourseId);

            modelBuilder.Entity<Post>()
                .HasMany(p => p.Comments)
                .WithOne(c => c.Post)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostImage>()
                .HasOne(p => p.Author)
                .WithMany(u => u.PostImages)
                .HasForeignKey(p => p.AuthorId);

            modelBuilder.Entity<PostImage>()
                .HasOne(p => p.Category)
                .WithMany(c => c.PostImages)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<PostImage>()
                .HasOne(p => p.Course)
                .WithMany(c => c.PostImages)
                .HasForeignKey(p => p.CourseId);

            modelBuilder.Entity<PostFile>()
                .HasOne(p => p.Author)
                .WithMany(u => u.PostFiles)
                .HasForeignKey(p => p.AuthorId);

            modelBuilder.Entity<PostFile>()
                .HasOne(p => p.Category)
                .WithMany(c => c.PostFiles)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<PostFile>()
                .HasOne(p => p.Course)
                .WithMany(c => c.PostFiles)
                .HasForeignKey(p => p.CourseId);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
               .HasOne(c => c.Post)
               .WithMany(u => u.Comments)
               .HasForeignKey(c => c.PostId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(u => u.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Course>()
                .HasOne(c => c.Department)
                .WithMany(d => d.Courses)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourseApplicationUser>()
                .HasOne(cu => cu.ApplicationUser)
                .WithMany(u => u.Courses)
                .HasForeignKey(cu => cu.ApplicationUserId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<CourseApplicationUser>()
               .HasOne(cu => cu.Course)
               .WithMany(u => u.ApplicationUsers)
               .HasForeignKey(cu => cu.CourseId)
               .OnDelete(DeleteBehavior.NoAction);

            //---many-many relationships -------------------

            // modelBuilder.Entity<CourseApplicationUser>().HasKey(cu => new { cu.ApplicationUserId, cu.CourseId });
            // modelBuilder.Entity<CourseCategory>().HasKey(cc => new { cc.CourseId, cc.CategoryId });

            //--- default values ---------------------------
            modelBuilder.Entity<Comment>()
              .Property(c => c.DateCreated)
              .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Post>()
                .Property(p => p.DateCreated)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<PostImage>()
                .Property(p => p.DateCreated)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<PostFile>()
                .Property(p => p.DateCreated)
                .HasDefaultValueSql("getdate()");
            //-----------------------------------------------

        }
    }
}
