using Kulinarka.Interfaces;
using Kulinarka.Middleware;
using Kulinarka.Models;
using Kulinarka.Profiles;
using Kulinarka.RepositoryInterfaces;
using Kulinarka.ServiceInterfaces;
using Kulinarka.Services;
using Kulinarka.SqlDbRepository;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Adds cors filters
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

//Adds Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(option => {
    option.IdleTimeout = TimeSpan.FromMinutes(10);
    option.Cookie.HttpOnly = true;
    option.IOTimeout = TimeSpan.FromSeconds(20);
});

//Add automappers
builder.Services.AddAutoMapper(typeof(UserAchievementProfile));


// Add services to the container.
builder.Configuration.AddJsonFile("configDb.json", optional: false, reloadOnChange: true);
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICookieService, CookieService>();
builder.Services.AddScoped<ISessionService, SessionService>();
builder.Services.AddScoped<ILoginService,LoginService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRecipeService, RecipeService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();
builder.Services.AddScoped<IAchievementService, AchievementService>();
builder.Services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();
builder.Services.AddScoped<IUserAchievementService, UserAchievementService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options => {
options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"));

});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseSession();

app.UseAuthorization();

app.UseReqestLogger();

app.MapControllers();

app.Run();
