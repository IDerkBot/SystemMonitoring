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
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using Newtonsoft.Json;
using System.IO;

namespace SystemMonitoring
{
    public partial class FieldMonitoring : Page
    {
        #region Fields
        public Sensor _newSensor = new Sensor();
        Seeding _selectedSeeding;
        int count = 0;
        SerialPort currentPort;
        double _days;
        #endregion
        public FieldMonitoring()
        {
            InitializeComponent();
            _selectedSeeding = dbMonitoringEntities.gc().Seedings.Where(x => x.IdField == DB.SelectSeeding.IdField).Single();
            DataContext = _selectedSeeding;
            // TODO: string
            if (_selectedSeeding.Culture != null)
            {
                Culture.ItemsSource = dbMonitoringEntities.gc().Cultures.ToList().Select(x => x.Name).Distinct();
                Culture.SelectedItem = _selectedSeeding.Culture.Name;
                _days = Math.Floor((DateTime.Now - DateTime.Parse(_selectedSeeding.Date)).TotalDays);
                List<Culture> cultures = dbMonitoringEntities.gc().Cultures.Where(x => x.Name == _selectedSeeding.Culture.Name).ToList();
                foreach (var cult in cultures)
                {
                    int perMin = int.Parse(cult.Period.Split('-')[0]);
                    int perMax = int.Parse(cult.Period.Split('-')[1]);
                    if (perMin <= _days && perMax >= _days)
                    {
                        Status.Content = cult.Status;
                        break;
                    }
                }
            }
            else
            {
                Culture.ItemsSource = dbMonitoringEntities.gc().Cultures.ToList().Distinct();
            }
            Soil.ItemsSource = new List<string> { "Чернозём", "Тундровые", "Подзолистые", "Болотные", "Серые лесные", "Луговые" };
            Soil.SelectedItem = _selectedSeeding.Field.Soil;
            var js = JsonConvert.DeserializeObject<List<SensorDetails>>(File.ReadAllText($@"{FileManager.GetAppdata()}\sensors.json"));
            DB.Childs = new List<SensorDetails>();
            ArduinoPortOpen();
            if (currentPort.IsOpen)
            {
                ReadDataArduino();
            }
        }
        #region Arduino
        void ArduinoPortOpen()
        {
            bool ArduinoPortFound = false;
            try
            {
                string[] ports = SerialPort.GetPortNames();
                foreach (string port in ports)
                {
                    currentPort = new SerialPort(port, 9600);
                    if (ArduinoDetected()) { ArduinoPortFound = true; break; }
                    else ArduinoPortFound = false;
                }
                if (!ArduinoPortFound) { mb.Show("Подключенные датчики не обнаружены"); Refresh.IsEnabled = false; return; }
                currentPort.BaudRate = 9600;
                currentPort.DtrEnable = true;
                currentPort.ReadTimeout = 2000;
                try { currentPort.Open(); }
                catch (Exception ex) { mb.Show($"Error 1:{ex.Message}"); }
            }
            catch (Exception ex) { mb.Show($"Error 2:{ex.Message}"); }
        }
        void AddSensor(Sensor sensor)
        {
            count++;
            Dispatcher.Invoke(() => Count.Text = count.ToString());
            var sens = new SensorDetails
            {
                ID = sensor.ID,
                Humidity = "",
                Temperature = "",
                Acidity = "",
                Asot = "",
                Calcium = "",
                Calium = "",
                Phosphorus = ""
            };
            Sensors.Children.Add(sens);
            DB.Childs.Add(sens);
        }
        void AddSensor(SensorDetails sensor)
        {
            count++;
            Count.Text = count.ToString();
            Sensors.Children.Add(sensor);
        }
        bool ArduinoDetected()
        {
            string returnMessage;
            try
            {
                currentPort.Open();
                returnMessage = currentPort.ReadLine();
                currentPort.Close();
                return !string.IsNullOrEmpty(returnMessage);
            }
            catch (Exception ex) { System.Windows.Forms.MessageBox.Show($"Error 3:{ex.Message}"); return false; }
        }
        SensorDetails CreateSensor(SerialPort sp)
        {
            //var ID = sp.ReadLine().Trim().Split('=');
            return new SensorDetails
            {
                ID = sp.ReadLine().Trim(), // ID=001
                Humidity = sp.ReadLine().Trim(), // Hum=0.00
                Temperature = sp.ReadLine().Trim(), // Temp=30.00
                Phosphorus = "",
                Acidity = "",
                Asot = "",
                Calium = "",
                Calcium = ""
            };
        }
        async Task<bool> Listening(SerialPort sp)
        {
            await Task.Run(() => sp.ReadLine());
            return sp.ReadLine().Contains("Start");
        }
        void ReadDataArduino()
        {
            bool endLine = false;
            SensorDetails lastSensor = new SensorDetails();
            while (!endLine)
            {
                SensorDetails sensor = CreateSensor(currentPort);
                if (lastSensor.ID != sensor.ID)
                {
                    lastSensor = sensor;
                    AddSensor(sensor);
                }
                else endLine = true;
            }
        }
        public void ClosePort() { currentPort.Close(); }
        #endregion
        #region Buttons Event
        void Refresh_Click(object sender, RoutedEventArgs e) { UploadSensor(); }
        void Add_Click(object sender, RoutedEventArgs e)
        {
            //currentPort.Close();
            List<SensorDetails> list = new List<SensorDetails>();
            for (int i = 0; i < Sensors.Children.Count; i++)
            { list.Add(Sensors.Children[i] as SensorDetails); }
            DB.Childs = list;
            ManagerPage.Page.Navigate(new AdminEditPages.AddSensor());
        }
        void Map_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new Map()); }
        void SaveToExcel_Click(object sender, RoutedEventArgs e)
        {
            Excel.Application ExcelApp = new Excel.Application();
            Excel._Workbook ExcelWorkBook;
            Excel.Worksheet ExcelWorkSheet;
            ExcelWorkBook = ExcelApp.Workbooks.Open($@"{FileManager.GetAppdata()}Отчёт-Шаблон.xlsx");
            ExcelWorkSheet = (Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

            // Заполнение ячеек
            // Установка даты в отчёт
            ExcelWorkSheet.Cells[4, 2] = DateTime.Now.ToShortDateString() + " года";

            Field field = dbMonitoringEntities.gc().Fields.Where(x => x.Id == DB.SelectSeeding.IdField).Single();

            ExcelWorkSheet.Cells[8, 6] = field.District;
            ExcelWorkSheet.Cells[9, 7] = field.Number;

            // Запись информации с датчиков
            for (int i = 0; i < Sensors.Children.Count; i++)
            {
                SensorDetails sensor = Sensors.Children[i] as SensorDetails;
                ExcelWorkSheet.Cells[17 + i, 2] = sensor.ID;
                ExcelWorkSheet.Cells[17 + i, 3] = sensor.Temperature;
                ExcelWorkSheet.Cells[17 + i, 4] = sensor.Acidity;
                ExcelWorkSheet.Cells[17 + i, 5] = sensor.Humidity;
                ExcelWorkSheet.Cells[17 + i, 6] = sensor.Phosphorus;
                ExcelWorkSheet.Cells[17 + i, 7] = sensor.Calium;
                ExcelWorkSheet.Cells[17 + i, 8] = sensor.Magniy;
                ExcelWorkSheet.Cells[17 + i, 9] = sensor.Calcium;
                ExcelWorkSheet.Cells[17 + i, 10] = sensor.Asot;
                ExcelWorkSheet.Cells[17 + i, 11] = sensor.Recomendation;
            }
            // Сохранение файла
            Settings settings = FileManager.GetSettings();
            ExcelApp.ActiveWorkbook.SaveAs($@"{settings.ReportsPath}\Отчёт-{DateTime.Now.ToShortDateString()}.xlsx");
            // Закрытие процесса Excel
            Process[] etc = Process.GetProcesses();
            foreach (Process anti in etc)
                if (anti.ProcessName.ToLower().Contains("excel")) anti.Kill();
        }
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
                sensor.Recomendation = GetRecomendation(sensor);
                Sensors.Children.Add(sensor);
            }
        }
        void UploadSensor()
        {
            //{
            //    if (!currentPort.IsOpen && repeat) { ArduinoPortOpen(); UploadSensor(false); }
            //    try { if (await Listening(currentPort)) ReadDataArduino(); }
            //    catch (Exception ex) { System.Windows.Forms.MessageBox.Show($"Error 4:{ex.Message} {ex.StackTrace}"); }
            //}
            try
            {
                if(DB.Childs.Count > 0)
                {
                    AddSensor(DB.Childs);
                }
                else if (DB.Childs.Count == 0) { mb.Show("Датчиков не обнаружено"); }
            }
            catch (Exception ex) { mb.Show($"Error 4:{ex.Message}"); }
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
        public void NavigateLoad()
        {
            //if (_newSensor.Humidity != null) System.Windows.Forms.MessageBox.Show($"{_newSensor}"); //AddSensor(sensor);
            //ManagerPage.FieldMonitoringPage = this;
            ChangeSize();
            UploadSensor();
            //ArduinoPortOpen();
            //await Task.Run(() => UploadSensor());
            //DataContext = _selectedSeeding;
            //foreach (SensorDetails child in DB.Childs) { AddSensor(child); }
        }
        string GetRecomendation(SensorDetails sensor)
        {
            var culture = dbMonitoringEntities.gc().Cultures.Where(x => x.Name == Culture.SelectedItem.ToString() && x.Status == Status.Content.ToString()).Single();
            string recomendation = "";
            double _acidityMin = (culture.Ph.Contains("-")) ? Convert.ToDouble(culture.Ph.Split('-')[0]) : Convert.ToDouble(culture.Ph);
            double _acidityMax = (culture.Ph.Contains("-")) ? Convert.ToDouble(culture.Ph.Split('-')[1]) : Convert.ToDouble(culture.Ph);
            if (Convert.ToDouble(sensor.Acidity) > _acidityMax) recomendation += "Необходимо добавить гипс.\n";
            else if (Convert.ToDouble(sensor.Acidity) < _acidityMin) recomendation += "Необходимо добавить известь.\n";

            double _temperatureMin = (culture.Temperature.Contains("-")) ? Convert.ToDouble(culture.Temperature.Split('-')[0]) : Convert.ToDouble(culture.Temperature);
            double _temperatureMax = (culture.Temperature.Contains("-")) ? Convert.ToDouble(culture.Temperature.Split('-')[1]) : Convert.ToDouble(culture.Temperature);
            if (Convert.ToDouble(sensor.Temperature) > _temperatureMax) recomendation += "Температура выше оптимального значения.\n";
            else if (Convert.ToDouble(sensor.Temperature) < _temperatureMin) recomendation += "Температура ниже оптимального значения.\n";

            double _asotMin = (culture.Nitrogen.Contains("-")) ? Convert.ToDouble(culture.Nitrogen.Split('-')[0]) : Convert.ToDouble(culture.Nitrogen);
            double _asotMax = (culture.Nitrogen.Contains("-")) ? Convert.ToDouble(culture.Nitrogen.Split('-')[1]) : Convert.ToDouble(culture.Nitrogen);
            if (Convert.ToDouble(sensor.Asot) > _asotMax) recomendation += "Азот в избытке.\n";
            else if (Convert.ToDouble(sensor.Asot) < _asotMin) recomendation += "Рекомендуется внести азот.\n";

            double _phosphorMin = (culture.Phosphor.Contains("-")) ? Convert.ToDouble(culture.Phosphor.Split('-')[0]) : Convert.ToDouble(culture.Phosphor);
            double _phosphorMax = (culture.Phosphor.Contains("-")) ? Convert.ToDouble(culture.Phosphor.Split('-')[1]) : Convert.ToDouble(culture.Phosphor);;
            if (Convert.ToDouble(sensor.Phosphorus) > _phosphorMax) recomendation += "Фосфор в избытке.\n";
            else if (Convert.ToDouble(sensor.Phosphorus) < _phosphorMin) recomendation += "Рекомендуется внести фосфор.\n";

            double _humidMin = (culture.Humidity.Contains("-")) ? Convert.ToDouble(culture.Humidity.Split('-')[0]) : Convert.ToDouble(culture.Humidity);
            double _humidMax = (culture.Humidity.Contains("-")) ? Convert.ToDouble(culture.Humidity.Split('-')[1]) : Convert.ToDouble(culture.Humidity);
            if (Convert.ToDouble(sensor.Humidity) > _humidMax) recomendation += "Влажность ниже оптимального.\n";
            else if (Convert.ToDouble(sensor.Humidity) < _humidMin) recomendation += "Влажность выше оптимального.\n";

            double _magnMin = (culture.Magnesium.Contains("-")) ? Convert.ToDouble(culture.Magnesium.Split('-')[0]) : Convert.ToDouble(culture.Magnesium);
            double _magnMax = (culture.Magnesium.Contains("-")) ? Convert.ToDouble(culture.Magnesium.Split('-')[1]) : Convert.ToDouble(culture.Magnesium);
            if (Convert.ToDouble(sensor.Magniy) > _magnMax) recomendation += "Магний в избытке.\n";
            else if (Convert.ToDouble(sensor.Magniy) < _magnMin) recomendation += "Рекомендуется внести магний.\n";

            double _calcMin = (culture.Calcium.Contains("-")) ? Convert.ToDouble(culture.Calcium.Split('-')[0]) : Convert.ToDouble(culture.Calcium);
            double _calcMax = (culture.Calcium.Contains("-")) ? Convert.ToDouble(culture.Calcium.Split('-')[1]) : Convert.ToDouble(culture.Calcium);
            if (Convert.ToDouble(sensor.Calcium) > _calcMax) recomendation += "Кальций в избытке.\n";
            else if (Convert.ToDouble(sensor.Calcium) < _calcMin) recomendation += "Рекомендуется внести кальций.\n";

            double _calMin = (culture.Potassium.Contains("-")) ? Convert.ToDouble(culture.Potassium.Split('-')[0]) : Convert.ToDouble(culture.Potassium);
            double _calMax = (culture.Potassium.Contains("-")) ? Convert.ToDouble(culture.Potassium.Split('-')[1]) : Convert.ToDouble(culture.Potassium);
            if (Convert.ToDouble(sensor.Calium) > _calMax) recomendation += "Калий в избытке.\n";
            else if (Convert.ToDouble(sensor.Calium) < _calMin) recomendation += "Рекомендуется внести калий.\n";
            return recomendation;
        }
        void Write_Click(object sender, RoutedEventArgs e)
        {
            List<SensorDetails> ls = new List<SensorDetails>();
            ls.AddRange(DB.Childs);
            File.WriteAllText($@"{FileManager.GetAppdata()}\sensors.json", JsonConvert.SerializeObject(ls));
        }
    }
}