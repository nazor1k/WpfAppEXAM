using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfAppEXAM.Models;

namespace WpfAppEXAM.Context
{
    public class ApplicationContext:DbContext
    {
        

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) {
            
            Database.EnsureCreated();
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table>(t =>
            {
                t.HasKey(x => x.Id);

                t.Property(x => x.Seats).IsRequired();

                t.Property(x => x.Description).IsRequired().HasMaxLength(256);

            });

            modelBuilder.Entity<User>(u =>
            {
                u.HasKey(x => x.Id);

                u.HasIndex(x => x.Login).IsUnique();

                u.Property(x => x.Login).IsRequired().HasMaxLength(256);

                u.Property(x=>x.FullName).IsRequired().HasMaxLength(256);

                u.Property(x => x.PasswordHash).IsRequired().HasMaxLength(256);

                u.Property(x => x.Role).HasConversion<string>().IsRequired(); 

                
            });


            modelBuilder.Entity<Reservation>(r =>
            {
                r.HasKey(x => x.Id);

                r.Property(x => x.DateFrom).IsRequired();

                r.Property(x => x.DateTo).IsRequired();

                r.HasOne(x => x.User)
                .WithMany(x => x.Reservations)
                .HasForeignKey(x => x.UserId);
                

                r.HasOne(x=>x.Table)
                .WithMany(x=>x.Reservations)
                .HasForeignKey(x=>x.TableId);

            });
        }


        public DbSet<Table> Tables { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
