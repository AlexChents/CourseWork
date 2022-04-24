using CourseChentsov.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CourseChentsov.Helpers
{
    public class CourseContext : DbContext
    {
        public CourseContext() : base("CConnection") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Package>().
                HasRequired(j => j.DepartmentRecipient).
                WithMany().
                WillCascadeOnDelete(false);
            modelBuilder.Entity<Package>().
                HasRequired(j => j.DepartmentSend).
                WithMany().
                WillCascadeOnDelete(false);

            modelBuilder.Entity<DistanceDelivery>().
                    HasRequired(j => j.RegionFirst).
                    WithMany().
                    WillCascadeOnDelete(false);
            modelBuilder.Entity<DistanceDelivery>().
                HasRequired(j => j.RegionFirst).
                WithMany().
                WillCascadeOnDelete(false);
        }

        public DbSet<Region> Regions { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<DistanceDelivery> DeliveryDistances { get; set; }
        public DbSet<BasicPriceDaysDelivery> BasicPriceDaysDeliveries { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<DispatchRegister> DispatchRegisters { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<Recipient> Recipients { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<StatusDepartment> StatusDepartments { get; set; }
        public DbSet<StatusPackage> StatusPackages { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<News> News { get; set; }
    }
}
