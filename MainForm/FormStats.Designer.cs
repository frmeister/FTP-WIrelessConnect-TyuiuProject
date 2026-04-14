namespace MainForm
{
    partial class FormStats
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStats));
            chartData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            dataGridView = new DataGridView();
            buttonBack = new Button();
            panel1 = new Panel();
            buttonChart = new Button();
            panel2 = new Panel();
            splitter1 = new Splitter();
            panel3 = new Panel();
            ((System.ComponentModel.ISupportInitialize)chartData).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // chartData
            // 
            chartArea1.Name = "ChartArea1";
            chartData.ChartAreas.Add(chartArea1);
            chartData.Dock = DockStyle.Fill;
            legend1.Name = "Legend1";
            chartData.Legends.Add(legend1);
            chartData.Location = new Point(0, 0);
            chartData.Name = "chartData";
            chartData.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chartData.Series.Add(series1);
            chartData.Size = new Size(924, 668);
            chartData.TabIndex = 0;
            chartData.Text = "chart1";
            // 
            // dataGridView
            // 
            dataGridView.AllowUserToOrderColumns = true;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView.Dock = DockStyle.Fill;
            dataGridView.Location = new Point(0, 0);
            dataGridView.Name = "dataGridView";
            dataGridView.RowHeadersWidth = 51;
            dataGridView.Size = new Size(759, 668);
            dataGridView.TabIndex = 0;
            // 
            // buttonBack
            // 
            buttonBack.Image = (Image)resources.GetObject("buttonBack.Image");
            buttonBack.Location = new Point(12, 12);
            buttonBack.Name = "buttonBack";
            buttonBack.Size = new Size(144, 79);
            buttonBack.TabIndex = 0;
            buttonBack.UseVisualStyleBackColor = true;
            buttonBack.Click += buttonBack_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(buttonChart);
            panel1.Controls.Add(buttonBack);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 668);
            panel1.Name = "panel1";
            panel1.Size = new Size(1693, 109);
            panel1.TabIndex = 2;
            // 
            // buttonChart
            // 
            buttonChart.Image = (Image)resources.GetObject("buttonChart.Image");
            buttonChart.Location = new Point(171, 12);
            buttonChart.Name = "buttonChart";
            buttonChart.Size = new Size(144, 79);
            buttonChart.TabIndex = 1;
            buttonChart.UseVisualStyleBackColor = true;
            buttonChart.Click += buttonChart_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(dataGridView);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(759, 668);
            panel2.TabIndex = 3;
            // 
            // splitter1
            // 
            splitter1.Location = new Point(759, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(10, 668);
            splitter1.TabIndex = 4;
            splitter1.TabStop = false;
            // 
            // panel3
            // 
            panel3.Controls.Add(chartData);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(769, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(924, 668);
            panel3.TabIndex = 5;
            // 
            // FormStats
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1693, 777);
            Controls.Add(panel3);
            Controls.Add(splitter1);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "FormStats";
            Text = "FormStats";
            ((System.ComponentModel.ISupportInitialize)chartData).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private DataGridView dataGridView;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartData;
        private Button buttonBack;
        private Panel panel1;
        private Panel panel2;
        private Splitter splitter1;
        private Panel panel3;
        private Button buttonChart;
    }
}