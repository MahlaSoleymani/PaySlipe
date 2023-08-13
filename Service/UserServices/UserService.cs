using AutoMapper;
using Common.Exceptions;
using Common.Utilities.Paging;
using Database.Repositories.RepositoryWrapper;
using Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Infrastructure;
using Service.Infrastructure.Services;
using Service.UserServices.Dto;
using Service.UserServices.Paging;

namespace Service.UserServices
{
    public class UserService: IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IJwtService _jwtService;
        private readonly IRepositoryWrapper _repository;

        public UserService(IMapper mapper, IJwtService jwtService, UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, IRepositoryWrapper repository)
        {
            _mapper = mapper;
            _jwtService = jwtService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _repository = repository;
        }

        public async Task<AccessToken> Login(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            if (user == null)
                throw new BadRequestException("نام کاربری یافت نشد");

            if (!user.IsActive )
                throw new BadRequestException("حساب کاربری شما غیرفعال شده است.");

            var checkPassResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);

            if (!checkPassResult.Succeeded)
                throw new BadRequestException("اطلاعات کاربری نادرست است.");

            user.LastLoginDate = DateTime.Now;
            await _userManager.UpdateAsync(user);

            var jwtResult = await _jwtService.GenerateAsync(user);

            return jwtResult;
        }

        public async Task<AccessToken> Token(LoginDto dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);

            if (user == null)
                throw new BadRequestException("اطلاعات کاربری نادرست است");

            var checkPassResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!checkPassResult.Succeeded)
                throw new BadRequestException("اطلاعات کاربری نادرست است.");

            var jwtResult = await _jwtService.GenerateAsync(user);

            return jwtResult;
        }

        public async Task AddUser(UserDto dto)
        {
            var exitUser = await _userManager.FindByNameAsync(dto.UserName);
            if (exitUser != null)
                throw new BadRequestException("نام کاربری تکراری می باشد");

            var userModel = dto.ToEntity(_mapper);
            userModel.IsActive = true;
            userModel.PhoneNumberConfirmed = true;

            var resultAddUser = await _userManager.CreateAsync(userModel, dto.Password);
            if (!resultAddUser.Succeeded)
                throw new LogicException("عملیات با خطا مواجه شد", null, resultAddUser.Errors);

            if (dto.Roles != null && dto.Roles.Any())
            {
                foreach (var id in dto.Roles)
                {
                    var roleSelected = await _roleManager.FindByIdAsync(id.ToString());
                    var addRole = await _userManager.AddToRoleAsync(userModel, roleSelected.Name);
                    if (!addRole.Succeeded)
                        throw new LogicException("عملیات با خطا مواجه شد", null, addRole.Errors);
                }
            }
        }



        public async Task EditUser(UserDto dto)
        {
            var currentUser = await _userManager.FindByIdAsync(dto.Id.ToString());

            var user = dto.ToEntity(_mapper, currentUser);

            await _userManager.UpdateAsync(user);

            await EditRole(user, dto.Roles);

            _repository.Save();
        }

        public async Task EditRole(User user, List<long> newRoleIds)
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var newRoles = _repository.Role.TableNoTracking
                .Where(x => newRoleIds.Contains(x.Id))
                .Select(x => x.Name)
                .ToList();
            await _userManager.AddToRolesAsync(user, newRoles);
        }

        //
        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public GetUserDto GetUser(int id)
        {
            var user = _repository.User.TableNoTracking
                .Include(x => x.UserRoles)
                .ThenInclude(x => x.Role)
                .Single(x => x.Id == id);

            var allRoles = GetRoles();
            var userRoles = allRoles
                .Select(role => new AllUserRoleDto()
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    RoleDescription = role.Description,
                    IsSelected = user.UserRoles.Any(x => x.RoleId == role.Id)
                }).ToList();

            var dto = new GetUserDto
            {
                FullName = user.FullName,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                UserRoles = userRoles
            };

            return dto;
        }

        public (IList<UserListDto>, int) GetUsers(UserPaginate paginate)
        {
            var model = _repository.User.TableNoTracking;
            (model, var count) = UserPaginate.GetPaginatedList(paginate, model);

            return (UserListDto.FromEntities(_mapper, model.ToList()), count);
        }

        public IList<RoleDto> GetRoles()
        {
            var roles = _repository.Role.TableNoTracking.ToList();

            return RoleDto.FromEntities(_mapper, roles);
        }
    }
}
