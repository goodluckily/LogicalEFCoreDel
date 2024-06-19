using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LogicalEFCoreDel.Extensions
{
    public static class SoftDeletedExtensions
    {
        /// <summary>
        /// 包括已软删除的记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="ignoreDeleted">是否在查询中包括已软删除的记录</param>
        /// <returns></returns>
        public static IQueryable<T> IncludeSoftDeleted<T>(this DbSet<T> dbSet, bool includeSoftDeleted = true) where T : class, ISoftDeletable
        {
            if (includeSoftDeleted)
            {
                return dbSet.IgnoreQueryFilters();
            }
            else
            {
                return dbSet;
            }
        }

        /// <summary>
        /// 进行逻辑删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dbSet"></param>
        /// <param name="entity"></param>
        /// <param name="IgnoreDeleted">是否软删除数据</param>
        /// <exception cref="InvalidOperationException"></exception>
        public static void SoftDelete<T>(this DbSet<T> dbSet, T entity, bool softDelete = true) where T : class, ISoftDeletable
        {
            if (dbSet != null)
            {
                var entityEntry = dbSet.Entry(entity);
                if (entityEntry == null)
                {
                    throw new InvalidOperationException("entityEntry not found for this dbSet.Entry(T)");
                };
                if (softDelete)
                {
                    var propertyDel = entityEntry?.Metadata.FindProperty("IsDeleted");
                    if (propertyDel != null && propertyDel.ClrType == typeof(bool?))
                    {
                        entityEntry!.Property("IsDeleted").CurrentValue = true;
                       
                    }
                    var propertyUpdate = entityEntry?.Metadata.FindProperty("UpdatedTime");
                    if (propertyUpdate != null && propertyUpdate.ClrType == typeof(DateTime?))
                    {
                        entityEntry!.Property("UpdatedTime").CurrentValue = DateTime.Now;

                    }
                    entityEntry!.State = EntityState.Modified;//标识修改
                }
                else
                {
                    entityEntry.State = EntityState.Deleted;//标识删除
                    dbSet.Remove(entity);
                }
            }
            else
            {
                throw new InvalidOperationException("Entry not found for this DbSet.");
            }
        }
    }
}
