using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biwen.EFCore.SoftDelete
{
    public static class DbContextExtensions
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
                    var del = entry.Entity as ISoftDeleted;
                    if (del != null && del!.ForceDelete != null && del.ForceDelete == true)
                    {
                        //如果是强制删除，直接删除
                        continue;
                    }

                    entry.State = EntityState.Modified;
                    entry.CurrentValues[nameof(ISoftDeleted.IsDeleted)] = true;
                }
            };
        }
    }
}