using Kulinarka.Interfaces;
using Kulinarka.Models;
using Kulinarka.Models.Responses;
using Kulinarka.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Kulinarka.Services
{
    public class LoginService: ILoginService
    {
        private readonly ICookieService cookieService;
        private readonly ISessionService sessionService;
        private readonly IUserService userService;
        public LoginService(ICookieService cookieService,ISessionService sessionService,IUserService userService)
        {
            this.cookieService = cookieService;
            this.sessionService = sessionService;
            this.userService = userService;

        }
        public async Task<Response<User>> LogInAsync(string username, string password,bool rememberMe)
        {
            var result = await userService.GetByUsernameAsync(username);
            if (!result.IsSuccess || result.Data.Password!=password)
                return Response<User>.Failure("Incorrect username or password",StatusCode.Unauthorized);
            SetUserSession(result.Data);
            if (rememberMe)
                SetLoginCookie(result.Data.Username);
            return Response<User>.Success(result.Data,StatusCode.OK);
        }
        public async Task<Response<User>>LogOutAsync()
        {
            var result = await GetSessionAsync();
            if (!result.IsSuccess)
                return result;
            sessionService.RemoveSession( SessionService.loginSession);
            cookieService.RemoveCookie( CookieService._loginCookie);
            return result;
        }
        private void SetUserSession(User user)
        {
            sessionService.SetSession<User>(SessionService.loginSession, user);
        }

        private void SetLoginCookie(string username)
        {
            cookieService.SetCookie( CookieService._loginCookie, username);
        }
        public async Task<Response<User>> GetSessionAsync()
        {
            string cookie = cookieService.GetCookie( CookieService._loginCookie);
            User user= sessionService.GetSession<User>( SessionService.loginSession);
            if (user == null && cookie == null)
                return Response<User>.Failure("Not logged in",StatusCode.Unauthorized);
            if (user != null)
                return Response<User>.Success(user, StatusCode.OK);
            return await LogInWithCookieAsync(cookie);
  
        }
        public async Task<Response<User>> LogInWithCookieAsync(string username)
        {
            var result = await userService.GetByUsernameAsync(username);
            if (!result.IsSuccess)
                return result;
            SetUserSession(result.Data);
            return Response<User>.Success(result.Data, StatusCode.OK);
        }

    }
}
