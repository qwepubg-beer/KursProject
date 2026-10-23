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
    /// Логика взаимодействия для RegPage.xaml
    /// </summary>
    public partial class RegPage : Page
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
        public RegPage()
        {
            InitializeComponent();
            loadpage();
        }
        void loadpage()
        {
            comboBxRole.SelectedIndex = 0;
        }
        bool Registration(string mail, string pas1, string pas2,int RoleID)
        {
            if (string.IsNullOrEmpty(mail) || string.IsNullOrEmpty(pas1) || string.IsNullOrEmpty(pas2))
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
                        if(IsValidEmail(mail))
                        {
                        Users user = Core.kursEntities.Users.FirstOrDefault(a => a.Email == mail);
                        if (user is null)
                        {
                                Users newUser = new Users()
                                {
                                    Email = mail,
                                    Password = GetHash(pas1),
                                    IsActive = true,
                                    RoleId = RoleID,
                                    CreatedAt = DateTime.Now
                                };
                                Core.kursEntities.Users.Add(newUser);
                                Core.kursEntities.SaveChanges();
                                UserProfiles userProfiles = new UserProfiles()
                                {
                                    ProfileID = newUser.UserId,
                                    StyleId = 1
                                };
                                Core.kursEntities.UserProfiles.Add(userProfiles);
                                Core.kursEntities.SaveChanges();
                                return true;
                            }
                        else
                        {
                            MessageBox.Show("Пользователь уже зарегистрирован!");
                            return false;
                        }
                        }
                        else
                        {
                            MessageBox.Show("Email не существует!");
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

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            if(comboBxRole.SelectedItem !=null)
            {
                if (Registration(TextBoxEmail.Text, PasswordBox.Password, PasswordBox2.Password, comboBxRole.SelectedIndex==0?1:2))
                {
                    MessageBox.Show("Вы зарегистрированы!");
                    mainWindow.MainFrame.NavigationService.Navigate(new AuthPage());
                }
            }
            else
            {

            }
            
        }
    }
}
