namespace Kulinarka.Interfaces
{
    public interface ISessionService
    {
        void SetSession<T>(string key, T value);
        T GetSession<T>( string key);
        void RemoveSession( string key);
    }
}
