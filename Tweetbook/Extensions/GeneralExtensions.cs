using System.Linq;
using System.Net.Http;
using System.Security.Permissions;
using Microsoft.AspNetCore.Http;

namespace Tweetbook.Extensions
{
    public static class GeneralExtensions
    {
        public static string GetUserId(this HttpContext httpContext)
        {
            return httpContext == null 
                ? string.Empty 
                : httpContext.User.Claims.Single(x => x.Type == "id").Value;
        }
    }
}