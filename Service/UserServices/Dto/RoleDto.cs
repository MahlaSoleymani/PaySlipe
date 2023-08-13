using Entities.Users;
using Service.Infrastructure;

namespace Service.UserServices.Dto
{
    public class RoleDto : BaseDto<RoleDto , Role>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class AllUserRoleDto
    {
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public bool IsSelected { get; set; }
    }
}
