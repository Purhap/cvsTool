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
using System.Diagnostics;

namespace cvsTool.View
{
    public partial class Chart : Form
    {
        public PersonForm parent;
        private PersonControllor controllor;
        private DataTable historyDataTable;
        private DataTable drawDt;

        private System.Windows.Forms.Timer chartTimer;
        public Chart(PersonForm p, PersonControllor _controllor)
        {
            parent = p;
            controllor = _controllor;
            historyDataTable = new DataTable();
            drawDt = new DataTable();
            InitializeComponent();            
            this.chart1.Location = new Point(10, 40);
            this.chart1.Size = new Size(1100, 600);
            onInit();
          
        }
        private void onInit()
        {
            var cols = new[] { historyDataTable.Columns["DateTime"] };
            historyDataTable.PrimaryKey = cols;


            Stopwatch sw = new Stopwatch();
            sw.Start();
            loadDataTable();
            sw.Stop();
            long t1 = sw.ElapsedMilliseconds;
            DateTime from = new DateTime(2017, 1, 1);
            DateTime to = new DateTime(2017, 1, 10);
            sw.Restart();
            drawCandleSticks(drawDt, from, to);
            sw.Stop();
            long t2 = sw.ElapsedMilliseconds;

        //    MessageBox.Show(string.Format("t1{0}, t2:{1}.", t1, t2));

            string title = controllor.Model._name;
            fillDataToChart();
            chartTimer = new System.Windows.Forms.Timer();
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
            //    e.Text = string.Format("Time:{0};数值:{1:0.000} {2:0.000}{3:0.000}{4:0.000}", dp.XValue, dp.YValues[0], dp.YValues[1], dp.YValues[2], dp.YValues[3]);

             //   chart1.Series[0].ToolTip = "#VALX:#VAL";
            }
        }

        private void Chart1_DoubleClick(object sender, EventArgs e)
        {
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 100;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
            chart1.ChartAreas[0].AxisY.ScrollBar.Enabled = true;
        }

        private void chartTimer_Tick(object sender, EventArgs e)
        {
           
          //  Series series = chart1.Series[0];
          //  //series.Points.Count = 50;
          ////  chart1.ChartAreas[0].AxisX.ScaleView.Size = 80;
          //  chart1.ChartAreas[0].AxisX.ScaleView.MinSize = 10;
          //  //    chart1.ChartAreas[0].AxisX.ScaleView.Position = series.Points.Count - 5;
          //  chart1.ChartAreas[0].AxisY.IsMarginVisible = true;
              
                
        }
       

