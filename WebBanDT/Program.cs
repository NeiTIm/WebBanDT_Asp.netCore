using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebBanDT.Models;
using WebBanDT.Models.Repository;

var builder = WebApplication.CreateBuilder(args);

//Connection db
builder.Services.AddDbContext<DataContext>(options =>{
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectedDb"]);
});
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => { 
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});
builder.Services.AddIdentity<AppUserModel,IdentityRole>()
	.AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();


builder.Services.Configure<IdentityOptions>(options =>
{
	// Password settings.
	options.Password.RequireDigit = true; // số
	options.Password.RequireLowercase = true;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequireUppercase = false;
	options.Password.RequiredLength = 4; //chiều dài p
	options.User.RequireUniqueEmail = true; //yc email
});

var app = builder.Build();
app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");
app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//phải để đúng vị trí
app.UseRouting();
app.UseAuthentication(); //xác thực


app.UseAuthorization();
//backend
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();

app.MapControllerRoute(
    name: "category",
    pattern: "category/{Slug?}",
defaults: new {controller="Category", action="Index" });
app.MapControllerRoute(
    name: "brand",
    pattern: "brand/{Slug?}",
defaults: new { controller = "Brand", action = "Index" });

// PHẢI TRƯỚC DEFAULT
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


SeedData.SeedingData(context);
app.Run();
