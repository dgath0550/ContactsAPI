using Microsoft.EntityFrameworkCore;

namespace ContactsAPI.Model
{
    public partial class contactsContext : DbContext
    {
        private readonly string connectionstring;
        //public contactsContext()
        //{
        //}

        public contactsContext(DbContextOptions<contactsContext> options, Globals globals)
            : base(options)
        {
            connectionstring = globals.ConnectionString; //get connection string from config file
        }

        public virtual DbSet<Address> Address { get; set; }
        public virtual DbSet<Contact> Contact { get; set; }
        public virtual DbSet<Name> Name { get; set; }
        public virtual DbSet<Phone> Phone { get; set; }
        //public virtual DbSet<PhoneType> PhoneType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(connectionstring);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.HasKey(e => e.IAddress);

                entity.HasIndex(e => e.IAddress)
                    .IsUnique();

                entity.Property(e => e.IAddress)
                    .HasColumnName("iAddress").ValueGeneratedOnAdd();

                entity.Property(e => e.City).HasColumnName("city");

                entity.Property(e => e.State).HasColumnName("state");

                entity.Property(e => e.Street).HasColumnName("street");

                entity.Property(e => e.Zip).HasColumnName("zip");
            });

            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.IAddress).HasColumnName("iAddress");

                entity.Property(e => e.IName).HasColumnName("iName");

            });

            modelBuilder.Entity<Name>(entity =>
            {
                entity.HasKey(e => e.IName);

                entity.HasIndex(e => e.IName)
                    .IsUnique();

                entity.Property(e => e.IName)
                    .HasColumnName("iName").ValueGeneratedOnAdd();

                entity.Property(e => e.First).HasColumnName("first");

                entity.Property(e => e.Last).HasColumnName("last");

                entity.Property(e => e.Middle).HasColumnName("middle");
            });

            modelBuilder.Entity<Phone>(entity =>
            {
                entity.HasKey(e => new { e.ContactId, e.type });

                entity.HasIndex(e => e.IPhone)
                    .IsUnique();

                entity.Property(e => e.ContactId).HasColumnName("ContactId");

                entity.Property(e => e.type).HasColumnName("type");

                entity.Property(e => e.IPhone).HasColumnName("iPhone").ValueGeneratedOnAdd();

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasColumnName("number");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
