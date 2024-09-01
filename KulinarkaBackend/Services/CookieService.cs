namespace Kulinarka.Services
{
    public class CookieService
    {
        public  const  string _loginCookie = "Credentials";
        public static void SetCookie(HttpContext httpContext,string key,string value,int? daysValid)
        {
            CookieOptions option = new CookieOptions();
            option.Expires = daysValid.HasValue ? DateTime.Now.AddDays(daysValid.Value) : DateTime.Now.AddYears(5);
             httpContext.Response.Cookies.Append(key, value, option);
        }
        public static string GetCookie(HttpContext httpContext, string key)
        {
            return httpContext.Request.Cookies[key];
        }
        public static void RemoveCookie(HttpContext httpContext, string key)
        {
            httpContext.Response.Cookies.Delete(key);
        }


    }
}
