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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormStats));
            chartData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            buttonBack = new Button();
            panel1 = new Panel();
            buttonSaveChart = new Button();
            buttonOptimizedChart = new Button();
            comboBoxFiles = new ComboBox();
            checkedListPoints = new CheckedListBox();
            panel2 = new Panel();
            numericMaxMeasurements = new NumericUpDown();
            panel3 = new Panel();
            splitter1 = new Splitter();
            checkedListBoxFiles = new CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)chartData).BeginInit();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericMaxMeasurements).BeginInit();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // chartData
            // 
            chartArea2.Name = "ChartArea1";
            chartData.ChartAreas.Add(chartArea2);
            chartData.Dock = DockStyle.Fill;
            legend2.Name = "Legend1";
            chartData.Legends.Add(legend2);
            chartData.Location = new Point(0, 0);
            chartData.Name = "chartData";
            chartData.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Excel;
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            chartData.Series.Add(series2);
            chartData.Size = new Size(1477, 658);
            chartData.TabIndex = 0;
            chartData.Text = "chart1";
            // 
            // buttonBack
            // 
            buttonBack.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonBack.Image = (Image)resources.GetObject("buttonBack.Image");
            buttonBack.Location = new Point(12, 26);
            buttonBack.Name = "buttonBack";
            buttonBack.Size = new Size(156, 81);
            buttonBack.TabIndex = 0;
            buttonBack.UseVisualStyleBackColor = true;
            buttonBack.Click += buttonBack_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(buttonSaveChart);
            panel1.Controls.Add(buttonOptimizedChart);
            panel1.Controls.Add(buttonBack);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 658);
            panel1.Name = "panel1";
            panel1.Size = new Size(1693, 119);
            panel1.TabIndex = 2;
            // 
            // buttonSaveChart
            // 
            buttonSaveChart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonSaveChart.Image = (Image)resources.GetObject("buttonSaveChart.Image");
            buttonSaveChart.Location = new Point(336, 26);
            buttonSaveChart.Name = "buttonSaveChart";
            buttonSaveChart.Size = new Size(156, 81);
            buttonSaveChart.TabIndex = 7;
            buttonSaveChart.UseVisualStyleBackColor = true;
            buttonSaveChart.Click += buttonSaveChart_Click;
            // 
            // buttonOptimizedChart
            // 
            buttonOptimizedChart.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            buttonOptimizedChart.Image = (Image)resources.GetObject("buttonOptimizedChart.Image");
            buttonOptimizedChart.Location = new Point(174, 26);
            buttonOptimizedChart.Name = "buttonOptimizedChart";
            buttonOptimizedChart.Size = new Size(156, 81);
            buttonOptimizedChart.TabIndex = 2;
            buttonOptimizedChart.UseVisualStyleBackColor = true;
            buttonOptimizedChart.Click += buttonOptimizedChart_Click_1;
            // 
            // comboBoxFiles
            // 
            comboBoxFiles.Dock = DockStyle.Bottom;
            comboBoxFiles.FormattingEnabled = true;
            comboBoxFiles.Location = new Point(0, 630);
            comboBoxFiles.Name = "comboBoxFiles";
            comboBoxFiles.Size = new Size(216, 28);
            comboBoxFiles.TabIndex = 5;
            // 
            // checkedListPoints
            // 
            checkedListPoints.Dock = DockStyle.Top;
            checkedListPoints.FormattingEnabled = true;
            checkedListPoints.Location = new Point(0, 27);
            checkedListPoints.Name = "checkedListPoints";
            checkedListPoints.Size = new Size(216, 466);
            checkedListPoints.TabIndex = 8;
            // 
            // panel2
            // 
            panel2.Controls.Add(checkedListBoxFiles);
            panel2.Controls.Add(checkedListPoints);
            panel2.Controls.Add(comboBoxFiles);
            panel2.Controls.Add(numericMaxMeasurements);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(216, 658);
            panel2.TabIndex = 6;
            // 
            // numericMaxMeasurements
            // 
            numericMaxMeasurements.Dock = DockStyle.Top;
            numericMaxMeasurements.Location = new Point(0, 0);
            numericMaxMeasurements.Name = "numericMaxMeasurements";
            numericMaxMeasurements.Size = new Size(216, 27);
            numericMaxMeasurements.TabIndex = 8;
            // 
            // panel3
            // 
            panel3.Controls.Add(chartData);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(216, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(1477, 658);
            panel3.TabIndex = 5;
            // 
            // splitter1
            // 
            splitter1.Location = new Point(216, 0);
            splitter1.Name = "splitter1";
            splitter1.Size = new Size(10, 658);
            splitter1.TabIndex = 7;
            splitter1.TabStop = false;
            // 
            // checkedListBoxFiles
            // 
            checkedListBoxFiles.Dock = DockStyle.Fill;
            checkedListBoxFiles.FormattingEnabled = true;
            checkedListBoxFiles.Location = new Point(0, 493);
            checkedListBoxFiles.Name = "checkedListBoxFiles";
            checkedListBoxFiles.Size = new Size(216, 137);
            checkedListBoxFiles.TabIndex = 8;
            // 
            // FormStats
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1693, 777);
            Controls.Add(splitter1);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "FormStats";
            Text = "FormStats";
            ((System.ComponentModel.ISupportInitialize)chartData).EndInit();
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericMaxMeasurements).EndInit();
            panel3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private System.Windows.Forms.DataVisualization.Charting.Chart chartData;
        private Button buttonBack;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Button buttonOptimizedChart;
        private ComboBox comboBoxFiles;
        private CheckedListBox checkedListPoints;
        private Button buttonSaveChart;
        private NumericUpDown numericMaxMeasurements;
        private Splitter splitter1;
        private CheckedListBox checkedListBoxFiles;
    }
}