using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cvsTool.Controllor;
using System.Windows.Forms.DataVisualization.Charting;
using cvsTool.Model;

namespace cvsTool.View
{
    public partial class Chart : Form
    {
        public PersonForm parent;
        private PersonControllor controllor;
        private DataTable dt;
        private DataTable drawDt;
        DateTime dt1;
        DateTime dt2;
        
        System.Windows.Forms.Timer chartTimer = new System.Windows.Forms.Timer();
        public Chart(PersonForm p, PersonControllor _controllor)
        {
            parent = p;
            controllor = _controllor;
            dt = new DataTable();
            drawDt = new DataTable();
            InitializeComponent();            
            this.chart1.Location = new Point(40, 40);
            this.chart1.Size = new Size(1000, 600);
            var cols = new[] { dt.Columns["DateTime"] };
            dt.PrimaryKey = cols;
            

            dt1 = DateTime.Now;
            loadDataTable();
            dt2 = DateTime.Now;

            DateTime from = new DateTime(2017, 1, 1);
            DateTime to = new DateTime(2017, 1, 2);
            drawCandleSticks(drawDt, from, to);

            string title = controllor.Model._name;
            fillDataToChart();
            chartTimer.Interval = 1000;
            chartTimer.Tick += chartTimer_Tick;
            chart1.DoubleClick += Chart1_DoubleClick;
            chart1.GetToolTipText += new EventHandler<ToolTipEventArgs>(chart1_GetToolTipText);
            chartTimer.Start();
        }

        private void chart1_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
               
