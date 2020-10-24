﻿using BillChopBE.DataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace BillChopBE.DataAccessLayer
{
    public class BillChopContext : DbContext
    {
        public BillChopContext() : base()
        {
        }

        public BillChopContext(DbContextOptions<BillChopContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<Bill> Bills => Set<Bill>();

        protected override void OnConfiguring(DbContextOptionsBuilder options) 
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer(ConnectionStringResolver.GetBillChopDbConnectionString());
            }

            base.OnConfiguring(options);
        }

        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            base.OnModelCreating(modelbuilder);

            modelbuilder.Entity<User>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelbuilder.Entity<User>()
                .HasMany(u => u.Expenses)
                .WithOne(e => e.Loanee)
                .OnDelete(DeleteBehavior.NoAction);

            modelbuilder.Entity<User>()
                .HasMany(u => u.Bills)
                .WithOne(b => b.Loaner)
                .OnDelete(DeleteBehavior.NoAction);

            modelbuilder.Entity<Group>()
                .HasMany(g => g.Users)
                .WithMany(u => u.Groups);

            modelbuilder.Entity<Group>()
                .HasMany(g => g.Bills)
                .WithOne(b => b.GroupContext);

            modelbuilder.Entity<Bill>()
                .HasMany(b => b.Expenses)
                .WithOne(e => e.Bill)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
