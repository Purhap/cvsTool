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
        private DataTable shareTable;
        public TestParam[] tp;
        public PersonForm view;
        public SimulationHouse(UInt16 artParallelNum, ref PersonForm argView)
        {            
            parallelNum = artParallelNum;
            view = argView;
            shareTable = new DataTable();
            simulations = new List<Simulation>(3);//,parallelNum>;
           
            loadTestParams();
                   
        }
        public void runParallelSimulation()
        {
            loadDataTable();
            for (int i = 0; i < parallelNum; i++)
            {
                simulations.Add(new Simulation(i.ToString(), ref shareTable, ref tp[i], ref view));
            }
            Parallel.Invoke(() => { simulations[0].runOnceSimulation(); },
                              () => { simulations[1].runOnceSimulation(); });

          //  simulations[0].runOnceSimulation();
        }

        private void loadTestParams()
        {
            UInt32 index = 0;
            tp = new TestParam[3];
            for (int i = 1; i < 2; i++)
            {
                for (int j = 1; j < 2; j++)
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
