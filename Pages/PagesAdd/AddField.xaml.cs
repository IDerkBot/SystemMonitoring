using MySql.Data.MySqlClient;
using System.Windows;
using System.Windows.Controls;

namespace SystemMonitoring.Pages.PagesAdd
{
    /// <summary>
    /// Логика взаимодействия для AddField.xaml
    /// </summary>
    public partial class AddField : Page
    {
        public AddField()
        {
            InitializeComponent();
        }
        void Cancel_Click(object sender, RoutedEventArgs e)
        { if (ManagerPage.Page.CanGoBack) ManagerPage.Page.GoBack(); }
        void Add_Click(object sender, RoutedEventArgs e)
        {
            var cmd = new MySqlCommand($"select * `districts` Where `name` = '{DataBank.SelectDistrict}' and `number` = '{FieldValue.Text}'", Database.Connect()).ExecuteNonQuery();
            if (cmd >= 1) { MessageBox.Show("Поле с таким номером в этом районе уже добавлено!", "Error"); }
            else
            {
                new MySqlCommand($"INSERT INTO `districts`(`name`, `number`) VALUES('{DataBank.SelectDistrict}', '{FieldValue.Text}')", Database.Connect()).ExecuteNonQuery();
                if (ManagerPage.Page.CanGoBack) ManagerPage.Page.GoBack();
            }
        }
    }
}
