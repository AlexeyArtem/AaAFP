using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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

namespace AaAFP2
{
    /// <summary>
    /// Логика взаимодействия для DataTablePicker.xaml
    /// </summary>
    public partial class DataTablePicker : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(DataTablePicker));
        public static readonly DependencyProperty IdRowProperty = DependencyProperty.Register("IdRow", typeof(string), typeof(DataTablePicker));

        public DataTablePicker()
        {
            InitializeComponent();
        }

        public string TitleWindow { get; set; }

        public IEnumerable ItemsSource
        {
            get { return GetValue(ItemsSourceProperty) as IEnumerable; }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public string IdRow 
        {
            get { return GetValue(IdRowProperty) as string; }
            set { SetValue(IdRowProperty, value); }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DataTablePickerWindow dataTableWindow = new DataTablePickerWindow() { DataContext = this, Title = TitleWindow };
            dataTableWindow.BtSelectRow.Click += delegate(object s, RoutedEventArgs args) 
            {
                DataGrid dataGrid = dataTableWindow.DataGrid;
                if (dataGrid.SelectedItem != null)
                {
                    TextBlock textBlockCell = dataGrid.Columns[0].GetCellContent(dataGrid.SelectedValue) as TextBlock;
                    IdRow = textBlockCell?.Text;
                    dataTableWindow.Close();
                }
                else MessageBox.Show("Необходимо выбрать строку", "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
            };

            dataTableWindow.ShowDialog();
        }
    }
}
