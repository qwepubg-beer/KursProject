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
        private Users currentUser;
        public AuthPage()
        {
            InitializeComponent();
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(TextBoxLogin.Text) ||
            string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите логин или пароль");
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
                        if (captcha.Visibility != Visibility.Visible)
                        {
                            CaptchaSwitch();
                        }
                        CaptchaChange();
                    }
                    return;
                }
                else
                {
                    MessageBox.Show("Пользователь успешно найден!");

                    switch (user.RoleId)
                    {
                        case 1:
                            break;
                        case 2:
                            break;

                    }
                }
        }
private void submitCaptcha_Click(object sender, RoutedEventArgs e)
        {
            if (captchaInput.Text != captcha.Text)
            {
                MessageBox.Show("Неверно введена капча", "Ошибка");
                CaptchaChange();

            }
            else
            {
                MessageBox.Show("Капча введена успешно, можете продолжить авторизацию", "Успех");
                CaptchaSwitch();
                failedAttempts = 0;
            }
        }
        private void textBox_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
            e.Command == ApplicationCommands.Cut ||
            e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }
        public void CaptchaChange()

        {

            String allowchar = " ";
            allowchar = "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            allowchar += "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,y,z";
            allowchar += "1,2,3,4,5,6,7,8,9,0";
            char[] a = { ',' };
            String[] ar = allowchar.Split(a);
            String pwd = "";
            string temp = "";
            Random r = new Random();

            for (int i = 0; i < 6; i++)
            {
                temp = ar[(r.Next(0, ar.Length))];
                pwd += temp;
            }
            captcha.Text = pwd;
        }
        public void CaptchaSwitch()
        {
            switch (captcha.Visibility)
            {
                case Visibility.Visible:
                    TextBoxLogin.Clear();
                    PasswordBox.Clear();
                    captcha.Visibility = Visibility.Hidden;
                    captchaInput.Visibility = Visibility.Hidden;
                    labelCaptcha.Visibility = Visibility.Hidden;
                    submitCaptcha.Visibility = Visibility.Hidden;
                    TextBoxLogin.Visibility = Visibility.Visible;
                    PasswordBox.Visibility = Visibility.Visible;
                    ButtonChangePassword.Visibility = Visibility.Visible;
                    ButtonEnter.Visibility = Visibility.Visible;
                    ButtonReg.Visibility = Visibility.Visible;
                    return;
                case Visibility.Hidden:
                    captcha.Visibility = Visibility.Visible;
                    captchaInput.Visibility = Visibility.Visible;
                    labelCaptcha.Visibility = Visibility.Visible;
                    submitCaptcha.Visibility = Visibility.Visible;

                    TextBoxLogin.Visibility = Visibility.Hidden;
                    PasswordBox.Visibility = Visibility.Hidden;

                    ButtonChangePassword.Visibility = Visibility.Hidden;
                    ButtonEnter.Visibility = Visibility.Hidden;
                    ButtonReg.Visibility = Visibility.Hidden;
                    return;
            }
        }
        private void ButtonChangePassword_Click(object sender, RoutedEventArgs e)
        {
           // NavigationService?.Navigate(new ChangePassPage());
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
}
