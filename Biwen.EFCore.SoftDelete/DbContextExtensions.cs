namespace Biwen.EFCore.SoftDelete
{
    public static class DbContextExtensions
    {
        /// <summary>
        /// 使用软删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="context"></param>
        internal static void UseSoftDelete<T>(this T context) where T : SoftDeleteDbContext
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
}