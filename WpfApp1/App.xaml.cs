using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Functions;
namespace WpfApp1
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var themeService = new ThemeService();
            themeService.Apply("Light");
            LoginWindow loginWin = new LoginWindow();
            loginWin.Show();
        }
    }
    }

