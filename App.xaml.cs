using System.Windows;

namespace SystemMonitoring
{
    public partial class App : Application
    {
        void BtnBackMove_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerPage.Page.CanGoBack)
            { ManagerPage.Page.GoBack(); }
        }
    }
}