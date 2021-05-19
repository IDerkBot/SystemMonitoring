using System.Windows;

namespace SystemMonitoring
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        void BtnBackMove_Click(object sender, RoutedEventArgs e)
        { ManagerPage.Page.GoBack(); }
    }
}
