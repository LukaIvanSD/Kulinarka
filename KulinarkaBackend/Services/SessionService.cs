using Kulinarka.Interfaces;
using Newtonsoft.Json;

namespace Kulinarka.Services
{
    public class SessionService : ISessionService
    {
        public const string loginSession = "User";
        public  void SetSession<T>(HttpContext httpContext, string key, T value)
        {
            var jsonString = JsonConvert.SerializeObject(value);
            httpContext.Session.SetString(key, jsonString);
        }
        public  T GetSession<T>(HttpContext httpContext, string key)
        {
            var jsonString = httpContext.Session.GetString(key);
            return jsonString != null ? JsonConvert.DeserializeObject<T>(jsonString) : default;
        }

        public  void RemoveSession(HttpContext httpContext, string key)
        {
            httpContext.Session.Remove(key);
        }
    }
}
