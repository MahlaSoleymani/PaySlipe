using Database.Contracts;
using Entities.Companies;
using Entities.Users;

namespace Database.Repositories.RepositoryWrapper
{
    public interface IRepositoryWrapper
    {
        void Save();
        IRepository<User> User { get; }
        IRepository<UserRole> UserRole { get; }
        IRepository<Role> Role { get; }
        IRepository<Company> Company { get; }
        
    }
}