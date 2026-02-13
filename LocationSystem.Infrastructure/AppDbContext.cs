﻿using LocationSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LocationSystem.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Additional model configuration can go here
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            // 配置Role和Permission的多对多关系
            modelBuilder.Entity<Role>()
                .HasMany(r => r.Permissions)
                .WithMany(p => p.Roles)
                .UsingEntity(j => j.ToTable("RolePermissions"));

            // 配置User和Role的多对多关系
            modelBuilder.Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(j => j.ToTable("UserRoles"));

            // 配置Permission的自引用关系（父子权限）
            modelBuilder.Entity<Permission>()
                .HasMany(p => p.ChildPermissions)
                .WithOne(p => p.Parent)
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 配置Menu的自引用关系（父子菜单）
            modelBuilder.Entity<Menu>()
                .HasMany(m => m.Children)
                .WithOne(m => m.Parent)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 配置Permission和Menu的多对多关系
            modelBuilder.Entity<PermissionMenu>()
                .HasOne(pm => pm.Permission)
                .WithMany(p => p.PermissionMenus)
                .HasForeignKey(pm => pm.PermissionId);

            modelBuilder.Entity<PermissionMenu>()
                .HasOne(pm => pm.Menu)
                .WithMany(m => m.PermissionMenus)
                .HasForeignKey(pm => pm.MenuId);
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<RentHouse> RentHousies { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<PermissionMenu> PermissionMenus { get; set; }
    }
}
