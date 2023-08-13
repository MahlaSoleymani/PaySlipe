using Entities.Infrastructure;
using Entities.Users;

namespace Entities.Companies
{
    public class Company : BaseEntity
    {
        public string CompanyName { get; set; }

        public long UserId { get; set; }
        public User User { get; set; }
    }
}
