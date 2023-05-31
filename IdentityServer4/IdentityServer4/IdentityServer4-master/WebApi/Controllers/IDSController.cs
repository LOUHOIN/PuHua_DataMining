using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("identity")]
    [Authorize("Api_Scope")]
    public class IDSController : ControllerBase
    {
        [Authorize]
        public IActionResult Get()
        {
            return new JsonResult(
                from claim in User.Claims select new { claim.Type, claim.Value });
            
        }
    }
}