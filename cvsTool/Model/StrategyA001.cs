using cvsTool.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cvsTool.Model.BaseLibrary;

// 策略：
//      T 日 成交额前50名的股票
//      T+1 日  如果依然高开，按开盘价买入
//      T+2 日  1按最高价卖出 2 按开盘价卖出  3 按收盘价卖出
//          testParam.K1  持仓时间 1天
//          testParam.K2  高开幅度 -0.1~ 0.1, 代表-10%--10%
//          testParam.K3    



namespace cvsTool.Model
{
   public class StrategyA001
    {
        public DataTable TradeDt;
        public DataTable StockDt;

        private string name;
        public UInt64 tradeTimes;

        public DateTime currentTime;
        private double currentPrice;
        public double profit;

        private TestParam testParam;
        private UInt64 badTradeTimes;
        private UInt64 goodTradeTimes;

        public delegate void UpdateCurrentStatusDelegate(string value, UInt16 index);
        public UpdateCurrentStatusDelegate updateCurrentStatusDelegate;

        public delegate void UpdateTradeLogParallelDelegate(string value);
        public UpdateTradeLogParallelDelegate updateTradeLogParallelDelegate;
        public StrategyA001(string argName, TestParam argTestParam, ref PersonForm view, ref UInt32 currentThreadNum)
        {
            currentThreadNum++;
            name = argName;
            testParam = argTestParam;
           
            tradeTimes = 0;
            badTradeTimes = 0;
            goodTradeTimes = 0;
            profit = 0;
            
            TradeDt = new DataTable();
            StockDt = new DataTable();
            initializeTradeDataTable();
            initializeStocksDataTable();
            updateCurrentStatusDelegate = new UpdateCurrentStatusDelegate(view.updateCurrentStatus);
            updateTradeLogParallelDelegate = new UpdateTradeLogParallelDelegate(view.updateTradeLogParallelTextBox);

        }

        private void initializeStocksDataTable()
        {
            string[] columnsNames = new string[] { "Name", "DateTime", "Quantity", "Volume" };
            int columnsNumber = 4;
            for (int i = 0; i < columnsNumber; i++)
            {
                if (i == 0)
                {
                    DataColumn dc = new DataColumn(columnsNames[i], typeof(string));
                    StockDt.Columns.Add(dc);

                }
                else if (i == 1)
                {
                    DataColumn dc = new DataColumn(columnsNames[i], typeof(DateTime));
                    StockDt.Columns.Add(dc);

                }
                else
                {
                    DataColumn dc = new DataColumn(columnsNames[i], typeof(double));
                    StockDt.Columns.Add(dc);
                }
            }
        }
        private void initializeTradeDataTable()
        {
            string[] columnsNames = new string[] { "Stock","Ranking","OpenTime","DayofWeek", "OpenPrice", "L/S", "Quantity", "CloseTime", "ClosePrice", "Profit" };

            for (int i = 0; i < 10; i++)
            {
                if ((i == 2) || (i == 7))
                {
                    DataColumn dc = new DataColumn(columnsNames[i], typeof(DateTime));
                    TradeDt.Columns.Add(dc);
                }
                else if(1 == i) 
                {
                    DataColumn dc = new DataColumn(columnsNames[i], typeof(int));
                    TradeDt.Columns.Add(dc);
                }
                else if((0==i)|| (i == 5)||( i == 3))
                {
                    DataColumn dc = new DataColumn(columnsNames[i], typeof(String));
                    TradeDt.Columns.Add(dc);
                }
                else
                {
                    DataColumn dc = new DataColumn(columnsNames[i], typeof(double));
                    TradeDt.Columns.Add(dc);
                }
            }
        }
        public bool getTop50VolumeStocks(DateTime datetime, ref DataTable outputTable)
        {
            List<DataTable> retList = new List<DataTable>(50);
            DataTable ret = new DataTable();
            ret = StockDt.Clone();
                        
            for (int TabIndex = 0; TabIndex < SimulationHouse.AllPricelistTable.Count; TabIndex++)
            {   
                //get stocks data by specified day
                var q = from myrow in SimulationHouse.AllPricelistTable[TabIndex].AsEnumerable()
                        where myrow.Field<DateTime>("DateTime") == datetime
                        select myrow;
                if (q.Count() > 0)
                {
                    DataTable tmp = new DataTable();
                    tmp = q.CopyToDataTable();
                    DataRow dr = ret.NewRow();
                    for (int j = 0; j < ret.Columns.Count; j++)
                    {
                        if (j == 0)
                        {
                            dr[j] = SimulationHouse.AllPricelistTable[TabIndex].TableName;
                        }
                        else if (j == 1)
                        {

                            dr[j] = Convert.ToDateTime(tmp.Rows[0]["DateTime"]);
                        }
                        else if (j == 2)
                        {

                            dr[j] = Convert.ToDouble(tmp.Rows[0]["Quantity"]);
                        }
                        else if (j == 3)
                        {

                            dr[j] = Convert.ToDouble(tmp.Rows[0]["Volume"]);
                        }
                    }
                    ret.Rows.Add(dr);
                }                
            }

            var query = from x in ret.AsEnumerable()
                        orderby x.Field<double>("Volume") descending
                        select x;
            if (query.Count() > 0)
            {
                outputTable = query.CopyToDataTable();
            }
            else
            {
                return false;
            }
            return true;
        }

              

