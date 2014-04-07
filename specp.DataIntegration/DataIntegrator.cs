using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;

using System.IO;
using specp.Domain.Repository;
using specp.Domain.Entities;
namespace specp.DataIntegration
{
    public class DataIntegrator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // data context
        static SVCDataContext _ctxSVC;
        static string _outError = "";
        static int _AppServiceRunId;

        static void TestNLog()
        {
            var basedirPath = AppDomain.CurrentDomain.BaseDirectory;
            logger.Trace("Sample trace message");
            logger.Debug("Sample debug message");
            logger.Info("Sample informational message");
            logger.Warn("Sample warning message");
            logger.Error("Sample error message");
            logger.Fatal("Sample fatal error message");

            // alternatively you can call the Log() method 
            // and pass log level as the parameter.
            logger.Log(LogLevel.Info, "Sample informational message");
        }

        static DataIntegrator()
        {
            logger.Info("Initializing...");

            logger.Trace("Getting data context... ");
            _ctxSVC = new SVCDataContext(Globals.ConnectionString);

            logger.Info("Initializing...Done");

        }

        public static void ServiceController()
        {
            //FTPTests();
            //return;

            //logger.Trace("Creating folders if not exist");
            //CreateFolders();

            //logger.Trace("1.	On <scheduled time to> exchange data with Tencia");
            try
            {
                //logger.Trace("1.1	Create Data Exchange Run Identifier <DXCHGRID>");
                _AppServiceRunId = StartAppServiceRun();
                logger.Info("Starting Run with _AppServiceRunId = {0} ", _AppServiceRunId);

                //logger.Trace("1.2	Create log file for Data Exchange Session/Run with < DXCHGRID>");
                //logger.Trace("1.3	Process Destination/Tencia Backlog");
                //ProcessDestination();
                //logger.Trace("1.4	Extract Transform and Stage (ETS)");
                //ETS();
                //logger.Trace("1.5	Upload Staged Data");
                //UploadStagedData();

                logger.Info("Finishing Run with _AppServiceRunId = {0} ", _AppServiceRunId);
                if (ETLUtil._TraxDIFTPTestMode.ToUpper() == "LOCAL")
                    new ETLSimulator().Process();
                else
                    new ETLSimulator().Process();

                logger.Trace("1.6	Notify Problems");
                NotifyProblem();


                logger.Info("Finishing Run with _AppServiceRunId = {0} ", _AppServiceRunId);
                FinishAppServiceRun();
                logger.Info("Finished Run with _AppServiceRunId = {0} ", _AppServiceRunId);
            }
            catch (Exception e)
            {
                logger.Fatal(e.Message);
            }
        }

        public static int StartAppServiceRun()
        {
          
           var res = _ctxSVC.SvcIns_AppServiceRun(3, DateTime.Now, specp.Domain.Entities.Com.Status.NEW, specp.Domain.Entities.Com.SysUsers.SERVICE, ref _outError);
           var am = GetAppMessage(_outError);
           if (am.Status == "ERR")
           {
               logger.Fatal(_outError);
               throw new Exception(_outError);
           }
         
               return res.FirstOrDefault().AppServiceRunID;
           
        }

        public static bool FinishAppServiceRun()
        {
            
            var res = _ctxSVC.SvcUpd_AppServiceRun(_AppServiceRunId, DateTime.Now, specp.Domain.Entities.Com.Status.CLOSED, specp.Domain.Entities.Com.SysUsers.SERVICE, ref _outError);
            var am = GetAppMessage(_outError);
            if (am.Status == "ERR")
            {
                logger.Fatal(_outError);
                throw new Exception(_outError);
            }

            return true;
                

        }
       

        static void NotifyProblem()
        {
            logger.Trace("2.2.5	Notify Problems");
            //1.	For each log entry in <TransferData> Log with status in (STAGED-PENDING, SENT-PENDING, SENT-REJECTED)
            //1.1	Add entry to Notification Body
            //1.2	Log entry to Log of current <DXCHGRID>
            //2.	Send Notification 

        }

        //static void FTPTests()
        //{
        //    var d = FTP.Dir(_TraxDIFTPServer, _TraxDIFTPUser, _TraxDIFTPPwd, _TraxDIRemoteFTPStagePath);
        //    var df = FTP.Dir(_TraxDIFTPServer, _TraxDIFTPUser, _TraxDIFTPPwd, _TraxDIRemoteFTPStagePath,"Contact_ContactID_20140331115438483.txt");
        //    var dferr = FTP.Dir(_TraxDIFTPServer, _TraxDIFTPUser, _TraxDIFTPPwd, _TraxDIRemoteFTPStagePath, "xContact_ContactID_20140331115438483.txt");

        //    logger.Trace(d);
        //    logger.Trace(df);
        //    logger.Trace(dferr);
        //}

        static specp.Domain.Entities.Com.AppMessage GetAppMessage(string msg)
        {
            try
            {
                var arr = msg.Split(new char[] { '~' });
                var am = new specp.Domain.Entities.Com.AppMessage { Status = arr[0], MessageId = arr[1], Message1 = arr[2], Message2 = arr[3], Message3 = arr[4] };
                return am;
            }
            catch (Exception e)
            {
                logger.Fatal(e.Message);
                var am = new specp.Domain.Entities.Com.AppMessage { Status = "ERR", MessageId = "SYS-10001", Message1 = "App Message from dependent component is incorrect format", Message2 = "", Message3 = "", SourceMessage=msg };
                logger.Fatal(am.Message1 + " : " + am.SourceMessage);

                return am;
            }


        }
    }
}
