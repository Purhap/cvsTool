using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvsTool.Model
{
    public class BaseLibrary
    {
        public struct TestParamsRange
        {
            public UInt16 k1;// hold time
            public double k2; //stop loss value
            public double k3; //wave ratio
            public double k4;

            public UInt16 k1_End;// hold time
            public double k2_End; //stop loss value
            public double k3_End; //wave ratio
            public double k4_End;

            public UInt16 k1_Step;// hold time
            public double k2_Step; //stop loss value
            public double k3_Step; //wave ratio
            public double k4_Step;
        }
        public struct Price
        {
            public double Open;
            public double High;
            public double Low;
            public double Close;    
            public Price(double argOpen, double argHigh, double argLow, double argClose) :this()
            {
                Open = argOpen;
                High = argHigh;
                Low = argLow;
                Close = argClose;

            }        
           
        }
        public struct TestParam
        {
            public UInt16 k1;// hold time
            public double k2; //stop loss value
            public double k3; //wave ratio
            public double k4;           
        }
        public enum PositionStatus
        {
            None,
            Long,
            Short,
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
            public double M30;
            public double M60;
            public double M120;
            public double M240;
        }
        public class MA
        {
            public UInt16 id;
            public UInt16 elementNumber;
            public double sum;
            public double ma;
            
            public MA(UInt16 nb)
            {
                id = nb;
                elementNumber = 0;
                sum = 0;
                ma = 0;
            }
        }
        public struct MA_TREND
        {
            public double M5_SlopeRatio;
            public double M10_SlopeRatio;
            public double M20_SlopeRatio;
            public double M30_SlopeRatio;
            public double M60_SlopeRatio;
            public double M120_SlopeRatio;
            public double M240_SlopeRatio;
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
    }
}
