using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.IO;

namespace cvsTool.Model
{
    public class Csv
    {
        public DataTable dataTable;
        string[] columnsNames = new string[]{"DateTime","BidOpen","BidHigh","BidLow","BidClose","AskOpen","AskHigh","AskLow","AskClose","Volume"};
        public Csv()
        {
            dataTable = new DataTable();
            foreach(string s in columnsNames)
            {
                dataTable.Columns.Add(s, System.Type.GetType("System.String"));
            }

        }

        public void AppendToDataTable(DataTable dt)
        {

        }
        public static Boolean SaveCSV(DataTable dt, string fullFileName)
        {           
            Boolean r = false;
            FileStream fs = new FileStream(fullFileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            string data = "";

            //写出列名称
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                data += dt.Columns[i].ColumnName.ToString();
                if (i < dt.Columns.Count - 1)
                {
                    data += ",";
                }
            }
            sw.WriteLine(data);

            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    data += dt.Rows[i][j].ToString();
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }

            sw.Close();
            fs.Close();

            r = true;
            return r;
        }

    }
}
