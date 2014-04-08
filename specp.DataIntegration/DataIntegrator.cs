using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.Data.Linq;
using System.IO;
using specp.Domain.Repository;
using specp.Domain.Entities;
namespace specp.DataIntegration
{
    public class DataIntegrator
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        // data context
        SVCDataContext _ctxSVC;
        string _outError = "";
        IUploader _upl;


        SvcIns_AppServiceRunResult _Run;
        List<SvcGet_AppServiceQueueResult> _Queue;
        SvcIns_AppServiceRunQueueResult _RunQueue;

        string _DIUploadPath;
          string _DISuccessPath;
         string _DIRejectedPath;
        public DataIntegrator()
        {
            logger.Info("Initializing...");

            logger.Trace("Getting data context... ");
            _ctxSVC = new SVCDataContext(Globals.ConnectionString);

            logger.Trace("Setting uploader based on mode " + ETLUtil._TraxDIFTPTestMode.ToUpper());
            //uploader local file system or remote ftp
            if (ETLUtil._TraxDIFTPTestMode.ToUpper() == "LOCAL")
            {
                _upl = new FTPSimulator();
                _DIUploadPath = ETLUtil._TraxDILocalUploadPath;
                _DISuccessPath = ETLUtil._TraxDILocalSuccessPath; ;
                _DIRejectedPath = ETLUtil._TraxDILocalRejectedPath; ;
            }
            else // remote FTP
            {
                _upl = new FTP();
                _DIUploadPath = ETLUtil._TraxDIFTPUploadPath;
                _DISuccessPath = ETLUtil._TraxDIFTPSuccessPath; ;
                _DIRejectedPath = ETLUtil._TraxDIFTPRejectedPath; ;
            }

            logger.Info("Initializing...Done");

        }

        public void Run()
        {
            //FTPTests();
            //return;
            //logger.Trace("Creating folders if not exist");
            //CreateFolders();

            //logger.Trace("1.	On <scheduled time to> exchange data with Tencia");
            try
            {
                //logger.Trace("1.1	Create Data Exchange Run Identifier <DXCHGRID>");
                StartAppServiceRun();
                logger.Info("Starting Run with _Run.AppServiceRunID = {0} ", _Run.AppServiceRunID);

                //logger.Trace("1.2	Create log file for Data Exchange Session/Run with < DXCHGRID>");

                logger.Info("1.3	Process Destination/Tencia Backlog");
                ProcessUploadedBacklog();

                logger.Info("1.4	Extract Transform and Stage (ETS)");
                ETS();

                logger.Info("1.5	Upload Staged Data");
                UploadStagedData();

               
                //if (ETLUtil._TraxDIFTPTestMode.ToUpper() == "LOCAL")
                //    new ETLSimulator().Process();
                //else  // Remote
                //    new ETLSimulator().Process();

                logger.Trace("1.6	Notify Problems");
                NotifyProblem();


                logger.Info("Finishing Run with _AppServiceRunId = {0} ", _Run.AppServiceRunID);
                FinishAppServiceRun();
                logger.Info("Finished Run with _AppServiceRunId = {0} ", _Run.AppServiceRunID);
            }
            catch (Exception e)
            {
                logger.Fatal(e.Message);
            }
        }

        void StartAppServiceRun()
        {

            _Run = _ctxSVC.SvcIns_AppServiceRun(3, DateTime.Now, specp.Domain.Entities.Com.Status.NEW, specp.Domain.Entities.Com.SysUsers.SERVICE, ref _outError).FirstOrDefault();
            var am = GetAppMessage(_outError);
            if (am.Status == "ERR")
            {
                logger.Fatal(_outError);
                throw new Exception(_outError);
            }

        }

        bool FinishAppServiceRun()
        {

            var res = _ctxSVC.SvcUpd_AppServiceRun(_Run.AppServiceRunID, DateTime.Now, specp.Domain.Entities.Com.Status.CLOSED, specp.Domain.Entities.Com.SysUsers.SERVICE, ref _outError);
            var am = GetAppMessage(_outError);
            if (am.Status == "ERR")
            {
                logger.Fatal(_outError);
                throw new Exception(_outError);
            }

            return true;


        }
        
