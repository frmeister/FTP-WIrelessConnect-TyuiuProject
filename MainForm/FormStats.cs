using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MainForm
{
    public partial class FormStats : Form
    {
        private DataTable table;
        public FormStats()
        {
            InitializeComponent();
        }
        public void ReceiveData(string data)
        {
            ParseDataToTable(data);
        }
        private void ParseDataToTable(string data)
        {
            string[] lines = data.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            table = new DataTable();
            table.Columns.Add("Точка", typeof(string));
            List<string> times = new List<string>();
            List<double[]> pointsData = new List<double[]>();

            foreach (string line in lines)
            {
                if (!line.Contains(":") || line.Contains("Протокол") || line.Contains("прибором") || line.Contains("°C"))
                    continue;
                string[] parts = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 18 && parts[0].Contains(":"))
                {
                    string time = parts[0].Replace(':', '-');
                    if (!times.Contains(time))
                        times.Add(time);
                    else
                        times.Add(time + "_" + times.Count);
                    double[] values = new double[16];
                    int startIdx = parts[1].Contains(",") ? 2 : 1;
                    for (int i = 0; i < 16 && startIdx + i < parts.Length; i++)
                    {
                        double.TryParse(parts[startIdx + i].Replace(',', '.'),
                            System.Globalization.NumberStyles.Any,
                            System.Globalization.CultureInfo.InvariantCulture, out values[i]);
                    }
                    pointsData.Add(values);
                }
            }
            for (int i = 0; i < times.Count; i++)
                table.Columns.Add(times[i], typeof(double));
            for (int point = 1; point <= 16; point++)
            {
                DataRow row = table.NewRow();
                row["Точка"] = $"М1 К{point}";
                for (int t = 0; t < times.Count; t++)
                    row[t + 1] = pointsData[t][point - 1];
                table.Rows.Add(row);
            }
            dataGridView.DataSource = table;
        }


        private void buttonBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonChart_Click(object sender, EventArgs e)
        {
            chartData.Series.Clear();
            chartData.Titles.Clear();
            if (dataGridView.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите точку(и) для построения графика!", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ChartArea ca = chartData.ChartAreas[0];
            ca.AxisX.CustomLabels.Clear();
            ca.AxisX.LabelStyle.Angle = -90;
            ca.AxisX.LabelStyle.Interval = 1;
            ca.AxisX.Interval = 1;
            ca.AxisX.Title = "Время";
            ca.AxisY.Title = "Температура, °C";
            ca.AxisY.Minimum = double.NaN;
            ca.AxisY.Maximum = double.NaN;
            ca.AxisY.IsStartedFromZero = false;
            double minY = double.MaxValue;
            double maxY = double.MinValue;

            foreach (DataGridViewRow selectedRow in dataGridView.SelectedRows)
            {
                string pointName = selectedRow.Cells["Точка"].Value.ToString();
                Series series = new Series
                {
                    Name = pointName,
                    ChartType = SeriesChartType.Line,
                    BorderWidth = 2
                };
                for (int col = 1; col < dataGridView.Columns.Count; col++)
                {
                    string time = dataGridView.Columns[col].HeaderText;
                    double value = Convert.ToDouble(selectedRow.Cells[col].Value);
                    series.Points.AddXY(col, value);
                    series.Points[col - 1].AxisLabel = time;
                    if (value < minY) minY = value;
                    if (value > maxY) maxY = value;
                }
                chartData.Series.Add(series);
            }
            double padding = (maxY - minY) * 0.1;
            ca.AxisY.Minimum = minY - padding;
            ca.AxisY.Maximum = maxY + padding;
        }
    }
}
