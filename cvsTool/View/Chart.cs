using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using cvsTool.Controllor;

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
            InitializeComponent();
            bidBp = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);
            askBp = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height);

            loadDataTable();
            string title = controllor.Model._name;
            drawCoordinate(bidBp, title, "M1");
            DateTime from = new DateTime(2017, 6, 11);
            DateTime to = new DateTime(2017, 6, 21);
            drawCandleSticks(dt, from, to);
            setChart();
        }
        public void drawCoordinate(Bitmap bp, string title, string timeFrameType)
        {
            g = Graphics.FromImage(bp);
            g.Clear(Color.BurlyWood);
            g.DrawString(title, new Font("Arial Black", 12), Brushes.White,120,0);
            //
            g.DrawLine(new Pen(Color.White, 3), 10, 10, 10, this.pictureBox1.Height-10);// Y
            g.DrawString("Price", new Font("Arial", 8), Brushes.White,15,10);

            g.DrawLine(new Pen(Color.White, 3), 10, this.pictureBox1.Height - 10, this.pictureBox1.Width - 10, this.pictureBox1.Height - 10);// X
            g.DrawString("Time", new Font("Arial", 8), Brushes.White, this.pictureBox1.Width - 30, this.pictureBox1.Height - 25);
            this.Invalidate();

        }

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
            setChart();
        }
        private void setChart()
        {
            if (this.radioButton1.Checked)
            {
                this.pictureBox1.Image = bidBp;
            }
            else
            {
                this.pictureBox1.Image = askBp;
            }
        }

        private void drawCandleSticks(DataTable dt, DateTime from, DateTime to)
        {
            DataTable drawTable = new DataTable();
            for(int i = 0; i< dt.Columns.Count; i++)
            {
               drawTable.Columns.Add(dt.Columns[i].ColumnName);                             
            }

          //  string = "DateTime >" + from.ToString;
           // DataRow[] matches = dt.Select("DateTime>= 'from' and DateTime <= 'to'");
          //  DataRow[] matches = dt.Select("DateTime between '" + from.ToString("MM/dd/yyyy/HH:mm:ss") +"'and'"+ to.ToString("MM/dd/yyyy/HH:mm:ss")+"'");
            DataRow[] matches = dt.Select("DateTime > '6/11/2017 12:00:00 AM'");
            foreach( DataRow r in matches)
            {
                drawTable.ImportRow(r);
            }
            // foreach()
            // plotOneCandle()
            //int oneWidth = bidBp.Width /

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
