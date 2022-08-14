using MySql.Data.MySqlClient;
using Ninject;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AaAFP2
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected static IKernel ninjectKernel = App.NinjectKernel;
        public BaseViewModel()
        {
            AddDbEntityCommand = new RelayCommand(AddDbEntity);
            RemoveDbEntityCommand = new RelayCommand(RemoveDbEntity);
            SaveChangesInDbCommand = new RelayCommand(SaveChangesInDb);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BaseDbEntities DbEntities { get; } = App.DbEntities;
        public ICommand AddDbEntityCommand { get; private set; }
        public ICommand RemoveDbEntityCommand { get; private set; }
        public ICommand SaveChangesInDbCommand { get; private set; }

        protected void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void SaveChangesInDb(object parameter) 
        {
            try
            {
                DbEntities.SaveChanges();
            }
            catch (Exception)
            {
                FastMessageBox.ShowError("При сохранении возникла ошибка. Попробуйте еще раз.");
            }
        }

        protected virtual void AddDbEntity(object dbEntity) 
        {
            try
            {
                if (dbEntity != null)
                    DbEntities.AddEntity(dbEntity);
            }
            catch (Exception)
            {
                FastMessageBox.ShowError("При добавлении возникла ошибка. Возможно, не все данные были заполнены. Попробуйте еще раз.");
            }
        }

        protected virtual void RemoveDbEntity(object dbEntity) 
        {
            try
            {
                if (dbEntity != null)
                    DbEntities.RemoveEntity(dbEntity);
            }
            catch (Exception) 
            {
                FastMessageBox.ShowError("При удалении возникла неизвестная ошибка. Попробуйте еще раз.");
            }
        }
    }
}
