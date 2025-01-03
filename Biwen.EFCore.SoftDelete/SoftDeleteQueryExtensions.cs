// Licensed to the Biwen.EFCore.SoftDelete under one or more agreements.
// The Biwen.EFCore.SoftDelete licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace Biwen.EFCore.SoftDelete;

/// <summary>
/// Extension methods for <see cref="IMutableEntityType" />.
/// </summary>
internal static class SoftDeleteQueryExtensions
{
    public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
    {
        var methodToCall = typeof(SoftDeleteQueryExtensions)
            .GetMethod(nameof(GetSoftDeleteFilter)!, BindingFlags.NonPublic | BindingFlags.Static)!
            .MakeGenericMethod(entityData.ClrType);
        var filter = methodToCall.Invoke(null, []);
        entityData.SetQueryFilter((LambdaExpression)filter!);
    }

    private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : class, ISoftDeleted
    {
        Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
        return filter;
    }
}