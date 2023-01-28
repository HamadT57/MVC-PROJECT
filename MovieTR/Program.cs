using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MovieTR.Data;
using MovieTR.Models;
using System.Configuration;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MovieTDBcontext>(
        option => option.UseSqlServer(@"Data Source=DESKTOP-A0NIFMH\SQLEXPRESS; Initial Catalog=MovieT0T;Integrated Security=True;TrustServerCertificate=True;"));
builder.Services.AddCoreAdmin();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("sfdsdsasdasdadfdsfdsfdsfisdjflkdsfldsfs6556456")),
        };
    }
);
builder.Services.AddAuthorization(option => {
    option.AddPolicy("IsAdmin", op =>
    {
        op.RequireClaim("MyRole", "1");

    });

    option.AddPolicy("IsUser", op =>
    {
        op.RequireClaim("MyRole", "0");

    });

    option.AddPolicy("IsUserOrAdmin", op =>
    {
        
        op.RequireClaim("MyRole", "0", "1");

    });

    

}
);


var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.Use(async (context, next) =>
{
    var token = context.Request.Cookies["Token"];
    context.Request.Headers.Add("Authorization", "Bearer " + token);

    await next();
});
app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=MovieT}/{action=SigninView}/{id?}");
app.UseCoreAdminCustomUrl("superadmin");


app.Run();
