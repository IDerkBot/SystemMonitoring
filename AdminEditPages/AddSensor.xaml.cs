using System.Windows;
using System.Windows.Controls;

namespace SystemMonitoring.AdminEditPages
{
    public partial class AddSensor : Page
    {
        public AddSensor() { InitializeComponent(); }
        public AddSensor(Sensor sensor)
        {
            InitializeComponent();
            DataContext = sensor;
        }
        void Back_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(ManagerPage.FieldMonitoringPage); }
        void Add_Click(object sender, RoutedEventArgs e)
        {
            DB.Childs.Add(new SensorDetails
            {
                ID = ID.Text,
                Humidity = Humidity.Text,
                Temperature = Temperature.Text,
                Acidity = Acidity.Text,
                Asot = Asot.Text,
                Calcium = Calcium.Text,
                Calium = Calium.Text,
                Magniy = Magniy.Text,
                Phosphorus = Phosphorus.Text
            });
            ManagerPage.Page.Navigate(ManagerPage.FieldMonitoringPage);
        }
    }
}