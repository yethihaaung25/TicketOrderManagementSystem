using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TOMS.DAO;
using TOMS.Services.Domains;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// for identity config
builder.Services.AddRazorPages();

builder.Services.AddSession();

// Declare the configuration
var config = builder.Configuration;
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(config.GetConnectionString("TOMSConnectionString")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>( option =>
{
	option.SignIn.RequireConfirmedAccount = true;
	option.Password.RequireDigit = true;
	option.Password.RequiredLength = 8;
	option.Password.RequireNonAlphanumeric = true;
	option.Password.RequireUppercase = false;
	option.Password.RequireLowercase = false;
}).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultUI().AddDefaultTokenProviders();
//register all of the domain services
builder.Services.AddScoped<IPassengerService, PassengerService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IBusLineService, BusLineService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddScoped<IPaymentTypeService, PaymentTypeService>();
builder.Services.AddScoped<ITicketOrderServices, TicketOrderService>();
builder.Services.AddScoped<ITicketOrderTransactionService, TicketOrderTransactionService>();
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

app.UseSession();

app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
	endpoints.MapRazorPages(); // for identity config
});

app.Run();
