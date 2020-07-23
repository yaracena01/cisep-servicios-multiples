using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace cisep.Models
{
    public partial class cisepDBContext : DbContext
    {
        public cisepDBContext()
        {
        }

        public cisepDBContext(DbContextOptions<cisepDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Services> Services { get; set; }
        public virtual DbSet<services_detail> Services_Details { get; set; }
        public virtual DbSet<Clients> Clients { get; set; }
        public virtual DbSet<Carrier> Carrier { get; set; }
        public virtual DbSet<flex_pay> Flex_pay { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server = DESKTOP-3Q8MJDT\\SQLEXPRESS; Database= cisepDB; Trusted_connection= true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Services>(entity =>
            {
                entity.ToTable("services");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(2000)
                    .IsUnicode(false);

                entity.Property(e => e.Photo)
                    .HasColumnName("photo")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Url)         
                    .HasColumnName("url")
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.UrlName)
                   .HasColumnName("urlName")
                   .HasMaxLength(200)
                   .IsUnicode(false);

                entity.Property(e => e.Type_Services)
                   .IsRequired()
                   .HasColumnName("type_services")
                   .IsUnicode(false);

            });
            
            modelBuilder.Entity<services_detail>(entity =>
            {
                entity.ToTable("services_detail");          

                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.Id_Services)                
                    .HasColumnName("id_services")                 
                    .IsUnicode(false);


                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Price)
                 .HasColumnName("price")
                 .IsUnicode(false);

                entity.HasOne(e => e.Services)
                      .WithMany(es => es.Services_Details);
            });

            modelBuilder.Entity<Clients>(entity =>
            {
                entity.ToTable("clients");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.First_name)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.Last_name)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
               

                entity.Property(e => e.Email)
                   .IsRequired()
                   .HasColumnName("email")
                   .HasMaxLength(100)
                   .IsUnicode(false);

                entity.Property(e => e.Address)
                  .IsRequired()
                  .HasColumnName("address")
                  .HasMaxLength(500)
                  .IsUnicode(false);

                entity.Property(e => e.City)
                  .IsRequired()
                  .HasColumnName("city")
                  .HasMaxLength(100)
                  .IsUnicode(false);

                entity.Property(e => e.State)
                  .IsRequired()
                  .HasColumnName("state")
                  .HasMaxLength(100)
                  .IsUnicode(false);

                entity.Property(e => e.Zip)
                  .IsRequired()
                  .HasColumnName("zip")
                  .HasMaxLength(5)
                  .IsUnicode(false);

                entity.Property(e => e.Phone)
                  .IsRequired()
                  .HasColumnName("phone")
                  .HasMaxLength(15)
                  .IsUnicode(false);

                entity.Property(e => e.Notification)
                .IsRequired()
                .HasColumnName("notification")
                .IsUnicode(false);

            });

            modelBuilder.Entity<flex_pay>(entity =>
            {
                entity.ToTable("flex_pay");

                entity.Property(e => e.Code).HasColumnName("code");

                entity.HasKey(e => e.Code);

                entity.Property(e => e.Amount)
                    .IsRequired()
                    .HasColumnName("amount")
                    .IsUnicode(false);

            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
