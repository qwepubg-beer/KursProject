
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using WpfApp1.res;
using static WpfApp1.Function;
//using ExcelApp = Microsoft.Office.Interop.Excel;
using WordApp = Microsoft.Office.Interop.Word;
namespace WpfApp1.Pages
{
    public partial class DiagramPage : System.Windows.Controls.Page
    {
        public DiagramPage()
        {
            InitializeComponent();

            TypeDiagram.SelectionChanged += TypeDiagram_SelectionChanged;
            Loaded += DiagramPage_Loaded;
        }

        private void DiagramPage_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDiagram();
        }

        private void TypeDiagram_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IsLoaded)
                UpdateDiagram();
        }

        private void UpdateDiagram()
        {
            if (TypeDiagram.SelectedIndex < 0)
                return;

            switch (TypeDiagram.SelectedIndex)
            {
                case 0:
                    Diagramm1();
                    break;
            }
        }

        void Diagramm1()
        {
            ChartEnergyBalance.ChartAreas.Clear();
            ChartEnergyBalance.Series.Clear();

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
            var intakeSeries = ChartEnergyBalance.Series["Приход калорий"];
            var expenditureSeries = ChartEnergyBalance.Series["Расход калорий"];

            intakeSeries.Points.Clear();
            expenditureSeries.Points.Clear();

            var weeklyData = GetWeeklyData();
            if (weeklyData.Count == 0)
                return;

            string[] dayNames = { "Вс", "Пн", "Вт", "Ср", "Чт", "Пт", "Сб" };

            for (int i = 0; i < weeklyData.Count; i++)
            {
                var data = weeklyData[i];
                string dayLabel = dayNames[(int)data.Date.DayOfWeek];

                if (i == weeklyData.Count - 1)
                    dayLabel += "\n(сегодня)";

                intakeSeries.Points.AddXY(dayLabel, Math.Round(data.Intake, 0));
                expenditureSeries.Points.AddXY(dayLabel, Math.Round(data.Expenditure, 0));
            }
        }

        private double GetActivityCalories(int userId, UserProfiles profile, DateTime dayStart)
        {
            if (profile.WeightKg == null)
                return 0;

            DateTime dayEnd = dayStart.AddDays(1);

            var userActivities = Core.kursEntities.UsersActivity
                .Where(ua => ua.UserID == userId && ua.Date >= dayStart && ua.Date < dayEnd)
                .ToList();

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

        private List<(DateTime Date, double Intake, double Expenditure)> GetWeeklyData()
        {
            var result = new List<(DateTime, double, double)>();
            var profile = Core.kursEntities.UserProfiles
                .FirstOrDefault(p => p.UserID == Core.user.UserId);

            if (profile == null)
                return result;

            double bmr = BMR(profile);
            DateTime today = DateTime.Today;

            for (int i = 6; i >= 0; i--)
            {
                DateTime day = today.AddDays(-i);
                DateTime dayStart = day.Date;

                double intake = CalorIcintake(Core.user.UserId, dayStart);
                double activityCalories = GetActivityCalories(Core.user.UserId, profile, dayStart);
                double expenditure = bmr + activityCalories;

                result.Add((day, intake, expenditure));
            }

            return result;
        }

        void Diagram1Word()
        {
            var weeklyData = GetWeeklyData();

            if (weeklyData.Count == 0)
            {
                MessageBox.Show(
                    "Нет данных для выгрузки.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Документ Word (*.docx)|*.docx",
                FileName = $"Отчет_о_питании_{DateTime.Today:yyyy-MM-dd}"
            };

            if (sfd.ShowDialog() != true)
                return;

            WordApp.Application wordApp = null;
            WordApp.Document doc = null;

            try
            {
                wordApp = new WordApp.Application();
                wordApp.Visible = false;
                doc = wordApp.Documents.Add();
                WordApp.Paragraph title = doc.Content.Paragraphs.Add();
                title.Range.Text = "Отчет о питании и энергобалансе за неделю";
                title.Range.Font.Name = "Times New Roman";
                title.Range.Font.Size = 16;
                title.Range.Font.Bold = 1;
                title.Format.SpaceAfter = 12;
                title.Alignment = WordApp.WdParagraphAlignment.wdAlignParagraphCenter;
                title.Range.InsertParagraphAfter();
                WordApp.Range tableRange = doc.Content.Paragraphs.Add().Range;
                WordApp.Table table = doc.Tables.Add(tableRange, weeklyData.Count + 1, 4);
                table.Borders.Enable = 1;
                table.Range.Font.Name = "Times New Roman";
                table.Range.Font.Size = 12;
                table.Cell(1, 1).Range.Text = "Дата";
                table.Cell(1, 2).Range.Text = "Приход (ккал)";
                table.Cell(1, 3).Range.Text = "Расход (ккал)";
                table.Cell(1, 4).Range.Text = "Баланс (ккал)";
                table.Rows[1].Range.Font.Bold = 1;

                for (int i = 0; i < weeklyData.Count; i++)
                {
                    var data = weeklyData[i];
                    double balance = data.Intake - data.Expenditure;

                    table.Cell(i + 2, 1).Range.Text = data.Date.ToString("dd.MM.yyyy");
                    table.Cell(i + 2, 2).Range.Text = Math.Round(data.Intake, 0).ToString();
                    table.Cell(i + 2, 3).Range.Text = Math.Round(data.Expenditure, 0).ToString();
                    table.Cell(i + 2, 4).Range.Text = Math.Round(balance, 0).ToString();
                }

                doc.SaveAs2(sfd.FileName);

                MessageBox.Show(
                    "Отчет успешно сохранен!",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при экспорте в Word: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                if (doc != null)
                {
                    doc.Close(WordApp.WdSaveOptions.wdDoNotSaveChanges);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                }

                if (wordApp != null)
                {
                    wordApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(wordApp);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }
        private void wordButton_Click(object sender, RoutedEventArgs e)
        {
            switch (TypeDiagram.SelectedIndex)
            {
                case 0:
                    Diagram1Word();
                    break;
            }
        }
        /*void Diagram1Excel()
        {
            var weeklyData = GetWeeklyData();

            if (weeklyData.Count == 0)
            {
                MessageBox.Show(
                    "Нет данных для выгрузки.",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "Документ Excel (*.xlsx)|*.xlsx",
                FileName = $"Отчет_о_питании_{DateTime.Today:yyyy-MM-dd}"
            };
            if (sfd.ShowDialog() != true)
                return;
            ExcelApp.Application excelApp = null;
            ExcelApp.Workbook workbook = null;
            ExcelApp.Worksheet worksheet = null;
            try
            {
                excelApp = new ExcelApp.Application();
                excelApp.Visible = false;
                workbook = excelApp.Workbooks.Add();
                worksheet = (ExcelApp.Worksheet)workbook.Sheets[1];
                worksheet.Name = "Энергобаланс";

                worksheet.Cells[1, 1] = "Дата";
                worksheet.Cells[1, 2] = "Приход (ккал)";
                worksheet.Cells[1, 3] = "Расход (ккал)";
                worksheet.Cells[1, 4] = "Баланс (ккал)";

                ExcelApp.Range headerRange = worksheet.Range["A1", "D1"];
                headerRange.Font.Bold = true;
                headerRange.Interior.Color =
                    System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGray);

                for (int i = 0; i < weeklyData.Count; i++)
                {
                    var data = weeklyData[i];
                    double balance = data.Intake - data.Expenditure;

                    worksheet.Cells[i + 2, 1] = data.Date.ToString("dd.MM.yyyy");
                    worksheet.Cells[i + 2, 2] = Math.Round(data.Intake, 0);
                    worksheet.Cells[i + 2, 3] = Math.Round(data.Expenditure, 0);
                    worksheet.Cells[i + 2, 4] = Math.Round(balance, 0);

                    if (balance < 0)
                    {
                        worksheet.Cells[i + 2, 4].Font.Color =
                            System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Red);
                    }
                }
                worksheet.Columns.AutoFit();
                workbook.SaveAs(sfd.FileName);

                MessageBox.Show(
                    "Отчет успешно сохранен в Excel!",
                    "Успех",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Ошибка при экспорте в Excel: {ex.Message}",
                    "Ошибка",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                if (worksheet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(worksheet);

                if (workbook != null)
                {
                    workbook.Close(false, Type.Missing, Type.Missing);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                }

                if (excelApp != null)
                {
                    excelApp.Quit();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
                }

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }*/
        private void excelButton_Click(object sender, RoutedEventArgs e)
        {
            switch (TypeDiagram.SelectedIndex)
            {
                case 0:
                    //Diagram1Excel();
                    break;
            }
        }
    }
}
