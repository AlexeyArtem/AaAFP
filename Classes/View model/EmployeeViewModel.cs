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
    public class EmployeeViewModel : DbEntityViewModel
    {
        private ObservableCollection<EmployeePosition> positions;
        public EmployeeViewModel() : base()
        {
            CurrentDbEntity = new Employee();
            AddPositionCommand = new RelayCommand(AddPosition);
            RemovePositionCommand = new RelayCommand(RemovePosition);
        }

        public new Employee CurrentDbEntity
        {
            get 
            {
                return (Employee)currentDbEntity;
            }
            set 
            {
                currentDbEntity = value;
                Positions = new ObservableCollection<EmployeePosition>(value.employees_positions);
                NotifyPropertyChanged();
            }
        }
        public decimal Salary { get; set; }
        public int CurrentIdPosition { get; set; }
        public ObservableCollection<EmployeePosition> Positions 
        {
            get
            {
                return positions;
            }
            private set 
            {
                positions = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand AddPositionCommand { get; }
        public ICommand RemovePositionCommand { get; }

        private void AddPosition(object parameter)
        {
            if (CurrentDbEntity.employees_positions.Where(p => p.IdPosition == CurrentIdPosition).Count() == 1) 
            {
                FastMessageBox.ShowInformation("Должность уже есть у сотрудника.");
                return;
            }

            var position = DbEntities.Positions.Local.Where(p => p.ID == CurrentIdPosition).FirstOrDefault();
            if (position == null) 
            {
                FastMessageBox.ShowInformation("Выберите существующую должность.");
                return;
            }

            EmployeePosition employeePosition = new EmployeePosition()
            {
                IdEmployee = CurrentDbEntity.ID,
                IdPosition = CurrentIdPosition,
                position = position
            };
            Positions.Add(employeePosition);
            AddDbEntity(employeePosition);
        }

        private void RemovePosition(object parameter)
        {
            if (parameter is EmployeePosition position)
            {
                Positions.Remove(position);
                RemoveDbEntity(position);
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

                foreach (var p in Positions)
                    p.IdEmployee = CurrentDbEntity.ID;
            }
            SaveChangesInDb(parameter);
        }

        public override void SetCurrentDbEntity(object dbEntity)
        {
            if (dbEntity is Employee employee)
                CurrentDbEntity = employee;
        }
    }
}
