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
using System.Windows.Forms.DataVisualization.Charting;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
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
        private void LoadEnergyData()
        {
            // ChartEnergyBalance.Series["Приход калорий"].Points.AddXY("Пн", 2500);
            // ChartEnergyBalance.Series["Расход калорий"].Points.AddXY("Пн", 2800);
        }

        private void excelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void wordButton_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
