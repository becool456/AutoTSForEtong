using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Data.Entity;
using AutoTSForETongImplOfData;
using AutoTSForETongDataFramework;
using AutoTSForETongModel;
using AutoTSForETongSysCore;
using AutoTSForETongSysCore.ImplOfSysCore;
using AutoTSForETongUserCore;
using Ninject;

namespace NinjectIOC
{
    /// <summary>B
    /// 实现注入接口IDenpendencyResolver
    /// </summary>
    public class NinjectDependencyResolver
       : IDependencyResolver
    {
        /// <summary>
        /// Ninject核心类
        /// </summary>
        private Ninject.IKernel kernel;
        /// <summary>
        /// 构造函数
        /// </summary>
        public NinjectDependencyResolver()
        {
            this.kernel = new Ninject.StandardKernel();
            this.kernel.Settings.InjectNonPublic = true;
            this.AddBindings();
        }

        /// <summary>
        /// 添加接口与继承类绑定
        /// </summary>
        private void AddBindings()
        {
            this.kernel.Bind<IUnitOfWork>().To<SqlUnitOfWorkContext>();

            //设置SqlDbContext为单例模式
            this.kernel.Bind<DbContext>().To<SqlDbContext>().InSingletonScope();

            this.kernel.Bind<IQuestionDB>().To<QuestionDB>();

            this.kernel.Bind<ISubjectDB>().To<SubjectDB>();

            this.kernel.Bind<IKnowledgeSiteDB>().To<KnowledgeSiteDB>();

            this.kernel.Bind<IExampapaperDB>().To<ExampaperDB>();

            this.kernel.Bind<IPaperExtraInfoDB>().To<PaperExtraInfoDB>();

            this.kernel.Bind<IMemberDB>().To<MemberDB>();

            this.kernel.Bind<IMSMapDB>().To<MSMapDB>();

            this.kernel.Bind<IGeneratePaper>().To<GeneratePaper>();

            this.kernel.Bind<IOperateQuestion>().To<OperateQuestion>();

            this.kernel.Bind<IPQMapDB>().To<PQMapDB>();

            this.kernel.Bind<IUserManager>().To<UserManager>();

            this.kernel.Bind<IKnowledgeSites>().To<KnowledgeSites>();

            this.kernel.Bind<IQuestionTypeDB>().To<QuestionTypeDB>();

        }

        /// <summary>
        /// 获取指定类型的对象实例
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public object GetService(Type serviceType)
        {
            return this.kernel.TryGet(serviceType);
        }

        /// <summary>
        /// 获取指定类型的对象实例的集合
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.kernel.GetAll(serviceType);
        }
    }
}
