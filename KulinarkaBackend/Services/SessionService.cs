using Kulinarka.Interfaces;
using Newtonsoft.Json;

namespace Kulinarka.Services
{
    public class SessionService : ISessionService
    {
        public const string loginSession = "User";
        private IHttpContextAccessor httpContextAccessor;
        public SessionService(IHttpContextAccessor httpContextAccessor) {
            this.httpContextAccessor = httpContextAccessor;
        }
        public  void SetSession<T>( string key, T value)
        {
            var jsonString = JsonConvert.SerializeObject(value);
            httpContextAccessor.HttpContext.Session.SetString(key, jsonString);
        }
        public  T GetSession<T>( string key)
        {
            var jsonString = httpContextAccessor.HttpContext.Session.GetString(key);
            return jsonString != null ? JsonConvert.DeserializeObject<T>(jsonString) : default;
        }

        public  void RemoveSession( string key)
        {
            httpContextAccessor.HttpContext.Session.Remove(key);
        }
    }
}
