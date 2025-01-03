// Licensed to the Biwen.EFCore.SoftDelete under one or more agreements.
// The Biwen.EFCore.SoftDelete licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Diagnostics.CodeAnalysis;

namespace Biwen.EFCore.SoftDelete;

/// <summary>
/// Base SoftDelete DbContext
/// </summary>
public abstract class SoftDeleteDbContext : DbContext
{
    protected SoftDeleteDbContext(DbContextOptions options) : base(options)
    {
        //使用软删除
        this.UseSoftDelete();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // 省略其它无关的代码
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // 省略其它无关的代码
            if (typeof(ISoftDeleted).IsAssignableFrom(entityType.ClrType))
            {
                entityType.AddSoftDeleteQueryFilter();
            }
        }
    }

    /// <summary>
    /// 默认不强制删除
    /// </summary>
    public bool ForceDelete { get; private set; } = false;

    /// <summary>
    /// 强制删除扩展
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="entity"></param>
    /// <param name="forceDelete">是否强制删除</param>
    /// <returns></returns>
    public virtual EntityEntry Remove<TEntity>([NotNull] TEntity entity, bool? forceDelete = false) where TEntity : class, ISoftDeleted
    {
        //强制删除
        if (forceDelete != null)
        {
            ForceDelete = forceDelete.Value;
        }
        return base.Remove(entity);
    }
}
