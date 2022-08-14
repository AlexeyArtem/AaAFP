using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace AaAFP2
{
    class MaterialModel : BaseModel
    {
        private ObservableCollection<RecordMaterial> availableMaterials;
        private ObservableCollection<RecordMaterial> shortageMaterials;

        public MaterialModel() 
        {
            availableMaterials = new ObservableCollection<RecordMaterial>();
            shortageMaterials = new ObservableCollection<RecordMaterial>();
            
            foreach (Order order in dbEntities.Orders.Local) 
            {
                INotifyPropertyChanged propertyChanged = (INotifyPropertyChanged)order;
                propertyChanged.PropertyChanged += WriteOffMaterials;
            }
            dbEntities.Orders.Local.CollectionChanged += Orders_CollectionChanged;
        }

        public ObservableCollection<RecordMaterial> AvailableMaterials
        {
            get 
            {
                CalculateAvailableMaterials();
                return availableMaterials;
            }
        }

        public ObservableCollection<RecordMaterial> ShortageMaterials 
        {
            get 
            {
                CalculateShortageMaterials();
                return shortageMaterials;
            }
        }

        private void Orders_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!(sender is ICollection<Order> orders)) return;

            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (Order order in e.NewItems)
                    {
                        INotifyPropertyChanged propertyChanged = (INotifyPropertyChanged)order;
                        propertyChanged.PropertyChanged += WriteOffMaterials;
                    }
                    break;

                case NotifyCollectionChangedAction.Remove:
                    if (e.OldItems is IList<Order> oldOrders)
                    {
                        foreach (Order order in oldOrders)
                        {
                            INotifyPropertyChanged propertyChanged = (INotifyPropertyChanged)order;
                            propertyChanged.PropertyChanged -= WriteOffMaterials;
                        }
                    }
                    break;

                case NotifyCollectionChangedAction.Reset:
                    foreach (Order order in orders)
                    {
                        INotifyPropertyChanged propertyChanged = (INotifyPropertyChanged)order;
                        propertyChanged.PropertyChanged -= WriteOffMaterials;
                    }
                    break;
            }
        }

        private void CalculateAvailableMaterials() 
        {
            //1. Получение всех приобретенных материалов
            var allMaterials = GetAllMaterials();

            //2. Получение списанных материалов
            var writeOffMaterials = GetWriteOffMaterials();

            //3. Получение зарезервированных материалов
            var reservedMaterials = GetReservedMaterials();

            availableMaterials.Clear();
            foreach (var material in dbEntities.Materials.Local)
            {
                var all = allMaterials.Where(a => a.Material.ID == material.ID).FirstOrDefault();
                var wm = writeOffMaterials.Where(w => w.Material.ID == material.ID).FirstOrDefault();
                var rm = reservedMaterials.Where(r => r.Material.ID == material.ID).FirstOrDefault();

                int quantity = (all?.QuantityUnits ?? 0) - (wm?.QuantityUnits ?? 0) - (rm?.QuantityUnits ?? 0);
                if (quantity > 0) 
                {
                    RecordMaterial recordMaterial = new RecordMaterial(material, quantity);
                    availableMaterials.Add(recordMaterial);
                }
            }
        }

        private void CalculateShortageMaterials()
        {
            //1. Получение всех приобретенных материалов
            var allMaterials = GetAllMaterials();

            //2. Получение списанных материалов
            var writeOffMaterials = GetWriteOffMaterials();

            //3. Получение зарезервированных материалов
            var reservedMaterials = GetReservedMaterials();

            shortageMaterials.Clear();
            foreach (var material in dbEntities.Materials.Local)
            {
                var all = allMaterials.Where(a => a.Material.ID == material.ID).FirstOrDefault();
                var wm = writeOffMaterials.Where(w => w.Material.ID == material.ID).FirstOrDefault();
                var rm = reservedMaterials.Where(r => r.Material.ID == material.ID).FirstOrDefault();

                int quantity = (all?.QuantityUnits ?? 0) - (wm?.QuantityUnits ?? 0) - (rm?.QuantityUnits ?? 0);
                if (quantity < 0)
                {
                    RecordMaterial recordMaterial = new RecordMaterial(material, quantity);
                    shortageMaterials.Add(recordMaterial);
                }
            }
        }

        private List<RecordMaterial> GetWriteOffMaterials() 
        {
            var writeOffMaterialsQuery = from w in dbEntities.WriteOffMaterials.Local
                                    group w by w.IdMaterial into g
                                    select new 
                                    { 
                                        Name = g.Key, 
                                        Material = g.Select(w => w.material).FirstOrDefault(), 
                                        Quantity = g.Sum(w => w.QuantityUnits) 
                                    };
            
            List<RecordMaterial> materials = new List<RecordMaterial>();
            foreach (var item in writeOffMaterialsQuery)
            {
                materials.Add(new RecordMaterial(item.Material, item.Quantity));
            }

            return materials;
        }

        private List<RecordMaterial> GetAllMaterials()
        {
            var allMaterialsQuery = from r in dbEntities.RecordsPackingList.Local
                               group r by r.IdMaterial into g
                               select new 
                               { 
                                   Name = g.Key, 
                                   Material = g.Select(r => r.material).FirstOrDefault(), 
                                   Quantity = g.Sum(r => r.QuantityUnits), 
                                   UnitPrice = g.Select(r => r.UnitPrice).FirstOrDefault() 
                               };

            List<RecordMaterial> materials = new List<RecordMaterial>();
            foreach (var item in allMaterialsQuery) 
            {
                materials.Add(new RecordMaterial(item.Material, item.Quantity, item.UnitPrice));
            }

            return materials;
        }

        private List<RecordMaterial> GetReservedMaterials() 
        {
            var reservedMaterialsQuery = from pm in dbEntities.ProductsMaterials.Local
                                    group pm by pm.IdMaterial into g
                                    select new
                                    {
                                        Name = g.Key,
                                        Product = g.Select(pm => pm.product).FirstOrDefault(),
                                        Material = g.Select(pm => pm.material).FirstOrDefault(), 
                                        Quantity = g.Sum(pm => pm.QuantityMaterial)
                                    };

            List<RecordMaterial> materials = new List<RecordMaterial>();
            foreach (var item in reservedMaterialsQuery)
            {
                int quantityProducts = item.Product.Quantity ?? 1;
                materials.Add(new RecordMaterial(item.Material, item.Quantity * quantityProducts));
            }

            return materials;
        }

        public List<RecordMaterial> GetReservedMaterials(Product product)
        {
            int quantityProducts = product.Quantity ?? 1;
            
            List<RecordMaterial> materials = new List<RecordMaterial>();
            foreach (var item in dbEntities.ProductsMaterials.Local.Where(pm => pm.IdProduct == product.ID))
            {
                RecordMaterial recordMaterial = new RecordMaterial(item.material, item.QuantityMaterial * quantityProducts, item.UnitPrice);
                materials.Add(recordMaterial);
            }

            return materials;
        }

        public void WriteOffMaterials(object sender, PropertyChangedEventArgs args) 
        {
            if (!(sender is Order order) || order.IdStatusOrder == null) return;

            if (args.PropertyName == "IdStatusOrder" && (TypeStatusOrder)order.IdStatusOrder == TypeStatusOrder.Completed) 
            {
                foreach (var product in order.products)
                {
                    var materials = GetReservedMaterials(product);
                    foreach (var m in materials)
                    {
                        WriteOffMaterial writeOffMaterial = new WriteOffMaterial()
                        {
                            Date = DateTime.Today,
                            IdMaterial = m.Material.ID,
                            QuantityUnits = m.QuantityUnits
                        };
                        dbEntities.AddEntity(writeOffMaterial);
                    }
                    INotifyPropertyChanged propertyChanged = (INotifyPropertyChanged)order;
                    propertyChanged.PropertyChanged -= WriteOffMaterials;
                }
            }
        }
    }
}
