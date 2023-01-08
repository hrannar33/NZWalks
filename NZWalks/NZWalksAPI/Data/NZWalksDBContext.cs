﻿using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Models.Domain;

namespace NZWalksAPI.Data
{
    public class NZWalksDBContext: DbContext
    {
        public NZWalksDBContext(DbContextOptions<NZWalksDBContext> options): base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.Role)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(x => x.RoleId);

            modelBuilder.Entity<User_Role>()
                .HasOne(x => x.User)
                .WithMany(y => y.UserRoles)
                .HasForeignKey(x => x.UserID);
        }

        public DbSet<Region> Regions { get; set; }

        public DbSet<Walk> Walk { get; set; }

        public DbSet<WalkDifficulty> WalkDifficulty{ get; set; }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User_Role> Users_Roles { get; set; }

    }
}