        private void loadDataTable()
        {
            if(controllor.Model.Csv.dataTable.Rows.Count>0)
            {
                this.historyDataTable = controllor.Model.Csv.dataTable;
            }
            else
            {
                Csv.ReadToDataTable(this.historyDataTable, "EUR2USD.csv");
                var query = historyDataTable.AsEnumerable().OrderBy(r => r["DateTime"]);
                historyDataTable = query.CopyToDataTable();
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
            double max = Convert.ToDouble(drawDt.Rows[0]["BidHigh"]);
            double min = Convert.ToDouble(drawDt.Rows[0]["BidLow"]);
            for (int count = 0; count < drawDt.Rows.Count; count++)
            {
                //storing Values for X axis  

                XPointMember[count] = string.Format("{0:yyMMdd HH:mm}", Convert.ToDateTime(drawDt.Rows[count]["DateTime"]));
                //storing values for Y Axis  
         
                YPointMember0[count] = Convert.ToDouble(drawDt.Rows[count]["BidHigh"]);
                YPointMember1[count] = Convert.ToDouble(drawDt.Rows[count]["BidLow"]);
                YPointMember2[count] = Convert.ToDouble(drawDt.Rows[count]["BidOpen"]);
                YPointMember3[count] = Convert.ToDouble(drawDt.Rows[count]["BidClose"]);
                max = Math.Max(max, YPointMember0[count]);
                min = Math.Min(min, YPointMember1[count]);
            }
           
            // chart1.Series[0].XValueType = ChartValueType.DateTime;
            //   chart1.Series[0].XValueType = ChartValueType.String;
            chart1.Series[0].Palette = ChartColorPalette.Bright;
            chart1.Series[0].YValuesPerPoint = 4;
            chart1.Series[0].YValueType = ChartValueType.Double;
            chart1.Series[0].XValueMember = "DateTime";
            chart1.Series[0].YValueMembers = "HighPrice, LowPrice, OpenPrice, ClosePrice";
         //   chart1.Series[0].IsXValueIndexed = true;
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
            chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;
            chart1.ChartAreas[0].AxisY.Minimum = min - 0.0010;
            chart1.ChartAreas[0].AxisY.Maximum = max + 0.0010;
            chart1.ChartAreas[0].AxisY.Interval = (max -min)/20;
            //chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "5", "Minute", "MA");
            chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "5", this.chart1.Series[0], this.chart1.Series["M5"]);
            chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "10", this.chart1.Series[0], this.chart1.Series["M10"]);
            chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "20", this.chart1.Series[0], this.chart1.Series["M20"]);
            chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "60", this.chart1.Series[0], this.chart1.Series["M60"]);
          
            chart1.ChartAreas[0].AxisX.LabelStyle.IsStaggered = true;
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");
            chart1.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dot;
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 100;
            //       chart1.Series[0].Label = "#VAL";
            //       chart1.Series[0].ToolTip = "#VALX:#VAL";

            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;//打开X轴滚动条
    //        chart1.ChartAreas[0].CursorY.IsUserEnabled = true;//打开Y轴滚动条
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;//打开X轴滚动条 区域
       //     chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;//打开Y轴滚动条 区域
            chart1.ChartAreas[0].CursorX.AutoScroll = true;
      //      chart1.ChartAreas[0].CursorY.AutoScroll = true;
            chart1.ChartAreas[0].AxisX.Interval = 2;
            chart1.ChartAreas[0].AxisX.LineWidth = 3;// x 轴粗细
            chart1.ChartAreas[0].AxisY.LineWidth = 3;// Y 轴粗细
            chart1.ChartAreas[0].AxisY.LineColor = ColorTranslator.FromHtml("#38587a");
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = ColorTranslator.FromHtml("#2c4c6d");
            chart1.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.DashDotDot;

            
            chart1.ChartAreas[0].AxisY.TextOrientation = TextOrientation.Rotated270;      //TextOrientation.Auto;
            chart1.ChartAreas[0].Position.X = 0;
            chart1.ChartAreas[0].Position.X = 0;
            chart1.ChartAreas[0].Position.Width = 100;
            chart1.ChartAreas[0].Position.Height = 100;
            chart1.ChartAreas[0].BackColor = Color.Transparent;
            chart1.ChartAreas[0].BorderColor = Color.Transparent;
            chart1.ChartAreas[0].BackGradientStyle = GradientStyle.TopBottom;
            chart1.BackColor = Color.Transparent;
            
        }

        private void drawCandleSticks(DataTable argDataTable, DateTime startTime, DateTime endTime)
        {
          var q = from myrow in historyDataTable.AsEnumerable()
                         where myrow.Field<DateTime>("DateTime") > startTime && myrow.Field<DateTime>("DateTime") < endTime
                    select myrow;
            drawDt = historyDataTable.Clone();
           
            drawDt = q.CopyToDataTable();
        }
        
        private void LoadChart_Click(object sender, EventArgs e)
        {
            fillDataToChart(dateTimePicker1.Value, dateTimePicker2.Value);
        }

        private void fillDataToChart_old(DateTime startTime, DateTime stopTime)
        {
            this.chart1.Series[0].ChartType = SeriesChartType.Candlestick;

            DataTable drawTable = historyDataTable.Clone();

            string ss = "DateTime >'" + startTime.ToString()+"' and DateTime < '"+ stopTime.ToString()+ "'";
            DataRow[] matches = this.historyDataTable.Select(ss);

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
        private void fillDataToChart(DateTime startTime, DateTime endTime)
        {
            this.chart1.Series[0].ChartType = SeriesChartType.Candlestick;

            DataTable drawTable = historyDataTable.Clone();

            var q = from myrow in historyDataTable.AsEnumerable()
                    where myrow.Field<DateTime>("DateTime") > startTime && myrow.Field<DateTime>("DateTime") < endTime
                    select myrow;
            this.drawDt = q.CopyToDataTable();

            string[] XPointMember = new string[this.drawDt.Rows.Count];
            double[] YPointMember0 = new double[drawDt.Rows.Count];
            double[] YPointMember1 = new double[drawDt.Rows.Count];
            double[] YPointMember2 = new double[drawDt.Rows.Count];
            double[] YPointMember3 = new double[drawDt.Rows.Count];
            double max = Convert.ToDouble(drawDt.Rows[0]["BidHigh"]);
            double min = Convert.ToDouble(drawDt.Rows[0]["BidLow"]);
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
                max = Math.Max(max, YPointMember0[count]);
                min = Math.Min(min, YPointMember1[count]);
            }

            //chart1.Series[0].XValueType = ChartValueType.DateTime;
            //chart1.Series[0].YValuesPerPoint = 4;
            //chart1.Series[0].YValueType = ChartValueType.Double;
            //chart1.Series[0].XValueMember = "DateStamp";
            //chart1.Series[0].YValueMembers = "HighPrice, LowPrice, OpenPrice, ClosePrice";
            //chart1.Series[0].IsXValueIndexed = true;
            //chart1.Series[0].BorderColor = System.Drawing.Color.Black;
            //chart1.Series[0].Color = System.Drawing.Color.Black;
            //chart1.Series[0].CustomProperties = "PriceDownColor=Green, PriceUpColor=Red";
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
            chart1.ChartAreas[0].AxisY.Minimum = min - 0.0010;
            chart1.ChartAreas[0].AxisY.Maximum = max + 0.0010;
            chart1.ChartAreas[0].AxisY.Interval = (max - min) / 20;
            //chart1.DataManipulator.FinancialFormula(FinancialFormula.MovingAverage, "15", "Daily", "MA");
            chart1.ChartAreas[0].AxisY.IsStartedFromZero = false;
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 150;

         //   chart1.ChartAreas[0].CursorX.IsUserEnabled = true;//打开X轴滚动条
                                                              //  chart1.ChartAreas[0].CursorY.IsUserEnabled = true;//打开Y轴滚动条
        //    chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;//打开X轴滚动条 区域
                                                                       // chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;//打开Y轴滚动条 区域
                                                                       //  chart1.ChartAreas[0].CursorX.AutoScroll = true;
                                                                       //  chart1.ChartAreas[0].CursorY.AutoScroll = true;
                                                                       // chart1.ChartAreas[0].AxisX.Interval = 5;
            

                

        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            this.dateTimePicker2.Value = this.dateTimePicker1.Value;
        }

        private void Chart_FormClosing(object sender, FormClosingEventArgs e)
        {
            chartTimer.Dispose();
        }
    }
}
