using Microsoft.EntityFrameworkCore;
using FreightBKShipping.Models;

namespace FreightBKShipping.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Year> Years { get; set; }
        public DbSet<Company> companies { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<ServiceGroup> ServiceGroups { get; set; }
        public DbSet<PayType> PayTypes { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<HsnSac> HsnSacs { get; set; }
        public DbSet<GstSlab> GstSlabs { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Cargo> Cargoes { get; set; }
        public DbSet<Branch> Branches { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountGroup> AccountGroups { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<VoucherDetail> VoucherDetails { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Bill> Bills { get; set; }
        public DbSet<BillDetail> BillDetails { get; set; }
        // public DbSet<BillRefDetail> BillRefDetails { get; set; }
        public DbSet<Notify> Notifies { get; set; }
        public DbSet<Vessel> Vessels { get; set; }
        public DbSet<RateMaster> RateMasters { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.UserEmail).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.UserMobile).IsUnique();
        }
    }
}
