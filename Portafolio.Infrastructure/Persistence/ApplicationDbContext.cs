using Microsoft.EntityFrameworkCore;
using Portafolio.Infrastructure.Persistence.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portafolio.Infrastructure.Persistence
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<PortfolioProfile> Profiles => Set<PortfolioProfile>();
        public DbSet<PortfolioProject> Projects => Set<PortfolioProject>();
        public DbSet<Technology> Technologies => Set<Technology>();
        public DbSet<ProjectTechnology> ProjectTechnologies => Set<ProjectTechnology>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            modelBuilder.ApplyAuditColumnOrder();
        }
    }
}
