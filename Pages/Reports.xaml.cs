using System;
using System.Collections.Generic;
using System.IO;
using Path = System.IO.Path;
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
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Win32;

namespace SystemMonitoring
{
    /// <summary>
    /// Логика взаимодействия для Reports.xaml
    /// </summary>
    public partial class Reports : Page
    {
        public Reports()
        {
            InitializeComponent();
        }
        public void ResizeReports()
        {
            var e = (DataBank.SizeWindow - 20) / 2;
            ExcelPanel.Width = e;
            WordPanel.Width = e;
        }

        void ChangedSize(object sender, SizeChangedEventArgs e)
        {
            ResizeReports();
        }
        // TODO: Сделать сохранение пути

        // TODO: Сделать отображение файлов по пути
        void FileSearch(string path)
        {
            //Уничтожить все объекты
            ExcelPanel.Children.Clear();
            WordPanel.Children.Clear();

            DataBank.Path = path;

            string[] filesExcel = Directory.GetFiles(path);
            List<string> listExcel = new List<string>();

            string[] filesWord = Directory.GetFiles(path);
            List<string> listWord = new List<string>();

            foreach (string file in filesExcel)
                if (Path.GetFileName(file).Contains("xlsm") || Path.GetFileName(file).Contains("xlsx"))
                    listExcel.Add(Path.GetFileName(file));
            foreach (string file in filesWord)
                if (Path.GetFileName(file).Contains("doc") || Path.GetFileName(file).Contains("docx"))
                    listWord.Add(Path.GetFileName(file));

            Size size = new Size(100, 25);

            //Excel
            if (listExcel.Count > 0)
            {
                for (int i = 0; i < listExcel.Count; i++)
                {
                    Label label = new Label
                    {
                        Name = "lE_" + i.ToString(),
                        Content = $"{listExcel[i]}",
                        Width = ExcelPanel.Width - size.Width,
                        Height = 25,
                        Margin = new Thickness(5, 27 * (i), 0, 0),
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    };
                    label.MouseDoubleClick += new MouseButtonEventHandler(OpenFile);
                    Button btn = new Button
                    {
                        Name = "btnE_" + i.ToString(),
                        Content = "Open",
                        Width = size.Width,
                        Height = size.Height,
                        Margin = new Thickness(top: 27 * (i), right: 10, left: 0, bottom: 0),
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        Cursor = Cursors.Hand,
                        BorderThickness = new Thickness(0),
                    };
                    btn.Click += new RoutedEventHandler(OpenFile);

                    ExcelPanel.Children.Add(label);
                    ExcelPanel.Children.Add(btn);
                }
            }
            else
            {
                Label lb = new Label
                {
                    Content = "Excel файлов не было обнаружено",
                    Width = 200,
                    Height = 25,
                    Margin = new Thickness(top: ExcelPanel.Height / 2, left: 0, right: 0, bottom: 0),
                };
                ExcelPanel.Children.Add(lb);
            }
            //Word
            if(listWord.Count > 0)
            {
                for (int i = 0; i < listWord.Count; i++)
                {
                    Label label = new Label
                    {
                        Name = "lW_" + i.ToString(),
                        Content = $"{listWord[i]}",
                        Width = ExcelPanel.Width - size.Width - 20,
                        Height = 25,
                        Margin = new Thickness(5, 27 * (i), 0, 0),
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    };
                    label.MouseDoubleClick += new MouseButtonEventHandler(OpenFile);
                    Button btn = new Button
                    {
                        Name = "btnW_" + i.ToString(),
                        Content = "Open",
                        Width = size.Width,
                        Height = size.Height,
                        Margin = new Thickness(ExcelPanel.Width - size.Width - 15, 27 * (i), 0, 0),
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        Cursor = Cursors.Hand,
                        BorderThickness = new Thickness(0),
                    };
                    btn.Click += new RoutedEventHandler(OpenFile);
                }
            }
            else
            {

            }
        }
        void OpenFile(object sender, EventArgs e)
        {
            string fileName = "";
            if (sender.ToString().Contains("Button"))
            {
                string[] btnName = (sender as Button).Name.Split('_').ToArray();
                if (btnName[0].Contains("E"))
                    fileName = (ExcelPanel.FindName($"lE_{btnName[1]}") as Label).Content.ToString();
                else if (btnName[0].Contains("W"))
                    fileName = (WordPanel.FindName($"lW_{btnName[1]}") as Label).Content.ToString();
            }
            else if (sender.ToString().Contains("Label"))
                fileName = (sender as Label).Content.ToString();

            string filePath = $@"{DataBank.Path}{fileName}";
            if (fileName.Contains("xlsm") || fileName.Contains("xlsx"))
            {
                Excel.Application exApp = new Excel.Application { Visible = true };
                exApp.Workbooks.Open(filePath);
            }
            else if (fileName.Contains("doc") || fileName.Contains("docx"))
            {
                Word.Application wdApp = new Word.Application { Visible = true };
                wdApp.Documents.Open(filePath);
            }
        }
        // Получить путь к файлу конфигурации (если его нет, то создать)
        string GetPathConfig()
        {
            string appdataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string path = $@"{appdataPath}\SystemMonitoring\config.txt";
            if (!File.Exists(path))
            {
                if (!Directory.Exists(appdataPath + @"\SystemMonitoring\"))
                    Directory.CreateDirectory(appdataPath + @"\SystemMonitoring\");
                File.Create(path).Dispose();
                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int i = 0; i <= 3; i++) writer.WriteLine();
                }
            }
            string config = appdataPath + @"\SystemMonitoring\config.txt";
            return config;
        }
        void Research_Click(object sender, EventArgs e)
        {
            //var dialog = new OpenFileDialog();

            //if (dialog.ShowDialog() == true)
            //    textBox1.Text = fbd.SelectedPath;
            //FileSearch(@textBox1.Text + @"\");
            //bool yes = false;
            //List<string> rows = new List<string>();
            //using (StreamReader reader = new StreamReader(GetPathConfig()))
            //{
            //    for (int i = 0; i < 3; i++) { rows.Add(reader.ReadLine()); }
            //    if (reader.ReadLine() != textBox1.Text + @"\") yes = true;
            //}
            //if (yes)
            //    using (StreamWriter writer = new StreamWriter(GetPathConfig()))
            //    {
            //        for (int i = 0; i < rows.Count; i++)
            //        {
            //            writer.WriteLine(rows[i]);
            //        }
            //        writer.WriteLine($@"{textBox1.Text}\");
            //    }






            //string path = GetPathConfig();
            //using (StreamWriter sw = fi1.CreateText())
            //{
            //    sw.WriteLine(@textBox1.Text);
            //}
        }
        void GetReportsPath(string path)
        {
            using (StreamReader sr = new FileInfo(path).OpenText())
            {
                List<string> lines = new List<string>();
                string line;
                while ((line = sr.ReadLine()) != null)
                    lines.Add(line);
            }
        }
    }
}
