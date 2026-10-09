using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MonProjetAirlplane.Models;

namespace MonProjetAirlplane.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Market> Markets { get; set; } 
        public DbSet<MarketDetails> MarketDetails { get; set; } 
        public DbSet<Flight> Flights { get; set; }
        public DbSet<UserViewModel> Users { get; set; }
        public new DbSet<SupplierViewModel> Suppliers { get; set; }
        public DbSet<ContractViewModel> Contracts { get; set; }
        public DbSet<DelayViewModel> Delays { get; set; }
        public DbSet<ArticleViewModel> Articles { get; set; }
        public DbSet<Alert> Alerts { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuration de la relation entre MarketDetails et Market
            modelBuilder.Entity<MarketDetails>()
                .HasOne(md => md.Market)
                .WithMany(m => m.MarketDetails)
                .HasForeignKey(md => md.EscaleDKey)
                .HasPrincipalKey(m => m.Escale_D);
        }
    } // <-- L'accolade de fermeture de la classe doit être ici, à la toute fin !
}