        void ProcessUploadedBacklog()
        {
            logger.Trace("2.2.2	Process Destination/TENCIA Backlog ");
            //logger.Trace("1.	For each log entry in <TransferData> log with as current Status in (SENT/SENT-PENDING)");
            //logger.Trace("1.1	When file still is in <designated folder at destination/tencia for Process> and Not Processed Within <DueTimeToProcess>");
            //logger.Trace("1.1.1	Set Status as SENT-PENDING");
            //logger.Trace("1.2	When file is in <designated folder at destination/tencia for Rejection>");
            //logger.Trace("1.2.1	Set Status as SENT-REJECTED");
            //logger.Trace("1.2.2	Move file to <designated folder at destination/tencia for Rejection Archive>");
            //logger.Trace("1.3	When file is in <designated folder at destination/tencia for Success>");
            //logger.Trace("1.3.1	Set Status as SENT-SUCCESS");
            //logger.Trace("1.3.2	Move file to <designated folder at destination/tencia for Success Archive>");
            //logger.Trace("1.4	Set BACKLOG_DXCHGRID as current <DXCHGRID>");

            GetAppServiceQueue(specp.Domain.Entities.Com.State.STAGED_SENT, -1, null);
            logger.Info("Total items in Destination queue = {0}", _Queue.Count());
            foreach (var qi in _Queue)  //100-TOBESENT
            {
                // initial state of queue item in this run
                AddAppServiceRunQueue(qi.AppServiceQueueID, qi.StateID, qi.RetryAttempts, qi.StatusID);

                switch (FindFileAtDestination(qi.FileName))
                {
                    case "UPLOADED":
                        UpdateAppServiceQueue(qi.AppServiceQueueID, qi.FileName, qi.StateID, qi.RetryAttempts+1, qi.StatusID);
                        AddAppServiceRunQueue(qi.AppServiceQueueID, qi.StateID, qi.RetryAttempts+1, qi.StatusID);
                        break;
                    case "SUCCESS":
                        UpdateAppServiceQueue(qi.AppServiceQueueID, qi.FileName, specp.Domain.Entities.Com.State.SENT_SUCCESS, 0, specp.Domain.Entities.Com.Status.CLOSED);
                        AddAppServiceRunQueue(qi.AppServiceQueueID, specp.Domain.Entities.Com.State.SENT_SUCCESS, 0, specp.Domain.Entities.Com.Status.CLOSED);
                        break;
                    case "REJECTED":
                        UpdateAppServiceQueue(qi.AppServiceQueueID, qi.FileName, specp.Domain.Entities.Com.State.SENT_REJECTED, 0, specp.Domain.Entities.Com.Status.CLOSED);
                        AddAppServiceRunQueue(qi.AppServiceQueueID, specp.Domain.Entities.Com.State.SENT_REJECTED, 0, specp.Domain.Entities.Com.Status.CLOSED);
                        break;
                    default: // MISSING
                        UpdateAppServiceQueue(qi.AppServiceQueueID, qi.FileName, specp.Domain.Entities.Com.State.SENT_MISSING, 0, specp.Domain.Entities.Com.Status.CLOSED);
                        AddAppServiceRunQueue(qi.AppServiceQueueID, specp.Domain.Entities.Com.State.SENT_MISSING, 0, specp.Domain.Entities.Com.Status.CLOSED);
                        break;
                }

            }
        }

        string FindFileAtDestination(string file)
        {
            // still in _DIUploadPath
            if (_upl.Dir(ETLUtil._TraxDIFTPServer, ETLUtil._TraxDIFTPUser, ETLUtil._TraxDIFTPPwd, _DIUploadPath, file) != null)
                return "UPLOADED";
            if (_upl.Dir(ETLUtil._TraxDIFTPServer, ETLUtil._TraxDIFTPUser, ETLUtil._TraxDIFTPPwd, _DISuccessPath, file) != null)
                return "SUCCESS";
            if (_upl.Dir(ETLUtil._TraxDIFTPServer, ETLUtil._TraxDIFTPUser, ETLUtil._TraxDIFTPPwd, _DIRejectedPath, file) != null)
                return "REJECTED";
            return "MISSING";
        }

