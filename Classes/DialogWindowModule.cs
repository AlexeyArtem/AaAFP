using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AaAFP2
{
    class DialogWindowModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Window>().To<OrderWindow>().Named(typeof(Order).Name);
            Bind<Window>().To<ClientWindow>().Named(typeof(Client).Name);
            Bind<Window>().To<BonusSalaryWindow>().Named(typeof(BonusSalary).Name);
            Bind<Window>().To<ContractWindow>().Named(typeof(Contract).Name);
            Bind<Window>().To<CostCategoryWindow>().Named(typeof(CostCategory).Name);
            Bind<Window>().To<DeductionSalaryWindow>().Named(typeof(DeductionFromSalary).Name);
            Bind<Window>().To<EmployeesPositionsWindow>().Named(typeof(Position).Name);
            Bind<Window>().To<EmployeeWindow>().Named(typeof(Employee).Name);
            Bind<Window>().To<FixedCostWindow>().Named(typeof(FixedCost).Name);
            Bind<Window>().To<ManufacturingOperationsWindow>().Named(typeof(ManufacturingOperation).Name);
            Bind<Window>().To<MaterialsWindow>().Named(typeof(Material).Name);
            Bind<Window>().To<OtherCostWindow>().Named(typeof(OtherCost).Name);
            Bind<Window>().To<PackingListWindow>().Named(typeof(PackingList).Name);
            Bind<Window>().To<PaymentWindow>().Named(typeof(ContractPayment).Name);
            Bind<Window>().To<PointsRwpWindow>().Named(typeof(AccrualPointsRwp).Name);
            Bind<Window>().To<ProductsTypeWindow>().Named(typeof(TypeProduct).Name);
            Bind<Window>().To<SalaryPrepayWindow>().Named(typeof(PaymentSalaryPrepay).Name);
            Bind<Window>().To<ProductWindow>().Named(typeof(Product).Name);
            Bind<Window>().To<RecommendationsWindow>().Named(typeof(Recommendation).Name);
            Bind<Window>().To<SettingsWindow>().Named("Settings");
            Bind<Window>().To<StatusWindowEnterprise>().Named("StatusEnterprise");
            Bind<Window>().To<AddRecommendationWindow>().Named("AddRecommendation");
            Bind<Window>().To<EquipmentsWindow>().Named("Equipment");

            Bind<IDialogWindowFactory>().To<DialogWindowFactory>();
        }
    }
}
