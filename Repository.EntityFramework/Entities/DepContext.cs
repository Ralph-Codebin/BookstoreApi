using Application.Factories.Abstractions;
using Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace Repository.EntityFramework.Entities
{
    public partial class DepContext : DbContext
    {
        private readonly Func<IValueObjectFactory> _valueObjectFactory;

        public DepContext(
            DbContextOptions<DepContext> options,
            Func<IValueObjectFactory> valueObjectFactory) : base(options)
        {
            _valueObjectFactory = valueObjectFactory;
        }        

        public virtual DbSet<DProductData> ProductData { get; set; }
        public virtual DbSet<UserData> UserData { get; set; }
        public virtual DbSet<DSubscriptionData> DSubscriptionData { get; set; }
        public virtual DbSet<SubscriptionData> SubscriptionData { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubscriptionData>(entity =>
            {
                entity.Property(e => e.id).IsUnicode(false);
            });

            modelBuilder.Entity<DSubscriptionData>(entity =>
            {
                entity.Property(e => e.id).IsUnicode(false);
            });

            modelBuilder.Entity<DProductData>(entity =>
            {                
                entity.Property(e => e.id).IsUnicode(false);
            });

            modelBuilder.Entity<UserData>(entity =>
            {
                entity.Property(e => e.id).IsUnicode(false);


            });
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
