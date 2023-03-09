using Microsoft.EntityFrameworkCore;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WebApp.Extensions;

//var builder = WebApplication.CreateBuilder(args);
//{
//    builder.Services.AddControllersWithViews();
//    builder.Services.AddDbContext<BlogDbContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//    builder.Services.AddScoped<IBlogRepository, BlogRepository>();
//    builder.Services.AddScoped<IDataSeeder, DataSeeder>();
//}

//var app = builder.Build();
//{
//    if(app.Environment.IsDevelopment())
//    {
//        app.UseDeveloperExceptionPage();
//    }
//    else
//    {
//        app.UseExceptionHandler("/Blog/Error");
//        app.UseHsts();
//    }
//    app.UseHttpsRedirection();
//    app.UseStaticFiles();
//    app.UseRouting();
//    app.MapControllerRoute(
//        name: "default",
//        pattern: "{controller=Blog}/{action=Index}/{id?}");
//}
//using (var scope = app.Services.CreateScope())
//{
//    var seeder= scope.ServiceProvider.GetRequiredService<IDataSeeder>();
//    seeder.Initialize();
//}

//app.MapControllerRoute(
//    name: "posts-by-category",
//    pattern: "blog/category/{slug}",
//    defaults: new { controller = "Blog", action = "Category" });
//app.MapControllerRoute(
//    name: "posts-by-tag",
//    pattern: "blog/tag/{slug}",
//    defaults: new { controller = "Blog", action = "Tag" });
//app.MapControllerRoute(
//    name: "single-post",
//    pattern: "blog/post/{year:int}/{month:int}/{day:int}/{slug}",
//    defaults: new { controller = "Blog", action = "Post" });
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Blog}/{action=Index}/{id?}");
//app.Run();

var builder = WebApplication.CreateBuilder(args);
{
    builder.ConfigureMvc()
        .ConfigureServices();
}

var app=builder.Build();
{
    app.UseRequestPipeline();
    app.UseBlogRoutes();
    app.UseDataSeeder();
}

app.Run();