        void ETS()
        {
            logger.Trace("2.2.3	Extract Transform and Stage (ETS)");
            //1.	For each log entry in <AppServiceQueue> Log with status as TOBESENT = 100
            GetAppServiceQueue(specp.Domain.Entities.Com.State.TO_BE_SENT, -1, null);
            logger.Info("Total items in ETS queue = {0}", _Queue.Count());
            foreach (var qi in _Queue)  //100-TOBESENT
            {

                //_QueueItem = qi;

                // initial state of queue item in this run
                AddAppServiceRunQueue(qi.AppServiceQueueID, qi.StateID, qi.RetryAttempts, qi.StatusID);
                
                //1.1	Extract and Transform/Map
                var data = string.Format("AppServiceQueueID {0} MapObjectID {1} MapObjectTargetID {2} StateID {3} StatusID {4}", qi.AppServiceQueueID, qi.MapObjectID, qi.MapObjectTargetID, qi.StateID, qi.StatusID);
                logger.Info(data);

                try
                {
                    //1.2	Create data files at <designated temp folder at source/trax>
                    //1.3	Move data in <designate staging folder at source/trax>
                    var file = CreateFile(qi.MapObjectID, data);

                    //1.4	Set Status as STAGED
                    UpdateAppServiceQueue(qi.AppServiceQueueID, file, specp.Domain.Entities.Com.State.TO_BE_SENT_STAGED, qi.RetryAttempts, qi.StatusID);

                    // updated state of queue item in this run
                    // 105 = STAGED, 36=CLOSED
                    AddAppServiceRunQueue(qi.AppServiceQueueID, specp.Domain.Entities.Com.State.TO_BE_SENT_STAGED, qi.RetryAttempts, specp.Domain.Entities.Com.Status.CLOSED);

                }
                catch (Exception e)
                {
                    logger.Error(e.Message);
                }

                //1.5	Set ETS_DXCHGRID as current <DXCHGRID>
                //1.6	Log entry to Log of current <DXCHGRID>



            }
           

          
        }
        void GetAppServiceQueue(int pStateID,int pAppServiceQueueID, string fileName)
        {
            _Queue = _ctxSVC.SvcGet_AppServiceQueue(pStateID, pAppServiceQueueID, fileName, ref _outError).ToList();
            var am = GetAppMessage(_outError);
            if (am.Status == "ERR")
            {
                logger.Fatal(_outError);
                throw new Exception(_outError);
            }
        }
        void AddAppServiceRunQueue(int pAppServiceQueueID, int pStateID, int pRetryAttempts, int pStatusID)
       {
           _RunQueue = _ctxSVC.SvcIns_AppServiceRunQueue(_Run.AppServiceRunID, pAppServiceQueueID,
               pStateID, pRetryAttempts, pStatusID, 
               specp.Domain.Entities.Com.SysUsers.SERVICE, 
               ref _outError).FirstOrDefault();

           var am = GetAppMessage(_outError);
           if (am.Status == "ERR")
           {
               logger.Fatal(_outError);
               throw new Exception(_outError);
           }
       }
        void UpdateAppServiceQueue(int pAppServiceQueueID, string pFileName, int pStateID, int pRetryAttempts, int pStatusID)
        {
            _ctxSVC.SvcUpd_AppServiceQueue(pAppServiceQueueID,pFileName,
                pRetryAttempts, pStateID, pStatusID,
                specp.Domain.Entities.Com.SysUsers.SERVICE,
                ref _outError);

            var am = GetAppMessage(_outError);
            if (am.Status == "ERR")
            {
                logger.Fatal(_outError);
                throw new Exception(_outError);
            }
        }
        
        
        void UploadStagedData()
        {
            logger.Trace("2.2.4	Upload Staged Data");
            //1.	For each log entry in <TransferData> Log with status in (STAGED, STAGED-PENDING)
            //1.1	Upload/FTP file with .tmp extension to <designated folder at destination/tencia>
            //1.1.1	Rename file to .txt extension at remote 
            //1.2	When Failure
            //1.2.1	Increase retry attempt count
            //1.2.2	When Not Processed Within <DueTimeToProcess>
            //1.2.2.1	Set Status as STAGED-PENDING
            //1.3	When Success
            //1.3.1	Set status as SENT
            //1.3.2	Move file to <designated folder at source/trax for Success Archive>
            //1.4	Set UPLOAD_DXCHGRID as current <DXCHGRID>
            //1.5	Log entry to Log of current <DXCHGRID>
            var files = Directory.GetFiles(ETLUtil._TraxDILocalStagePath);

            // Create remote stage folder
            //upl.CreateFolder(_TraxDIFTPServer, _TraxDIFTPUser, _TraxDIFTPPwd, _TraxDIRemoteFTPStagePath);

            //logger.Trace("FTP files...{0}", _TraxDIFTPTestMode);
            foreach (var f in files)
            {
                logger.Info("Uploading " + f);

                GetAppServiceQueue(-1, -1, Path.GetFileName(f));
                var qi = _Queue.FirstOrDefault();
                if (qi == null)
                {
                    logger.Error(Path.GetFileName(f) + " not found in queue");
                    continue;
                }

                // initial state of queue item in this run
                //AddAppServiceRunQueue(qi.AppServiceQueueID, qi.StateID, qi.RetryAttempts, qi.StatusID);

                // ftp success
                if (_upl.Upload(ETLUtil._TraxDIFTPServer, ETLUtil._TraxDIFTPUser, ETLUtil._TraxDIFTPPwd, _DIUploadPath , Path.GetFileName(f), f))
                {
                    // move to archive
                    File.Move(f, Path.Combine(ETLUtil._TraxDILocalArchivePath, Path.GetFileName(f)));

                    UpdateAppServiceQueue(qi.AppServiceQueueID,qi.FileName,specp.Domain.Entities.Com.State.STAGED_SENT , 0, qi.StatusID);
                    // updated state of queue item in this run
                    AddAppServiceRunQueue(qi.AppServiceQueueID, specp.Domain.Entities.Com.State.STAGED_SENT, 0, qi.StatusID);
                    logger.Info("Archived file {0}", f);
                }
                else
                {
                    UpdateAppServiceQueue(qi.AppServiceQueueID, qi.FileName, specp.Domain.Entities.Com.State.STAGED_PENDING, qi.RetryAttempts+1, qi.StatusID);
                    // updated state of queue item in this run
                    AddAppServiceRunQueue(qi.AppServiceQueueID, specp.Domain.Entities.Com.State.STAGED_PENDING, qi.RetryAttempts+1, qi.StatusID);
                    logger.Error("Cannot uploaded file {0}", f);
                }

              

            }
        }
        void NotifyProblem()
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

