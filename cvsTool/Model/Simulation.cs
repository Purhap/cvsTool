using cvsTool.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static cvsTool.Model.BaseLibrary;


namespace cvsTool.Model
{
    public class Simulation
    {
        private string name;        
        public UInt64 tradeTimes;

        public DateTime currentTime;
        private double currentPrice;
        public double profit;

        private TestParam testParam;
        private UInt64 badTradeTimes;
        private UInt64 goodTradeTimes;

        public UInt64 simulationIndex;
        private PositionStatus currentStatus;
        private PostionStruct currentPosition;
        private Trend currentPriceTrend;
        private Trend lastPriceTrend;
        public DataTable TradeDt;

        private MA_VALUE currentMA;
        private MA_VALUE lastMA;
        private MA_TREND currentMATrend;
        private MA_TREND lastMATrend;

        public delegate void UpdateCurrentStatusDelegate(string value, UInt16 index);
        public UpdateCurrentStatusDelegate updateCurrentStatusDelegate;

        public delegate void UpdateTradeLogParallelDelegate(string value);
        public UpdateTradeLogParallelDelegate updateTradeLogParallelDelegate;
        public Simulation(string argName, ref TestParam argTestParam, ref PersonForm view)
        {
            name = argName;            
            testParam = argTestParam;
            currentStatus = PositionStatus.None;
            currentPosition.closePrice = 0.0;
            tradeTimes = 0;
            badTradeTimes = 0;
            goodTradeTimes = 0;
            profit = 0;
            simulationIndex = 0;
            currentPriceTrend = Trend.None;
            lastPriceTrend = Trend.None;
            TradeDt = new DataTable();
           
            updateCurrentStatusDelegate = new Model.Simulation.UpdateCurrentStatusDelegate(view.updateCurrentStatus);
            updateTradeLogParallelDelegate = new Model.Simulation.UpdateTradeLogParallelDelegate(view.updateTradeLogParallelTextBox);
            

        }
        public void runOnceSimulation()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();      
            initializeTradeDataTable();
            onInit();
            runSimulate();
            string filename = string.Format("TI_{0:yyyyMMddHHmm}_BY{1}.csv", DateTime.Now, name);
            string ss = String.Format("Trade,{0}, profit,{1}, Good Trade, {2}, Bad Trade,{3}, K1,{4}, K2, {5}, K3,{6}, File name,{7} \r\n", tradeTimes, profit, goodTradeTimes, badTradeTimes, testParam.k1, testParam.k2, testParam.k3, filename);
            updateTradeLogParallelDelegate(ss);
            sw.Stop();
            Csv.SaveCSV(TradeDt, filename, ss);
            string str = String.Format("Thread {0} : {1:yyyy-MM-dd HH:mm:ss} Complete, Spend {2}s.\r\n", name, DateTime.Now,Convert.ToUInt32(sw.ElapsedMilliseconds/1000));
            updateTradeLogParallelDelegate(str);


        }
        private void runSimulate()
        {
            int end = SimulationHouse.shareTable.Rows.Count;
            
            for (int i = 200; i < end; i++)
            {
                monitorPriceTrend(i);

                if (i % 10000 == 0)
                {
                    string ss = String.Format("Thread {0} : {1:yyyy-MM-dd HH:mm:ss}", name, SimulationHouse.shareTable.Rows[i]["DateTime"]);
                    updateCurrentStatusDelegate(ss, Convert.ToUInt16(name));
                }
                
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
            if (now.DayOfWeek == DayOfWeek.Friday)
            {
                ret = false;
            }
            return ret;
        }

        private DateTime getCurrentTime(int i)
        {
            DateTime ret = Convert.ToDateTime(SimulationHouse.shareTable.Rows[i]["DateTime"]);
            return ret;
        }

        private void runCloseShortStrategy(int index)
        {
            if ((index - currentPosition.index) == testParam.k1)
            {
                closeShortPosition(index);
                estimateThisTrade();
            }
            else if (stopCondition())
            {
                closeShortPosition(index);
                estimateThisTrade();
            }
            else if (judgeWrong())
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
                if ((currentPrice - currentPosition.openPrice) < testParam.k2 * (-1))
                {
                    ret = true;
                }
            }
            else if (currentPosition.status == PositionStatus.Short)
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
            if (currentPosition.status == PositionStatus.Long) //long
            {
                if (this.currentPrice <= (this.currentPosition.openPrice - 0.0005))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (currentPosition.status == PositionStatus.Short) //short
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
            this.currentPosition.closeTime = (DateTime)SimulationHouse.shareTable.Rows[index]["DateTime"];

            this.currentPosition.closePrice = currentPrice;
            this.currentPosition.profit = (currentPosition.openPrice - currentPosition.closePrice) * 10000;
            profit += this.currentPosition.profit;
            currentStatus = PositionStatus.None;
        }
        private void closeLongPosition(int index)
        {
            this.tradeTimes++;
            this.currentPosition.closeTime = (DateTime)SimulationHouse.shareTable.Rows[index]["DateTime"];

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
            currentPrice = (double)SimulationHouse.shareTable.Rows[index]["BidClose"];
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
            if ((index - currentPosition.index) == 7)
            {
                closeLongPosition(index);
                estimateThisTrade();
            }
            else if (stopCondition())
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
            else if (currentPosition.profit > 0)
            {
                goodTradeTimes++;
            }

            TradeDt.Rows.Add(currentPosition.openTime, currentPosition.openPrice, currentPosition.status.ToString(), currentPosition.quantity, currentPosition.closeTime, currentPosition.closePrice, currentPosition.profit);
        }
        
        private void runOpenPositionStrategy(int index)
        {
            if ((currentMATrend.M5_SlopeRatio > 0) && (currentMATrend.M10_SlopeRatio > 0) && (currentMATrend.M20_SlopeRatio > 0))
            {
                if ((currentMA.M5 < currentMA.M10) && (currentMA.M5 < currentMA.M20))
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
            this.currentPosition.openTime = (DateTime)SimulationHouse.shareTable.Rows[index]["DateTime"];
            currentStatus = this.currentPosition.status;
        }

        private void openLongPosition(int index)
        {
            this.tradeTimes++;
            this.currentPosition.status = PositionStatus.Long;
            this.currentPosition.index = index;
            this.currentPosition.openPrice = currentPrice;
            this.currentPosition.openTime = (DateTime)SimulationHouse.shareTable.Rows[index]["DateTime"];
            currentStatus = this.currentPosition.status;

        }

        private double MA(int index, UInt16 nb)
        {
            double returnValue = 0.0;

            for (int i = 0; i < nb; i++)
            {
                returnValue += (double)SimulationHouse.shareTable.Rows[index - i]["BidClose"];
            }
            returnValue = returnValue / nb;
            return returnValue;
        }

        private void initializeTradeDataTable()
        {
            string[] columnsNames = new string[] { "OpenTime", "OpenPrice", "L/S", "Quantity", "CloseTime", "ClosePrice", "Profit" };

            for (int i = 0; i < 7; i++)
            {
                if ((i == 0) || (i == 4))
                {
                    DataColumn dc = new DataColumn(columnsNames[i], typeof(DateTime));
                    TradeDt.Columns.Add(dc);
                }
                else if (i == 2)
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
    }
}
