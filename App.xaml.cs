using System.Windows;

namespace SystemMonitoring
{
    public partial class App : Application
    {
        void BtnBackMove_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerPage.Page.Content.ToString().Contains("FieldMonitoring"))
                ManagerPage.Page.Navigate(new FieldSelect());
            if (ManagerPage.Page.Content.ToString().Contains("FieldSelect"))
                ManagerPage.Page.Navigate(new AdminMenu());
            else if (ManagerPage.Page.CanGoBack)
                ManagerPage.Page.GoBack();
        }
    }
}