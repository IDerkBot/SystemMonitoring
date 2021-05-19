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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SystemMonitoring
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainPage.Navigate(new Auth());
            ManagerPage.Page = MainPage;
        }

        void MainPage_ContentRendered(object sender, EventArgs e)
        {
            
        }

        void ChangedSizeWindow(object sender, SizeChangedEventArgs e)
        {
            if(MainW.WindowState == WindowState.Maximized)
                DataBank.SizeWindow = SystemParameters.PrimaryScreenWidth;
            else DataBank.SizeWindow = MainW.Width;
        }
    }
}
