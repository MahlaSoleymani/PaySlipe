using Entities.Users;
using Service.Infrastructure;

namespace Service.UserServices.Dto
{
    public class UserDto : BaseDto<UserDto , User>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<long> Roles { get; set; }

    }
}
