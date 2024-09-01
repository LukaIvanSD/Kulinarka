namespace Kulinarka.Interfaces
{
    public interface ICookieService
    {
        void SetCookie(HttpContext httpContext, string key, string value, int yearsValid=5);
        string GetCookie(HttpContext httpContext, string key);
        void RemoveCookie(HttpContext httpContext, string key);
        void SetLoginCookie(HttpContext httpContext, string username, int yearsValid=5);
    }
}
