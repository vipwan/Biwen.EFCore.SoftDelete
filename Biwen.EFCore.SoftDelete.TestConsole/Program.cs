// See https://aka.ms/new-console-template for more information

using Biwen.EFCore.SoftDelete.TestConsole;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Biwen.EFCore.SoftDelete.TestConsole.Domains;
using Biwen.EFCore.SoftDelete;

var serviceProvider = new ServiceCollection()
    .AddDbContext<TestDbContext>(options =>
    {
        options.UseInMemoryDatabase("test");
    })
    //.Decorate<TestDbContext>((ctx) => new SoftDeleteDecorator<TestDbContext>(ctx).DbContext)
    .BuildServiceProvider();


serviceProvider.GetRequiredService<TestDbContext>().Database.EnsureCreated();

using var sp = serviceProvider.CreateScope();
var db = sp.ServiceProvider.GetRequiredService<TestDbContext>();

int i = 1;
while (i <= 5)
{
    db.Blogs.Add(new Blog
    {
        Title = $"test {Guid.NewGuid()}",
        Content = "test content",
        Tags = "test",
        AuthorId = 1,
    });
    i++;
}

db.SaveChanges();

var blogs = db.Blogs.ToList();
foreach (var blog in blogs)
{
    Console.WriteLine($"{blog.Id}-{blog.Title}-{blog.IsDeleted}");
}

Console.WriteLine("-----↑----初始化的数据-----↑--------");

//Delete 1 模拟软删除
var blog1 = db.Blogs.FirstOrDefault(x => x.Id == 1);
db.Remove(blog1!);
db.SaveChanges();

//Delete 2 模拟软删除
var blog2 = db.Blogs.FirstOrDefault(x => x.Id == 2);
db.Remove(blog2!);
db.SaveChanges();


//Delete 3 模拟强制删除
var blog3 = db.Blogs.FirstOrDefault(x => x.Id == 3);
db.Remove(blog3!, true);
db.SaveChanges();


var blogs2 = db.Blogs.ToList();
foreach (var blog in blogs2)
{
    Console.WriteLine($"{blog.Id}-{blog.Title}-{blog.IsDeleted}");
}

Console.WriteLine("-----↑----删除后的数据-------↑-------");



var blogs3 = db.Blogs.IgnoreQueryFilters().ToList();
foreach (var blog in blogs3)
{
    Console.WriteLine($"{blog.Id}-{blog.Title}-{blog.IsDeleted}");
}

Console.WriteLine("-----↑----包含软删除数据--------↑------");


Console.WriteLine("Hello, World!");
Console.ReadLine();