using cvsTool.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
        private UInt16 parallelNum;
        private UInt16 startIndex;
        private UInt16 endIndex;
        public List <Simulation> simulations;
        public static DataTable shareTable;
        public static TestParam[] tp;
        public PersonForm view;
        public SimulationHouse(UInt16 artParallelNum,UInt16 argStartIndex, UInt16 argEndIndex, ref PersonForm argView)
        {            
            parallelNum = artParallelNum;
            startIndex = argStartIndex;
            endIndex = argEndIndex;
            view = argView;
            shareTable = new DataTable();
            simulations = new List<Simulation>(1000);
           
            loadTestParams();
                   
        }
        public void runParallelSimulation()
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
                    Console.WriteLine("exception: {0}{1} from : {2}{3} content:{4}",item.InnerException.GetType(), Environment.NewLine, item.InnerException.Source, Environment.NewLine, item.InnerException.Message);
                }
            }
        }
        private void createSimulationAndRun(int i)
        {
            Simulation oneSimulation = new Simulation(i.ToString(), ref tp[i], ref view);
            simulations.Add(oneSimulation);
            oneSimulation.runOnceSimulation();
        }

        private void loadTestParams()
        {
            UInt32 index = 0;
            tp = new TestParam[1000];
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    for (int k = 0; k < 10; k++)
                    {
                        tp[index].k1 = i;
                        tp[index].k2 = 0.0001 * j;
                        tp[index].k3 = 0.0001 * k;
                        index++;
                    }
                }
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
    }
}
