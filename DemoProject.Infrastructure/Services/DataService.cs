using DemoProject.Domain.Entities;
using DemoProject.Infrastructure.MockDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using System.Xml.Linq;

namespace DemoProject.Infrastructure.Services
{
    public class DataService : IDataService
    {
        /// <summary>
        /// Our mocked database context.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Memory cache for storing data. Simulates a real distributed cache.
        /// </summary>
        private readonly IMemoryCache _cache;

        /// <summary>
        /// Cache expiration time (TTL).
        private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

        public DataService(AppDbContext dbContext, IMemoryCache cache)
        {
            _dbContext = dbContext;
            _cache = cache;
        }

        /// <inheritdoc />
        public async Task<List<FreeServiceCompany>> GetFreeCompaniesByIdAsync(string? query, bool? isActive = null)
        {
            var cacheKey = $"free_companies_{query}_{isActive?.ToString()}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;

                var q = _dbContext.FreeServiceCompanies.AsQueryable();

                if (!string.IsNullOrEmpty(query))
                    q = q.Where(c => c.CIN.Contains(query, StringComparison.OrdinalIgnoreCase));

                // apply filter if isActive is provided...
                if (isActive.HasValue)
                    q = q.Where(c => c.IsActive == isActive.Value);
                
                return await q.ToListAsync();
            }) ?? new List<FreeServiceCompany>();
        }

        /// <inheritdoc />
        public async Task<List<PremiumServiceCompany>> GetPremiumCompaniesByIdAsync(string? query, bool? isActive = null)
        {
            var cacheKey = $"premium_companies_{query}_{isActive?.ToString()}";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _cacheDuration;

                var q = _dbContext.PremiumServiceCompanies.AsQueryable();

                if (!string.IsNullOrEmpty(query))
                    q = q.Where(c => c.CompanyIdentificationNumber.Contains(query, StringComparison.OrdinalIgnoreCase));

                // apply filter if isActive is provided...
                if (isActive.HasValue)
                    q = q.Where(c => c.IsActive == isActive.Value);

                return await q.ToListAsync();
            }) ?? new List<PremiumServiceCompany>();
        }

        /// <inheritdoc />
        public Task<VerificationData> GetVerificationByIdAsync(Guid verificationId)
        {
            // we will not use cache for this type of data...
            // let's assume that this data is not frequently accessed,
            // so we can save some memory by not caching it
            return _dbContext.Verifications.FirstOrDefaultAsync(v => v.VerificationId == verificationId);
        }

        /// <inheritdoc />
        public async Task SeedDataAsync()
        {
            if (!_dbContext.FreeServiceCompanies.Any())
            {
                var freeJson = File.ReadAllText("Resources/free_service_companies.json");
                var freeCompanies = JsonSerializer.Deserialize<List<FreeServiceCompany>>(freeJson);

                if (freeCompanies != null)
                {
                    _dbContext.FreeServiceCompanies.AddRange(freeCompanies);
                    await _dbContext.SaveChangesAsync();
                }
            }

            if (!_dbContext.PremiumServiceCompanies.Any())
            {
                var premiumJson = File.ReadAllText("Resources/premium_service_companies.json");
                var premiumCompanies = JsonSerializer.Deserialize<List<PremiumServiceCompany>>(premiumJson);

                if (premiumCompanies != null)
                {
                    _dbContext.PremiumServiceCompanies.AddRange(premiumCompanies);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }

        /// <inheritdoc />
        public async Task<bool> StoreVerificationDataAsync(VerificationData verificationData)
        {
            if (verificationData == null)
                return false;

            try
            {
                await _dbContext.Verifications.AddAsync(verificationData);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
