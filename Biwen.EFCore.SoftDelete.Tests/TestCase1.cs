using Biwen.EFCore.SoftDelete.TestConsole;
using Biwen.EFCore.SoftDelete.TestConsole.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Biwen.EFCore.SoftDelete.Tests
{
    public class SoftDeleteTests(ITestOutputHelper outputHelper)
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void TestCase1(int databaseId)
        {
            var serviceProvider = new ServiceCollection()
            .AddDbContext<TestDbContext>(options =>
            {
                options.UseInMemoryDatabase($"test-{databaseId}");
            })
            .BuildServiceProvider();

            serviceProvider.GetRequiredService<TestDbContext>().Database.EnsureCreated();

            using var sp = serviceProvider.CreateScope();
            var db = sp.ServiceProvider.GetRequiredService<TestDbContext>()!;

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

                //插入成功
                Assert.True(blog.Id > 0);
            }

            var blogs = db.Blogs.ToList();

            Assert.True(blogs.Count > 0);

            foreach (var blog in blogs)
            {
                outputHelper.WriteLine($"{blog.Id}-{blog.Title}-{blog.IsDeleted}");
            }

            //Delete 1 SoftDeleted
            var blog1 = db.Blogs.FirstOrDefault(x => x.Id == ids[0]);
            db.Remove(blog1!);
            db.SaveChanges();

            var deleted1 = db.Blogs.IgnoreQueryFilters().FirstOrDefault(blog1 => blog1.Id == ids[0]);
            Assert.NotNull(deleted1);//软删除
            Assert.True(deleted1.IsDeleted);//删除后标记为true

            //Delete 2 SoftDeleted
            var blog2 = db.Blogs.FirstOrDefault(x => x.Id == ids[1]);
            db.Remove(blog2!);
            db.SaveChanges();

            var deleted2 = db.Blogs.IgnoreQueryFilters().FirstOrDefault(blog1 => blog1.Id == ids[1]);
            Assert.NotNull(deleted2);//软删除
            Assert.True(deleted2.IsDeleted);//删除后标记为true

            //Delete 3 HardDeleted
            var blog3 = db.Blogs.FirstOrDefault(x => x.Id == ids[2]);
            if (blog3 != null)
            {
                db.Remove(blog3!, true);
                db.SaveChanges();
                outputHelper.WriteLine($"-----↑---- hard deleted id = {ids[2]}-------↑-------");
            }

            var deleted3 = db.Blogs.IgnoreQueryFilters().FirstOrDefault(blog1 => blog1.Id == ids[2]);
            Assert.Null(deleted3);//硬删除

            var blogs3 = db.Blogs.IgnoreQueryFilters().ToList();
            foreach (var blog in blogs3)
            {
                outputHelper.WriteLine($"{blog.Id}-{blog.Title}-{blog.IsDeleted}");
            }
        }
    }
}