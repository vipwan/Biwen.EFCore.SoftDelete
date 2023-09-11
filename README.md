# Biwen.EFCore.SoftDelete

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/vipwan/Biwen.EFCore.SoftDelete/blob/master/LICENSE.txt) 
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://github.com/vipwan/Biwen.EFCore.SoftDelete/pulls) 

## 
- 实现DbContext的软删除功能


## NuGet 包

- dotnet add package Biwen.EFCore.SoftDelete --version 1.0.0


## 开发环境

* Windows 10
* [Rider 2022](https://www.jetbrains.com/rider) / [Visual Studio 2022](https://visualstudio.microsoft.com) / [Visual Studio Code](https://code.visualstudio.com)
* [.NET 7.0](https://dotnet.microsoft.com/download/dotnet/7.0)
  

## 运行环境
- [.NET 6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
- [.NET 7.0](https://dotnet.microsoft.com/download/dotnet/7.0)

## Easy to Use


### Step 1 Model定义


```csharp

[PrimaryKey("Id")]
public class Blog : ISoftDeleted
{
    //...
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public int AuthorId { get; set; }

    //请注意这里的IsDeleted默认必须是false!表示未删除
    public bool IsDeleted { get; set; } = false;
    //强制删除标记，如果为true,则不会进入软删除流程，直接删除
    public bool? ForceDelete { get; set; }

}

```

### Step 2  继承SoftDeleteDbContext

```csharp

public class TestDbContext : SoftDeleteDbContext
{
	public TestDbContext(DbContextOptions<TestDbContext> options)
		: base(options)
	{
           //使用软删除
           this.UseSoftDelete();
	}
    //...
	public DbSet<Blog> Blogs { get; set; }

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		//根据情况自定义表名
        //...
	}
}

```

### step 2 注册DbContext


  ```csharp

var serviceProvider = new ServiceCollection()
    .AddDbContext<TestDbContext>(options =>
    {
	    //使用你的数据库引擎
        options.UseInMemoryDatabase("test");
    })
    .BuildServiceProvider();

   ```

### step 3 注意事项

```csharp

//1. 请使用 DbSet.Remove 方法，不可使用批量删除方法:ExecuteDelete(),ExecuteDeleteAsync()


```


### Enjoy ! 

```csharp

//Delete 1 模拟软删除
var blog1 = db.Blogs.FirstOrDefault(x => x.Id == 1);
db.Remove(blog1!);
db.SaveChanges();

//Delete 3 模拟强制删除
var blog3 = db.Blogs.FirstOrDefault(x => x.Id == 3);
db.Remove(blog3!, true);
db.SaveChanges();

```



## License 
- MIT

## 联系我
- QQ:552175420
- Email: vipwan#sina.com

## 项目地址

- [GitHub][(https://github.com/vipwan)](https://github.com/vipwan/Biwen.Settings)
