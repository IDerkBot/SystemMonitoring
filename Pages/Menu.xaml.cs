using System.Windows;
using System.Windows.Controls;

namespace SystemMonitoring
{
    /// <summary>
    /// Логика взаимодействия для Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
        public Menu()
        {
            InitializeComponent();
        }

        void BtnMoveField_Click(object sender, RoutedEventArgs e)
        { ManagerPage.Page.Navigate(new FieldSelect()); }
        void BtnMoveReports_Click(object sender, RoutedEventArgs e)
        { ManagerPage.Page.Navigate(new Reports()); }
        void BtnMoveDump_Click(object sender, RoutedEventArgs e)
        { ManagerPage.Page.Navigate(new Dump()); }
        void BtnMoveUsers_Click(object sender, RoutedEventArgs e)
        { ManagerPage.Page.Navigate(new Users()); }
    }
}