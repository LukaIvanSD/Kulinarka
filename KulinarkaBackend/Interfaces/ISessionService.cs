namespace Kulinarka.Interfaces
{
    public interface ISessionService
    {
        void SetSession<T>(HttpContext httpContext, string key, T value);
        T GetSession<T>(HttpContext httpContext, string key);
        void RemoveSession(HttpContext httpContext, string key);
    }
}
