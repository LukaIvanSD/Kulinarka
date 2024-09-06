using Kulinarka.Interfaces;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class CookieService: ICookieService
    {
        public  const  string _loginCookie = "Credentials";
        private IHttpContextAccessor httpContextAccessor;
        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }
        public  void SetCookie(string key,string value,int yearsValid=5)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = DateTime.Now.AddYears(yearsValid);
            httpContextAccessor.HttpContext.Response.Cookies.Append(key, value, option);
        }
        public  string GetCookie( string key)
        {
            return httpContextAccessor.HttpContext.Request.Cookies[key];
        }
        public  void RemoveCookie(string key)
        {
            httpContextAccessor.HttpContext.Response.Cookies.Delete(key);
        }

        public  void SetLoginCookie(string username, int yearsValid=5) {
            SetCookie(_loginCookie, username, yearsValid);
        } 

    }
}
