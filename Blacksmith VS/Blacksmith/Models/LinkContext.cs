using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blacksmith.Models
{
//    public class LinkContext : IdentityDbContext<User>
//    {
//        public LinkContext() : base("Blacksmith")
//        {
//            // http://stackoverflow.com/questions/28531201/entitytype-identityuserlogin-has-no-key-defined-define-the-key-for-this-entit
//            Database.SetInitializer<LinkContext>(null);
//            Configuration.LazyLoadingEnabled = false;
//            Configuration.ProxyCreationEnabled = false;
//        }
//
//        public static LinkContext Create()
//        {
//            return new LinkContext();
//        }
//        
//        //        public DbSet<User> Users { get; set; }
//        //        public DbSet<Role> Roles { get; set; }  
//        public DbSet<Link> Links { get; set; }
//        public DbSet<Comment> Comments { get; set; }
//
//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            base.OnModelCreating(modelBuilder);
//            
//            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
//            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
//
//            // Configure Asp Net Identity Tables
//            modelBuilder.Entity<User>().ToTable("User");
//            modelBuilder.Entity<User>().Property(u => u.PasswordHash).HasMaxLength(500);
//            modelBuilder.Entity<User>().Property(u => u.SecurityStamp).HasMaxLength(500);
//            modelBuilder.Entity<User>().Property(u => u.PhoneNumber).HasMaxLength(50);
//        }
//
//        public DbQuery<T> Query<T>() where T : class
//        {
//            return Set<T>().AsNoTracking();
//        } 
//
//    }

    public class LinkContext : DbContext
    {
        public LinkContext() : base("Blacksmith")
        {
            
        }

        public DbSet<Link> Links { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}