        public void closePosition()
        {

        }

        public void monitorPositon()
        {

        }
        public int getPrice(string name, DateTime dateTime, ref Price price)
        {
            int ret = 0;
            //find name matched dataTable
            var q = from x in SimulationHouse.AllPricelistTable.AsEnumerable()
                    where x.TableName == name
                    select x;
            if (q.Count() == 0)
            {
                ret = 1;  // stock is invalid
                return ret;
            }
            int index = SimulationHouse.AllPricelistTable.IndexOf((q.ToList())[0]);

            DataTable dt = SimulationHouse.AllPricelistTable[index];


            // find datetime matched row
            var q2 = from myrow in dt.AsEnumerable()
                        where myrow.Field<DateTime>("DateTime") == dateTime
                        select myrow;
            if(q2.Count()==0)
            {
                ret = 2;  //datetime is not trade day
                return ret;
            }
            DataTable final = q2.CopyToDataTable();

            //extract 4 kind of price
            price.Open = Convert.ToDouble(final.Rows[0]["Open"]);
            price.High = Convert.ToDouble(final.Rows[0]["High"]);
            price.Low = Convert.ToDouble(final.Rows[0]["Low"]);
            price.Close = Convert.ToDouble(final.Rows[0]["Close"]);

            return ret;
            
        }
        public void runSimulation()
        {
            int badTime = 0;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            DateTime dt = Convert.ToDateTime("2017-4-1");// DateTime.ParseExact("2017/04/01", "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            for (int i = 0; i < 400; i++)
            {                
                dt = dt.AddDays(1);
                if (dt > DateTime.Now)
                    break;

                bool ret = getTop50VolumeStocks(dt, ref StockDt);
                if (ret == false)
                {                    
                    continue;
                }
                    
                for (int j = 0; j < 50; j++)
                {
                    ret = doOneTrade(StockDt.Rows[j]["Name"].ToString(), dt, 100000.0, j+1);
                    if(ret == false)
                    {
                        badTime++;
                    }
                }
                string ss = String.Format("Thread {0} : {1:yyyy-MM-dd HH:mm:ss}", name, dt);
                updateCurrentStatusDelegate(ss, Convert.ToUInt16(name));
               // Console.WriteLine("Day:{0}---{1}",i, dt.ToShortDateString());
            }
            sw.Stop();
            string filename = string.Format("S001-{0:yyyyMMddHHmm}T{1}.csv", DateTime.Now, name);
            profit = Convert.ToDouble(TradeDt.Compute("Sum(Profit)", "true"));
            tradeTimes = Convert.ToUInt64( TradeDt.Rows.Count);

            var q = from r in TradeDt.AsEnumerable()
                    where r.Field<double>("Profit") > 0
                    select r;
            goodTradeTimes = Convert.ToUInt64(q.Count());

            var q1 = from r in TradeDt.AsEnumerable()
                    where r.Field<double>("Profit") < 0
                    select r;
            badTradeTimes = Convert.ToUInt64(q1.Count());

            string ss1 = String.Format("{0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}\r\n", tradeTimes, profit, goodTradeTimes, badTradeTimes, testParam.k1, testParam.k2, testParam.k3, name, Convert.ToUInt32(sw.ElapsedMilliseconds / 1000), filename);
            updateTradeLogParallelDelegate(ss1);
            Csv.SaveCSV(TradeDt, filename, ss1);
         //   Console.WriteLine("Bad time is {0}.", badTime);
         //   Console.WriteLine("Cost tim is {0}s.", sw.ElapsedMilliseconds/1000);
         //   Console.WriteLine("Total Profit is {0}.", Convert.ToDouble( TradeDt.Compute("Sum(Profit)", "true")));
        }
        public void openPosition(string name,DateTime dt, double yuan)
        {

        }
        public bool doOneTrade(string name, DateTime dt, double yuan, int Ranking)
        {
            Price Ref = new Price(0.0, 0.0, 0.0, 0.0);
            Price Open = new Price(0.0,0.0,0.0,0.0);
            Price Close = new Price(0.0, 0.0, 0.0, 0.0);
            DateTime T0 = dt;
            DateTime T1 = T0.AddDays(1);
            DateTime T2;
            
            int code = getPrice(name, T0, ref Ref);
            if(code !=0)
            {
                return false;
            }
            code = getPrice(name, T1,ref Open);

            if (code == 1)  //stock is invalid.
            { return false; }
            else if (code == 2) // trade time is not illegal. Should find next day
            {
                int waitday = 0;
                while ((code == 2) && (waitday < 10))
                {
                    waitday++;
                    T1 = T1.AddDays(1);
                    code = getPrice(name, T1, ref Open);
                }
                if (waitday >= 10)
                { return false; }  //hart to trade. give up this operation
            }
            else  // best T1 time
            {
            }

            if( Open.Open<Ref.Close*(1+ testParam.k2)) // open position condition check
            {  //高开幅度
                return false;
            }
            //calculate stock quantity to trade
            double tmp = Math.Floor(yuan /(100.0 * Open.Open));
            int quantity = Convert.ToInt32(Math.Floor(yuan / (100.0 * Open.Open))) *100;

            //Get T2 Price
            T2 = T1.AddDays(1);
            code = getPrice(name, T2, ref Close);
            if (code == 1)  //stock is invalid.
                return false;
            else if (code == 2) // trade time is not illegal. Should find next day
            {
                int waitday = 0;
                while ((code == 2) && (waitday < 20))
                {
                    waitday++;
                    T2 = T2.AddDays(1);
                    code = getPrice(name, T2, ref Close);
                }
                if (waitday >= 20)
                    return false;  //hart to trade. give up this operation
            }
            else  // best T2 time
            {
            }

            //Calculate Profits 
            double profit = 0.0;
            double buyPrice = Open.Open;
         //   double sellPrice = Close.High;
            double sellPrice = Close.Open;
            sellPrice *= (1 - 0.003);
            profit = quantity * (sellPrice - buyPrice);

            profit -= quantity * sellPrice * 0.001; //印花税
            profit -= quantity * sellPrice * 0.00025; //卖出 佣金
            profit -= quantity * buyPrice * 0.00025; //买入 佣金
            profit -= quantity * sellPrice * 0.00002; //卖出 过户费
            profit -= quantity * buyPrice * 0.00002; //买入 过户费

            //save trade data to DateTable
            DataRow dr = TradeDt.NewRow();

            TradeDt.Rows.Add(name, Ranking, T1,T1.DayOfWeek.ToString() ,buyPrice, "L", quantity, T2, sellPrice, profit);
            return true;
        }
        public void OneDayTask(int i)
        {

        }
    }

}
