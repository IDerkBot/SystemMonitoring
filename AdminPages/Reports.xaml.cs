using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Excel = Microsoft.Office.Interop.Excel;
using Path = System.IO.Path;
using WinForms = System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;

namespace SystemMonitoring
{
    public partial class Reports : Page
    {
        public Reports() { InitializeComponent(); }
        void FileSearch(string path)
        {
            //Уничтожить все объекты
            ExcelPanel.Children.Clear();
            WordPanel.Children.Clear();
            DB.Path = path;
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
            //Excel
            if (listExcel.Count > 0)
            {
                for (int i = 0; i < listExcel.Count; i++)
                {
                    StackPanel stack = new StackPanel { Orientation = Orientation.Horizontal };
                    Label label = new Label
                    {
                        Content = $"{listExcel[i]}",
                        Width = WindowWidth / 2 - 105,
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        DataContext = $"{listExcel[i]}"
                    };
                    label.MouseDoubleClick += new MouseButtonEventHandler(OpenFile);
                    Button btn = new Button
                    {
                        Content = "Открыть",
                        Width = 100,
                        DataContext = $"{listExcel[i]}"
                    };
                    btn.Click += new RoutedEventHandler(OpenFile);
                    stack.Children.Add(label);
                    stack.Children.Add(btn);
                    ExcelPanel.Children.Add(stack);
                }
            }
            //Word
            if(listWord.Count > 0)
            {
                for (int i = 0; i < listWord.Count; i++)
                {
                    StackPanel stack = new StackPanel { Orientation = Orientation.Horizontal };
                    Label label = new Label
                    {
                        Content = $"{listWord[i]}",
                        Width = WindowWidth / 2 - 105,
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        DataContext = $"{listWord[i]}"
                    };
                    label.MouseDoubleClick += new MouseButtonEventHandler(OpenFile);
                    Button btn = new Button
                    {
                        Content = "Открыть",
                        Width = 100,
                        DataContext = $"{listWord[i]}"
                    };
                    btn.Click += new RoutedEventHandler(OpenFile);
                    stack.Children.Add(label);
                    stack.Children.Add(btn);
                    WordPanel.Children.Add(stack);
                }
            }
        }
        void OpenFile(object sender, EventArgs e)
        {
            string fileName = (sender.ToString().Contains("Button")) ? $"{(sender as Button).DataContext}" : $"{(sender as Label).DataContext}";
            string filePath = $@"{DB.Path}{fileName}";
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
        void Research_Click(object sender, RoutedEventArgs e)
        {
            WinForms.FolderBrowserDialog folderDialog = new WinForms.FolderBrowserDialog { ShowNewFolderButton = false };
            if (folderDialog.ShowDialog() == WinForms.DialogResult.OK) FilePath.Text = folderDialog.SelectedPath;
            FileSearch($@"{FilePath.Text}\");
            CheckChangePath();
        }
        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as Page).Width = WindowWidth;
            FilePath.Text = GetReportsPath();
        }
        void Page_SizeChanged(object sender, SizeChangedEventArgs e) { (sender as Page).Width = WindowWidth; }
        void FilePath_TextChanged(object sender, TextChangedEventArgs e) { FileSearch($@"{FilePath.Text}\"); }
        string GetReportsPath() { return FileManager.GetSettings().ReportsPath; }
        void CheckChangePath()
        {
            if (GetReportsPath() != FilePath.Text)
            {
                Settings settings = FileManager.GetSettings();
                settings.ReportsPath = @FilePath.Text;
                FileManager.SetSettings(settings);
            }
        }
    }
}