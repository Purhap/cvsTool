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

namespace cvsTool.View
{
    public partial class Chart : Form
    {
        public PersonForm parent;
        private PersonControllor controllor;
        private DataTable dt;
        private DataTable drawDt;

        private Bitmap bidBp;
        private Bitmap askBp;
        private Graphics g;
        public Chart(PersonForm p, PersonControllor _controllor)
        {
            parent = p;
            controllor = _controllor;
            dt = new DataTable();
            drawDt = new DataTable();
            InitializeComponent();
       //     bidBp = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
       //    askBp = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);

            loadDataTable();

            DateTime from = new DateTime(2017, 6, 11);
            DateTime to = new DateTime(2017, 6, 21);
            drawCandleSticks(drawDt, from, to);

            string title = controllor.Model._name;
            fillDataToChart();
       //     drawCoordinate(bidBp, title, "M1");

       //  this.chart1.DataSource = drawDt;
       //  this.chart1.DataBind();
       // Series bidSeries = new Series(title + " M1" + " BID");

            


            // setChart();
        }
        //public void drawCoordinate(Bitmap bp, string title, string timeFrameType)
        //{
        //    g = Graphics.FromImage(bp);
        //    g.Clear(Color.BurlyWood);
        //    g.DrawString(title, new Font("Arial Black", 12), Brushes.White,120,0);
        //    //
        //    g.DrawLine(new Pen(Color.White, 3), 10, 10, 10, this.pictureBox1.Height-10);// Y
        //    g.DrawString("Price", new Font("Arial", 8), Brushes.White,15,10);

        //    g.DrawLine(new Pen(Color.White, 3), 10, this.pictureBox1.Height - 10, this.pictureBox1.Width - 10, this.pictureBox1.Height - 10);// X
        //    g.DrawString("Time", new Font("Arial", 8), Brushes.White, this.pictureBox1.Width - 30, this.pictureBox1.Height - 25);
        //    this.Invalidate();

        //}

        private void loadDataTable()
        {
            if(controllor.Model.csv.dataTable.Rows.Count>0)
            {
                this.dt = controllor.Model.csv.dataTable.Clone();
            }
            else
            {
                controllor.Model.csv.ReadToDataTable(this.dt, "EUR2USD.csv");
                
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
             //   this.chart1.Series[0] = bidBp;
           }
           else
           {
             //   this.pictureBox1.Image = askBp;
           }
        }
        private void fillDataToChart()
        {
            this.chart1.Series[0].ChartType = SeriesChartType.Candlestick;

            //drawDt = dt;
            string[] XPointMember = new string[this.drawDt.Rows.Count];
            double[] YPointMember0 = new double[drawDt.Rows.Count];
            double[] YPointMember1 = new double[drawDt.Rows.Count];
            double[] YPointMember2 = new double[drawDt.Rows.Count];
            double[] YPointMember3 = new double[drawDt.Rows.Count];
            for (int count = 0; count < drawDt.Rows.Count; count++)
            {
                //storing Values for X axis  
                XPointMember[count] = drawDt.Rows[count]["DateTime"].ToString().Substring(10,8);
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
            chart1.ChartAreas[0].CursorY.IsUserEnabled = true;//打开Y轴滚动条
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;//打开X轴滚动条 区域
            chart1.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;//打开Y轴滚动条 区域
            chart1.ChartAreas[0].CursorX.AutoScroll = true;
            chart1.ChartAreas[0].CursorY.AutoScroll = true;
            // chart1.ChartAreas[0].AxisX.Interval = 5;
            


        }

        private void drawCandleSticks(DataTable argDataTable, DateTime from, DateTime to)
        {
            DataTable drawTable = new DataTable();
            for(int i = 0; i< this.dt.Columns.Count; i++)
            {
               drawTable.Columns.Add(this.dt.Columns[i].ColumnName);                             
            }

            DataRow[] matches = this.dt.Select("DateTime > '6/1/2017 9:00:00 AM'and DateTime < '6/28/2017 9:30:00 AM'");
           // DataRow[] matches = this.dt.Select("DateTime < '6/28/2017 9:10:00 AM'");
            foreach ( DataRow r in matches)
            {
                drawTable.ImportRow(r);
            }

            this.drawDt = drawTable;
        }

        private void plotOneCandle(Point position, int open, int close, int high, int low, int width)
        {
            if (open >= close)
            {
                Pen candlePen = new Pen(Brushes.Green);
             }
          //  g.DrawRectangle(Pens.Green,new Rectangle(position.X,open,)
          //  g.DrawString("Price", new Font("Arial", 8), Brushes.White, 15, 10);
        }

    }
}
