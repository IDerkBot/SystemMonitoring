using System;
using System.Windows;

namespace SystemMonitoring
{
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
            if (ManagerPage.Page.Content.ToString().Contains("Auth") || ManagerPage.Page.Content.ToString().Contains("AdminMenu"))
                Back.Visibility = Visibility.Hidden;
            else Back.Visibility = Visibility.Visible;
            if (ManagerPage.Page.Content.ToString().Contains("FieldMonitoring"))
                ManagerPage.FieldMonitoringPage.NavigateLoad();
        }
        void ChangedSizeWindow(object sender, SizeChangedEventArgs e)
        {
            if(MainW.WindowState == WindowState.Maximized)
                DB.SizeWindow = SystemParameters.PrimaryScreenWidth;
            else DB.SizeWindow = MainW.Width;
        }
        void MainW_Closed(object sender, EventArgs e)
        {
            //if (ManagerPage.Page.Content.ToString().Contains("FieldMonitoring")) ManagerPage.FieldMonitoringPage.ClosePort();
            Close();
        }
    }
}