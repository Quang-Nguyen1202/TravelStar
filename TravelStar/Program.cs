using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravelStar.Business.Implements;
using TravelStar.Business.Interfaces;
using TravelStar.Business.Option;
using TravelStar.Entities;
using TravelStar.Site.Permission;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("TravelStarContextConnection") ?? throw new InvalidOperationException("Connection string 'TravelStarContextConnection' not found.");

//Permission
//builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionProlicyProvider>();
//builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();

builder.Services.AddDbContext<TravelStarContext>(options =>
    options.UseSqlServer(connectionString));



//builder.Services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = false)
//    .AddEntityFrameworkStores<TravelStarContext>();

builder.Services.AddIdentity<AppUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddEntityFrameworkStores<TravelStarContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services
    .Configure<EmailOptions>(builder.Configuration.GetSection("EmailOptions"));

// Add services of application
builder.Services
    .AddTransient<ICityBo, CityBo>()
    .AddTransient<IDistrictBo, DistrictBo>()
    .AddTransient<IWardBo, WardBo>()
    .AddTransient<IHotelBo, HotelBo>()
    .AddTransient<IRoomBo, RoomBo>()
    .AddTransient<ICustomerBo, CustomerBo>()
    .AddTransient<IBookingBo, BookingBo>()
    .AddTransient<IEmailService, EmailService>()
    .AddTransient<IResourceReader, ResourceReader>(); ;

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");

    //Must be add to use Identity
    endpoints.MapRazorPages();
});

app.Run();