        specp.Domain.Entities.Com.AppMessage GetAppMessage(string msg)
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
                var am = new specp.Domain.Entities.Com.AppMessage { Status = "ERR", MessageId = "SYS-10001", Message1 = "App Message from dependent component is incorrect format", Message2 = "", Message3 = "", SourceMessage = msg };
                logger.Fatal(am.Message1 + " : " + am.SourceMessage);

                return am;
            }


        }
        void CreateFolders()
        {
            if (!Directory.Exists(ETLUtil._TraxDILocalStagePath)) Directory.CreateDirectory(ETLUtil._TraxDILocalStagePath);
            if (!Directory.Exists(ETLUtil._TraxDILocalUploadPath)) Directory.CreateDirectory(ETLUtil._TraxDILocalUploadPath);
            if (!Directory.Exists(ETLUtil._TraxDILocalSuccessPath)) Directory.CreateDirectory(ETLUtil._TraxDILocalSuccessPath);
            if (!Directory.Exists(ETLUtil._TraxDILocalRejectedPath)) Directory.CreateDirectory(ETLUtil._TraxDILocalRejectedPath);
            if (!Directory.Exists(ETLUtil._TraxDILocalArchivePath)) Directory.CreateDirectory(ETLUtil._TraxDILocalArchivePath);
        }
        string CreateFile(int MapObjectID,string data)
        {
            string dataType = (MapObjectID == 9) ? "ORGANISATION" : "INVOICE";
            var file = String.Format("{0}_{1:yyyyMMddhhmmssfff}.txt", dataType, DateTime.Now);
            var path = Path.Combine(ETLUtil._TraxDILocalStagePath, file);

            using (TextWriter tw = new StreamWriter(path))
            {
                tw.WriteLine("Test data file " + file + " " + data);
                tw.Close();
            }
            return file;

        }
        void TestNLog()
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
    }
}
