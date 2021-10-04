using System.Windows;
using System.Windows.Controls;

namespace SystemMonitoring
{
    public partial class AdminMenu : Page
    {
        public AdminMenu() { InitializeComponent(); }
        void BtnMoveField_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new AdminPages.FieldSelect()); }
        void BtnMoveReports_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new AdminPages.Reports()); }
        void BtnMoveCultures_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new AdminPages.Cultures()); }
        void BtnMoveUsers_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new AdminPages.Users()); }
    }
}