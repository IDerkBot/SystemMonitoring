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
            if (_selectedSeeding.Culture != null)
            {
                CultureCB.ItemsSource = dbMonitoringEntities.gc().Cultures.ToList().Select(x => x.Name).Distinct();
                CultureCB.SelectedItem = _selectedSeeding.Culture.Name;
                if(_selectedSeeding.Date == null)
                {
                    _selectedSeeding.Date = DateTime.Now.ToString().Split(' ')[0];
                    dbMonitoringEntities.gc().Seedings.Add(_selectedSeeding);
                    dbMonitoringEntities.gc().SaveChanges();
                }
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
                CultureCB.ItemsSource = dbMonitoringEntities.gc().Cultures.ToList().Select(x => x.Name).Distinct();
            }
            Soil.ItemsSource = new List<string> { "Чернозём", "Тундровые", "Подзолистые", "Болотные", "Серые лесные", "Луговые" };
            Soil.SelectedItem = _selectedSeeding.Field.Soil;
            var js = JsonConvert.DeserializeObject<List<SensorDetails>>(File.ReadAllText($@"{FileManager.GetAppdata()}\sensors.json"));
            DB.Childs = new List<SensorDetails>();
            ArduinoPortOpen();
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
                    if (true) { ArduinoPortFound = true; break; }
                }
                if (!ArduinoPortFound) { mb.Show("Подключенные датчики не обнаружены"); Refresh.IsEnabled = false; return; }
                currentPort.BaudRate = 9600;
                currentPort.DtrEnable = true;
                currentPort.ReadTimeout = 2000;
                try {
                    currentPort.Open();
                    currentPort.DataReceived += CurrentPort_DataReceived;
                    //ReadDataArduino();
                }
                catch (Exception ex) { mb.Show($"Error 1:{ex.Message}"); }
            }
            catch (Exception ex) { mb.Show($"Error 2:{ex.Message}"); }
        }
        void CurrentPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var str1 = currentPort.ReadLine().Trim();
            var str2 = currentPort.ReadLine().Trim();
            var str3 = currentPort.ReadLine().Trim();
            string ID = "";
            string Humidity = "";
            string Temperature = "";
            if (str1.Contains("ID")) ID = str1.Split('=')[1];
            if (str2.Contains("ID")) ID = str2.Split('=')[1];
            if (str3.Contains("ID")) ID = str3.Split('=')[1];

            if (str1.Contains("Hum")) Humidity = str1.Split('=')[1];
            if (str2.Contains("Hum")) Humidity = str2.Split('=')[1];
            if (str3.Contains("Hum")) Humidity = str3.Split('=')[1];

            if (str1.Contains("Temp")) Temperature = str1.Split('=')[1];
            if (str2.Contains("Temp")) Temperature = str2.Split('=')[1];
            if (str3.Contains("Temp")) Temperature = str3.Split('=')[1];
            if (ID.Length < 2) ID = "0";
            if (Humidity.Length < 2) Humidity = "0";
            if (Temperature.Length < 2) Temperature = "0";
            if (Humidity.Contains('.')) Humidity = Humidity.Replace('.', ',');
            if (Temperature.Contains('.')) Temperature = Temperature.Replace('.', ',');
            Sensor sensorDetails = new Sensor()
            {
                ID = ID,
                Humidity = Humidity,
                Temperature = Temperature
            };
            if(DB.Childs.Where(x => x.ID.Contains(ID)).Count() == 0) AddSensor(sensorDetails);
            currentPort.Close();
            
        }
        void AddSensor(Sensor sensor)
        {
            Dispatcher.Invoke(() =>
            {
                SensorDetails sd = new SensorDetails()
                {
                    ID = sensor.ID,
                    Temperature = sensor.Temperature,
                    Humidity = sensor.Humidity,
                    Calcium = sensor.Calcium,
                    Calium = sensor.Calium,
                    Asot = sensor.Asot,
                    Acidity = sensor.Acidity,
                    Phosphorus = sensor.Phosphorus,
                    Culture = CultureCB.SelectedItem.ToString()
                };
                sd.Recomendation = GetRecomendation(sd);
                AddSensor(sd);
            });
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
            catch (Exception ex) { mb.Show($"Error 3:{ex.Message}"); return false; }
        }
        public void ClosePort() {
            if(currentPort.IsOpen) currentPort.Close();
        }
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
            mb.Show("Отчёт успешно сохранён");
        }
        #endregion
        void ChangeSize()
        {
            Sensors.Width = WindowWidth - 20;
            Sensors.Height = WindowHeight - 20 - 150;
        }
        void AddSensor(List<SensorDetails> sensors)
        {
            Dispatcher.Invoke(() => {
                Sensors.Children.Clear();
                count = 0;
                foreach (var sensor in sensors)
                {
                    count++;
                    Count.Text = count.ToString();
                    sensor.Recomendation = GetRecomendation(sensor);
                    sensor.PercentDeviation = GetPercent(sensor);
                    if (sensor.PercentDeviation <= 15) sensor.Background = new SolidColorBrush(Color.FromRgb(46, 204, 113));
                    else if (sensor.PercentDeviation > 15 && sensor.PercentDeviation <= 30) sensor.Background = new SolidColorBrush(Color.FromRgb(243, 156, 18));
                    else if (sensor.PercentDeviation > 30) sensor.Background = new SolidColorBrush(Color.FromRgb(231, 76, 60));
                    Sensors.Children.Add(sensor);
                }
            });
        }
        async void UploadSensor()
        {
            if(DB.Childs.Count > 0) await Task.Run(()=> AddSensor(DB.Childs));
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
            var culture = dbMonitoringEntities.gc().Cultures.Where(x => x.Name == CultureCB.SelectedItem.ToString() && x.Status == Status.Content.ToString()).Single();
            string recomendation = "";
            double _acidityMin = (culture.Ph.Contains("-")) ? Convert.ToDouble(culture.Ph.Split('-')[0]) : Convert.ToDouble(culture.Ph);
            double _acidityMax = (culture.Ph.Contains("-")) ? Convert.ToDouble(culture.Ph.Split('-')[1]) : Convert.ToDouble(culture.Ph);
            if (Convert.ToDouble(sensor.Acidity) == 0) recomendation = "";
            else if (Convert.ToDouble(sensor.Acidity) > _acidityMax) recomendation += "Необходимо добавить гипс.\n";
            else if (Convert.ToDouble(sensor.Acidity) < _acidityMin) recomendation += "Необходимо добавить известь.\n";

            double _temperatureMin = (culture.Temperature.Contains("-")) ? Convert.ToDouble(culture.Temperature.Split('-')[0]) : Convert.ToDouble(culture.Temperature);
            double _temperatureMax = (culture.Temperature.Contains("-")) ? Convert.ToDouble(culture.Temperature.Split('-')[1]) : Convert.ToDouble(culture.Temperature);
            double _tempMid = (_temperatureMax + _temperatureMin) / 2 - Convert.ToDouble(sensor.Temperature);
            if (Convert.ToDouble(sensor.Temperature) == 0) recomendation = "";
            else if (Convert.ToDouble(sensor.Temperature) > _temperatureMax) recomendation += $"Температура выше оптимального значения на {_tempMid}°C\n";
            else if (Convert.ToDouble(sensor.Temperature) < _temperatureMin) recomendation += $"Температура ниже оптимального значения на {_tempMid}°C\n";

            double _asotMin = (culture.Nitrogen.Contains("-")) ? Convert.ToDouble(culture.Nitrogen.Split('-')[0]) : Convert.ToDouble(culture.Nitrogen);
            double _asotMax = (culture.Nitrogen.Contains("-")) ? Convert.ToDouble(culture.Nitrogen.Split('-')[1]) : Convert.ToDouble(culture.Nitrogen);
            double _asotMid = (_asotMax + _asotMin) / 2 - Convert.ToDouble(sensor.Humidity);
            if (Convert.ToDouble(sensor.Asot) == 0) recomendation = "";
            else if (Convert.ToDouble(sensor.Asot) > _asotMax) recomendation += $"Азот в избытке на {_asotMid}мг/кг\n";
            else if (Convert.ToDouble(sensor.Asot) < _asotMin) recomendation += $"Рекомендуется внести азот на {_asotMid}мг/кг\n";

            double _phosphorMin = (culture.Phosphor.Contains("-")) ? Convert.ToDouble(culture.Phosphor.Split('-')[0]) : Convert.ToDouble(culture.Phosphor);
            double _phosphorMax = (culture.Phosphor.Contains("-")) ? Convert.ToDouble(culture.Phosphor.Split('-')[1]) : Convert.ToDouble(culture.Phosphor);
            double _phosMid = (_phosphorMax + _phosphorMin) / 2 - Convert.ToDouble(sensor.Humidity);
            if (Convert.ToDouble(sensor.Phosphorus) == 0) recomendation = "";
            else if (Convert.ToDouble(sensor.Phosphorus) > _phosphorMax) recomendation += $"Фосфор в избытке на {_phosMid}мг/кг\n";
            else if (Convert.ToDouble(sensor.Phosphorus) < _phosphorMin) recomendation += $"Рекомендуется внести фосфор на {_phosMid}мг/кг\n";

            double _humidMin = (culture.Humidity.Contains("-")) ? Convert.ToDouble(culture.Humidity.Split('-')[0]) : Convert.ToDouble(culture.Humidity);
            double _humidMax = (culture.Humidity.Contains("-")) ? Convert.ToDouble(culture.Humidity.Split('-')[1]) : Convert.ToDouble(culture.Humidity);
            double _humMid = (_humidMax + _humidMin) / 2 - Convert.ToDouble(sensor.Humidity);
            if (Convert.ToDouble(sensor.Humidity) == 0) recomendation = "";
            else if (Convert.ToDouble(sensor.Humidity) > _humidMax) recomendation += $"Влажность ниже оптимального на {_humMid}%\n";
            else if (Convert.ToDouble(sensor.Humidity) < _humidMin) recomendation += $"Влажность выше оптимального на {_humMid}%\n";

            double _magnMin = (culture.Magnesium.Contains("-")) ? Convert.ToDouble(culture.Magnesium.Split('-')[0]) : Convert.ToDouble(culture.Magnesium);
            double _magnMax = (culture.Magnesium.Contains("-")) ? Convert.ToDouble(culture.Magnesium.Split('-')[1]) : Convert.ToDouble(culture.Magnesium);
            double _magnMid = (_magnMax + _magnMin) / 2 - Convert.ToDouble(sensor.Humidity);
            if (Convert.ToDouble(sensor.Magniy) == 0) recomendation = "";
            else if (Convert.ToDouble(sensor.Magniy) > _magnMax) recomendation += $"Магний в избыткена {_magnMid}мг/кг\n";
            else if (Convert.ToDouble(sensor.Magniy) < _magnMin) recomendation += $"Рекомендуется внести магний на {_magnMid}мг/кг\n";

            double _calcMin = (culture.Calcium.Contains("-")) ? Convert.ToDouble(culture.Calcium.Split('-')[0]) : Convert.ToDouble(culture.Calcium);
            double _calcMax = (culture.Calcium.Contains("-")) ? Convert.ToDouble(culture.Calcium.Split('-')[1]) : Convert.ToDouble(culture.Calcium);
            double _calcMid = (_calcMax + _calcMin) / 2 - Convert.ToDouble(sensor.Humidity);
            if (Convert.ToDouble(sensor.Calcium) == 0) recomendation = "";
            else if (Convert.ToDouble(sensor.Calcium) > _calcMax) recomendation += $"Кальций в избытке на {_calcMid}мг/кг\n";
            else if (Convert.ToDouble(sensor.Calcium) < _calcMin) recomendation += $"Рекомендуется внести кальций на {_calcMid}мг/кг\n";

            double _calMin = (culture.Potassium.Contains("-")) ? Convert.ToDouble(culture.Potassium.Split('-')[0]) : Convert.ToDouble(culture.Potassium);
            double _calMax = (culture.Potassium.Contains("-")) ? Convert.ToDouble(culture.Potassium.Split('-')[1]) : Convert.ToDouble(culture.Potassium);
            double _calMid = (_calMax + _calMin) / 2 - Convert.ToDouble(sensor.Humidity);
            if (Convert.ToDouble(sensor.Calium) == 0) recomendation = "";
            else if (Convert.ToDouble(sensor.Calium) > _calMax) recomendation += $"Калий в избытке на {_calMid}мг/кг\n";
            else if (Convert.ToDouble(sensor.Calium) < _calMin) recomendation += $"Рекомендуется внести калий на {_calMid}мг/кг\n";
            return recomendation;
        }
        void Write_Click(object sender, RoutedEventArgs e)
        {
            List<SensorDetails> ls = new List<SensorDetails>();
            ls.AddRange(DB.Childs);
            File.WriteAllText($@"{FileManager.GetAppdata()}\sensors.json", JsonConvert.SerializeObject(ls));
        }
        bool start = true;
        void Culture_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!start)
            {
                _selectedSeeding.IdCulture = dbMonitoringEntities.gc().Cultures.Where(x => x.Name == CultureCB.SelectedItem.ToString() && x.Period.Contains("00")).Select(x => x.Id).Single();
                _selectedSeeding.Culture = dbMonitoringEntities.gc().Cultures.Where(x => x.Name == CultureCB.SelectedItem.ToString() && x.Period.Contains("00")).Single();
                CultureCB.IsEnabled = false;
                _selectedSeeding.IdField = dbMonitoringEntities.gc().Fields.Where(x => x.Id == _selectedSeeding.IdField).Select(x => x.Id).Single();
                _selectedSeeding.Field = dbMonitoringEntities.gc().Fields.Where(x => x.Id == _selectedSeeding.IdField).Single();
                _selectedSeeding.Id = DB.SelectSeeding.Id;
                var a1 = dbMonitoringEntities.gc().Fields.Where(x => x.Id == DB.SelectSeeding.IdField).Single();
                var a2 = dbMonitoringEntities.gc().Cultures.Where(x => x.Name == CultureCB.SelectedItem.ToString() && x.Period.Contains("00")).Single();
                Seeding seeding = new Seeding()
                {
                    Id = DB.SelectSeeding.Id,
                    Field = a1,
                    Culture = a2,
                    Date = DateTime.Now.ToString().Split(' ')[0]
                };
                dbMonitoringEntities.gc().Seedings.Add(seeding);
                dbMonitoringEntities.gc().SaveChanges();
            }
            else
            {
                start = !start;
            }
        }
        void Date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedSeeding.Date = (sender as DatePicker).SelectedDate.ToString();
            dbMonitoringEntities.gc().Seedings.Add(_selectedSeeding);
            dbMonitoringEntities.gc().SaveChanges();
        }
        void Save_Click(object sender, RoutedEventArgs e)
        { File.WriteAllText(FileManager.GetSensorsJson(), JsonConvert.SerializeObject(DB.Childs)); }
        void SaveToExcelSensors_Click(object sender, RoutedEventArgs e)
        {
            var list = JsonConvert.DeserializeObject<List<SensorDetails>>(File.ReadAllText(FileManager.GetSensorsJson()));

            Excel.Application ExcelApp = new Excel.Application();
            Excel._Workbook ExcelWorkBook;
            Excel.Worksheet ExcelWorkSheet;
            ExcelWorkBook = ExcelApp.Workbooks.Open($@"{FileManager.GetAppdata()}Отчёт-Сенсор-Шаблон.xlsx");
            ExcelWorkSheet = (Excel.Worksheet)ExcelWorkBook.Worksheets.get_Item(1);

            // Заполнение ячеек
            // Установка даты в отчёт
            ExcelWorkSheet.Cells[4, 2] = DateTime.Now.ToShortDateString() + " года";

            Field field = dbMonitoringEntities.gc().Fields.Where(x => x.Id == DB.SelectSeeding.IdField).Single();

            ExcelWorkSheet.Cells[8, 6] = field.District;
            ExcelWorkSheet.Cells[9, 7] = field.Number;
            ExcelWorkSheet.Cells[10, 5] = field.Position;

            ExcelWorkSheet.Cells[12, 4] = field.Number;
            ExcelWorkSheet.Cells[13, 4] = field.Soil;

            foreach (var sensor in list)
            {
                ExcelWorkSheet.Cells[14, 4] = sensor.ID;

                var sensors = list.Where(x => x.ID == sensor.ID).ToList();
                for (int i = 0; i < sensors.Count(); i++)
                {
                    SensorDetails _sensor = sensors[i];
                    ExcelWorkSheet.Cells[17 + i, 3] = _sensor.Temperature;
                    ExcelWorkSheet.Cells[17 + i, 4] = _sensor.Acidity;
                    ExcelWorkSheet.Cells[17 + i, 5] = _sensor.Humidity;
                    ExcelWorkSheet.Cells[17 + i, 6] = _sensor.Phosphorus;
                    ExcelWorkSheet.Cells[17 + i, 7] = _sensor.Calium;
                    ExcelWorkSheet.Cells[17 + i, 8] = _sensor.Magniy;
                    ExcelWorkSheet.Cells[17 + i, 9] = _sensor.Calcium;
                    ExcelWorkSheet.Cells[17 + i, 10] = _sensor.Asot;
                    ExcelWorkSheet.Cells[17 + i, 11] = _sensor.Recomendation;
                }
                // Сохранение файла
                Settings settings = FileManager.GetSettings();
                ExcelApp.ActiveWorkbook.SaveAs($@"{settings.ReportsPath}\Отчёт-Сенсор{sensor.ID}-{DateTime.Now.ToShortDateString()}.xlsx");
                // Закрытие процесса Excel
                Process[] etc = Process.GetProcesses();
                foreach (Process anti in etc)
                    if (anti.ProcessName.ToLower().Contains("excel")) anti.Kill();
                mb.Show($"Отчёт по датчику №{sensor.ID} успешно сохранён");
            }
        }
        int GetPercent(SensorDetails sensor)
        {
            int max = 0;
            var culture = dbMonitoringEntities.gc().Cultures.Where(x => x.Name == CultureCB.SelectedItem.ToString() && x.Status == Status.Content.ToString()).ToList().Single();

            double acidityMiddle = (culture.Ph.Contains('-')) ? (double.Parse(culture.Ph.Split('-')[0]) + double.Parse(culture.Ph.Split('-')[1])) / 2 : double.Parse(culture.Ph);
            double acidityPercent = Math.Floor(Convert.ToDouble(sensor.Acidity) / acidityMiddle * 100);

            double asotMiddle = (culture.Nitrogen.Contains('-')) ? (double.Parse(culture.Nitrogen.Split('-')[0]) + double.Parse(culture.Nitrogen.Split('-')[1])) / 2 : double.Parse(culture.Nitrogen);
            var asotPercent = Math.Floor(double.Parse(sensor.Asot) / asotMiddle * 100);

            double calciumMiddle = (culture.Calcium.Contains('-')) ? (double.Parse(culture.Calcium.Split('-')[0]) + double.Parse(culture.Calcium.Split('-')[1])) / 2 : double.Parse(culture.Calcium);
            var calciumPercent = Math.Floor(double.Parse(sensor.Calcium) / calciumMiddle * 100);

            double caliumMiddle = (culture.Potassium.Contains('-')) ? (double.Parse(culture.Potassium.Split('-')[0]) + double.Parse(culture.Potassium.Split('-')[1])) / 2 : double.Parse(culture.Potassium);
            var caliumPercent = Math.Floor(double.Parse(sensor.Calium) / caliumMiddle * 100);

            double humidityMiddle = (culture.Humidity.Contains('-')) ? (double.Parse(culture.Humidity.Split('-')[0]) + double.Parse(culture.Humidity.Split('-')[1])) / 2 : double.Parse(culture.Humidity);
            var humidityPercent = Math.Floor(double.Parse(sensor.Humidity) / humidityMiddle * 100);

            double magniyMiddle = (culture.Magnesium.Contains('-')) ? (double.Parse(culture.Magnesium.Split('-')[0]) + double.Parse(culture.Magnesium.Split('-')[1])) / 2 : double.Parse(culture.Magnesium);
            var magniyPercent = Math.Floor(double.Parse(sensor.Magniy) / magniyMiddle * 100);

            double phosphorMiddle = (culture.Phosphor.Contains('-')) ? (double.Parse(culture.Phosphor.Split('-')[0]) + double.Parse(culture.Phosphor.Split('-')[1])) / 2 : double.Parse(culture.Phosphor);
            var phosphorPercent = Math.Floor(double.Parse(sensor.Phosphorus) / phosphorMiddle * 100);

            double temperatureMiddle = (culture.Temperature.Contains('-')) ? (double.Parse(culture.Temperature.Split('-')[0]) + double.Parse(culture.Temperature.Split('-')[1])) / 2 : double.Parse(culture.Temperature);
            var temperaturePercent = Math.Floor(double.Parse(sensor.Temperature) / temperatureMiddle * 100);

            if (acidityPercent > max) max = int.Parse(acidityPercent.ToString());
            if (asotPercent > max) max = int.Parse(asotPercent.ToString());
            if (calciumPercent > max) max = int.Parse(calciumPercent.ToString());
            if (caliumPercent > max) max = int.Parse(caliumPercent.ToString());
            if (humidityPercent > max) max = int.Parse(humidityPercent.ToString());
            if (magniyPercent > max) max = int.Parse(magniyPercent.ToString());
            if (phosphorPercent > max) max = int.Parse(phosphorPercent.ToString());
            if (temperaturePercent > max) max = int.Parse(temperaturePercent.ToString());

            return max;
        }
    }
}