using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Timers;

namespace SystemMonitoring
{
    public partial class FieldMonitoring : Page
    {
        Field _selectedField;
        public FieldMonitoring(string district, string field)
        {
            InitializeComponent();
            _selectedField = dbMonitoringEntities.gc().Fields.Where(x => x.District == district && x.Number == field).Single();
            DataContext = _selectedField;
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            ChangeSize();
            int timeout = 2000;
            bool ArduinoPortFound = false;
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    currentPort = new SerialPort(port, 9600);
                    if (ArduinoDetected())
                    {
                        ArduinoPortFound = true;
                        break;
                    }
                    else ArduinoPortFound = false;
                }
            }
            catch { }

            if (!ArduinoPortFound) return;
            System.Threading.Thread.Sleep(500); // немного подождем

            currentPort.BaudRate = 9600;
            currentPort.DtrEnable = true;
            currentPort.ReadTimeout = timeout;
            try { currentPort.Open(); }
            catch { }

            aTimer = new Timer(timeout);
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        void ChangeSize()
        {
            Sensors.Width = WindowWidth - 20;
            Sensors.Height = WindowHeight - 20 - 150;
        }
        int count = 0;
        void AddSensor()
        {
            count++;
            StackPanel sp = new StackPanel
            {
                Name = $"Sensor_{count}",
                Orientation = Orientation.Horizontal
            };
            TextBlock tbID = new TextBlock
            {
                Name = $"ID_{count}",
                Height = 20,
                FontSize = 14,
                Text = sensor1.ID
            };
            Border br1 = new Border
            {
                BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(1),
                Margin = new Thickness(10,0,10,0)
            };
            TextBlock tbHum = new TextBlock
            {
                Name = $"Hum_{count}",
                Height = 20,
                FontSize = 14,
                Text = sensor1.humidity
            };
            sp.Children.Add(tbID);
            sp.Children.Add(br1);
            sp.Children.Add(tbHum);
        }

        Timer aTimer;
        SerialPort currentPort;
        delegate void updateDelegate(string txt);
        bool ArduinoDetected()
        {
            try
            {
                currentPort.Open();
                System.Threading.Thread.Sleep(1000);
                string returnMessage = currentPort.ReadLine();
                currentPort.Close();
                return !string.IsNullOrEmpty(returnMessage);
            }
            catch (Exception) { return false; }
        }
        Sensor sensor1;
        void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            if (!currentPort.IsOpen) return;
            try // так как после закрытия окна таймер еще может выполнится или предел ожидания может быть превышен
            {
                // удалим накопившееся в буфере
                currentPort.DiscardInBuffer();
                // считаем последнее значение
                //var strFromPort = currentPort.ReadExisting();
                sensor1 = new Sensor
                {
                    ID = currentPort.ReadLine(),
                    humidity = currentPort.ReadLine(),
                    temperature = currentPort.ReadLine()
                };
                lblPortData.Dispatcher.BeginInvoke(new updateDelegate(updateTextBox), sensor1.humidity);
            }
            catch { }
        }

        void updateTextBox(string txt)
        {
            lblPortData.Content = txt;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}