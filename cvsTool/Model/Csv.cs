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
        public string name;
        public DataTable dataTable;
        // public DateTime StartTime;
        //public DateTime EndTime;
        
        string[] columnsNames = new string[]{"DateTime","BidOpen","BidHigh","BidLow","BidClose","AskOpen","AskHigh","AskLow","AskClose","Volume"};
        public Csv(string argName)
        {
            name = argName;
            dataTable = new DataTable();
            foreach(string s in columnsNames)
            {
                dataTable.Columns.Add(s, System.Type.GetType("System.String"));
            }

        }

        public void AppendToDataTable(DataTable dt)
        {

        }
        public void ReadToDataTable(DataTable dt, string fileName)
        {
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);

           // StreamReader sr = new StreamReader(fs, Encoding.Unicode);
            StreamReader sr = new StreamReader(fs, Encoding.UTF8);
            //string fileContent = sr.ReadToEnd();
            //encoding = sr.CurrentEncoding;
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                if (IsFirst == true)
                {
                    tableHead = strLine.Split(',');
                    IsFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        if (i == 0)
                        {
                            DataColumn dc = new DataColumn(tableHead[i],typeof(DateTime));
                            dt.Columns.Add(dc);
                        }
                        else
                        {
                            DataColumn dc = new DataColumn(tableHead[i],typeof(double));
                            dt.Columns.Add(dc);
                        }
                  
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }
         //   if (aryLine != null && aryLine.Length > 0)
         //   {
         //       dt.DefaultView.Sort = tableHead[0] + " " + "asc";
         //   }

            sr.Close();
            fs.Close();
          //  return dt;
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
