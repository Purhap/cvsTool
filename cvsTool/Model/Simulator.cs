using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cvsTool.Model.BaseLibrary;


namespace cvsTool.Model
{
     public class Simulator
    {
        public DataTable simulateDt;
        public UInt64 tradeTimes;

        public DateTime currentTime;
        public double profit;
        public DataTable TradeDt;

        public int simulationIndex;
        private PositionStatus currentStatus;
        private PostionStruct currentPosition;
        private Trend currentPriceTrend;
        private Trend lastPriceTrend;

        private MA_VALUE currentMA;
        private MA_VALUE lastMA;
        private MA_TREND currentMATrend;
        private MA_TREND lastMATrend;

        public delegate void UpdateTradeLogDelegate(string value);
        public UpdateTradeLogDelegate updateTradeLogDelegate;

        public delegate void UpdateCurrentTickDelegate(string value);
        public UpdateCurrentTickDelegate updateCurrentTickDelegate;

        public DateTime startSimulateTime;
        public DateTime finishSimulateTime;
        public DateTime loadDataTableCompleteTime;
        
        public static TestParam testParam;
        private double currentPrice;
        private int badTradeTimes;
        private int goodTradeTimes;

        public Simulator()
        {
            simulateDt = new DataTable();
            currentStatus= PositionStatus.None;
            currentPosition.closePrice = 0.0;
            tradeTimes = 0;
            badTradeTimes = 0;
            goodTradeTimes = 0;
            profit = 0;
            simulationIndex = 0;
            currentPriceTrend = Trend.None;
            lastPriceTrend = Trend.None;
            
        }

