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

    }
}
