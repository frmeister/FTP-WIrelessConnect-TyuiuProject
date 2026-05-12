using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace MainForm
{
    public partial class FormStats : Form
    {
        private List<FileData> filesData = new List<FileData>();
        private bool isCompareMode = false;
        private readonly Color[] pointColors = new Color[]
        {
            Color.Blue, Color.Red, Color.Green, Color.Orange, Color.Purple, Color.Brown,
            Color.Cyan, Color.Magenta, Color.DarkBlue, Color.DarkRed, Color.DarkGreen,
            Color.DarkOrange, Color.DarkViolet, Color.HotPink, Color.Teal, Color.Olive
        };
        private readonly Color[] fileColors = new Color[]
        {
            Color.Blue, Color.Red, Color.Green, Color.Orange, Color.Purple, Color.Brown,
            Color.Cyan, Color.Magenta, Color.DarkBlue, Color.DarkRed, Color.DarkGreen,
            Color.DarkOrange, Color.DarkViolet, Color.HotPink, Color.Teal, Color.Olive
        };
        private readonly ChartDashStyle[] lineStyles = new ChartDashStyle[]
        {
            ChartDashStyle.Solid, ChartDashStyle.Dash, ChartDashStyle.Dot,ChartDashStyle.DashDot, ChartDashStyle.DashDotDot  
        };

        private class FileData
        {
            public string FileName { get; set; }
            public List<string> Times { get; set; }
            public List<List<double>> PointsValues { get; set; }
            public double[] AvgByPoint { get; set; }
            public double OverallAverage { get; set; }
        }

        public FormStats()
        {
            InitializeComponent();
            var ca = chartData.ChartAreas[0];
            if (comboBoxFiles != null)
            {
                comboBoxFiles.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBoxFiles.SelectedIndexChanged += (s, e) => { if (!isCompareMode) UpdateChart(); };
            }
            if (numericMaxMeasurements != null)
            {
                numericMaxMeasurements.Minimum = 10;
                numericMaxMeasurements.Maximum = 1000;
                numericMaxMeasurements.Value = 100;
                numericMaxMeasurements.ValueChanged += (s, e) =>
                {
                    if (!isCompareMode)
                        UpdateChart();
                };
            }
            if (checkedListPoints != null)
            {
                checkedListPoints.Items.Clear();
                for (int i = 1; i <= 16; i++)
                    checkedListPoints.Items.Add($"М1 К{i}", i <= 3);
                checkedListPoints.ItemCheck += (s, e) =>
                {
                    if (!isCompareMode)
                        this.BeginInvoke(new Action(() => UpdateChart()));
                };
            }
            if (chartData != null)
            {
                ca.AxisX.Title = "Номер измерения";
                ca.AxisY.Title = "Температура, °C";
                ca.AxisX.LabelStyle.Angle = -45;
            }
        }

        public void AddFileData(string data, string fileName)
        {
            if (string.IsNullOrEmpty(data) || data.Length < 100)
            {
                MessageBox.Show($"Файл {fileName} пуст или слишком мал!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var parsed = ParseData(data);
            parsed.FileName = fileName;
            filesData.Add(parsed);
            if (comboBoxFiles != null)
            {
                comboBoxFiles.Items.Add(fileName);
                if (comboBoxFiles.Items.Count == 1)
                    comboBoxFiles.SelectedIndex = 0;
            }
            if (checkedListBoxFiles != null)
            {
                checkedListBoxFiles.Items.Add(fileName, false);
            }
            MessageBox.Show($"Добавлен файл: {fileName}\nИзмерений: {parsed.Times.Count}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private FileData ParseData(string data)
        {
            var result = new FileData
            {
                Times = new List<string>(),
                PointsValues = new List<List<double>>(),
                AvgByPoint = new double[16]
            };
            for (int i = 0; i < 16; i++)
                result.PointsValues.Add(new List<double>());
            var lines = data.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (line.Contains("Протокол") || line.Contains("прибором") || line.Contains("°C"))
                    continue;
                if (!line.Contains(":")) continue;
                var parts = Regex.Split(line.Trim(), @"\s+");
                if (parts.Length >= 2 && parts[0].Contains(":"))
                {
                    result.Times.Add(parts[0]);
                    int startIdx = 1;
                    if (parts.Length > startIdx && (parts[startIdx].Contains(",") || parts[startIdx].Contains(".")))
                    {
                        double testVal;
                        if (double.TryParse(parts[startIdx].Replace(',', '.'),
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture, out testVal))
                        {
                            if (testVal > -30 && testVal < 30)
                                startIdx++;
                        }
                    }
                    for (int i = 0; i < 16 && startIdx + i < parts.Length; i++)
                    {
                        double value = 0;
                        string valStr = parts[startIdx + i].Trim().Replace(',', '.');
                        valStr = Regex.Replace(valStr, @"[^0-9.\-]", "");
                        double.TryParse(valStr,
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture, out value);
                        result.PointsValues[i].Add(value);
                    }
                }
            }
            double totalSum = 0;
            for (int point = 0; point < 16; point++)
            {
                result.AvgByPoint[point] = result.PointsValues[point].Count > 0
                    ? result.PointsValues[point].Average()
                    : 0;
                totalSum += result.AvgByPoint[point];
            }
            result.OverallAverage = totalSum / 16;
            return result;
        }

        private void ConfigureAxisX(ChartArea ca, int dataCount)
        {
            if (dataCount <= 20)
                ca.AxisX.Interval = 1;
            else if (dataCount <= 50)
                ca.AxisX.Interval = 2;
            else if (dataCount <= 100)
                ca.AxisX.Interval = 5;
            else if (dataCount <= 200)
                ca.AxisX.Interval = 10;
            else if (dataCount <= 500)
                ca.AxisX.Interval = 20;
            else
                ca.AxisX.Interval = 40;

            if (ca.AxisX.Interval <= 2)
                ca.AxisX.LabelStyle.Angle = -45;
            else if (ca.AxisX.Interval <= 10)
                ca.AxisX.LabelStyle.Angle = -60;
            else
                ca.AxisX.LabelStyle.Angle = -90;
        }

        private void UpdateChart()
        {
            if (comboBoxFiles == null || comboBoxFiles.SelectedIndex < 0) return;
            if (filesData.Count == 0) return;
            if (checkedListPoints == null) return;
            chartData.Series.Clear();
            var data = filesData[comboBoxFiles.SelectedIndex];
            var selectedPoints = new List<int>();
            for (int i = 0; i < checkedListPoints.Items.Count; i++)
            {
                if (checkedListPoints.GetItemChecked(i))
                    selectedPoints.Add(i);
            }
            if (selectedPoints.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы одну точку для отображения!");
                return;
            }
            int maxMeasurements = data.PointsValues[0].Count;
            if (numericMaxMeasurements != null)
                maxMeasurements = Math.Min((int)numericMaxMeasurements.Value, data.PointsValues[0].Count);
            var ca = chartData.ChartAreas[0];
            ConfigureAxisX(ca, maxMeasurements);
            double minY = double.MaxValue, maxY = double.MinValue;
            int pointIdx_counter = 0;
            foreach (int pointIndex in selectedPoints)
            {
                var series = new Series
                {
                    Name = checkedListPoints.Items[pointIndex].ToString(),
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    Color = pointColors[pointIdx_counter % pointColors.Length],
                    BorderDashStyle = ChartDashStyle.Solid 
                };
                var values = data.PointsValues[pointIndex];
                for (int t = 0; t < maxMeasurements && t < values.Count; t++)
                {
                    string time = t < data.Times.Count ? data.Times[t] : "";
                    int pointIdx = series.Points.AddXY(t, values[t]);
                    series.Points[pointIdx].AxisLabel = time;
                    series.Points[pointIdx].ToolTip = $"{data.FileName}\nВремя: {time}\nТемпература: {values[t]:F2}°C";
                    if (values[t] < minY) minY = values[t];
                    if (values[t] > maxY) maxY = values[t];
                }
                chartData.Series.Add(series);
                pointIdx_counter++;
            }
            if (chartData.Series.Count > 0 && maxY > minY)
            {
                double padding = (maxY - minY) * 0.1;
                if (padding == 0) padding = 1;
                ca.AxisY.Minimum = minY - padding;
                ca.AxisY.Maximum = maxY + padding;
            }
            chartData.Titles.Clear();
            chartData.Titles.Add($"{data.FileName} - первые {maxMeasurements} измерений");
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            isCompareMode = false;
            chartData.Titles.Clear();
            if (comboBoxFiles != null) comboBoxFiles.Enabled = true;
            if (checkedListPoints != null) checkedListPoints.Enabled = true;
            if (comboBoxFiles != null && comboBoxFiles.SelectedIndex >= 0)
                UpdateChart();
        }

        private void buttonOptimizedChart_Click_1(object sender, EventArgs e)
        {
            if (filesData.Count < 2)
            {
                MessageBox.Show("Для сравнения нужно минимум 2 файла!\nПолучите еще один файл.",
                    "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            List<FileData> selectedFiles = new List<FileData>();
            for (int i = 0; i < checkedListBoxFiles.CheckedItems.Count; i++)
            {
                string fileName = checkedListBoxFiles.CheckedItems[i].ToString();
                FileData file = filesData.FirstOrDefault(f => f.FileName == fileName);
                if (file != null)
                    selectedFiles.Add(file);
            }
            if (selectedFiles.Count < 2)
            {
                MessageBox.Show("Выберите минимум 2 файла для сравнения в списке файлов!");
                return;
            }
            var selectedPoints = new List<int>();
            for (int i = 0; i < checkedListPoints.Items.Count; i++)
            {
                if (checkedListPoints.GetItemChecked(i))
                    selectedPoints.Add(i);
            }
            if (selectedPoints.Count == 0)
            {
                MessageBox.Show("Выберите точки для сравнения!");
                return;
            }
            isCompareMode = true;
            chartData.Series.Clear();
            var ca = chartData.ChartAreas[0];
            ca.AxisX.Title = "Номер измерения";
            ca.AxisY.Title = "Температура, °C";
            int maxMeasurements = selectedFiles.Min(f => f.PointsValues[0].Count);
            maxMeasurements = Math.Min((int)numericMaxMeasurements.Value, maxMeasurements);
            ConfigureAxisX(ca, maxMeasurements);
            double minY = double.MaxValue, maxY = double.MinValue;
            int fileIndex = 0;

            foreach (var file in selectedFiles)
            {
                Color fileColor = fileColors[fileIndex % fileColors.Length];
                int pointIndexInFile = 0;

                foreach (int pointIndex in selectedPoints)
                {
                    string pointName = checkedListPoints.Items[pointIndex].ToString();
                    var series = new Series
                    {
                        Name = $"{file.FileName} - {pointName}",
                        ChartType = SeriesChartType.Line,
                        BorderWidth = 2,
                        Color = fileColor,
                        BorderDashStyle = lineStyles[pointIndexInFile % lineStyles.Length] 
                    };

                    var values = file.PointsValues[pointIndex];
                    for (int t = 0; t < maxMeasurements && t < values.Count; t++)
                    {
                        string time = t < file.Times.Count ? file.Times[t] : "";
                        int pointIdx = series.Points.AddXY(t, values[t]);
                        if (!string.IsNullOrEmpty(time))
                            series.Points[pointIdx].AxisLabel = time;
                        series.Points[pointIdx].ToolTip = $"{file.FileName}\nТочка: {pointName}\nВремя: {time}\nТемпература: {values[t]:F2}°C";

                        if (values[t] < minY) minY = values[t];
                        if (values[t] > maxY) maxY = values[t];
                    }
                    chartData.Series.Add(series);
                    pointIndexInFile++;
                }
                fileIndex++;
            }
            if (maxY > minY)
            {
                double padding = (maxY - minY) * 0.1;
                if (padding == 0) padding = 1;
                ca.AxisY.Minimum = minY - padding;
                ca.AxisY.Maximum = maxY + padding;
            }
            string title = "Сравнительный анализ: ";
            for (int i = 0; i < selectedFiles.Count; i++)
            {
                title += selectedFiles[i].FileName;
                if (i < selectedFiles.Count - 1)
                    title += " vs ";
            }
            chartData.Titles.Clear();
            chartData.Titles.Add(title);
            chartData.Legends[0].Docking = Docking.Top;
            chartData.Legends[0].Alignment = StringAlignment.Center;

            if (comboBoxFiles != null) comboBoxFiles.Enabled = false;
            if (checkedListPoints != null) checkedListPoints.Enabled = false;
        }

        private void buttonSaveChart_Click(object sender, EventArgs e)
        {
            if (chartData.Series.Count == 0)
            {
                MessageBox.Show("Нет графика для сохранения!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PNG изображение|*.png";
            saveDialog.Title = "Сохранить график";
            saveDialog.FileName = $"График_{DateTime.Now:yyyyMMdd_HHmmss}.png";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    chartData.SaveImage(saveDialog.FileName, ChartImageFormat.Png);
                    MessageBox.Show($"График сохранен!\nПуть: {saveDialog.FileName}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}