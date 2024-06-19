using System.Linq.Expressions;
using System.Reflection.Emit;
using LogicalEFCoreDel.Extensions;
using LogicalEFCoreDel.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;

namespace LogicalEFCoreDel
{
    /// <summary>
    /// 全局过滤删除标识
    /// </summary>
    public class LogicalDbContext : DbContext
    {
        public LogicalDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<UserInfo> UserInfo { get; set; }
        public DbSet<AttachMent> AttachMent { get; set; }

        /// <summary>
        /// 实现创建和更新时间自动更新
        /// </summary>
        /// <remarks>我们可以通过重载SaveChangesAsync方法，在提交数据时，对创建时间和更新时间进行设置。</remarks>
        /// <returns></returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            //add
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
            foreach (var entityEntry in entries)
            {
                var property = entityEntry.Metadata.FindProperty("CreatedTime");
                if (property != null && property.ClrType == typeof(DateTime?))
                {
                    entityEntry.Property("CreatedTime").CurrentValue = DateTime.Now;
                }
            }

            ////Del
            //entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
            //foreach (var entityEntry in entries)
            //{
            //    var property = entityEntry.Metadata.FindProperty("IsDeleted");
            //    if (property != null && property.ClrType == typeof(bool?))
            //    {
            //        entityEntry.Property("IsDeleted").CurrentValue = true;
            //    }
            //}

            //update
            entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
            foreach (var entityEntry in entries)
            {
                var property = entityEntry.Metadata.FindProperty("UpdatedTime");
                if (property != null && property.ClrType == typeof(DateTime?))
                {
                    entityEntry.Property("UpdatedTime").CurrentValue = DateTime.Now;
                }
            }
            
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            //add
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added).ToList();
            foreach (var entityEntry in entries)
            {
                var property = entityEntry.Metadata.FindProperty("CreatedTime");
                if (property != null && property.ClrType == typeof(DateTime?))
                {
                    entityEntry.Property("CreatedTime").CurrentValue = DateTime.Now;
                }
            }

            ////Del
            //entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Deleted).ToList();
            //foreach (var entityEntry in entries)
            //{
            //    var property = entityEntry.Metadata.FindProperty("IsDeleted");
            //    if (property != null && property.ClrType == typeof(bool?))
            //    {
            //        entityEntry.Property("IsDeleted").CurrentValue = true;
            //        entityEntry.State = EntityState.Modified;//标识修改
            //    }
            //}

            //update
            entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Modified).ToList();
            foreach (var entityEntry in entries)
            {
                var property = entityEntry.Metadata.FindProperty("UpdatedTime");
                if (property != null && property.ClrType == typeof(DateTime?))
                {
                    entityEntry.Property("UpdatedTime").CurrentValue = DateTime.Now;
                }
            }
            
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }


        /// <summary>
        /// 实现模型主键设置和软删除
        /// </summary>
        /// <remarks>该接口中有Id和IsDeleted字段，用来标识主键和是否删除(软删除)。</remarks>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entityTypes = modelBuilder.Model.GetEntityTypes();
            foreach (var entityType in entityTypes)
            {
                if (typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.Name)
                        .HasKey("Id");
                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(ConvertFilterExpression<ISoftDeletable>(e => !(e.IsDeleted ?? false), entityType.ClrType));
                }
            }
        }
        private static LambdaExpression ConvertFilterExpression<TInterface>(Expression<Func<TInterface, bool>> filterExpression, Type entityType)
        {
            var newParam = Expression.Parameter(entityType);
            var newBody = ReplacingExpressionVisitor.Replace(filterExpression.Parameters.Single(), newParam, filterExpression.Body);
            return Expression.Lambda(newBody, newParam);
        }

    }
}
