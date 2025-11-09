using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.Entities;
using Repositories.FluentAPIs;

namespace Repositories.DBContext
{
    public class Co_ownershipAndCost_sharingDbContext : DbContext
    {
        public Co_ownershipAndCost_sharingDbContext(DbContextOptions<Co_ownershipAndCost_sharingDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<CarDetail> CarDetails { get; set; }
        public DbSet<CarUser> CarUsers { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Form> Forms { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PercentOwnership> PercentOwnership { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<Maintenance> Maintenances { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CarConfiguration());
            modelBuilder.ApplyConfiguration(new CarDetailConfiguration());
            modelBuilder.ApplyConfiguration(new CarUserConfiguration());
            modelBuilder.ApplyConfiguration(new ContractConfiguration());
            modelBuilder.ApplyConfiguration(new FormConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new PaymentConfiguration());
            modelBuilder.ApplyConfiguration(new PercentOwnershipConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserGroupConfiguration());
            modelBuilder.ApplyConfiguration(new VoteConfiguration());
            modelBuilder.ApplyConfiguration(new MaintenanceConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
