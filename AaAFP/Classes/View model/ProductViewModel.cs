using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Data.Entity;

namespace AaAFP2
{
    class ProductViewModel : DbEntityViewModel
    {
        public ProductViewModel() : base()
        {
            CurrentDbEntity = new Product();
            CurrentProductMaterial = new ProductMaterial() { QuantityMaterial = 1, UnitPrice = 1 };
            CurrentOperationProduct = new OperationProduct() { Quantity = 1 };

            AddMaterialCommand = new RelayCommand(AddMaterial);
            RemoveMaterialCommand = new RelayCommand(RemoveMaterial);
            AddOperationCommand = new RelayCommand(AddOperation);
            RemoveOperationCommand = new RelayCommand(RemoveOperation);
        }
        public new Product CurrentDbEntity
        {
            get 
            {
                return (Product)currentDbEntity;
            }
            set 
            {
                currentDbEntity = value;
                Materials = new ObservableCollection<ProductMaterial>(value.products_materials);
                Operations = new ObservableCollection<OperationProduct>(value.operaions_products);
            }
        }
        public ObservableCollection<ProductMaterial> Materials { get; private set; }
        public ObservableCollection<OperationProduct> Operations { get; private set; }
        
        public ICommand AddMaterialCommand { get; }
        public ICommand RemoveMaterialCommand { get; }
        public ICommand AddOperationCommand { get; }
        public ICommand RemoveOperationCommand { get; }

        public ProductMaterial CurrentProductMaterial { get; }
        public OperationProduct CurrentOperationProduct { get; }

        private void AddMaterial(object parameter) 
        {
            ProductMaterial productMaterial = new ProductMaterial()
            {
                IdProduct = CurrentDbEntity.ID,
                IdMaterial = CurrentProductMaterial.IdMaterial,
                QuantityMaterial = CurrentProductMaterial.QuantityMaterial,
                UnitPrice = CurrentProductMaterial.UnitPrice,
                material = DbEntities.Materials.Where(m => m.ID == CurrentProductMaterial.IdMaterial).FirstOrDefault()
            };
            Materials.Add(productMaterial);
            AddDbEntity(productMaterial);
        }

        private void RemoveMaterial(object parameter) 
        {
            if (parameter is ProductMaterial pm) 
            {
                RemoveDbEntity(pm);
                Materials.Remove(pm);
            }
        }

        private void AddOperation(object parameter)
        {
            OperationProduct operationProduct = new OperationProduct()
            {
                IdProduct = CurrentDbEntity.ID,
                IdOperation = CurrentOperationProduct.IdOperation,
                Quantity = CurrentOperationProduct.Quantity,
                manufacturing_operations = DbEntities.ManufacturingOperations.Where(o => o.ID == CurrentOperationProduct.IdOperation).FirstOrDefault()
            };
            Operations.Add(operationProduct);
            AddDbEntity(operationProduct);
        }

        private void RemoveOperation(object parameter) 
        {
            if (parameter is OperationProduct op) 
            {
                RemoveDbEntity(op);
                Operations.Remove(op);
            }
        }

        protected override void AddOrUpdateCurrentDbEntity(object parameter)
        {
            isUndoChanges = false;
            var entry = DbEntities.Entry(currentDbEntity);

            if (entry.State == EntityState.Detached) 
            {
                AddDbEntity(CurrentDbEntity);
                SaveChangesInDb(parameter);

                foreach (var material in Materials)
                    material.IdProduct = CurrentDbEntity.ID;

                foreach (var operation in Operations)
                    operation.IdProduct = CurrentDbEntity.ID;
            }
            SaveChangesInDb(parameter);
        }

        public override void SetCurrentDbEntity(object dbEntity)
        {
            if (dbEntity is Product product) 
            {
                CurrentDbEntity = product;
            }
        }
    }
}
