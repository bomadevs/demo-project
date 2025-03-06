using DemoProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DemoProject.Infrastructure.MockDB
{
    /// <summary>
    /// Simulated database context for in-memory storage.
    /// This context is used to mock a database
    /// </summary>
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Represents a collection of free service companies, simulating a database table...
        /// </summary>
        public DbSet<FreeServiceCompany> FreeServiceCompanies { get; set; }

        /// <summary>
        /// Represents a collection of premium service companies, simulating a database table...
        /// </summary>
        public DbSet<PremiumServiceCompany> PremiumServiceCompanies { get; set; }

        /// <summary>
        /// Represents a collection of verification data, simulating a database table...
        /// </summary>
        public DbSet<VerificationData> Verifications { get; set; }
    }
}
