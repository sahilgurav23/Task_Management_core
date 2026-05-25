using Data.Entities;
using DataAccess.Context;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    /// <summary>
    /// Implementation of IProfileRepository using Entity Framework Core.
    /// </summary>
    public class ProfileRepository : IProfileRepository
    {
        private readonly ApplicationDbContext context;

        public ProfileRepository(ApplicationDbContext Context)
        {
            context = Context;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileRepository"/>.
        /// </summary>
        public async Task<Profile> GetById(Guid id)
        {
            return await context.Profiles.Where(p => p.Id == id).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Updates the profile entity and commits changes to the database.
        /// </summary>
        public async Task UpdateProfile(Profile profile)
        {
            context.Profiles.Update(profile);
            await context.SaveChangesAsync();
        }

    }
}
