using MahApps.Metro.Controls;
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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls.Dialogs;

namespace AaAFP2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    
    public partial class MainWindow : MetroWindow
    {
        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = DataContext as MainViewModel;
        }

        private void DgOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkBoxFilterProductsByOrder?.IsChecked != null && (bool)checkBoxFilterProductsByOrder?.IsChecked) 
            {
                if(DgOrders.SelectedItem is Order order)
                {
                    DgProducts.ItemsSource = viewModel.DbEntities.Products.Local.Where(p => p.IdOrder == order.ID);  
                }
            }
        }

        private void DgEmployees_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkBoxFilterRwpByEmployee?.IsChecked != null && (bool)checkBoxFilterRwpByEmployee?.IsChecked)
            {
                if (DgEmployees.SelectedItem is Employee employee)
                {
                    DgAccrualPointsRwp.ItemsSource = viewModel.DbEntities.AccrualsPointsRwp.Local.Where(a => a.IdEmployee == employee.ID);
                }
            }
        }

        private void DgContracts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (checkBoxFilterPaymentsByContract?.IsChecked != null && (bool)checkBoxFilterPaymentsByContract?.IsChecked)
            {
                if (DgContracts.SelectedItem is Contract contract)
                {
                    DgContractPayments.ItemsSource = viewModel.DbEntities.ContractPayments.Local.Where(p => p.IdContract == contract.ID);
                }
            }
        }

        private void CheckBoxFilterProductsByOrder_Checked(object sender, RoutedEventArgs e)
        {
            DgOrders_SelectionChanged(null, null);
        }

        private void CheckBoxFilterProductsByOrder_Unchecked(object sender, RoutedEventArgs e)
        {
            DgProducts.ItemsSource = viewModel.DbEntities.Products.Local;
        }

        private void CheckBoxFilterPaymentsByContract_Checked(object sender, RoutedEventArgs e)
        {
            DgContracts_SelectionChanged(null, null);
        }

        private void CheckBoxFilterPaymentsByContract_Unchecked(object sender, RoutedEventArgs e)
        {
            DgContractPayments.ItemsSource = viewModel.DbEntities.ContractPayments.Local;
        }

        private void CheckBoxFilterRwpByEmployee_Checked(object sender, RoutedEventArgs e)
        {
            DgEmployees_SelectionChanged(null, null);
        }

        private void CheckBoxFilterRwpByEmployee_Unchecked(object sender, RoutedEventArgs e)
        {
            DgAccrualPointsRwp.ItemsSource = viewModel.DbEntities.AccrualsPointsRwp.Local;
        }
    }
}
