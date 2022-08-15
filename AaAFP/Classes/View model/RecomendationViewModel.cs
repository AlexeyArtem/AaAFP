using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AaAFP2
{
    public class RecomendationViewModel : DbEntityViewModel
    {
        private ICommand openAddRecommendationDialog;
        private DialogsWindows dialogs;

        public RecomendationViewModel() : base()
        {
            CurrentDbEntity = new Recommendation();
            CurrentDbEntity.conditions_finace = new ConditionFinance() { IsPredictedProfitNegative = 0, IsWorkingCapitalEnoughFixedCosts = 0, IsWokingCapitalEnoughMaterial = 0, IsWorkingCapitalEnoughSalary = 0, IsWorkingCapitalNotEnough = 0 };
            AddDbEntity(CurrentDbEntity.conditions_finace);
            CurrentDbEntity.conditions_manufacturing = new ConditionManufacturing() { ProductsProductionExceedsPredicted = 0 };
            AddDbEntity(CurrentDbEntity.conditions_manufacturing);
            CurrentDbEntity.conditions_orders_and_employees = new ConditionOrdersAndEmployees() { HasCompletedOrdersUnpaid = 0, HasEmployeesManyPosition = 0, HasIncompleteOrdersProductionTimeViolations = 0 };
            AddDbEntity(CurrentDbEntity.conditions_orders_and_employees);

            dialogs = ninjectKernel.Get<DialogsWindows>();
        }

        public ICommand OpenAddRecommendationDialog 
        {
            get 
            {
                return openAddRecommendationDialog ?? (openAddRecommendationDialog = new RelayCommand(obj => dialogs.ShowDialog("AddRecommendation")));
            }
        }

        public new Recommendation CurrentDbEntity
        {
            get
            {
                return (Recommendation)currentDbEntity;
            }
            set
            {
                currentDbEntity = value;
            }
        }

        protected override void RemoveDbEntity(object parameter)
        {
            if (parameter is Recommendation recommendation) 
            {
                var conditionFinance = recommendation.conditions_finace;
                var conditionManufacturing = recommendation.conditions_manufacturing;
                var conditionsOrdersAndEmployees = recommendation.conditions_orders_and_employees;

                base.RemoveDbEntity(recommendation);
                base.RemoveDbEntity(conditionFinance);
                base.RemoveDbEntity(conditionManufacturing);
                base.RemoveDbEntity(conditionsOrdersAndEmployees);
            }
        }

        public override void SetCurrentDbEntity(object dbEntity)
        {
            if (dbEntity is Recommendation recommendation)
            {
                CurrentDbEntity = recommendation;
            }
        }
    }
}
