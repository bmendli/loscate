using Loscate.DTO.Firebase;
using Loscate.Site.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Loscate.Site.Controllers.User
{
    [Authorize]
    [Route("api/user/[controller]")]
    public class GetFirebaseUserController : Controller
    {
        [HttpGet]
        public FirebaseUser Get()
        {
            return User.ToFirebaseUser();
        }
    }
}