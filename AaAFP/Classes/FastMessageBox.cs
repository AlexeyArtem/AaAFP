using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AaAFP2
{
    static class FastMessageBox
    {
        public static void ShowInformation(string caption)
        {
            MessageBox.Show(caption, "Уведомление", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void ShowError(string caption) 
        {
            MessageBox.Show(caption, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
