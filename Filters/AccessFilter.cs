using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BankAccounts.Filters
{
    public class AccessFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            int? userId = context.HttpContext.Session.GetInt32("UserId");
            var parametro = context.HttpContext.Request.Path.Value;
            var result = parametro.Split("/");

            if (result.Length >= 2)
            {
                if (int.TryParse(result[2], out int resultint) && userId != null)
                {
                    if (resultint != userId)
                    {
                        context.Result = new RedirectToActionResult(
                            "Index",
                            "Accounts",
                            new { id = userId }
                        );
                    }
                }
            }
        }
    }
}
