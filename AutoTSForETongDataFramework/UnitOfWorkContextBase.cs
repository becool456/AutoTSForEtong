using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


using AutoTSForETongModel;

namespace AutoTSForETongDataFramework
{
    /// <summary>
    ///     单元操作实现基类
    /// </summary>
    public abstract class UnitOfWorkContextBase : IUnitOfWorkContext
    {
        #region 属性
        /// <summary>
        /// 获取 当前使用的数据访问上下文对象
        /// </summary>
        protected abstract DbContext Context { get; }

        /// <summary>
        ///     获取 当前单元操作是否已被提交
        /// </summary>
        public bool IsCommitted { get; private set; }

        #endregion

        #region 方法
        /// <summary>
        ///     提交当前单元操作的结果
        /// </summary>
        /// <returns></returns>
        public int Commit()
        {
            if (IsCommitted)
            {
                return 0;
            }
            try
            {
                int result = Context.SaveChanges();
                IsCommitted = true;
                return result;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        ///     把当前单元操作回滚成未提交状态
        /// </summary>
        public void Rollback()
        {
            IsCommitted = false;
        }

        public void Dispose()
        {
            if (!IsCommitted)
            {
                Commit();
            }
            Context.Dispose();
        }

        /// <summary>
        ///   为指定的类型返回 System.Data.Entity.DbSet，这将允许对上下文中的给定实体执行 CRUD 操作。
        /// </summary>
        /// <typeparam name="TEntity"> 应为其返回一个集的实体类型。 </typeparam>
        /// <returns> 给定实体类型的 System.Data.Entity.DbSet 实例。 </returns>
        public DbSet<TEntity> Set<TEntity>() where TEntity : Entity
        {
            return Context.Set<TEntity>();
        }

        /// <summary>
        ///     注册一个新的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterNew<TEntity>(TEntity entity) where TEntity : Entity
        {
            EntityState state = Context.Entry(entity).State;
            if (state == EntityState.Detached)
            {
                Context.Entry(entity).State = EntityState.Added;
            }
            IsCommitted = false;
        }

        /// <summary>
        ///     批量注册多个新的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <param name="entities"> 要注册的对象集合 </param>
        public void RegisterNew<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        {
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entities)
                {
                    RegisterNew(entity);
                }
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }

        /// <summary>
        ///     注册一个更改的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterModified<TEntity>(TEntity entity) where TEntity : Entity
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Context.Set<TEntity>().Attach(entity);
            }
            Context.Entry(entity).State = EntityState.Modified;
            IsCommitted = false;
        }

        /// <summary>
        ///   注册一个删除的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <param name="entity"> 要注册的对象 </param>
        public void RegisterDeleted<TEntity>(TEntity entity) where TEntity : Entity
        {
            Context.Entry(entity).State = EntityState.Deleted;
            IsCommitted = false;
        }

        /// <summary>
        ///   批量注册多个删除的对象到仓储上下文中
        /// </summary>
        /// <typeparam name="TEntity"> 要注册的类型 </typeparam>
        /// <param name="entities"> 要注册的对象集合 </param>
        public void RegisterDeleted<TEntity>(IEnumerable<TEntity> entities) where TEntity : Entity
        {
            try
            {
                Context.Configuration.AutoDetectChangesEnabled = false;
                foreach (TEntity entity in entities)
                {
                    RegisterDeleted(entity);
                }
            }
            finally
            {
                Context.Configuration.AutoDetectChangesEnabled = true;
            }
        }
        #endregion
    }
}
