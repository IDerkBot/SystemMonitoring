using System.Windows;
using System.Windows.Controls;

namespace SystemMonitoring
{
    public partial class AdminMenu : Page
    {
        public AdminMenu() { InitializeComponent(); }
        void BtnMoveField_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new FieldSelect()); }
        void BtnMoveReports_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new Reports()); }
        void BtnMoveDump_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new Dump()); }
        void BtnMoveUsers_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new Users()); }
    }
}