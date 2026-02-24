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
        public DbSet<Job> Jobs { get; set; }
        public DbSet<GridLayout> GridLayouts { get; set; }
        public DbSet<Reportdata> Reportdata { get; set; }
        public DbSet<EinvConfig> EinvConfigs { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<UserLoginSession> UserLoginSessions { get; set; }

        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<BillRefDetail> BillRefDetails { get; set; }

        public DbSet<CompanySubscription> CompanySubscriptions { get; set; }

        //added by dhruv
        public DbSet<WpMailConfig> wpMailConfigs { get; set; }
        public DbSet<SendWpMail> SendWpMails { get; set; }
        public DbSet<UserBranch> UserBranches { get; set; }
        public DbSet<Lr> Lrs { get; set; }
        public DbSet<DocumentsSaved> DocumentsSaved { get; set; }

        public DbSet<LRDetail> LRDetails { get; set; } 
        public DbSet<LRJournal> LRJournals { get; set; }
        //end dhruv
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(u => u.UserEmail).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.UserMobile).IsUnique();
            modelBuilder.Entity<Bill>().Ignore(b => b.partyname);
            modelBuilder.Entity<Bill>().Ignore(b => b.Vouchname);
            modelBuilder.Entity<Bill>().Ignore(b => b.posname);
            modelBuilder.Entity<Bill>().Ignore(b => b.branchname);
            modelBuilder.Entity<Cargo>().Ignore(b => b.cargohsnname);
            modelBuilder.Entity<Bill>()
        .HasOne(b => b.Party)
        .WithMany() // or .WithMany(a => a.Bills) if Account has a collection
        .HasForeignKey(b => b.BillPartyId)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.PlaceOfSupply)
                .WithMany() // or .WithMany(s => s.Bills)
                .HasForeignKey(b => b.BillPlaceOfSupply)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Voucher)
                .WithMany() // or .WithMany(s => s.Bills)
                .HasForeignKey(b => b.BillVoucherId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Bill>()
               .HasOne(b => b.branch)
               .WithMany() // or .WithMany(s => s.Bills)
               .HasForeignKey(b => b.BillBranchId)
               .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    }
}
