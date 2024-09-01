using Kulinarka.Interfaces;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class CookieService: ICookieService
    {
        public  const  string _loginCookie = "Credentials";
        public  void SetCookie(HttpContext httpContext,string key,string value,int yearsValid=5)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddYears(yearsValid);
            httpContext.Response.Cookies.Append(key, value, option);
        }
        public  string GetCookie(HttpContext httpContext, string key)
        {
            return httpContext.Request.Cookies[key];
        }
        public  void RemoveCookie(HttpContext httpContext, string key)
        {
            httpContext.Response.Cookies.Delete(key);
        }

        public  void SetLoginCookie(HttpContext httpContext, string username, int yearsValid=5) {
            SetCookie(httpContext, _loginCookie, username, yearsValid);
        } 

    }
}
