using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Ninject.Modules;
using Model;

namespace BusinessLogic
{
    public class SimpleConfigModule : NinjectModule
    {
        public override void Load()
        {
            Bind<AppDbContext>().ToSelf().InSingletonScope();
            Bind<IRepository<Book>>().To<EntityRepository<Book>>().InSingletonScope();
            Bind<IRepository<Reader>>().To<EntityRepository<Reader>>().InSingletonScope();
        }
    }
}
