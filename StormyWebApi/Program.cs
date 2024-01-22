using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StormyLib.Contexts;
using StormyLib.DbInteraction;
using StormyLib.Models;
using StormyWebApi.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add Authentication
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder();

// Add DbContexts
builder.Services.AddDbContext<BloggingContext>(options =>
    options.UseSqlite($"DataSource={builder.Configuration.GetConnectionString("BlogDb")}"));
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite($"DataSource={builder.Configuration.GetConnectionString("AuthDb")}"));

// Add services to the container.
builder.Services.AddScoped<IDbInteraction<Blog>, BlogDbInteraction>();
builder.Services.AddScoped<IDbInteraction<Post>, PostDbInteraction>();

// Add Authentication
builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddApiEndpoints();

// Add controllers.
builder.Services.AddControllers();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo {
        Version = "v1",
        Title = "Stormy Web Api"
        });
    });

var app = builder.Build();

app.MapIdentityApi<User>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => 
        {
          c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stormy Web Api v1");
        });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
