using Microsoft.EntityFrameworkCore;
using PalmFarm.Data;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<TrackfarmContext>(
#if(DEBUG)
        options => options.UseSqlServer("Data Source=.\\SQLEXPRESS;Initial Catalog=db_a7e8dd_palmfarm;Integrated Security=True"));
#else
        options => options.UseSqlServer("Data Source=SQL5109.site4now.net;Initial Catalog=db_a7e8dd_palmfarm;User Id=db_a7e8dd_palmfarm_admin;Password=P@ssw0rd"));
#endif
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromDays(1000);
});

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
app.UseStaticFiles();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Farms}/{action=Login}/{id?}");

app.Run();
