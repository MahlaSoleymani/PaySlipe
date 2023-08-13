using Common.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebFramework.Filters;

namespace WebFramework.Api
{
    [Authorize]
    [ApiController]
    [ApiResultFilter]
    [Route("api/v{version:apiVersion}/[controller]")]// api/v1/[controller]
    // [TypeFilter(typeof(SimpleAuthorizeAttribute))]
    public class BaseController : ControllerBase
    {
        //public UserRepository UserRepository { get; set; } => property injection
        public bool UserIsAutheticated => HttpContext.User.Identity.IsAuthenticated;
        public int? UserId =>UserIsAutheticated?  int.Parse(HttpContext.User.Identity.GetUserId()):(int?) null ;
    }

    [Authorize]
    [ApiController]
    [ApiResultFilter]
    [Route("api/v{version:apiVersion}/[controller]")]//
    public class BaseCommonController : ControllerBase
    {
        //public UserRepository UserRepository { get; set; } => property injection
        public bool UserIsAutheticated => HttpContext.User.Identity.IsAuthenticated;
        public int? UserId => UserIsAutheticated ? int.Parse(HttpContext.User.Identity.GetUserId()) : (int?)null;
    }


    [ApiController]
    [ApiResultFilter]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FreeBaseController : ControllerBase
    {
        public bool UserIsAutheticated => HttpContext.User.Identity.IsAuthenticated;
        public int? UserId => UserIsAutheticated ? int.Parse(HttpContext.User.Identity.GetUserId()) : (int?)null;
    }
}
