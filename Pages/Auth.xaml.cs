using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SystemMonitoring
{
    public partial class Auth : Page
    {
        public Auth()
        { InitializeComponent(); }
        bool CheckFieldInEmpty()
        {
            bool error = false;
            if (LoginTB.Text.Length <= 4 && LoginTB.Text.Length < 8)
            {
                //LoginTB.Background = Class.FlatColors.Red;
                //LoginTB.ForeColor = colorField;
                //PasswordTB.Background = Class.FlatColors.Red;
                //PasswordTB.ForeColor = colorField;
                MessageBox.Show("В поле логин и в поле пароль введенно мало символов!\nЛогин должен состоять из 5 и больше символов.\nПароль должен состоять из 8 и больше символов.", "Error");
                error = true;
            }
            else if (LoginTB.Text.Length <= 4 && PasswordPB.Password.Length > 8)
            {
                //LoginTB.Background = Class.FlatColors.Red;
                //LoginTB.ForeColor = colorField;
                MessageBox.Show("В поле логин введенно мало символов!\nЛогин должен состоять из 5 и больше символов.", "Error");
                error = true;
            }
            else if (LoginTB.Text.Length > 4 && PasswordPB.Password.Length < 8)
            {
                //PasswordTB.Background = Brushes.PaleGreen;
                //PasswordTB.ForeColor = colorField;
                MessageBox.Show("В поле пароль введенно мало символов!\nПароль должен состоять из 8 и больше символов.", "Error");
                error = true;
            }
            return error;
        }
        bool CheckAuthData()
        {
            string auth = LoginTB.Text;
            string pass = PasswordPB.Password;
            bool error = false;
            var users = Database.GetUserData(auth);
            foreach (var user in users)
            {
                if (user.login != auth && user.password != pass)
                {
                    MessageBox.Show("Данные не верны", "Not found user");
                    error = true;
                }
                else if (user.login == null)
                {
                    MessageBox.Show("Пользователь с таким логином не найден!", "Not found user");
                    error = true;
                }
                else if (user.password != pass)
                {
                    MessageBox.Show("Неверный пароль!", "Error authorization");
                    error = true;
                }
                DataBank.Access = user.access;
            }
            return error;
        }
        void RememberData()
        {
            bool distinct = false;
            using (var reader = new StreamReader(FileManager.GetPathConfig()))
                if (reader.ReadLine() != LoginTB.Text || reader.ReadLine() != PasswordPB.Password) distinct = true;
            if (distinct) 
                using (var writer = new StreamWriter(FileManager.GetPathConfig()))
                {
                    writer.WriteLine(LoginTB.Text);
                    writer.WriteLine(PasswordPB.Password);
                    writer.WriteLine(true);
                }
        }
        void LogInBtn_Click(object sender, RoutedEventArgs e)
        {
            if (CheckFieldInEmpty()) return;
            if (CheckAuthData()) return;
            if (RememberCB.IsChecked.Value) RememberData();
            ManagerPage.Page.Navigate(new Menu());
        }
        void KeyPress(object sender, KeyEventArgs e)
        { if (e.Key == Key.Enter) LogInBtn_Click(LogInBtn, null); }
        void Load(object sender, RoutedEventArgs e)
        {
            using (var reader = new StreamReader(FileManager.GetPathConfig()))
            {
                var login = reader.ReadLine();
                var password = reader.ReadLine();
                var isRemember = reader.ReadLine() == "True";
                if (isRemember)
                {
                    LoginTB.Text = login;
                    PasswordPB.Password = password;
                    RememberCB.IsChecked = isRemember;
                }
            }
        }
    }
}
