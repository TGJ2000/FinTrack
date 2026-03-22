using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTrack.Controllers
{
    public class BaseController : ControllerBase
    {
        protected int GetUserId()
        {
            return int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
    }
}