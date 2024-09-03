namespace Kulinarka.Interfaces
{
    public interface ILoginService
    {
        Task<bool> IsLoggedInAsync();
        Task LogInWithCookieAsync(string username);
    }
}
