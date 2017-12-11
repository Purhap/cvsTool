using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using cvsTool.View;
using cvsTool.Model;
using fxcore2;
using System.Configuration;
using System.Threading;
using System.Data;



namespace cvsTool.Controllor
{
    public class PersonControllor
    {
        public PersonForm View;

        public Person Model;
        public DataTable InstrumentDT;
        private Thread getHistoryThread;
        private Thread simulateThread;

        public delegate void UpdateUIDelegate(int value, int count);
        public UpdateUIDelegate updateProcessDelegate;

        public delegate void UpdateLogDelegate(string value);
        public UpdateLogDelegate updateLogDelegate;

        public PersonControllor(PersonForm view)
        {
            Model = new Person() { ID = "1", Name = "None" };
            
            this.View = view;

            this.View.Controllor = this;

            this.InstrumentDT = Model.Csv.dataTable;
                        
        }

        public void UpdatePerson()
        {
            UpdateToDataBase(Model);
        }
        public void updateNames(string argName)
        {
            Model.Name = argName;
            Model.Csv.name = argName;            
        }

        private void UpdateToDataBase(Person p)
        {
            System.Windows.Forms.MessageBox.Show("ID:" + p.ID + "Name:" + p.Name);
        }
        public static void PrintPrices(O2GSession session, O2GResponse response)
        {
            Console.WriteLine("Request with RequestID={0} is completed:", response.RequestID);
            O2GResponseReaderFactory factory = session.getResponseReaderFactory();
            if (factory != null)
            {
                O2GMarketDataSnapshotResponseReader reader = factory.createMarketDataSnapshotReader(response);
                for (int ii = reader.Count - 1; ii >= 0; ii--)
                {
                    if (reader.isBar)
                    {
                        Console.WriteLine("DateTime={0:yyyyMMdd hh:mm:ss}, BidOpen={1}, BidHigh={2}, BidLow={3}, BidClose={4}, AskOpen={5}, AskHigh={6}, AskLow={7}, AskClose={8}, Volume={9}",
                                Convert.ToDateTime(reader.getDate(ii)), reader.getBidOpen(ii), reader.getBidHigh(ii), reader.getBidLow(ii), reader.getBidClose(ii),
                                reader.getAskOpen(ii), reader.getAskHigh(ii), reader.getAskLow(ii), reader.getAskClose(ii), reader.getVolume(ii));
                    }
                    else
                    {
                        Console.WriteLine("DateTime={0}, Bid={1}, Ask={2}", reader.getDate(ii), reader.getBidClose(ii), reader.getAskClose(ii));
                    }
                }
            }
        }

        public void storeHistoryPriceToDataTable(O2GSession session, O2GResponse response, string sInstrument)
        {
         //   string ss = string.Format("Request with RequestID={0} is completed:", response.RequestID);
        //    updateLogDelegate(ss);
            DataTable tmpTable = InstrumentDT.Clone();
            O2GResponseReaderFactory factory = session.getResponseReaderFactory();
            if (factory != null)
            {
                O2GMarketDataSnapshotResponseReader reader = factory.createMarketDataSnapshotReader(response);
                              
                for (int ii = reader.Count - 1; ii >= 0; ii--)
                {
                    if (reader.isBar)
                    {
                        InstrumentDT.Rows.Add(reader.getDate(ii), reader.getBidOpen(ii), reader.getBidHigh(ii), reader.getBidLow(ii), reader.getBidClose(ii),
                                reader.getAskOpen(ii), reader.getAskHigh(ii), reader.getAskLow(ii), reader.getAskClose(ii), reader.getVolume(ii));
                        tmpTable.Rows.Add(reader.getDate(ii), reader.getBidOpen(ii), reader.getBidHigh(ii), reader.getBidLow(ii), reader.getBidClose(ii),
                                reader.getAskOpen(ii), reader.getAskHigh(ii), reader.getAskLow(ii), reader.getAskClose(ii), reader.getVolume(ii));

                    }
                    else
                    {
                        InstrumentDT.Rows.Add(reader.getDate(ii), reader.getBidClose(ii), reader.getAskClose(ii));
                        tmpTable.Rows.Add(reader.getDate(ii), reader.getBidClose(ii), reader.getAskClose(ii));

                    }
                }
                SaveDataToFile(tmpTable, sInstrument);
                               
            }
        }

        private void writeHistoryPriceToFile( string sInstrument)
        {
            string fileName = sInstrument.Replace("/", "2");
            fileName += ".csv";            
            Csv.SaveCSV(InstrumentDT, fileName);
        }

        private void SaveDataToFile(DataTable dt, string sInstrument)
        {
            string fileName = sInstrument.Replace("/", "2");
            fileName += ".csv";
            Csv.AppendToCSV(dt, fileName);
        }
        /// <summary>
        /// Print process name and sample parameters
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="loginPrm"></param>
        /// <param name="prm"></param>
        private void PrintSampleParams(string procName, LoginParams loginPrm, SampleParams prm)
        {
           // Console.WriteLine("{0}: Instrument='{1}', Timeframe='{2}', DateFrom='{3}', DateTo='{4}'", procName, prm.Instrument, prm.Timeframe, prm.DateFrom.ToString("MM.dd.yyyy HH:mm:ss"), prm.DateTo.ToString("MM.dd.yyyy HH:mm:ss"));

            updateLogDelegate(string.Format("{0}: Instrument='{1}', Timeframe='{2}', DateFrom='{3}', DateTo='{4}'", procName, prm.Instrument, prm.Timeframe, prm.DateFrom.ToString("MM.dd.yyyy HH:mm:ss"), prm.DateTo.ToString("MM.dd.yyyy HH:mm:ss")));
            
        }

