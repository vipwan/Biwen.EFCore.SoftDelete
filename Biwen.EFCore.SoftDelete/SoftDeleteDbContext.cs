using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Biwen.EFCore.SoftDelete
{
    /// <summary>
    /// Base SoftDelete DbContext
    /// </summary>
    public abstract class SoftDeleteDbContext : DbContext
    {
        protected SoftDeleteDbContext(DbContextOptions options) : base(options)
        {
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

    /// <summary>
    /// Extension methods for <see cref="IMutableEntityType" />.
    /// </summary>
    static class SoftDeleteQueryExtension
    {
        public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodToCall = typeof(SoftDeleteQueryExtension)
                .GetMethod(nameof(GetSoftDeleteFilter)!, BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(entityData.ClrType);
            var filter = methodToCall.Invoke(null, Array.Empty<object>());
            entityData.SetQueryFilter((LambdaExpression)filter!);
        }

        private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : class, ISoftDeleted
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
            return filter;
        }
    }
}