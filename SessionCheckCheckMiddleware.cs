using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace maturitetna;


    public class SessionCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Check if a specific session variable exists
            if (context.Session.GetString("id") == null)
            {
                // Redirect or take appropriate action if the session is not valid
               // context.Response.Redirect("Home/Index");
                return;
            }

            // Continue with the pipeline if the session is valid
            await _next(context);
        }
        
    }

public class SessionCheckFilterHairdresser : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Check session and redirect if necessary
        if (context.HttpContext.Session.GetString("rights") == null)
        {
            context.Result = new RedirectResult("Home/Login");
        }

        if (context.HttpContext.Session.GetString("rights") == "u")
        {
            context.Result = new RedirectResult("/User/Index");
        }
        
        if (context.HttpContext.Session.GetString("rights") == "a")
        {
            context.Result = new RedirectResult("/Admin/Index");
        }
        
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Additional logic after the action executes
    }
}

public class SessionCheckFilterAdmin : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Check session and redirect if necessary
        if (context.HttpContext.Session.GetString("rights") == null)
        {
            context.Result = new RedirectResult("Home/Login");
        }

        if (context.HttpContext.Session.GetString("rights") == "u")
        {
            context.Result = new RedirectResult("/User/Index");
        }
        
        if (context.HttpContext.Session.GetString("rights") == "h")
        {
            context.Result = new RedirectResult("/Hairdresser/Index");
        }
        
        
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Additional logic after the action executes
    }
}

public class SessionCheckFilterUser : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // Check session and redirect if necessary
        if (context.HttpContext.Session.GetString("rights") == null)
        {
            context.Result = new RedirectResult("Home/Login");
        }
        
        if (context.HttpContext.Session.GetString("rights") == "a")
        {
            context.Result = new RedirectResult("/Admin/Index");
        }
        
        if (context.HttpContext.Session.GetString("rights") == "h")
        {
            context.Result = new RedirectResult("/Hairdresser/Index");
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        // Additional logic after the action executes
    }
}
