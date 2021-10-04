using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SystemMonitoring
{
    public partial class Auth : Page
    {
        public Auth() { InitializeComponent(); }
        bool CheckFieldInEmpty()
        {
            bool error = false;
            if (LoginTB.Text.Length <= 4 || LoginTB.Text == null)
            {
                error = true;
                MessageBox.Show("В поле логин введено мало символов!\nЛогин должен состоять из 5 и больше символов.", "Error");
                LoginTB.Background = ColorFlat.Error;
                LoginTB.Foreground = ColorFlat.White;
            }
            if (PasswordPB.Password.Length < 8 || PasswordPB.Password == null)
            {
                error = true;
                MessageBox.Show("В поле пароль введено мало символов!\nПароль должен состоять из 8 и больше символов.", "Error");
                PasswordPB.Background = ColorFlat.Error;
                PasswordPB.Foreground = ColorFlat.White;
            }
            return error;
        }
        bool CheckAuthData()
        {
            User user;
            bool error = false;
            if (dbMonitoringEntities.gc().Users.Where(x => x.Login == LoginTB.Text && x.Password == PasswordPB.Password).Count() == 1)
                user = dbMonitoringEntities.gc().Users.Where(x => x.Login == LoginTB.Text && x.Password == PasswordPB.Password).Single();
            else if (dbMonitoringEntities.gc().Users.Where(x => x.Login == LoginTB.Text && x.Password == PasswordPB.Password).Count() == 0)
                error = true;
            if(error) MessageBox.Show("Логин или пароль не верный");
            return error;
        }
        void RememberData()
        {
            Settings settings = FileManager.GetSettings();
            settings.Remember = true;
            settings.Login = LoginTB.Text;
            settings.Password = PasswordPB.Password;
            FileManager.SetSettings(settings);
        }
        void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFieldInEmpty()) return;
            if (CheckAuthData()) return;
            if (RememberCB.IsChecked.Value) RememberData();
            ManagerPage.Page.Navigate(new AdminMenu());
        }
        void KeyPress(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) LogInBtn_Click(LogInBtn, null); }
        void Load(object sender, RoutedEventArgs e)
        {
            Settings settings = FileManager.GetSettings();
            if (settings.Remember)
            {
                LoginTB.Text = settings.Login;
                PasswordPB.Password = settings.Password;
                RememberCB.IsChecked = settings.Remember;
            }
        }
        void RegInBtn_Click(object sender, RoutedEventArgs e) { ManagerPage.Page.Navigate(new AuthReg.Reg()); }
    }
}