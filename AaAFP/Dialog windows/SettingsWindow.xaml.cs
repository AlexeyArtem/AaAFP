using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace AaAFP2
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {

        public SettingsWindow(EnterpriseInfo info)
        {
            InitializeComponent();
        }

        private void buttonMakeChanges_Click(object sender, RoutedEventArgs e)
        {
            tbName.IsEnabled = true;
            udCapital.IsEnabled = true;
        }

        private void buttonSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            tbName.IsEnabled = false;
            udCapital.IsEnabled = false;
            FastMessageBox.ShowInformation("Для актуализации данных перезапустите программу.");
        }
    }
}
