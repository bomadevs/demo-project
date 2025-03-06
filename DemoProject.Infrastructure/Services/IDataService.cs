using DemoProject.Domain.Entities;

namespace DemoProject.Infrastructure.Services
{
    /// <summary>
    /// Defines methods for retrieving and managing company data.
    /// This interface supports fetching both free and premium companies with filtering,
    /// along with verification data.
    /// </summary>
    public interface IDataService
    {
        /// <summary>
        /// Retrieves a list of free service companies with filtering by company id.
        /// Data is cached to optimize performance.
        /// </summary>
        Task<List<FreeServiceCompany>> GetFreeCompaniesByIdAsync(string? query, bool? isActive = null);

        /// <summary>
        /// Retrieves a list of premium service companies with filtering by company id.
        /// Data is cached to optimize performance.
        /// </summary>
        Task<List<PremiumServiceCompany>> GetPremiumCompaniesByIdAsync(string? query, bool? isActive = null);

        /// <summary>
        /// Seeds the mocked DB with initial data from JSON files...
        /// </summary>
        Task SeedDataAsync();

        /// <summary>
        /// Retrieves verification data based on verificationId from DB.
        /// Could be another service also...
        /// </summary>
        Task<VerificationData> GetVerificationByIdAsync(Guid verificationId);

        /// <summary>
        /// Store verification data in DB.
        /// Just return simple true if data is stored successfully...
        /// </summary>
        Task<bool> StoreVerificationDataAsync(VerificationData verificationData);
    }
}
