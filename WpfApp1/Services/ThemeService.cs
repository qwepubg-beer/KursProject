using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.Services
{
    internal class ThemeService
    {
        public interface IThemeService
        {
            void Apply(string themeName);
        }

        public class ThemeService : IThemeService
        {
            public void Apply(string themeName)
            {
                if (themeName != "Light" && themeName != "Dark")
                {
                    themeName = "Light";
                }
                try
                {
                    var themeDict = new ResourceDictionary
                    {
                        Source = new Uri($"pack://application:,,,/res/Themes/{themeName}.xaml", UriKind.Absolute)
                    };
                    Application.Current.Resources.MergedDictionaries.Clear();
                    Application.Current.Resources.MergedDictionaries.Add(themeDict);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось загрузить тему {themeName}.\nОшибка: {ex.Message}");
                }
            }
        }
}
