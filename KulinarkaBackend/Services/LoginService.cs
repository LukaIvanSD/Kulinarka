using Kulinarka.Interfaces;
using Kulinarka.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class LoginService: ILoginService
    {
        private readonly AppDbContext dbContext;
        private readonly HttpContext context;
        private readonly ICookieService cookieService;
        private readonly ISessionService sessionService;
        public LoginService(AppDbContext dbContext,IHttpContextAccessor httpContextAccessor,ICookieService cookieService,ISessionService sessionService)
        {
            this.dbContext = dbContext;
            this.context = httpContextAccessor.HttpContext;
            this.cookieService = cookieService;
            this.sessionService = sessionService;

        }
        public async Task<User> LogInAsync(string username, string password,bool rememberMe)
        {
            User user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
            if (user == null)
                return null;
            SetUserSession(user);
            if (rememberMe)
                SetLoginCookie(user.Username);
            return user;
        }
        public async Task<bool>LogOutAsync()
        {
            if (!await IsLoggedInAsync())
                return false;
            sessionService.RemoveSession(context, SessionService.loginSession);
            cookieService.RemoveCookie(context, CookieService._loginCookie);
            return true;
        }
        private void SetUserSession(User user)
        {
            sessionService.SetSession<User>(context, SessionService.loginSession, user);
        }

        private void SetLoginCookie(string username)
        {
            cookieService.SetCookie(context, CookieService._loginCookie, username);
        }
        public async Task<bool> IsLoggedInAsync()
        {
            string cookie = cookieService.GetCookie(context, CookieService._loginCookie);
            User user= sessionService.GetSession<User>(context, SessionService.loginSession);
            if (user == null && cookie == null)
                return false;
            if (user == null)
                await LogInWithCookieAsync(cookie);
            return true;
        }
        public async Task LogInWithCookieAsync(string username)
        {
            User user = await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            sessionService.SetSession<User>(context, SessionService.loginSession,user);
        }
        public async Task<User> GetSessionAsync()
        {
            await IsLoggedInAsync();
            return sessionService.GetSession<User>(context, SessionService.loginSession);
        }

    }
}
