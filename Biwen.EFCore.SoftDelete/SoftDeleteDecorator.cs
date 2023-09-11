//using Microsoft.EntityFrameworkCore;
//namespace Biwen.EFCore.SoftDelete
//{
//    public class SoftDeleteDecorator<T> where T : DbContext
//    {
//        private readonly T _dbContext;

//        public SoftDeleteDecorator(T dbContext)
//        {
//            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
//            _dbContext.SavingChanges += (sender, args) =>
//            {
//                var entries = _dbContext.ChangeTracker.Entries().Where(x => x.Entity is ISoftDeleted && x.State == EntityState.Deleted);
//                foreach (var entry in entries)
//                {
//                    var del = entry.Entity as ISoftDeleted;
//                    if (del != null && del!.ForceDelete != null && del.ForceDelete == true)
//                    {
//                        //如果是强制删除，直接删除
//                        continue;
//                    }

//                    entry.State = EntityState.Modified;
//                    entry.CurrentValues[nameof(ISoftDeleted.IsDeleted)] = true;
//                }
//            };
//        }
//        public T DbContext => _dbContext;
//    }

//}