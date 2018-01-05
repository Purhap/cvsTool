using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cvsTool.Model.BaseLibrary;

// 策略：
//      T 日 成交额前50名的股票
//      T+1 日  如果依然高开，按开盘价买入
//      T+2 日  1按最高价卖出 2 按开盘价卖出  3 按收盘价卖出
//
//



namespace cvsTool.Model
{
    class StrategyA001
    {
        public DataTable TradeDt;
        public DataTable StockDt;

        public delegate void UpdateCurrentStatusDelegate(string value);
        public UpdateCurrentStatusDelegate updateCurrentStatusDelegate;
        public StrategyA001()
        {
            TradeDt = new DataTable();
            StockDt = new DataTable();
            initializeTradeDataTable();
            initializeStocksDataTable();
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
            string[] columnsNames = new string[] { "Stock","OpenTime", "OpenPrice", "L/S", "Quantity", "CloseTime", "ClosePrice", "Profit" };

            for (int i = 0; i < 8; i++)
            {
                if ((i == 1) || (i == 5))
                {
                    DataColumn dc = new DataColumn(columnsNames[i], typeof(DateTime));
                    TradeDt.Columns.Add(dc);
                }
                else if((0==i)|| (i == 3))
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

            //  foreach (DataTable dt in SimulationHouse.AllPricelistTable)
            for (int TabIndex = 0; TabIndex < SimulationHouse.AllPricelistTable.Count; TabIndex++)
            {
                // DataTable dt in SimulationHouse.AllPricelistTable)
                {
                    DataTable tmp = new DataTable();
                    // tmp = dt;
                    var q = from myrow in SimulationHouse.AllPricelistTable[TabIndex].AsEnumerable()
                            where myrow.Field<DateTime>("DateTime") == datetime
                            select myrow;
                    if (q.Count() > 0)
                    {


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
            DateTime dt = Convert.ToDateTime("2017-8-8");// DateTime.ParseExact("2017/04/01", "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
            for (int i = 0; i < 60; i++)
            {
                bool ret = getTop50VolumeStocks(dt, ref StockDt);
                if (ret == false)
                    continue;
                for (int j = 0; j < 50; j++)
                {
                    ret = doOneTrade(StockDt.Rows[j]["Name"].ToString(), dt, 100000.0);
                    if(ret == false)
                    {
                        badTime++;
                    }
                }

                dt = dt.AddDays(1);
                Console.WriteLine("{0}{1}",i, dt.ToShortDateString());
            }
            Csv.SaveCSV(TradeDt, "T50"+ DateTime.Now.ToShortTimeString() +".csv");
            Console.WriteLine("Bad time is {0}.", badTime);
        }
        public void openPosition(string name,DateTime dt, double yuan)
        {

        }
        public bool doOneTrade(string name, DateTime dt, double yuan)
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
                return false;
            else if(code ==2) // trade time is not illegal. Should find next day
            {
                int waitday = 0;
                while((code ==2)&&(waitday<10))
                {
                    waitday++;
                    T1 = T1.AddDays(waitday);
                    code = getPrice(name, T1, ref Open);                    
                }
                if (waitday >= 10)
                    return false;  //hart to trade. give up this operation
            }
            else  // best T1 time
            {
            }

            if( Open.Open<Ref.Close) // open position condition check
            {
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
                    T2 = T2.AddDays(waitday);
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
            profit = quantity * (Close.High - Open.Open);
            profit -= quantity * Close.High * 0.001; //印花税
            profit -= quantity * Close.High * 0.00025; //卖出 佣金
            profit -= quantity * Open.Open * 0.00025; //买入 佣金
            profit -= quantity * Close.High * 0.00002; //卖出 过户费
            profit -= quantity * Open.Open * 0.00002; //买入 过户费

            //save trade data to DateTable
            DataRow dr = TradeDt.NewRow();
            TradeDt.Rows.Add(name, T1, Open.Open, "L", quantity, T2, Close.High, profit);
            return true;
        }
        public void OneDayTask(int i)
        {

        }
    }

}
