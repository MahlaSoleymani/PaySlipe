using Database.Contexts;
using Database.Contracts;
using Entities.Companies;
using Entities.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Database.Repositories.RepositoryWrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IServiceProvider _services;


        private IRepository<User> _user;
        private IRepository<UserRole> _userRole;
        private IRepository<Role> _role;

        private IRepository<Company> _company;


        public RepositoryWrapper(ApplicationDbContext dbContext, IServiceProvider services)
        {
            _dbContext = dbContext;
            _services = services;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public IRepository<User> User => _user ??= _services.GetRequiredService<IRepository<User>>();
        public IRepository<UserRole> UserRole => _userRole ??= _services.GetRequiredService<IRepository<UserRole>>();
        public IRepository<Role> Role => _role ??= _services.GetRequiredService<IRepository<Role>>();
        public IRepository<Company> Company => _company ??= _services.GetRequiredService<IRepository<Company>>();
    }
}