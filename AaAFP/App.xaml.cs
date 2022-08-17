using MySql.Data.MySqlClient;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Core;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AaAFP2
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly BaseDbEntities DbEntities;
        public static readonly IKernel NinjectKernel;

        static App()
        {
            NinjectKernel = new StandardKernel(new ModelsModule(), new DialogWindowModule());
            DbEntities = NinjectKernel.Get<BaseDbEntities>();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Thread thread = new Thread(LoadSplashScreen);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();

            try
            {
                DbEntities.Load();
                MainWindow mainWindow = new MainWindow();

                thread.Abort();
                mainWindow.Show();
                mainWindow.Activate();
                FastMessageBox.ShowInformation("Данная версия программы является ознакомительной. Все внесенные изменения сохраняются только в рамках текущего сеанса. После перезапуска программы все изменения буду удалены.");
            }
            catch (Exception ex)
            {
                thread.Abort();

                if (ex is MySqlException || ex is EntityException)
                {
                    FastMessageBox.ShowError("При соединении с сервером возникла ошибка. В работе сервера возможны перебои. Проверьте соединение с интернетом и повторите попытку.");
                }
                else if (ex.Message.Contains("MySql.Data.MySqlClient"))
                {
                    FastMessageBox.ShowError("При запуске программы возникла ошибка. Для работы программы необходимо установить MySQL Connector/NET версии 6.10.7.");
                }
                else
                {
                    FastMessageBox.ShowError("При запуске программы возникла неизвестная ошибка. Подробности: " + ex.Message);
                }
                Shutdown();
            }
        }

        private void LoadSplashScreen() 
        {
            SplashScreenWindow splashScreen = new SplashScreenWindow();
            splashScreen.Show();
            System.Windows.Threading.Dispatcher.Run();
        }
    }
}
