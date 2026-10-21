using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.res;
using Excel = Microsoft.Office.Interop.Excel;
using Word = Microsoft.Office.Interop.Word;
using static WpfApp1.Function;
namespace WpfApp1.Pages
{
    public partial class DiagramPage : Page
    {
        public DiagramPage()
        {
            InitializeComponent();
            ChartArea mainArea = new ChartArea("MainArea");
            mainArea.AxisX.Title = "Дни недели";
            mainArea.AxisY.Title = "Калории (ккал)";
            mainArea.AxisX.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            mainArea.AxisY.MajorGrid.LineColor = System.Drawing.Color.LightGray;
            ChartEnergyBalance.ChartAreas.Add(mainArea);
            var intakeSeries = new Series("Приход калорий")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                Color = System.Drawing.Color.MediumSeaGreen
            };
            var expenditureSeries = new Series("Расход калорий")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                Color = System.Drawing.Color.Crimson
            };
            ChartEnergyBalance.Series.Add(intakeSeries);
            ChartEnergyBalance.Series.Add(expenditureSeries);
            LoadEnergyData();
        }
        private double GetActivityCalories(int userId, UserProfiles profile, DateTime dayStart)
        {
            if (profile.WeightKg == null) return 0;

            DateTime dayEnd = dayStart.AddDays(1);
            var userActivities = Core.kursEntities.UsersActivity.Where(ua => ua.UserID == userId && ua.Date >= dayStart && ua.Date < dayEnd).ToList();
            double totalActivityCalories = 0;
            foreach (var ua in userActivities)
            {
                if (ua.Activity != null)
                {
                    double timeInHours = ua.TimeMin / 60.0;
                    totalActivityCalories += ua.Activity.MET * profile.WeightKg.Value * timeInHours;
                }
            }
            return totalActivityCalories;
        }
        private void LoadEnergyData()
        {
            var profile = Core.kursEntities.UserProfiles.FirstOrDefault(p => p.ProfileID == Core.user.UserId);
            if (profile == null) return;
            double bmr = BMR(profile);
            var intakeSeries = ChartEnergyBalance.Series["Приход калорий"];
            var expenditureSeries = ChartEnergyBalance.Series["Расход калорий"];
            intakeSeries.Points.Clear();
            expenditureSeries.Points.Clear();
            DateTime today = DateTime.Today;
            string[] dayNames = { "Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб" };
            for (int i = 6; i >= 0; i--)
            {
                DateTime day = today.AddDays(-i);
                DateTime dayStart = day.Date;
                double intake = CalorIcintake(Core.user.UserId, dayStart);
                double activityCalories = GetActivityCalories(Core.user.UserId, profile, dayStart);
                double expenditure = bmr + activityCalories;
                string dayLabel = dayNames[(int)day.DayOfWeek];
                if (i == 0) dayLabel += "\n(сегодня)";
                intakeSeries.Points.AddXY(dayLabel, Math.Round(intake, 0));
                expenditureSeries.Points.AddXY(dayLabel, Math.Round(expenditure, 0));
            }
        }

        private void excelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void wordButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
