namespace Kulinarka.Interfaces
{
    public interface ICookieService
    {
        void SetCookie( string key, string value, int yearsValid=5);
        string GetCookie( string key);
        void RemoveCookie( string key);
        void SetLoginCookie( string username, int yearsValid=5);
    }
}
