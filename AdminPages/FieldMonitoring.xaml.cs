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
using System.Threading;
using mb = System.Windows.Forms.MessageBox;

namespace SystemMonitoring
{
    public partial class FieldMonitoring : Page
    {
        #region Fields
        public Sensor _newSensor = new Sensor();
        Seeding _selectedSeeding;
        int count = 0;
        SerialPort currentPort;
        #endregion
        public FieldMonitoring()
        {
            InitializeComponent();
            _selectedSeeding = dbMonitoringEntities.gc().Seedings.Where(x => x.IdField == DB.SelectSeeding.IdField).Single();
            DataContext = _selectedSeeding;
            DB.Childs = new List<SensorDetails>();
        }
        #region Arduino
        //void ArduinoPortOpen()
        //{
        //    bool ArduinoPortFound = false;
        //    try
        //    {
        //        string[] ports = SerialPort.GetPortNames();
        //        foreach (string port in ports)
        //        {
        //            currentPort = new SerialPort(port, 9600);
        //            if (ArduinoDetected()) { ArduinoPortFound = true; break; }
        //            else ArduinoPortFound = false;
        //        }
        //        if (!ArduinoPortFound) { System.Windows.Forms.MessageBox.Show("Подключенные датчики не обнаружены"); Refresh.IsEnabled = false; return; }
        //        currentPort.BaudRate = 9600;
        //        currentPort.DtrEnable = true;
        //        currentPort.ReadTimeout = 2000;
        //        try { currentPort.Open(); }
        //        catch (Exception ex) { System.Windows.Forms.MessageBox.Show($"Error 1:{ex.Message}"); }
        //    }
        //    catch (Exception ex) { System.Windows.Forms.MessageBox.Show($"Error 2:{ex.Message}"); }
        //}
        //void AddSensor(Sensor sensor)
        //{
        //    count++;
        //    Dispatcher.Invoke(() => Count.Text = count.ToString());
        //    Sensors.Children.Add(new SensorDetails(){
        //        ID = sensor.ID,
        //        Humidity = sensor.Humidity,
        //        Temperature = sensor.Temperature
        //    });
        //}
        //void AddSensor(SensorDetails sensor)
        //{
        //    count++;
        //    Count.Text = count.ToString();
        //    Sensors.Children.Add(sensor);
        //}
        //bool ArduinoDetected()
        //{
        //    string returnMessage;
        //    try
        //    {
        //        currentPort.Open();
        //        returnMessage = currentPort.ReadLine();
        //        currentPort.Close();
        //        return !string.IsNullOrEmpty(returnMessage);
        //    }
        //    catch (Exception ex) { System.Windows.Forms.MessageBox.Show($"Error 3:{ex.Message}"); return false; }
        //}
        //Sensor CreateSensor(SerialPort sp)
        //{
        //    //var ID = sp.ReadLine().Trim().Split('=');
        //    return new Sensor
        //    {
        //        ID = sp.ReadLine().Trim(), // ID=001
        //        Humidity = sp.ReadLine().Trim(), // Hum=0.00
        //        Temperature = sp.ReadLine().Trim() // Temp=30.00
        //    };
        //}
        //async Task<bool> Listening(SerialPort sp)
        //{
        //    await Task.Run(() => sp.ReadLine());
        //    return sp.ReadLine().Contains("Start");
        //}
        //void ReadDataArduino()
        //{
        //    bool endLine = false;
        //    Sensor lastSensor = new Sensor();
        //    while (!endLine)
        //    {
        //        Sensor sensor = CreateSensor(currentPort);
        //        if (lastSensor.ID != sensor.ID)
        //        {
        //            lastSensor = sensor;
        //            AddSensor(sensor);
        //        }
        //        else endLine = true;
        //    }
        //}
        public void ClosePort() { currentPort.Close(); }
        #endregion
        void ChangeSize()
        {
            Sensors.Width = WindowWidth - 20;
            Sensors.Height = WindowHeight - 20 - 150;
        }
        void AddSensor(List<SensorDetails> sensors)
        {
            Sensors.Children.Clear();
            count = 0;
            foreach (var sensor in sensors)
            {
                count++;
                Count.Text = count.ToString();
                Sensors.Children.Add(sensor);
            }
        }
        void Refresh_Click(object sender, RoutedEventArgs e) { UploadSensor(); }
        void UploadSensor()
        {
            //if (count == 0)
            //{
            //    if (!currentPort.IsOpen && repeat) { ArduinoPortOpen(); UploadSensor(false); }
            //    try { if (await Listening(currentPort)) ReadDataArduino(); }
            //    catch (Exception ex) { System.Windows.Forms.MessageBox.Show($"Error 4:{ex.Message} {ex.StackTrace}"); }
            //}
            //else if (count > 0) UpdateSensors();
            
            try
            {
                if(DB.Childs.Count > 0) AddSensor(DB.Childs);
                else if(DB.Childs.Count == 0) { mb.Show("Датчиков не обнаружено"); }
            }
            catch (Exception ex) { mb.Show($"Error 4:{ex.Message}"); }
            //else if (count > 0) UpdateSensors();
        }
        async void UpdateSensors()
        {
            await Task.Run(() =>
            {
                while (true)
                {

                }
            });
        }
        void Add_Click(object sender, RoutedEventArgs e)
        {
            //currentPort.Close();
            List<SensorDetails> list = new List<SensorDetails>();
            for (int i = 0; i < Sensors.Children.Count; i++)
            { list.Add(Sensors.Children[i] as SensorDetails); }
            DB.Childs = list;
            ManagerPage.Page.Navigate(new AdminEditPages.AddSensor());
        }
        public void NavigateLoad()
        {
            //if (_newSensor.Humidity != null) System.Windows.Forms.MessageBox.Show($"{_newSensor}"); //AddSensor(sensor);
            //ManagerPage.FieldMonitoringPage = this;
            ChangeSize();
            //ArduinoPortOpen();
            //await Task.Run(() => UploadSensor());
            //DataContext = _selectedSeeding;
            //foreach (SensorDetails child in DB.Childs) { AddSensor(child); }

        }
    }
}