using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

using AutoTSForETongModel;

namespace AutoTSForETongDataFramework
{
    /// <summary>
    ///     EntityFramework仓储操作基类
    /// </summary>
    /// <typeparam name="TEntity">动态实体类型</typeparam>
    public abstract class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : Entity
    {
        #region 属性

        /// <summary>
        ///     获取 仓储上下文的实例
        /// </summary>
        [Ninject.Inject]
        public IUnitOfWork UnitOfWork { get; set; }

        /// <summary>
        ///     获取或设置 EntityFramework的数据仓储上下文
        /// </summary>
        protected IUnitOfWorkContext EFContext
        {
            get
            {
                if (UnitOfWork is IUnitOfWorkContext)
                {
                    return UnitOfWork as IUnitOfWorkContext;
                }
                throw new Exception(string.Format("数据仓储上下文对象类型不正确，应为IRepositoryContext，实际为 {0}", UnitOfWork.GetType().Name));
            }
        }

        /// <summary>
        ///     获取 当前实体的查询数据集
        /// </summary>
        public virtual IQueryable<TEntity> Entities
        {
            get { return EFContext.Set<TEntity>(); }
        }

        #endregion

        #region 公共方法

        /// <summary>
        ///     插入实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Insert(TEntity entity, bool isSave = true)
        {
            EFContext.RegisterNew(entity);
            return isSave ? EFContext.Commit() : 0;
        }

        /// <summary>
        ///     批量插入实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Insert(IEnumerable<TEntity> entities, bool isSave = true)
        {
            EFContext.RegisterNew(entities);
            return isSave ? EFContext.Commit() : 0;
        }

        /// <summary>
        ///     删除指定编号的记录
        /// </summary>
        /// <param name="id"> 实体记录编号 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(object id, bool isSave = true)
        {
            TEntity entity = EFContext.Set<TEntity>().Find(id);
            return entity != null ? Delete(entity, isSave) : 0;
        }

        /// <summary>
        ///     删除实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(TEntity entity, bool isSave = true)
        {
            EFContext.RegisterDeleted(entity);
            return isSave ? EFContext.Commit() : 0;
        }

        /// <summary>
        ///     删除实体记录集合
        /// </summary>
        /// <param name="entities"> 实体记录集合 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(IEnumerable<TEntity> entities, bool isSave = true)
        {
            EFContext.RegisterDeleted(entities);
            return isSave ? EFContext.Commit() : 0;
        }

        /// <summary>
        ///     删除所有符合特定表达式的数据
        /// </summary>
        /// <param name="predicate"> 查询条件谓语表达式 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Delete(Expression<Func<TEntity, bool>> predicate, bool isSave = true)
        {
            List<TEntity> entities = EFContext.Set<TEntity>().Where(predicate).ToList();
            return entities.Count > 0 ? Delete(entities, isSave) : 0;
        }

        /// <summary>
        ///     更新实体记录
        /// </summary>
        /// <param name="entity"> 实体对象 </param>
        /// <param name="isSave"> 是否执行保存 </param>
        /// <returns> 操作影响的行数 </returns>
        public virtual int Update(TEntity entity, bool isSave = true)
        {
            EFContext.RegisterModified(entity);
            return isSave ? EFContext.Commit() : 0;
        }

        /// <summary>
        ///     查找指定主键的实体记录
        /// </summary>
        /// <param name="key"> 指定主键 </param>
        /// <returns> 符合编号的记录，不存在返回null </returns>
        public virtual TEntity GetByKey(object key)
        {
            return EFContext.Set<TEntity>().Find(key);
        }

        /// <summary>
        /// 保存当前的挂起的增删改记录
        /// </summary>
        /// <returns>受影响的行数</returns>
        public virtual int SaveChanges()
        {
            return EFContext.Commit();
        }

        #endregion
    }
}
