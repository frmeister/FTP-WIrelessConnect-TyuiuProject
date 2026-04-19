using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Text.RegularExpressions;

namespace MainForm
{
    public partial class FormStats : Form
    {
        private List<FileData> filesData = new List<FileData>();
        private bool isCompareMode = false;
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
                    if (numericMaxMeasurements.Value > 100)
                    {
                        ca.AxisX.Interval = 5;
                    }
                    if (numericMaxMeasurements.Value > 250)
                    {
                        ca.AxisX.Interval = 10;
                    }
                    if (numericMaxMeasurements.Value > 500)
                    {
                        ca.AxisX.Interval = 20;
                    }
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
            MessageBox.Show($"Добавлен файл: {fileName}\nИзмерений: {parsed.Times.Count}",  "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            double minY = double.MaxValue, maxY = double.MinValue;

            foreach (int pointIndex in selectedPoints)
            {
                var series = new Series
                {
                    Name = checkedListPoints.Items[pointIndex].ToString(),
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2
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
            }

            if (chartData.Series.Count > 0 && maxY > minY)
            {
                double padding = (maxY - minY) * 0.1;
                if (padding == 0) padding = 1;
                chartData.ChartAreas[0].AxisY.Minimum = minY - padding;
                chartData.ChartAreas[0].AxisY.Maximum = maxY + padding;
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

            if (comboBoxFiles.SelectedIndex < 0) return;
            int file1Index = comboBoxFiles.SelectedIndex;
            int file2Index = (file1Index + 1) % filesData.Count;
            var file1 = filesData[file1Index];
            var file2 = filesData[file2Index];
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
            ca.AxisX.LabelStyle.Angle = -90; 
            ca.AxisX.Interval = 5;

            int maxMeasurements = Math.Min(50, Math.Min(file1.PointsValues[0].Count, file2.PointsValues[0].Count));
            double minY = double.MaxValue, maxY = double.MinValue;
            foreach (int pointIndex in selectedPoints)
            {
                string pointName = checkedListPoints.Items[pointIndex].ToString();
                var series1 = new Series
                {
                    Name = $"{file1.FileName} - {pointName}",
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    Color = System.Drawing.Color.Blue
                };
                var series2 = new Series
                {
                    Name = $"{file2.FileName} - {pointName}",
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2,
                    Color = System.Drawing.Color.Red,
                    BorderDashStyle = ChartDashStyle.Dash
                };

                var values1 = file1.PointsValues[pointIndex];
                var values2 = file2.PointsValues[pointIndex];

                for (int t = 0; t < maxMeasurements; t++)
                {
                    string time1 = t < file1.Times.Count ? file1.Times[t] : "";
                    string time2 = t < file2.Times.Count ? file2.Times[t] : "";
                    if (t < values1.Count)
                    {
                        int pointIndex1 = series1.Points.AddXY(t, values1[t]);
                        series1.Points[pointIndex1].AxisLabel = time1;
                        series1.Points[pointIndex1].ToolTip = $"{file1.FileName}\nВремя: {time1}\nТемпература: {values1[t]:F2}°C";
                    }

                    if (t < values2.Count)
                    {
                        int pointIndex2 = series2.Points.AddXY(t, values2[t]);
                        series2.Points[pointIndex2].AxisLabel = time2;
                        series2.Points[pointIndex2].ToolTip = $"{file2.FileName}\nВремя: {time2}\nТемпература: {values2[t]:F2}°C";
                    }
                    if (t < values1.Count && values1[t] < minY) minY = values1[t];
                    if (t < values1.Count && values1[t] > maxY) maxY = values1[t];
                    if (t < values2.Count && values2[t] < minY) minY = values2[t];
                    if (t < values2.Count && values2[t] > maxY) maxY = values2[t];
                }
                chartData.Series.Add(series1);
                chartData.Series.Add(series2);
            }

            if (maxY > minY)
            {
                double padding = (maxY - minY) * 0.1;
                ca.AxisY.Minimum = minY - padding;
                ca.AxisY.Maximum = maxY + padding;
            }
            chartData.Titles.Clear();
            chartData.Titles.Add($"Сравнение: {file1.FileName} (синий) vs {file2.FileName} (красный)");

            if (comboBoxFiles != null) comboBoxFiles.Enabled = false;
            if (checkedListPoints != null) checkedListPoints.Enabled = false;
        }

        private void buttonSaveChart_Click(object sender, EventArgs e)
        {
            if (chartData.Series.Count == 0)
            {
                MessageBox.Show("Нет графика для сохранения!", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PNG изображение|*.png|JPEG изображение|*.jpg|BMP изображение|*.bmp";
            saveDialog.Title = "Сохранить график";
            saveDialog.FileName = $"График_{DateTime.Now:yyyyMMdd_HHmmss}.png";

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    int width = chartData.Width;
                    int height = chartData.Height;
                    using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(width, height))
                    {
                        chartData.DrawToBitmap(bitmap, new System.Drawing.Rectangle(0, 0, width, height));
                        bitmap.Save(saveDialog.FileName);
                    }
                    MessageBox.Show($"График сохранен!\nПуть: {saveDialog.FileName}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка",    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}