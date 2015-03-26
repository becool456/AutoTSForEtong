using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using AutoTSForETongDataFramework;

namespace AutoTSForETongImplOfData
{
    /// <summary>
    /// SqlDbContext项目单元类
    /// </summary>
    public class SqlUnitOfWorkContext : UnitOfWorkContextBase
    {
        /// <summary>
        /// 获取当前的数据上下文对象
        /// </summary>
        protected override DbContext Context
        {
            get
            {
                return SqlContext;
            }
        }

        /// <summary>
        /// 获取当前DearDream项目数据上下文访问对象
        /// </summary>
        [Ninject.Inject]
        public DbContext SqlContext { get; set; }

    }
}
