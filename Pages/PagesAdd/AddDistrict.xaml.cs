using System;
using System.Windows;
using System.Windows.Controls;

namespace SystemMonitoring.Pages.PagesAdd
{
    /// <summary>
    /// Логика взаимодействия для AddDistrict.xaml
    /// </summary>
    public partial class AddDistrict : Page
    {
        public AddDistrict()
        {
            InitializeComponent();
        }
        void Cancel_Click(object sender, RoutedEventArgs e)
        { ManagerPage.Page.GoBack(); }
        void Add_Click(object sender, RoutedEventArgs e)
        {
            var name = DistrictName.Text;
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }
            DataBank.DistrictName = name;
            ManagerPage.Page.GoBack();
        }
    }
}
