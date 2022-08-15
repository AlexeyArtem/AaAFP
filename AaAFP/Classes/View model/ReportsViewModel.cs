using Ninject;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AaAFP2
{
    enum TypeReport 
    {
        OnClients,
        OnEmployees,
        OnFinance,
        OnOrders,
        OnSalaries
    }

    class ReportsViewModel : BaseViewModel
    {
        private ReportsModel reportsModel;
        private DataView currentReport;
        private ICommand refreshReportCommand;

        public ReportsViewModel() 
        {
            this.reportsModel = ninjectKernel.Get<ReportsModel>();
            refreshReportCommand = new RelayCommand(RefreshReport);
            DateStart = DateTime.Today;
            DateEnd = DateStart;
        }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public TypeReport CurrentTypeReport { get; set; }
        public DataView CurrentReport 
        {
            get 
            {
                return currentReport;
            }
            private set 
            {
                currentReport = value;
                NotifyPropertyChanged();
            } 
        }
        public ICommand RefreshReportCommand => refreshReportCommand;

        public void RefreshReport(object parameter) 
        {
            try
            {
                switch (CurrentTypeReport)
                {
                    case TypeReport.OnClients:
                        CurrentReport = reportsModel.GetReportOnClients(DateStart, DateEnd).DefaultView;
                        break;
                    case TypeReport.OnEmployees:
                        CurrentReport = reportsModel.GetReportOnEmployees(DateStart, DateEnd).DefaultView;
                        break;
                    case TypeReport.OnFinance:
                        CurrentReport = reportsModel.GetReportOnFinance(DateStart, DateEnd).DefaultView;
                        break;
                    case TypeReport.OnOrders:
                        CurrentReport = reportsModel.GetReportOnOrders(DateStart, DateEnd).DefaultView;
                        break;
                    case TypeReport.OnSalaries:
                        CurrentReport = reportsModel.GetSalaryReport().DefaultView;
                        break;
                }
            }
            catch(Exception ex) 
            {
                FastMessageBox.ShowError("При построении отчета возникла ошибка. Подробности: " + ex.Message);
            }
        }
    }
}
