using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    class ModelsModule : NinjectModule
    {
        public override void Load()
        {
            Bind<BaseDbEntities>().To<LocalDbEntities>().InSingletonScope(); // Локальная база данных (сохранение данных только в текущей сессии)
            //Bind<BaseDbEntities>().ToSelf().InSingletonScope();

            Bind<MaterialModel>().ToSelf().InSingletonScope();
            Bind<FixedCostsModel>().ToSelf().InSingletonScope();
            Bind<ProductModel>().ToSelf();
            Bind<SalaryModel>().ToSelf();
            Bind<FinanceModel>().ToSelf();
            Bind<RecommendationModel>().ToSelf();
            Bind<ReportsModel>().ToSelf();
        }
    }
}