                //分别显示x轴和y轴的数值，其中{1:F3},表示显示的是float类型，精确到小数点后3位。  
                e.Text = string.Format("Time:{0};数值:{1:F3} ", dp.XValue, dp.YValues[0]);
            }
        }

        private void Chart1_DoubleClick(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 30;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart1.ChartAreas[0].AxisY.ScrollBar.Enabled = true;
        }

        private void chartTimer_Tick(object sender, EventArgs e)
        {
           
            Series series = chart1.Series[0];
            //series.Points.Count = 50;
          //  chart1.ChartAreas[0].AxisX.ScaleView.Size = 80;
            chart1.ChartAreas[0].AxisX.ScaleView.MinSize = 10;
            //    chart1.ChartAreas[0].AxisX.ScaleView.Position = series.Points.Count - 5;
            chart1.ChartAreas[0].AxisY.IsMarginVisible = true;
              
                
        }
       

        private void loadDataTable()
        {
            if(controllor.Model.Csv.dataTable.Rows.Count>0)
            {
                this.dt = controllor.Model.Csv.dataTable;
            }
            else
            {
                Csv.ReadToDataTable(this.dt, "EUR2USD.csv");
                
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            //setChart();
        }
        private void setChart()
        {
            if (this.radioButton1.Checked)
            {             
           }
           else
           {          
           }
        }
        private void fillDataToChart()
        {
            
            this.chart1.Series[0].ChartType = SeriesChartType.Candlestick;
            this.chart1.Series.Add(new Series("M5"));
            this.chart1.Series[1].ChartType= SeriesChartType.Spline;
            this.chart1.Series.Add(new Series("M10"));
            this.chart1.Series[2].ChartType = SeriesChartType.Spline;
            this.chart1.Series.Add(new Series("M20"));
            this.chart1.Series[3].ChartType = SeriesChartType.Spline;
            this.chart1.Series.Add(new Series("M60"));
            this.chart1.Series[4].ChartType = SeriesChartType.Spline;
            //drawDt = dt;
            string[] XPointMember = new string[this.drawDt.Rows.Count];
            double[] YPointMember0 = new double[drawDt.Rows.Count];
            double[] YPointMember1 = new double[drawDt.Rows.Count];
            double[] YPointMember2 = new double[drawDt.Rows.Count];
            double[] YPointMember3 = new double[drawDt.Rows.Count];
            for (int count = 0; count < drawDt.Rows.Count; count++)
            {
                //storing Values for X axis  

                XPointMember[count] = string.Format("{0:yyyyMMdd HH:mm}", Convert.ToDateTime(drawDt.Rows[count]["DateTime"]));
                //storing values for Y Axis  
                //YPointMember0[count] = Convert.ToDouble(drawDt.Rows[count]["BidOpen"]);
                //YPointMember1[count] = Convert.ToDouble(drawDt.Rows[count]["BidHigh"]);
                //YPointMember2[count] = Convert.ToDouble(drawDt.Rows[count]["BidLow"]);
                //YPointMember3[count] = Convert.ToDouble(drawDt.Rows[count]["BidClose"]);

                YPointMember0[count] = Convert.ToDouble(drawDt.Rows[count]["BidHigh"]);
                YPointMember1[count] = Convert.ToDouble(drawDt.Rows[count]["BidLow"]);
                YPointMember2[count] = Convert.ToDouble(drawDt.Rows[count]["BidOpen"]);
                YPointMember3[count] = Convert.ToDouble(drawDt.Rows[count]["BidClose"]);


            }
           // chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.Series[0].XValueType = ChartValueType.String;
            chart1.Series[0].YValuesPerPoint = 4;
            chart1.Series[0].YValueType = ChartValueType.Double;
            chart1.Series[0].XValueMember = "DateStamp";
            chart1.Series[0].YValueMembers = "HighPrice, LowPrice, OpenPrice, ClosePrice";
            chart1.Series[0].IsXValueIndexed = true;
         //   chart1.Series[0].IsValueShownAsLabel = true;
            chart1.Series[0].BorderColor = System.Drawing.Color.Black;
            chart1.Series[0].Color = System.Drawing.Color.Black;
            chart1.Series[0].CustomProperties = "PriceDownColor=Green, PriceUpColor=Red, MaxPixelPointWidth=15";
            chart1.Series[0].Points.DataBindXY(XPointMember, YPointMember0, YPointMember1, YPointMember2, YPointMember3);
         //   chart1.Series[0].LegendToolTip = "Target Output";
         //   chart1.Series[0].LegendText = "Target Output";
            // chart1.Series[0].BorderWidth = 2;

            // chart1.Series[0].BorderColor = System.Drawing.Color.Black;
            //chart1.Series[0].Color = System.Drawing.Color.Black;

            // chart1.Series[0]["PriceUpColor"] = "Red";
            // chart1.Series[0]["PointWidth"] = "0.10";
            // chart1.Series[0]["PriceDownColor"] = "Green";
            // chart1.Series[0]["OpenCloseStyle"] = "Triangle";
            //chart1.ChartAreas[0].AxisY.Minimum = 100;
            //chart1.ChartAreas[0].AxisY.Maximum = 180;
            //chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "5", "Minute", "MA");
            chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "5", this.chart1.Series[0], this.chart1.Series["M5"]);
            chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "10", this.chart1.Series[0], this.chart1.Series["M10"]);
            chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "20", this.chart1.Series[0], this.chart1.Series["M20"]);
            chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "60", this.chart1.Series[0], this.chart1.Series["M60"]);
            chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;
       //     chart1.ChartAreas[0].AxisX.TextOrientation = TextOrientation.Stacked;
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;//打开X轴滚动条
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;//打开Y轴滚动条
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;//打开X轴滚动条 区域
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;//打开Y轴滚动条 区域
            chart1.ChartAreas[0].CursorX.AutoScroll = true;
            chart1.ChartAreas[0].CursorY.AutoScroll = true;
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.LineWidth = 20;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 100;




        }

        private void drawCandleSticks(DataTable argDataTable, DateTime from, DateTime to)
        {
            DataTable drawTable = new DataTable();
            for (int i = 0; i < this.dt.Columns.Count; i++)
            {
                drawTable.Columns.Add(this.dt.Columns[i].ColumnName);
            }

            
        
            DataRow[] matches = this.dt.Select("DateTime > '6/1/2017 9:00:00 AM'and DateTime < '6/3/2017 9:30:00 AM'");
            foreach ( DataRow r in matches)
            {
                drawTable.ImportRow(r);
            }

            this.drawDt = drawTable;
        }

       

        private void LoadChart_Click(object sender, EventArgs e)
        {
            fillDataToChart(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void fillDataToChart(DateTime startTime, DateTime stopTime)
        {
            this.chart1.Series[0].ChartType = SeriesChartType.Candlestick;

            DataTable drawTable = dt.Clone();

            string ss = "DateTime >'" + startTime.ToString()+"' and DateTime < '"+ stopTime.ToString()+ "'";
            DataRow[] matches = this.dt.Select(ss);

            foreach (DataRow r in matches)
            {
                drawTable.ImportRow(r);
            }

            this.drawDt = drawTable;

            string[] XPointMember = new string[this.drawDt.Rows.Count];
            double[] YPointMember0 = new double[drawDt.Rows.Count];
            double[] YPointMember1 = new double[drawDt.Rows.Count];
            double[] YPointMember2 = new double[drawDt.Rows.Count];
            double[] YPointMember3 = new double[drawDt.Rows.Count];
            for (int count = 0; count < drawDt.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = string.Format("{0:yyyyMMdd HH:mm}", Convert.ToDateTime(drawDt.Rows[count]["DateTime"]));
                //storing values for Y Axis  
                //YPointMember0[count] = Convert.ToDouble(drawDt.Rows[count]["BidOpen"]);
                //YPointMember1[count] = Convert.ToDouble(drawDt.Rows[count]["BidHigh"]);
                //YPointMember2[count] = Convert.ToDouble(drawDt.Rows[count]["BidLow"]);
                //YPointMember3[count] = Convert.ToDouble(drawDt.Rows[count]["BidClose"]);

                YPointMember0[count] = Convert.ToDouble(drawDt.Rows[count]["BidHigh"]);
                YPointMember1[count] = Convert.ToDouble(drawDt.Rows[count]["BidLow"]);
                YPointMember2[count] = Convert.ToDouble(drawDt.Rows[count]["BidOpen"]);
                YPointMember3[count] = Convert.ToDouble(drawDt.Rows[count]["BidClose"]);


            }
           
            chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.Series[0].YValuesPerPoint = 4;
            chart1.Series[0].YValueType = ChartValueType.Double;
            chart1.Series[0].XValueMember = "DateStamp";
            chart1.Series[0].YValueMembers = "HighPrice, LowPrice, OpenPrice, ClosePrice";
            chart1.Series[0].IsXValueIndexed = true;
            chart1.Series[0].BorderColor = System.Drawing.Color.Black;
            chart1.Series[0].Color = System.Drawing.Color.Black;
            chart1.Series[0].CustomProperties = "PriceDownColor=Green, PriceUpColor=Red";
            chart1.Series[0].Points.DataBindXY(XPointMember, YPointMember0, YPointMember1, YPointMember2, YPointMember3);


            chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "5", this.chart1.Series[0], this.chart1.Series["M5"]);
            chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "10", this.chart1.Series[0], this.chart1.Series["M10"]);
            chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "20", this.chart1.Series[0], this.chart1.Series["M20"]);
            chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "60", this.chart1.Series[0], this.chart1.Series["M60"]);
            
            // chart1.Series[0].BorderWidth = 2;

            // chart1.Series[0].BorderColor = System.Drawing.Color.Black;
            //chart1.Series[0].Color = System.Drawing.Color.Black;

            // chart1.Series[0]["PriceUpColor"] = "Red";
            // chart1.Series[0]["PointWidth"] = "0.10";
            // chart1.Series[0]["PriceDownColor"] = "Green";
            // chart1.Series[0]["OpenCloseStyle"] = "Triangle";
            //chart1.ChartAreas[0].AxisY.Minimum = 100;
            //chart1.ChartAreas[0].AxisY.Maximum = 180;
            //chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "15", "Daily", "MA");
            chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;

            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;//打开X轴滚动条
          //  chart1.ChartAreas[0].CursorY.IsUserEnabled = true;//打开Y轴滚动条
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;//打开X轴滚动条 区域
           // chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;//打开Y轴滚动条 区域
          //  chart1.ChartAreas[0].CursorX.AutoScroll = true;
          //  chart1.ChartAreas[0].CursorY.AutoScroll = true;
            // chart1.ChartAreas[0].AxisX.Interval = 5;


        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            this.dateTimePicker2.Value = this.dateTimePicker1.Value;
        }
    }
}
