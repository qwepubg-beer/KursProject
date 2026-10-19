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
using WpfApp1.Pages;
using WpfApp1.res;
using static WpfApp1.Function;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.NavigationService.Navigate(new AuthPage());
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if(MainFrame.NavigationService.CanGoBack)
            {
                MainFrame.NavigationService.GoBack();
            }
        }
        void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MessageBox.Show("Вы уверены, что хотите закрыть окно?", "Message",
            MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.No)
            {
                Core.Clear();
                e.Cancel = true;
            }
            else {  e.Cancel = false; }
        }
        public void LoadDate()
        {
            if (IsEnter() && Core.user.IsActive) 
            {
                switch (Core.user.RoleId)
                { 
                    case 1: User_menu.Visibility = Visibility.Visible;
                        break;
                    case 2: 
                        break;
                    case 3:
                        Admin_menu.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void Account_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new ProfilePage());
        }

        private void Users_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new AdminListUser());
        }

        private void Diagrams_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new DiagramPage());
        }

        private void Measurement_Click(object sender, RoutedEventArgs e)
        {
            //MainFrame.NavigationService.Navigate(new AuthPage());
        }

        private void Records_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new RecordsPage());
        }
    }
}
