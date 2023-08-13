using Entities.Users;
using Service.Infrastructure;

namespace Service.UserServices.Dto
{
    public class GetUserDto : BaseDto<GetUserDto , User>
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsSuspended { get; set; }
        public List<AllUserRoleDto> UserRoles { get; set; }

    }
}
