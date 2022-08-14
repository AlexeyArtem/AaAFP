using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AaAFP2
{
    class PackingListViewModel : DbEntityViewModel
    {
        public PackingListViewModel() : base()
        {
            CurrentDbEntity = new PackingList();
            CurrentRecord = new RecordPackingList() { QuantityUnits = 1, UnitPrice = 1 };

            AddRecordCommand = new RelayCommand(AddRecord);
            RemoveRecordCommand = new RelayCommand(RemoveRecord);
        }
        public new PackingList CurrentDbEntity
        {
            get 
            {
                return (PackingList)currentDbEntity;
            }
            set 
            {
                currentDbEntity = value;
                RecordsPackingList = new ObservableCollection<RecordPackingList>(value.records_packing_list);
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<RecordPackingList> RecordsPackingList { get; private set; }
        public ICommand AddRecordCommand { get; }
        public ICommand RemoveRecordCommand { get; }
        public RecordPackingList CurrentRecord { get; }

        private void AddRecord(object parameter)
        {
            RecordPackingList record = new RecordPackingList()
            {
                IdMaterial = CurrentRecord.IdMaterial,
                QuantityUnits = CurrentRecord.QuantityUnits,
                UnitPrice = CurrentRecord.UnitPrice,
                material = DbEntities.Materials.Where(m => m.ID == CurrentRecord.IdMaterial).FirstOrDefault()
            };
            CurrentDbEntity.records_packing_list.Add(record);
            RecordsPackingList.Add(record);
        }

        private void RemoveRecord(object parameter)
        {
            if (parameter is RecordPackingList record)
            {
                RemoveDbEntity(record);
                RecordsPackingList.Remove(record);
            }
        }

        public override void SetCurrentDbEntity(object dbEntity)
        {
            if (dbEntity is PackingList packingList)
                CurrentDbEntity = packingList;
        }

    }
}
