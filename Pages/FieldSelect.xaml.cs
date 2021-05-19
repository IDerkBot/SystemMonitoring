using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SystemMonitoring
{
    /// <summary>
    /// Логика взаимодействия для FieldSelect.xaml
    /// </summary>
    public partial class FieldSelect : Page
    {
        public FieldSelect()
        { InitializeComponent(); }
        struct TDistricts
        {
            public int id;
            public string name, number, field;
        }
        List<TDistricts> GetDistricts()
        {
            var tbDistrict = new List<TDistricts>();
            TDistricts tmp;
            var cmd = new MySqlCommand("select * from `districts`", Database.Connect());
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    tmp.id = int.Parse(reader["id"].ToString());
                    tmp.name = reader["name"].ToString();
                    tmp.number = reader["number"].ToString();
                    tmp.field = reader["position"].ToString();
                    tbDistrict.Add(tmp);
                }
            }
            return tbDistrict;
        }
        void GetDistrict()
        {
            CBDistrict.ItemsSource = null;
            var districts = new List<string>();
            var tbDistrict = GetDistricts();
            for (int i = 0; i < tbDistrict.Count; i++) districts.Add(tbDistrict[i].name.ToString());
            if (!string.IsNullOrWhiteSpace(DataBank.DistrictName))
            { districts.Add(DataBank.DistrictName); }
            var items = districts.Distinct().ToArray();
            CBDistrict.ItemsSource = items;
        }
        void Load(object sender, RoutedEventArgs e)
        { GetDistrict(); }
        void GetFields()
        {
            CBField.Items.Clear();
            var tbDistrict = GetDistricts();
            var fields = tbDistrict.Where(district => district.name == (string)CBDistrict.SelectedItem).Select(district => district.number);
            foreach (var item in fields) CBField.Items.Add(item);
        }
        void DistrictSelectChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CBDistrict.SelectedItem == null) CBDistrict.SelectedItem = DataBank.SelectDistrict;
            else DataBank.SelectDistrict = CBDistrict.SelectedItem.ToString();
            GB.IsEnabled = true;
            GetFields();
        }

        void FieldDistrictChanged(object sender, SelectionChangedEventArgs e)
        { BtnNext.IsEnabled = true; }

        void Next_Click(object sender, RoutedEventArgs e)
        { ManagerPage.Page.Navigate(new FieldMonitoring()); }

        void AddDistrict_Click(object sender, RoutedEventArgs e)
        { ManagerPage.Page.Navigate(new Pages.PagesAdd.AddDistrict()); }

        void AddField_Click(object sender, RoutedEventArgs e)
        { ManagerPage.Page.Navigate(new Pages.PagesAdd.AddField()); }
    }
}