        public void startGetHistroyPrice()
        {
            getHistoryThread = new Thread(new ThreadStart(getHistoryPrice));
            getHistoryThread.Start();
        }
        public void startSimulate()
        {
            simulateThread = new Thread(new ThreadStart(Model.simulator.startSimulate));
            simulateThread.Start();
        }
        public void stopGetHistroyPrice()
        {
            if(getHistoryThread!=null)
            {
                getHistoryThread.Abort();
            }            
        }
        public void getHistoryPrice()
        {
            O2GSession session = null;

            try
            {
                LoginParams loginParams = new LoginParams(ConfigurationManager.AppSettings);
                SampleParams sampleParams = new SampleParams(ConfigurationManager.AppSettings);

                PrintSampleParams("GetHistPrices", loginParams, sampleParams);

                 session = O2GTransport.createSession();
                SessionStatusListener statusListener = new SessionStatusListener(session, loginParams.SessionID, loginParams.Pin);
                session.subscribeSessionStatus(statusListener);
                statusListener.Reset();
                session.login(loginParams.Login, loginParams.Password, loginParams.URL, loginParams.Connection);
                if (statusListener.WaitEvents() && statusListener.Connected)
                {
                    ResponseListener responseListener = new ResponseListener(session);
                    session.subscribeResponse(responseListener);
                    GetHistoryPrices(session, sampleParams.Instrument, sampleParams.Timeframe, sampleParams.DateFrom, sampleParams.DateTo, responseListener);
                    Console.WriteLine("Done!");
                    updateLogDelegate("Done!");
                    PersonForm.EnableStandby();
             //       writeHistoryPriceToFile(sampleParams.Instrument);
             //       updateNames(sampleParams.Instrument);
                    statusListener.Reset();
                    session.logout();
                    statusListener.WaitEvents();
                    session.unsubscribeResponse(responseListener);
                }
                session.unsubscribeSessionStatus(statusListener);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
               // updateLogDelegate(string.Format("Exception: {0}", e.ToString()));
            }
            finally
            {
                if (session != null)
                {
                    session.Dispose();
                }
            }
        }

        /// <summary>
        /// Request historical prices for the specified timeframe of the specified period
        /// </summary>
        /// <param name="session"></param>
        /// <param name="sInstrument"></param>
        /// <param name="sTimeframe"></param>
        /// <param name="dtFrom"></param>
        /// <param name="dtTo"></param>
        /// <param name="responseListener"></param>
        public void GetHistoryPrices(O2GSession session, string sInstrument, string sTimeframe, DateTime dtFrom, DateTime dtTo, ResponseListener responseListener)
        {
            O2GRequestFactory factory = session.getRequestFactory();
            O2GTimeframe timeframe = factory.Timeframes[sTimeframe];
            if (timeframe == null)
            {
                throw new Exception(string.Format("Timeframe '{0}' is incorrect!", sTimeframe));
            }
            O2GRequest request = factory.createMarketDataSnapshotRequestInstrument(sInstrument, timeframe, 300);
            DateTime dtFirst = dtTo;

            do // cause there is limit for returned candles amount
            {
                factory.fillMarketDataSnapshotRequestTime(request, dtFrom, dtFirst, false);
                responseListener.SetRequestID(request.RequestID);
                session.sendRequest(request);
                if (!responseListener.WaitEvents())
                {
                    throw new Exception("Response waiting timeout expired");
                }
                // shift "to" bound to oldest datetime of returned data
                O2GResponse response = responseListener.GetResponse();
                if (response != null && response.Type == O2GResponseType.MarketDataSnapshot)
                {
                    O2GResponseReaderFactory readerFactory = session.getResponseReaderFactory();
                    if (readerFactory != null)
                    {
                        O2GMarketDataSnapshotResponseReader reader = readerFactory.createMarketDataSnapshotReader(response);
                        if (reader.Count > 0)
                        {
                            if (DateTime.Compare(dtFirst, reader.getDate(0)) != 0)
                            {
                                dtFirst = reader.getDate(0); // earliest datetime of returned data
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                          //  Console.WriteLine("0 rows received");
                            updateLogDelegate(string.Format("0 rows received"));
                            break;
                        }
                    }
                    // PrintPrices(session, response);
                    storeHistoryPriceToDataTable(session, response, sInstrument);
                    // DateTime.Subtraction(dtTo, dtFirst)/ Subtraction
                    long percent = (dtTo.Ticks - dtFirst.Ticks) * 100 / (dtTo.Ticks - dtFrom.Ticks);
                    
                    updateProcessDelegate((int)percent, this.InstrumentDT.Rows.Count);
                }
                else
                {
                    break;
                }
            } while (dtFirst > dtFrom);
        }

    }
}
