using Kulinarka.Models;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics;
using System.Threading;

namespace Kulinarka.Middleware
{
    public class RequestLogger
    {
        private readonly RequestDelegate next;
        private List<Log> logs;
        private const int batchSize = 10;
        private int currentBatchSize = 0;
        private readonly object lockObj = new object();
        private readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private readonly IServiceScopeFactory scopeFactory;

        public RequestLogger(RequestDelegate next, IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            this.next = next;
            logs = new List<Log>();
        }

        public async Task Invoke(HttpContext context)
        {
            DateTime startTime = DateTime.UtcNow;
            await next.Invoke(context);
            TimeSpan duration = DateTime.UtcNow - startTime;
            if (context.Response.StatusCode == 401)
                return;

            Log newLog = CreateLog(context, duration);

            lock (lockObj)
            {
                logs.Add(newLog);
                currentBatchSize++;
            }

            if (currentBatchSize >= batchSize)
            {
                await SaveLogsToDbAsync();
            }
        }

        private Log CreateLog(HttpContext context, TimeSpan duration)
        {
            return new Log
            {
                Method = context.Request.Method,
                Path = context.Request.Path,
                Duration = duration.TotalMilliseconds,
                UserAgent = context.Request.Headers["User-Agent"],
                Referer = context.Request.Headers["Referer"]
            };
        }

        private async Task SaveLogsToDbAsync()
        {
            List<Log> logsToSave;

            await semaphore.WaitAsync(); // Čekanje na slobodan prolaz kroz semafor

            try
            {
                lock (lockObj)
                {
                    logsToSave = logs;
                    logs = new List<Log>();
                    currentBatchSize = 0;
                }
                using (var scope = scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    await dbContext.Logs.AddRangeAsync(logsToSave);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving logs: {ex.Message}");
            }
            finally
            {
                semaphore.Release();
            }
        }

    }
    public static class RequestLoggerExtensions
    {
        public static IApplicationBuilder UseReqestLogger(this IApplicationBuilder builder) {
            return builder.UseMiddleware<RequestLogger>();
        }
    }

}
