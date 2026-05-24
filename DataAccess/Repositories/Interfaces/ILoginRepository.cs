using Data.Entities;

namespace DataAccess.Repositories.Interfaces
{
    public interface ILoginRepository
    {
        Task<Profile?> GetProfileByCredentialsAsync(string email, string password);
    }
}
