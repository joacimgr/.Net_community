using DistroLabCommunity.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistroLabCommunity.Data {
    /// <summary>
    /// This is the DBContext for users and messages
    /// </summary>
    public class CommunityContext : DbContext{

        public CommunityContext(DbContextOptions<CommunityContext> options)
            : base(options) {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }

        /// <summary>
        /// Default values set for Messages and Users
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Message>()
                .Property(b => b.Opened)
                .HasDefaultValue(false);
            modelBuilder.Entity<Message>()
                .Property(b => b.Removed)
                .HasDefaultValue(false);
            modelBuilder.Entity<Message>()
                .Property(b => b.TimeStamp)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<User>()
                .Property(b => b.Created)
                .HasDefaultValueSql("getdate()");
            modelBuilder.Entity<UserLogin>()
                .Property(b => b.Login)
                .HasDefaultValueSql("getdate()");
        }
    }
}
