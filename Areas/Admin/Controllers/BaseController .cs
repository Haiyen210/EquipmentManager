using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace EquipmentManager.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]
    public class BaseController : Controller, IActionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var log = context.HttpContext.Session.GetString("AdminLogin");
            if (context.HttpContext.Session.GetString("AdminLogin") == null)
            {
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { Controller = "Login", Action = "Index", Areas = "Admin" })
                );
            }
            base.OnActionExecuting(context);
        }

    }
}
