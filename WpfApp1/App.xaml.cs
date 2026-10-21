using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
            protected override void OnStartup(StartupEventArgs e)
            {
                base.OnStartup(e);
                string savedTheme = "Light";
                try
                {
                    var repo = new SettingsRepository();
                    savedTheme = repo.GetTheme();
                }
                catch (Exception ex)
                {
                }
                ApplyTheme(savedTheme);
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            public void ApplyTheme(string themeName)
            {
                if (themeName != "Light" && themeName != "Dark")
                    themeName = "Light";

                var themeDict = new ResourceDictionary
                {
                    // Путь к словарю. Обратите внимание на формат pack URI
                    Source = new Uri($"pack://application:,,,/Themes/{themeName}.xaml", UriKind.Absolute)
                };

                // Очищаем текущие и добавляем новый
                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(themeDict);
            }
        }
    }

