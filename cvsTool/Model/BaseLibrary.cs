using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvsTool.Model
{
    public class BaseLibrary
    {
        public struct TestParam
        {
            public int k1;// hold time
            public double k2; //stop loss value
            public double k3; //wate ratio
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
    }
}
