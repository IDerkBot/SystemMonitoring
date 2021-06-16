using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SystemMonitoring
{
    public partial class FieldSelect : Page
    {
        public FieldSelect()
        {
            InitializeComponent();
            CBDistrict.ItemsSource = dbMonitoringEntities.gc().Fields.Select(x => x.District).ToList();
        }
        void DistrictSelectChanged(object sender, SelectionChangedEventArgs e)
        {
            GB.IsEnabled = true;
            string selectDistrict = (sender as ComboBox).SelectedItem.ToString();
            CBField.ItemsSource = dbMonitoringEntities.gc().Fields.Where(x => x.District.Contains(selectDistrict)).Select(x => x.Number).ToList();
        }
        void FieldDistrictChanged(object sender, SelectionChangedEventArgs e) { BtnNext.IsEnabled = true; }
        void Next_Click(object sender, RoutedEventArgs e) {
            Field field = dbMonitoringEntities.gc().Fields.Where(x => x.District == CBDistrict.SelectedItem.ToString() && x.Number == CBField.SelectedItem.ToString()).Single();
            Seeding seed = new Seeding();
            if(dbMonitoringEntities.gc().Seedings.Where(x => x.IdField == field.Id).ToList().Count > 0)
                seed = dbMonitoringEntities.gc().Seedings.Where(x => x.IdField == field.Id).Single();
            else
            {
                seed.IdField = field.Id;
                dbMonitoringEntities.gc().Seedings.Add(seed);
                dbMonitoringEntities.gc().SaveChanges();
            }
            DB.SelectSeeding = seed;
            ManagerPage.FieldMonitoringPage = new FieldMonitoring();
            ManagerPage.Page.Navigate(ManagerPage.FieldMonitoringPage);
        }
        void AddDistrict_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new Pages.PagesAdd.AddDistrict()); }
        void AddField_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new Pages.PagesAdd.AddField(CBDistrict.SelectedItem.ToString())); }
        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(DB.DistrictName))
            {
                List<string> districts = dbMonitoringEntities.gc().Fields.Select(x => x.District).ToList();
                districts.Add(DB.DistrictName);
                CBDistrict.ItemsSource = districts;
                CBDistrict.SelectedItem = DB.DistrictName;
                var fields = dbMonitoringEntities.gc().Fields.Where(x => x.District == DB.DistrictName).ToList();
                if (fields.Count() > 0)
                    CBField.ItemsSource = fields.Select(x => x.Number);
            }
        }
    }
}