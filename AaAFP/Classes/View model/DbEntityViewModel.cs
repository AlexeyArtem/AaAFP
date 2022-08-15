using System.Windows.Input;
using System.Data.Entity;
using System.ComponentModel;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AaAFP2
{
    public class DbEntityViewModel : BaseViewModel
    {
        protected object currentDbEntity;
        protected bool isUndoChanges;

        public DbEntityViewModel() : base()
        {
            isUndoChanges = true;
            AddCopyCurrentDbEntityCommand = new RelayCommand(AddCopyCurrentDbEntity);
            AddOrUpdateCurrentEntityCommand = new RelayCommand(AddOrUpdateCurrentDbEntity);
            UndoChangesCommand = new RelayCommand(UndoChanges);
        }

        public virtual object CurrentDbEntity 
        {
            get 
            {
                return currentDbEntity;
            }
            set 
            {
                currentDbEntity = value;
                NotifyPropertyChanged();
            }
        }
        public ICommand AddCopyCurrentDbEntityCommand { get; }
        public ICommand AddOrUpdateCurrentEntityCommand { get; }
        public ICommand UndoChangesCommand { get; }

        protected object CopyObject(object obj) 
        {
            Type objectType = obj.GetType();
            object copyObject = Activator.CreateInstance(objectType);

            FieldInfo[] myObjectFields = obj.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo fi in myObjectFields)
            {
                fi.SetValue(copyObject, fi.GetValue(obj));
            }

            return copyObject;
        }

        protected virtual void AddOrUpdateCurrentDbEntity(object parameter)
        {
            isUndoChanges = false;
            var entry = DbEntities.Entry(currentDbEntity);

            if (entry.State == EntityState.Detached)
                AddDbEntity(currentDbEntity);
            SaveChangesInDb(parameter);
        }

        protected virtual void AddCopyCurrentDbEntity(object parameter) 
        {
            object obj = CopyObject(currentDbEntity);
            AddDbEntity(obj);
        }

        public virtual void SetCurrentDbEntity(object dbEntity) 
        {
            CurrentDbEntity = dbEntity;
        }

        public virtual void UndoChanges(object parameter) 
        {
            if (isUndoChanges) 
            {
                DbEntities.UndoUnsavedChanges();
            }
        }
    }
}
