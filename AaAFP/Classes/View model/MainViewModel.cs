using Ninject;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AaAFP2
{
    class MainViewModel : BaseViewModel
    {
        private DialogsWindows dialogsWindows;

        private ReportsModel reportsModel;
        private MaterialModel materialModel;
        private SalaryModel salaryModel;
        private FixedCostsModel costsModel;

        private ICommand refreshShortageMaterialsCommand;
        private ICommand refreshAvailableMaterialsCommand;
        private ICommand refreshSalaryReportCommand;

        public MainViewModel() : base() 
        {
            dialogsWindows = ninjectKernel.Get<DialogsWindows>();
            materialModel = ninjectKernel.Get<MaterialModel>();
            salaryModel = ninjectKernel.Get<SalaryModel>();
            reportsModel = ninjectKernel.Get<ReportsModel>();
            costsModel = ninjectKernel.Get<FixedCostsModel>();

            ReportsViewModel = new ReportsViewModel();

            OpenDialogWindowCommand = new RelayCommand(OpenDialogWindow);
            OpenAddDbEntityDialogCommand = new RelayCommand(OpenAddDbEntityDialog);
            OpenEditDbEntityDialogCommand = new RelayCommand(OpenEditDbEntityDialog);
            PaySalariesCommand = new RelayCommand(PaySalaries);
        }

        public ICommand OpenDialogWindowCommand { get; }
        public ICommand OpenAddDbEntityDialogCommand { get; }
        public ICommand OpenEditDbEntityDialogCommand { get; }
        public ICommand PaySalariesCommand { get; }
        public ICommand RefreshSalaryReportCommand 
        {
            get 
            {
                return refreshSalaryReportCommand ?? (refreshSalaryReportCommand = new RelayCommand(obj => NotifyPropertyChanged("SalaryReport")));
            }
        }
        public ICommand RefreshAvailableMaterialsCommand 
        {
            get 
            {
                return refreshAvailableMaterialsCommand ?? (refreshAvailableMaterialsCommand = new RelayCommand(obj => NotifyPropertyChanged("AvailableMaterials")));
            }
        }
        public ICommand RefreshShortageMaterialsCommand
        {
            get
            {
                return refreshShortageMaterialsCommand ?? (refreshShortageMaterialsCommand = new RelayCommand(obj => { NotifyPropertyChanged("ShortageMaterials"); }));
            }
        }
        public ReportsViewModel ReportsViewModel { get; }
        public DataView SalaryReport 
        {
            get 
            {
                var report = reportsModel.GetSalaryReport();
                return report.DefaultView;
            }
        }
        public ObservableCollection<RecordMaterial> AvailableMaterials => materialModel.AvailableMaterials;
        public ObservableCollection<RecordMaterial> ShortageMaterials => materialModel.ShortageMaterials;

        private void PaySalaries(object parameter) 
        {
            try
            {
                salaryModel.PaySalaries();
                NotifyPropertyChanged("SalaryReport");
            }
            catch 
            {
                FastMessageBox.ShowError("При выплате зарплат возникла неизвестная ошибка. Попробуйте еще раз.");
            }
        }

        private void OpenDialogWindow(object parameter) 
        {
            string name = (parameter as string) ?? "";
            dialogsWindows.ShowDialog(name);
        }

        private void OpenAddDbEntityDialog(object parameter) 
        {
            if (parameter is Type typeEntity)
            {
                dialogsWindows.ShowAddDbEntityDialog(typeEntity);
            }
        }

        private void OpenEditDbEntityDialog(object parameter)
        {
            if (parameter == null)
            {
                FastMessageBox.ShowInformation("Выберите данные для редактирования.");
                return;
            }
            dialogsWindows.ShowEditDbEntityDialog(parameter);
        }

        protected override void RemoveDbEntity(object parameter)
        {
            if (parameter == null)
            {
                FastMessageBox.ShowInformation("Выберите строку для удаления.");
                return;
            }

            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выполнить удаление?", "Уведомление", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    base.RemoveDbEntity(parameter);
                    SaveChangesInDb(null);
                }
                catch (Exception)
                {
                    FastMessageBox.ShowError("При удалении возникла неизветная ошибка. Попробуйте еще раз.");
                }
            }
        }
    }
}
