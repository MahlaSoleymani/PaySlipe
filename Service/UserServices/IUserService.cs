
using Service.Infrastructure;
using Service.UserServices.Dto;
using Service.UserServices.Paging;

namespace Service.UserServices
{
    public interface IUserService
    {
        Task<AccessToken> Login(LoginDto dto);
        Task<AccessToken> Token(LoginDto dto);

        Task AddUser(UserDto dto);
        // Task AddRole(RoleDto dro);
        Task EditUser(UserDto dto);
        // Task EditRole(RoleDto dto);
        void DeleteUser(int id);
        GetUserDto GetUser(int id);
        (IList<UserListDto> , int ) GetUsers(UserPaginate paginate);
        IList<RoleDto> GetRoles();
    }
}
