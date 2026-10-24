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
using WpfApp1.Functions;
using WpfApp1.res;
using static WpfApp1.Function;
namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        private int failedAttempts = 0;
        public AuthPage()
        {
            InitializeComponent();
        }
        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите логин или пароль");
                return;
            }
            if (CapchaPanel.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Сначала подтвердите капчу!");
                return;
            }
            string hashedPassword = GetHash(PasswordBox.Password);
            var user = Core.kursEntities.Users.AsNoTracking().FirstOrDefault(u => u.Email == TextBoxLogin.Text && u.Password == hashedPassword);

            if (user == null)
            {
                MessageBox.Show("Пользователь с такими данными не найден!");
                failedAttempts++;

                if (failedAttempts >= 3)
                {
                    if (CapchaPanel.Visibility != Visibility.Visible)
                    {
                        CaptchaSwitch(true);
                    }
                    CaptchaChange();
                }
                return;
            }
            else
            {
                Core.user = user;
                Core.SelectProfile();
                mainWindow.LoadDate();
            }
        }
        private void submitCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (captchaInput.Text != captcha.Text)
            {
                MessageBox.Show("Неверно введена капча", "Ошибка");
                CaptchaChange();
                captchaInput.Clear();
            }
            else
            {
                MessageBox.Show("Капча введена успешно, можете продолжить авторизацию", "Успех");
                CaptchaSwitch(false);
                failedAttempts = 0;
                captchaInput.Clear();
            }
        }

        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy || e.Command == ApplicationCommands.Cut || e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        public void CaptchaChange()
        {
            string allowchar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz12324567890";
            string pwd = "";
            Random r = new Random();
            for (int i = 0; i < 6; i++)
            {
                pwd += allowchar[r.Next(0, allowchar.Length)];
            }
            captcha.Text = pwd;
        }
        public void CaptchaSwitch(bool showCaptcha)
        {
            if (showCaptcha)
            {
                CapchaPanel.Visibility = Visibility.Visible;
                MainFiels.Visibility = Visibility.Collapsed;
            }
            else
            {
                CapchaPanel.Visibility = Visibility.Collapsed;
                MainFiels.Visibility = Visibility.Visible;
            }
        }
        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new RegPage());
        }
        private void txtHintLogin_MouseLeftButtonUp(object sender,MouseButtonEventArgs e)
        {
            TextBoxLogin.Focus();
        }
        private void txtHintPass_MouseLeftButtonUp(object sender,MouseButtonEventArgs e)
        {
            PasswordBox.Focus();
        }

    }

}

