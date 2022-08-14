using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AaAFP2
{
    public class StatusEnterpriseViewModel : BaseViewModel
    {
        private FinanceModel financeModel;
        private RecommendationModel recommendationModel;
        private TimePeriod timePeriod;

        public StatusEnterpriseViewModel() 
        {
            financeModel = ninjectKernel.Get<FinanceModel>();
            recommendationModel = ninjectKernel.Get<RecommendationModel>();
            timePeriod = new TimePeriod(TypeTimePeriod.Month, DateTime.Today);
        }

        public int CountCurrentOrders
        {
            get 
            {
                return DbEntities.Orders.Local.Where(o => o.IdStatusOrder != (int)TypeStatusOrder.Completed).Count();
            }
        }
        public TimePeriod TimePeriod
        {
            get 
            {
                return timePeriod;
            }
        }
        public List<FinancialIndicators> CurrentFinance 
        {
            get 
            {
                var value = new List<FinancialIndicators>() { financeModel.GetFinancialIndicators(TimePeriod.Start, DateTime.Today) };
                return value;
            }
        }
        public List<FinancialIndicators> PredictedFinance 
        {
            get 
            {
                var value = new List<FinancialIndicators>() { financeModel.GetFinancialIndicators(TimePeriod.Start, TimePeriod.End) };
                return value;
            }
        }
        public FinanceModel Finance 
        {
            get 
            {
                return financeModel;
            }
        
        }
        public Recommendation Recommendation 
        {
            get 
            {
                var recomendation = recommendationModel.FindRecommendation();
                return recomendation;
            }
        }
    }
}
