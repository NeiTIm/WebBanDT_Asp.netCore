using Microsoft.EntityFrameworkCore;
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
    options.IOTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});
var app = builder.Build();
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

app.UseRouting();

app.UseAuthorization();
//backend
app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}");
var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


SeedData.SeedingData(context);
app.Run();
