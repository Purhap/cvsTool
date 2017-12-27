using cvsTool.View;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static cvsTool.Model.BaseLibrary;

namespace cvsTool.Model
{
    public class SimulationHouse
    {
        private UInt16 parallelNum;
        public List <Simulation> simulations;
        public static DataTable shareTable;
        public static TestParam[] tp;
        public PersonForm view;
        public SimulationHouse(UInt16 artParallelNum, ref PersonForm argView)
        {            
            parallelNum = artParallelNum;
            view = argView;
            shareTable = new DataTable();
            simulations = new List<Simulation>(400);//,parallelNum>;
           
            loadTestParams();
                   
        }
        public void runParallelSimulation()
        {
            loadDataTable();
        //    for (int i = 0; i < parallelNum; i++)
       //     {
        //        simulations.Add(new Simulation(i.ToString(), ref shareTable, ref tp[i], ref view));
         //   }
           Parallel.For(0, parallelNum, new ParallelOptions() { MaxDegreeOfParallelism = 2 }, i => { createSimulationAndRun(i); });
          //  Parallel.Invoke(() => { simulations[0].runOnceSimulation(); },
           //                   () => { simulations[1].runOnceSimulation(); });

          //  simulations[0].runOnceSimulation();
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
            tp = new TestParam[500];
            for (int i = 1; i < 3; i++)
            {
                for (int j = 1; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
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
            shareTable.DefaultView.Sort = "DateTime ASC";
            shareTable = shareTable.DefaultView.ToTable();
        }
    }
}
