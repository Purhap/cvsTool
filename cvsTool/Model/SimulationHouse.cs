using cvsTool.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static cvsTool.Model.BaseLibrary;

namespace cvsTool.Model
{
    public class SimulationHouse
    {
        public UInt32 currentThreadNum;
        private UInt16 parallelNum;
        private UInt16 startIndex;
        private UInt16 endIndex;
        public TestParamsRange tpr;
      //  public List <Simulation> simulations;
        public List<StrategyA001> simulations;
        public static DataTable shareTable;
        public static List<DataTable> AllPricelistTable;
        public List<TestParam> listTestParams ;
        public PersonForm view;
        public SimulationHouse(UInt16 artParallelNum,UInt16 argStartIndex, UInt16 argEndIndex, TestParamsRange argTpr, ref PersonForm argView)
        {            
            parallelNum = artParallelNum;
            startIndex = argStartIndex;
            endIndex = argEndIndex;
            tpr = argTpr;
            view = argView;
            shareTable = new DataTable();
            simulations = new List<StrategyA001>(1000);
            listTestParams = new List<TestParam>();
            loadTestParams();
            currentThreadNum = 0;
            AllPricelistTable = new List<DataTable>();


        }
        public void runParallelSimulation()
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();
            loadTestParams();
            loadMultiFiles2DataTable();
            sw.Stop();
            Console.WriteLine("Cost {0}ms",sw.ElapsedMilliseconds);

            // StrategyA001 SA001 = new StrategyA001();
            //SA001.runSimulation();

            try
            {
                Parallel.For(startIndex, endIndex, new ParallelOptions() { MaxDegreeOfParallelism = parallelNum }, i => { createSimulationAndRun(i); });
            }
            catch (AggregateException err)
            {
                foreach (Exception item in err.InnerExceptions)
                {
                    Console.WriteLine("exception: {0}{1} from : {2}{3} content:{4}", item.InnerException.GetType(), Environment.NewLine, item.InnerException.Source, Environment.NewLine, item.InnerException.Message);
                }
            }
        }
        public void runParallelSimulation_old()
        {
            loadDataTable();
            try
            {
                Parallel.For(startIndex, endIndex, new ParallelOptions() { MaxDegreeOfParallelism = parallelNum }, i => { createSimulationAndRun(i); });
            }
            catch (AggregateException err)
            {
                foreach (Exception item in err.InnerExceptions)
                {
                    Console.WriteLine("exception: {0}{1} from : {2}{3} content:{4}", item.InnerException.GetType(), Environment.NewLine, item.InnerException.Source, Environment.NewLine, item.InnerException.Message);
                }
            }
        }
        private void createSimulationAndRun_old(int i)
        {
          //  Simulation oneSimulation = new Simulation(i.ToString(), listTestParams[i], ref view, ref currentThreadNum);
          //  simulations.Add(oneSimulation);
          //  oneSimulation.runOnceSimulation();
        }
        private void createSimulationAndRun(int i)
        {
            StrategyA001 oneSimulation = new StrategyA001(i.ToString(), listTestParams[i], ref view, ref currentThreadNum);
            simulations.Add(oneSimulation);
            oneSimulation.runSimulation();
           
            //    Simulation oneSimulation = new Simulation(i.ToString(), listTestParams[i], ref view, ref currentThreadNum);
            //    simulations.Add(oneSimulation);
           // oneSimulation.runOnceSimulation();
        }

        private void loadTestParams()
        {         
            for (UInt16 i = tpr.k1; i < tpr.k1_End;)
            {
                for (double j = tpr.k2; j < tpr.k2_End;)
                {
                    for (double n = tpr.k3; n < tpr.k3_End;)
                    {
                        TestParam tp = new TestParam();
                        tp.k1 = i;
                        tp.k2 = j;
                        tp.k3 = n;
                        listTestParams.Add(tp);
                  
                        n += tpr.k3_Step;
                    }
                    j += tpr.k2_Step;
                }
                i += tpr.k1_Step;
            }            
        }

        private void loadDataTable()
        {
            Csv.ReadToDataTable(shareTable, "EUR2USD.csv");
            var query = shareTable.AsEnumerable().OrderBy(r => r["DateTime"]);
            shareTable = query.CopyToDataTable();
                        
          //  shareTable.DefaultView.Sort = "DateTime ASC";
           // shareTable = shareTable.DefaultView.ToTable();            
        }
        private void loadMultiFiles2DataTable()
        {
            string path = @"D:\new_tdx\T0002\export";
          //  string path = @"D:\works\Astocks\export";
            DirectoryInfo folder = new DirectoryInfo(path);
            foreach (FileInfo file in folder.GetFiles("*.txt"))
            {
                DataTable dt = new DataTable();
                
                Csv.ReadAStocksDataTable(dt, file.FullName);

                var query = dt.AsEnumerable().OrderBy(r => r["DateTime"]);
                if (query.Count() > 0)
                {
                    dt = query.CopyToDataTable();                
                    string name = file.Name;
                    name = name.Remove(name.Length - 4, 4);
                    dt.TableName = name;
                    AllPricelistTable.Add(dt);
                }
            }         
        }
    }
}
