using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

public class Authorized_Access_Only : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var httpContext = context.HttpContext;

        if (httpContext.Session.GetString("usr") == null)
        {
            context.Result = new RedirectToActionResult("Index", "Login", null);
        }
    }
}