        private void initializeTradeDataTable()
        {
            string[] columnsNames = new string[] { "OpenTime", "OpenPrice", "L/S","Quantity", "CloseTime", "ClosePrice",  "Profit"};
            
            for(int i = 0; i<7; i++)
            {
                if ((i == 0)||(i==4))
                {
                    DataColumn dc = new DataColumn(columnsNames[i], typeof(DateTime));
                    TradeDt.Columns.Add(dc);
                }
                else if(i ==2)
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

        public void startSimulate_bak()
        {
            for (UInt16 i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    for (int k = 0; k < 6; k++)
                    {
                        testParam.k1 = i;
                        testParam.k2 = 0.0001 * j;
                        testParam.k3 = 0.0001 * k;
                        runOnceSimulation();
                    }
                }
            }
        }
        public void startSimulate()
        {
            testParam.k1 = 1;
            testParam.k2 = 0.0001;
            testParam.k3 = 0.0001 ;
            Parallel.Invoke(() => runOnceSimulation(),
                () => runOnceSimulation2(),
                () => runOnceSimulation(),
                () => runOnceSimulation2());
        }
      

        private void runOnceSimulation()
        {
            simulationIndex++;
            string ss = "NO. ";
            ss += String.Format("{0} ", simulationIndex);
            startSimulateTime = DateTime.Now;
            ss += "\r\nStart Time: ";
            ss += String.Format("{0:yyyy-MM-dd HH:mm:ss}", startSimulateTime);
            updateCurrentTickDelegate(ss);
            if(simulateDt.Rows.Count<3)
            { 
               loadData();
            }
            loadDataTableCompleteTime = DateTime.Now;
           
            TradeDt = new DataTable();
            initializeTradeDataTable();
            onInit();
            runSimulate();
            finishSimulateTime = DateTime.Now;
            ss += "\r\nComplete Time:";
            ss += String.Format("{0:yyyy-MM-dd HH:mm:ss}", finishSimulateTime);

            ss += "\r\nSimulation Last Period:";
            ss += String.Format("{0}", finishSimulateTime.Subtract(loadDataTableCompleteTime));

            ss += "\r\nTrade information:";
            ss += String.Format("\r\nTrade:{0} profit:{1} ", tradeTimes, profit);
            ss += String.Format("\r\nGood Trade:{0} Bad Trade:{1} ", goodTradeTimes, badTradeTimes);
            updateCurrentTickDelegate(ss);
            string filename = string.Format("TradeInformation{0:yyyyMMddHHmm}.csv", DateTime.Now);
            
            ss += "\r\nSave Trade data in " + filename;
            string comments = string.Format("\r\n K1: {0},K2: {1},K3: {2}", testParam.k1, testParam.k2, testParam.k3);
            Csv.SaveCSV(TradeDt, filename, comments);
            updateCurrentTickDelegate(ss);
           // simulateDt.Clear();
          //  TradeDt.Clear();
            TradeDt.Dispose();

        }

        private void runOnceSimulation2()
        {
            simulationIndex++;
            string ss = "NO. ";
            ss += String.Format("{0} ", simulationIndex);
            startSimulateTime = DateTime.Now;
            ss += "\r\nStart Time: ";
            ss += String.Format("{0:yyyy-MM-dd HH:mm:ss}", startSimulateTime);
            updateCurrentTickDelegate(ss);
            if (simulateDt.Rows.Count < 3)
            {
                loadData();
            }
            loadDataTableCompleteTime = DateTime.Now;
            

            TradeDt = new DataTable();
            initializeTradeDataTable();
            onInit();
            runSimulate();
            finishSimulateTime = DateTime.Now;
            ss += "\r\nComplete Time:";
            ss += String.Format("{0:yyyy-MM-dd HH:mm:ss}", finishSimulateTime);

            ss += "\r\nSimulation Last Period:";
            ss += String.Format("{0}", finishSimulateTime.Subtract(loadDataTableCompleteTime));

            ss += "\r\nTrade information:";
            ss += String.Format("\r\nTrade:{0} profit:{1} ", tradeTimes, profit);
            ss += String.Format("\r\nGood Trade:{0} Bad Trade:{1} ", goodTradeTimes, badTradeTimes);
            updateCurrentTickDelegate(ss);
            string filename = string.Format("TradeInformation{0:yyyyMMddHHmm}.csv", DateTime.Now);

            ss += "\r\nSave Trade data in " + filename;
            string comments = string.Format("\r\n K1: {0},K2: {1},K3: {2}", testParam.k1, testParam.k2, testParam.k3);
            Csv.SaveCSV(TradeDt, filename, comments);
            updateCurrentTickDelegate(ss);
            // simulateDt.Clear();
            //  TradeDt.Clear();
            TradeDt.Dispose();

        }

        private void loadData()
        {
            Csv.ReadToDataTable(simulateDt, "EUR2USD.csv");
            simulateDt.DefaultView.Sort = "DateTime ASC";
            simulateDt = simulateDt.DefaultView.ToTable();
        }

        private void runSimulate()
        {
            int end = simulateDt.Rows.Count;
            for (int i = 200; i < end; i++)
            {
                monitorPriceTrend(i);

                //do not update process
                //if (i % 10000 == 0)
                //{
                //    string ss = "Start Time: ";
                //    ss += String.Format("{0:yyyy-MM-dd HH:mm:ss}", startSimulateTime);

                //    ss += "\r\n";
                //    ss += String.Format("{0:yyyy-MM-dd HH:mm:ss}", simulateDt.Rows[i]["DateTime"]);

                //    if (loadDataTableCompleteTime > startSimulateTime)
                //    {
                //        ss += "\r\n";
                //        ss += "Load Data Period: ";

                //        ss += String.Format("{0}", loadDataTableCompleteTime.Subtract(startSimulateTime));
                //        if (finishSimulateTime > loadDataTableCompleteTime)
                //        {
                //            ss += "\r\n";
                //            ss += "Complete Time:";
                //            ss += String.Format("{0:yyyy-MM-dd HH:mm:ss}", finishSimulateTime);
                //        }

                //    }
                //    updateCurrentTickDelegate(ss);
               // }
                switch (currentStatus)
                {
                    case PositionStatus.None:
                        if (openPositionCondition(i))
                        {
                            runOpenPositionStrategy(i);
                        }
                        break;
                    case PositionStatus.Long:
                        runCloseLongStrategy(i);
                        break;
                    case PositionStatus.Short:
                        runCloseShortStrategy(i);
                        break;
                }
            }
        }

        private bool openPositionCondition(int i)
        {
            bool ret = true;
            DateTime now = getCurrentTime(i);
            if(now.DayOfWeek == DayOfWeek.Friday)
            {
                ret = false;
            }
            return ret;
        }

        private DateTime getCurrentTime(int i)
        {
            DateTime ret = Convert.ToDateTime(simulateDt.Rows[i]["DateTime"]);
            return ret;
        }

        private void runCloseShortStrategy(int index)
        {
            if ((index - currentPosition.index) == testParam.k1)
            {
                closeShortPosition(index);
                estimateThisTrade();
            }
            else if(stopCondition())
            {
                closeShortPosition(index);
                estimateThisTrade();
            }
            else if(judgeWrong())
            {
                closeShortPosition(index);
                estimateThisTrade();
            }
        }

        private bool judgeWrong()
        {
            bool ret = false;
            if (currentPosition.status == PositionStatus.Long)
            {
                if ((currentPrice - currentPosition.openPrice) < testParam.k2*(-1))
                {
                    ret = true;
                }
            }
            else if(currentPosition.status == PositionStatus.Short)
            {
                if ((currentPrice - currentPosition.openPrice) > testParam.k2)
                {
                    ret = true;
                }
            }
            return ret;
        }

        private bool stopCondition()
        {
            if(currentPosition.status == PositionStatus.Long) //long
            {
                if(this.currentPrice <= (this.currentPosition.openPrice-0.0005))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if(currentPosition.status == PositionStatus.Short) //short
            {
                if (this.currentPrice >= (this.currentPosition.openPrice + 0.0005))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }            
        }

        private void closeShortPosition(int index)
        {
            this.tradeTimes++;
            this.currentPosition.closeTime = (DateTime)simulateDt.Rows[index]["DateTime"];
            
            this.currentPosition.closePrice = currentPrice;
            this.currentPosition.profit = (currentPosition.openPrice - currentPosition.closePrice) * 10000;
            profit += this.currentPosition.profit;
            currentStatus = PositionStatus.None;
        }
        private void closeLongPosition(int index)
        {
            this.tradeTimes++;
            this.currentPosition.closeTime = (DateTime)simulateDt.Rows[index]["DateTime"];
            
            this.currentPosition.closePrice = currentPrice;

            this.currentPosition.profit = (currentPosition.closePrice - currentPosition.openPrice) * 10000;
            profit += this.currentPosition.profit;
            currentStatus = PositionStatus.None;
        }
        private void onInit()
        {
            this.tradeTimes = 0;
            profit = 0;
            goodTradeTimes = 0;
            badTradeTimes = 0;
        }
        private void monitorPriceTrend(int index)
        {
            currentPrice = (double)simulateDt.Rows[index]["BidClose"];
            lastMA = currentMA;

            currentMA.M5 = MA(index, 5);
            currentMA.M10 = MA(index, 10);
            currentMA.M20 = MA(index, 20);
            currentMA.M60 = MA(index, 60);
            currentMA.M120 = MA(index, 120);

            lastMATrend = currentMATrend;
            currentMATrend.M5_SlopeRatio = currentMA.M5 - lastMA.M5;
            currentMATrend.M10_SlopeRatio = currentMA.M10 - lastMA.M10;
            currentMATrend.M20_SlopeRatio = currentMA.M20 - lastMA.M20;
            currentMATrend.M60_SlopeRatio = currentMA.M60 - lastMA.M60;
            currentMATrend.M120_SlopeRatio = currentMA.M120 - lastMA.M120;
           

            lastPriceTrend = currentPriceTrend;
            if (currentMA.M5 > currentMA.M10)
            {                
                this.currentPriceTrend = Trend.Ascend;
            }
            else if (currentMA.M5 < currentMA.M10)
            {
                this.currentPriceTrend = Trend.Descend;
            }
                        
        }


        private void runCloseLongStrategy(int index)
        {
            if ((index -currentPosition.index)==7)
            {
                closeLongPosition(index);
                estimateThisTrade();
            }     
            else if(stopCondition())
            {
                closeLongPosition(index);
                estimateThisTrade();
            }
            else if (judgeWrong())
            {
                closeLongPosition(index);
                estimateThisTrade();
            }
        }

        private void estimateThisTrade()
        {
            if (currentPosition.profit < 0)
            {
                badTradeTimes++;
            }
            else if(currentPosition.profit >0)
            {
                goodTradeTimes++;
            }
            
            TradeDt.Rows.Add(currentPosition.openTime,currentPosition.openPrice,currentPosition.status.ToString(),currentPosition.quantity, currentPosition.closeTime, currentPosition.closePrice,currentPosition.profit);
        }

  

        private void runOpenPositionStrategy(int index)
        {
           if((currentMATrend.M5_SlopeRatio>0)&&(currentMATrend.M10_SlopeRatio > 0 )&& (currentMATrend.M20_SlopeRatio > 0))                
            {
                if((currentMA.M5 < currentMA.M10)&& (currentMA.M5 < currentMA.M20))
                {
                    if (waveRate() > testParam.k3)
                    {
                        openLongPosition(index);
                    }
                }                
            }
            else if ((currentMATrend.M5_SlopeRatio < 0) && (currentMATrend.M10_SlopeRatio < 0) && (currentMATrend.M20_SlopeRatio < 0))
            {
                if ((currentMA.M5 > currentMA.M10) && (currentMA.M5 > currentMA.M20))
                {
                    if (waveRate() > testParam.k3)
                    {
                        openAskPosition(index);
                    }
                }
            }
        }

        private double waveRate()
        {
            double waveS = Math.Abs(currentMA.M5 - currentMA.M10);
            double waveM = Math.Abs(currentMA.M5 - currentMA.M20);
            double waveL = Math.Abs(currentMA.M5 - currentMA.M60);
            return Math.Sqrt(waveS * waveS + waveM * waveM);
        }

        private void openAskPosition(int index)
        {
            this.tradeTimes++;
            this.currentPosition.status = PositionStatus.Short;
            this.currentPosition.index = index;
            this.currentPosition.openPrice = currentPrice;
            this.currentPosition.openTime = (DateTime)simulateDt.Rows[index]["DateTime"];
            currentStatus = this.currentPosition.status;
        }

        private void openLongPosition(int index)
        {
            this.tradeTimes++;
            this.currentPosition.status = PositionStatus.Long;
            this.currentPosition.index = index;
            this.currentPosition.openPrice = currentPrice;
            this.currentPosition.openTime = (DateTime)simulateDt.Rows[index]["DateTime"];
            currentStatus = this.currentPosition.status;

        }

        private double MA(int index,UInt16 nb)
        {
            double returnValue = 0.0;

            for (int i = 0; i < nb; i++)
            {
                returnValue += (double)simulateDt.Rows[index-i]["BidClose"];
            }
            returnValue = returnValue / nb;
            return returnValue;
        }


    }
}
