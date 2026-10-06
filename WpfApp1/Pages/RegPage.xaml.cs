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

namespace WpfApp1.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
            loadpage();
        }
        void loadpage()
        {
            comboBxRole.SelectedIndex = 0;
        }
        void IsEmptyFields()
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) ||string.IsNullOrEmpty(TextBoxFio.Text) || string.IsNullOrEmpty(PasswordBox.Password) || string.IsNullOrEmpty(PasswordBox2.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }
           
        }
        bool Registration(string log, string pas1, string pas2, string lastName)
        {
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(TextBoxFio.Text) || string.IsNullOrEmpty(PasswordBox.Password) || string.IsNullOrEmpty(PasswordBox2.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return false;
            }
            else
            {
                if(pas1== pas2)
                {
                    if (CheakCorrectPassword(pas1))
                    {
                        Users user = Core.kursEntities.Users.FirstOrDefault(a => a.Login == log);
                        if (user is null)
                        {
                            Users newUser = new Users()
                            {
                                Login =log,
                                Password = pas1,
                                IsActive = true,
                            };
                        }
                        else
                        {
                            MessageBox.Show("Пользователь уже зарегистрирован!");
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Пароли не совпадают");
                    return false;
                }
                
            }
        }
        bool CheakCorrectPassword(string password) 
        {
            if (password.Length >= 6)
            {
                bool en = true;
                bool number = false;

                for (int i = 0; i < password.Length; i++)
                {
                    if (password[i] >= '0' && password[i] <= '9') number =true;
                    else if (!((password[i] >= 'A' && password[i] <='Z') || (password[i] >= 'a' && password[i] <= 'z'))) en =false;
                }
                if (!en)
                {
                    MessageBox.Show("Используйте только английскую расскладку!");
                    return false;
                }
                else if (!number)
                { 
                    MessageBox.Show("Добавьте хотябы одну цифру!");
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else{
                return false;
            }
        }
    }
}
