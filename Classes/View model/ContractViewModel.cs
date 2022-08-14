using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AaAFP2
{
    public class ContractViewModel : DbEntityViewModel
    {
        private int idOrder;
        private decimal orderPrimeCost;
        private ProductModel productModel;

        public ContractViewModel() : base() 
        {
            CurrentDbEntity = new Contract();
            productModel = new ProductModel();
        }

        public new Contract CurrentDbEntity
        {
            get 
            {
                return (Contract)currentDbEntity;
            }
            set 
            {
                currentDbEntity = value;
                NotifyPropertyChanged();
            }
        }

        public int IdOrder 
        {
            get 
            {
                return idOrder;
            }
            set 
            {
                idOrder = value;
                CurrentDbEntity.IdOrder = value;
                OrderPrimeCost = productModel.GetPrimeCostOrder(idOrder);
                NotifyPropertyChanged();
            }
        }

        public decimal OrderPrimeCost 
        {
            get 
            {
                return orderPrimeCost;
            }
            set 
            {
                orderPrimeCost = value;
                NotifyPropertyChanged();
            }
        }

        public override void SetCurrentDbEntity(object dbEntity)
        {
            if (dbEntity is Contract contract) 
            {
                CurrentDbEntity = contract;
                IdOrder = contract.IdOrder;
            }
        }
    }
}
