using Data.Entities;

namespace DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Handles database operations for the ProjectTask entity.
    /// </summary>
    public interface IProjectTaskRepository
    {
        /// <summary>
        /// Saves a new task into the database.
        /// </summary>
        Task<Guid> Create(ProjectTask task);
    }
}
