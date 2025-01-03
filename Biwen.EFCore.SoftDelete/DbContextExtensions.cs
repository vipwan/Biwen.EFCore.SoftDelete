// Licensed to the Biwen.EFCore.SoftDelete under one or more agreements.
// The Biwen.EFCore.SoftDelete licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Biwen.EFCore.SoftDelete;

internal static class DbContextExtensions
{
    /// <summary>
    /// 使用软删除
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="context"></param>
    public static void UseSoftDelete<T>(this T context) where T : SoftDeleteDbContext
    {
        context.SavingChanges += (sender, args) =>
        {
            var entries = context.ChangeTracker.Entries().Where(x => x.Entity is ISoftDeleted && x.State == EntityState.Deleted);
            foreach (var entry in entries)
            {
                if (context.ForceDelete)
                {
                    continue;
                }

                entry.State = EntityState.Modified;
                entry.CurrentValues[nameof(ISoftDeleted.IsDeleted)] = true;
            }
        };
    }

}