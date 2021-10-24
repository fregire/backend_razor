using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BadNews.Elevation
{
    public class ElevationMiddleware
    {
        private readonly RequestDelegate next;
    
        public ElevationMiddleware(RequestDelegate next)
        {
            this.next = next;
        }
    
        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path == "/elevation")
            {
                var queries = context.Request.Query;

                if (queries.Count == 0)
                {
                    context.Response.Cookies.Delete(ElevationConstants.CookieName);
                    context.Response.Redirect("/");
                    return;
                }

                if (context.Request.Cookies.ContainsKey(ElevationConstants.CookieName))
                {
                    context.Response.Redirect("/");
                    return;
                }
                
                foreach (var query in queries)
                {
                    if (query.Key == "up")
                    {
                        context.Response.Cookies.Append(
                            ElevationConstants.CookieName, 
                            ElevationConstants.CookieValue,
                            new CookieOptions
                            {
                                HttpOnly = true
                            });
                        context.Response.Redirect("/");
                        return;
                    }
                }

                context.Response.Cookies.Delete(ElevationConstants.CookieName);
                context.Response.Redirect("/");
            }

            await next(context);
        }
    }
}
