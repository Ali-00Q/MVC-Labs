using CourseLab.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace CourseLab.DAL.Database
{
    public class CourseLabDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public CourseLabDbContext(DbContextOptions<CourseLabDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}
