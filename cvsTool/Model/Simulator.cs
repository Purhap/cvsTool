using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvsTool.Model
{
     public class Simulator
    {
        public DataTable simulateDt;
        public UInt64 tradeTimes;

        public double profit;
        public DataTable TradeDt;

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



        public struct TestParamStruct
        {
            double k1;
            double k2;
        }
        public enum PositionStatus{
            None,
            Bid,
            Ask,
        }
        public enum Trend
        {
            None,
            Ascend,
            Descend
        }
        public struct MA_VALUE
        {
            public double M5;
            public double M10;
            public double M20;
            public double M60;
            public double M120;
        }
        public struct MA_TREND
        {
            public double M5_SlopeRatio;
            public double M10_SlopeRatio;
            public double M20_SlopeRatio;
            public double M60_SlopeRatio;
            public double M120_SlopeRatio;
        }

        public struct PostionStruct
        {
            public PositionStatus status;
            public int index;

            public DateTime openTime;
            public DateTime closeTime;
            public double openPrice;
            public double closePrice;
            public double profit;
            internal double quantity;
        }
        
        public TestParamStruct testParam;
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
            currentPriceTrend = Trend.None;
            lastPriceTrend = Trend.None;

            initializeTradeDataTable();

        }

        private void initializeTradeDataTable()
        {
            string[] columnsNames = new string[] { "OpenTime", "OpenPrice", "BidAsk","Quantity", "CloseTime", "ClosePrice",  "Profit"};
            TradeDt = new DataTable();
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

        public void startSimulate()
        {
            startSimulateTime = DateTime.Now;
            string ss = "Start Time: ";            
            ss += String.Format("{0:yyyy-MM-dd HH:mm:ss}", startSimulateTime);
            updateCurrentTickDelegate(ss);
            
            loadData();
            loadDataTableCompleteTime = DateTime.Now;
            loadTestParam();
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
            string filename = string.Format("TradeInformation{0:yyyyMMddHHmm}.csv",DateTime.Now);
            Csv.SaveCSV(TradeDt, filename);
            ss += "\r\nSave Trade data in " + filename;
            updateCurrentTickDelegate(ss);

        }
        private void loadData()
        {
            Csv.ReadToDataTable(simulateDt, "EUR2USD.csv");
            simulateDt.DefaultView.Sort = "DateTime ASC";
            simulateDt = simulateDt.DefaultView.ToTable();
        }
        private void loadTestParam()
        {

        }
        private void runSimulate()
        {
            int end = simulateDt.Rows.Count;
            for (int i = 200; i < end; i++)
            {
                monitorPriceTrend(i);
                if (i % 10000 == 0)
                {
                    string ss = "Start Time: ";
                    ss += String.Format("{0:yyyy-MM-dd HH:mm:ss}", startSimulateTime);

                    ss += "\r\n";
                    ss += String.Format("{0:yyyy-MM-dd HH:mm:ss}", simulateDt.Rows[i]["DateTime"]);

                    if (loadDataTableCompleteTime > startSimulateTime)
                    {
                        ss += "\r\n";
                        ss += "Load Data Period: ";

                        ss += String.Format("{0}", loadDataTableCompleteTime.Subtract(startSimulateTime));
                        if (finishSimulateTime > loadDataTableCompleteTime)
                        {
                            ss += "\r\n";
                            ss += "Complete Time:";
                            ss += String.Format("{0:yyyy-MM-dd HH:mm:ss}", finishSimulateTime);
                        }

                    }
                    updateCurrentTickDelegate(ss);
                }
                switch (currentStatus)
                {
                    case PositionStatus.None:
                        runOpenPositionStrategy(i);
                        break;
                    case PositionStatus.Bid:
                        runCloseBidStrategy(i);
                        break;
                    case PositionStatus.Ask:
                        runCloseAskStrategy(i);
                        break;
                }
            }
        }

        private void runCloseAskStrategy(int index)
        {
            if ((index - currentPosition.index) == 7)
            {
                closeAskPosition(index);
                estimateThisTrade();
            }
        }

        private void closeAskPosition(int index)
        {
            this.tradeTimes++;
            this.currentPosition.closeTime = (DateTime)simulateDt.Rows[index]["DateTime"];
            
            this.currentPosition.closePrice = currentPrice;
            this.currentPosition.profit = (currentPosition.openPrice - currentPosition.closePrice) * 10000;
            profit += this.currentPosition.profit;
            currentStatus = PositionStatus.None;
        }
        private void closeBidPosition(int index)
        {
            this.tradeTimes++;
            this.currentPosition.closeTime = (DateTime)simulateDt.Rows[index]["DateTime"];
            
            this.currentPosition.closePrice = currentPrice;

            this.currentPosition.profit = (currentPosition.closePrice - currentPosition.openPrice) * 10000;
            profit += this.currentPosition.profit;
            currentStatus = PositionStatus.None;
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


        private void runCloseBidStrategy(int index)
        {
            if ((index -currentPosition.index)==7)
            {
                closeBidPosition(index);
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
                    openBidPosition(index);
                }                
            }
            else if ((currentMATrend.M5_SlopeRatio < 0) && (currentMATrend.M10_SlopeRatio < 0) && (currentMATrend.M20_SlopeRatio < 0))
            {
                if ((currentMA.M5 > currentMA.M10) && (currentMA.M5 > currentMA.M20))
                {
                    openAskPosition(index);
                }
            }
        }

        private void openAskPosition(int index)
        {
            this.tradeTimes++;
            this.currentPosition.status = PositionStatus.Bid;
            this.currentPosition.index = index;
            this.currentPosition.openPrice = currentPrice;
            this.currentPosition.openTime = (DateTime)simulateDt.Rows[index]["DateTime"];
            currentStatus = this.currentPosition.status;
        }

        private void openBidPosition(int index)
        {
            this.tradeTimes++;
            this.currentPosition.status = PositionStatus.Ask;
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
