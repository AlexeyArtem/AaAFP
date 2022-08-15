using Ninject;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AaAFP2
{
    public class FixedCostsViewModel : DbEntityViewModel
    {
        private TypeTimePeriod typePeriod;
        private bool isMontly;
        private bool isWeekly;
        private FixedCostsModel fixedCostsModel;

        public FixedCostsViewModel() : base()
        {
            CurrentDbEntity = new FixedCost();
            fixedCostsModel = ninjectKernel.Get<FixedCostsModel>();
        }

        public new FixedCost CurrentDbEntity
        {
            get 
            {
                return (FixedCost)currentDbEntity;
            }
            set 
            {
                var fixedCost = value;
                currentDbEntity = fixedCost;

                WeeklyCost weekCost = DbEntities.WeeklyCosts.Local.Where(c => c.IdFixedCost == fixedCost.ID).FirstOrDefault();
                MonthlyCost monthlyCost = DbEntities.MonthlyCosts.Local.Where(c => c.IdFixedCost == fixedCost.ID).FirstOrDefault();
                if (weekCost != null)
                {
                    IsWeekly = true;
                    DayAccrual = weekCost.DayAccrual;
                }
                else if (monthlyCost != null)
                {
                    IsMontly = true;
                    DayAccrual = monthlyCost.DayAccrual;
                }

                NotifyPropertyChanged();
            }
        }
        public int DayAccrual { get; set; } = 1;

        public bool IsMontly 
        {
            get 
            {
                return isMontly;
            }
            set 
            {
                if (value) typePeriod = TypeTimePeriod.Month;
                isMontly = value;
            }
        }

        public bool IsWeekly
        {
            get
            {
                return isWeekly;
            }
            set
            {
                if (value) typePeriod = TypeTimePeriod.Week;
                isWeekly = value;
            }
        }

        protected override void AddOrUpdateCurrentDbEntity(object parameter)
        {
            isUndoChanges = false;
            var entry = DbEntities.Entry(currentDbEntity);

            if (entry.State == EntityState.Detached)
                fixedCostsModel.AddCost(CurrentDbEntity, typePeriod, DayAccrual);

            SaveChangesInDb(parameter);
        }

        public override void SetCurrentDbEntity(object dbEntity)
        {
            if (dbEntity is FixedCost fixedCost)
                CurrentDbEntity = fixedCost;
        }
    }
}
