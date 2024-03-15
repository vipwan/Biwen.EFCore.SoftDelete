# Biwen.EFCore.SoftDelete

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/vipwan/Biwen.EFCore.SoftDelete/blob/master/LICENSE.txt) 
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-brightgreen.svg)](https://github.com/vipwan/Biwen.EFCore.SoftDelete/pulls) 

## 
- 实现DbContext的软删除功能


## NuGet 包

- dotnet add package Biwen.EFCore.SoftDelete --version 1.1.1


## 开发环境

* Windows 10
* [Rider 2022](https://www.jetbrains.com/rider) / [Visual Studio 2022](https://visualstudio.microsoft.com) / [Visual Studio Code](https://code.visualstudio.com)
* [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
  

## 运行环境
- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)

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
}

```

### Step 2  继承SoftDeleteDbContext

```csharp

public class TestDbContext : SoftDeleteDbContext
{
	public TestDbContext(DbContextOptions<TestDbContext> options)
		: base(options)
	{
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

### step 2 装饰器注册


  ```csharp

var serviceProvider = new ServiceCollection()
    .AddDbContext<TestDbContext>(options =>
    {
	    //使用你的数据库引擎
        options.UseInMemoryDatabase("test");
    })
    //注册装饰器 important!
    .Decorate<TestDbContext>((ctx) => new SoftDeleteDecorator<TestDbContext>(ctx).DbContext)
    .BuildServiceProvider();

   ```

### step 3 注意事项

```csharp

//1. 请使用 DbSet.Remove 方法，不可使用批量删除方法:ExecuteDelete(),ExecuteDeleteAsync()


```


### Enjoy ! 


## License 
- MIT

## 联系我
- QQ:552175420
- Email: vipwan#sina.com

## 项目地址

- [GitHub][(https://github.com/vipwan)](https://github.com/vipwan/Biwen.Settings)
