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
    .Decorate<TestDbContext>((ctx) => new SoftDeleteDecorator<TestDbContext>(ctx).DbContext)
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


Console.WriteLine("---------初始化的数据--------------");


var blogs = db.Blogs.ToList();
foreach (var blog in blogs)
{
    Console.WriteLine($"{blog.Id}-{blog.Title}-{blog.IsDeleted}");
}

//Delete 1
var blog1 = db.Blogs.FirstOrDefault(x => x.Id == 1);
db.Blogs.Remove(blog1!);
db.SaveChanges();

//Delete 2
var blog2 = db.Blogs.FirstOrDefault(x => x.Id == 2);
db.Remove(blog2!);
db.SaveChanges();

//多删除.
//var blog2 = await db.Blogs.Where(x => x.Id == 2).ExecuteDeleteAsync();

Console.WriteLine("---------删除后的数据--------------");


var blogs2 = db.Blogs.ToList();
foreach (var blog in blogs2)
{
    Console.WriteLine($"{blog.Id}-{blog.Title}-{blog.IsDeleted}");
}

var blogs3 = db.Blogs.IgnoreQueryFilters().ToList();
foreach (var blog in blogs3)
{
    Console.WriteLine($"{blog.Id}-{blog.Title}-{blog.IsDeleted}");
}

Console.WriteLine("---------包含软删除数据-------------------");


Console.WriteLine("Hello, World!");
Console.ReadLine();