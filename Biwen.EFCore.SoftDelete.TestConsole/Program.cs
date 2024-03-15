// See https://aka.ms/new-console-template for more information

using Biwen.EFCore.SoftDelete.TestConsole;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Biwen.EFCore.SoftDelete.TestConsole.Domains;

var serviceProvider = new ServiceCollection()
    .AddDbContext<TestDbContext>(options =>
    {
        //options.UseInMemoryDatabase("test");
        options.UseSqlite("Data Source=../../../test.db");
    })
    //.Decorate<TestDbContext>((ctx) => new SoftDeleteDecorator<TestDbContext>(ctx).DbContext)
    .BuildServiceProvider();


serviceProvider.GetRequiredService<TestDbContext>().Database.EnsureCreated();

using var sp = serviceProvider.CreateScope();
var db = sp.ServiceProvider.GetRequiredService<TestDbContext>()!;

//delete all rows
db.Database.ExecuteSqlRaw("DELETE FROM blogs");


Console.WriteLine("-----↑----init and  clear db -----↑--------");


List<int> ids = [];

int i = 1;
while (i <= 5)
{
    var blog = new Blog
    {
        Title = $"test {Guid.NewGuid()}",
        Content = "test content",
        Tags = "test",
        AuthorId = 1,
    };


    db.Blogs.Add(blog);
    i++;
    db.SaveChanges();

    ids.Add(blog.Id);
}


var blogs = db.Blogs.ToList();
foreach (var blog in blogs)
{
    Console.WriteLine($"{blog.Id}-{blog.Title}-{blog.IsDeleted}");
}

Console.WriteLine("-----↑----init db rows-----↑--------");

//Delete 1 SoftDeleted
var blog1 = db.Blogs.FirstOrDefault(x => x.Id == ids[0]);
db.Remove(blog1!);
db.SaveChanges();

//Delete 2 SoftDeleted
var blog2 = db.Blogs.FirstOrDefault(x => x.Id == ids[1]);
db.Remove(blog2!);
db.SaveChanges();


//Delete 3 HardDeleted
var blog3 = db.Blogs.FirstOrDefault(x => x.Id == ids[2]);
if (blog3 != null)
{
    db.Remove(blog3!, true);
    db.SaveChanges();
    Console.WriteLine($"-----↑---- hard deleted id = {ids[2]}-------↑-------");
}


var blogs2 = db.Blogs.ToList();
foreach (var blog in blogs2)
{
    Console.WriteLine($"{blog.Id}-{blog.Title}-{blog.IsDeleted}");
}

Console.WriteLine("-----↑----After Deleted (dont have soft deleted rows)-------↑-------");


var blogs3 = db.Blogs.IgnoreQueryFilters().ToList();
foreach (var blog in blogs3)
{
    Console.WriteLine($"{blog.Id}-{blog.Title}-{blog.IsDeleted}");
}

Console.WriteLine("-----↑----Contains All (have soft deleted rows) --------↑------");


Console.WriteLine("Hello, World!");
Console.ReadLine();