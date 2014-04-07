using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using System.Configuration;
using System.IO;
namespace specp.DataIntegration
{
    class ETLSimulator: IETL
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        //private static string _TraxDILocalStagePath;
        //private static string _TraxDILocalFTPPath;
        //private static string _TraxDILocalArchivePath;

        public ETLSimulator()
        {

        }

        public void Process()
        {
            ProcessDestination();
            ETS();
            UploadStagedData();
        }
        void CreateFolders()
        {
            if (!Directory.Exists(ETLUtil._TraxDILocalStagePath)) Directory.CreateDirectory(ETLUtil._TraxDILocalStagePath);
            if (!Directory.Exists(ETLUtil._TraxDILocalFTPPath)) Directory.CreateDirectory(ETLUtil._TraxDILocalFTPPath);
            if (!Directory.Exists(ETLUtil._TraxDILocalArchivePath)) Directory.CreateDirectory(ETLUtil._TraxDILocalArchivePath);
        }

        void ProcessDestination()
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



        }

        void ETS()
        {
            logger.Trace("2.2.3	Extract Transform and Stage (ETS)");
            //1.	For each log entry in <TransferData> Log with status as TOBESENT
            //1.1	Extract and Transform/Map
            //1.2	Create data files at <designated temp folder at source/trax>
            //1.3	Move data in <designate staging folder at source/trax>
            //1.4	Set Status as STAGED
            //1.5	Set ETS_DXCHGRID as current <DXCHGRID>
            //1.6	Log entry to Log of current <DXCHGRID>

            CreateTestFile("Org");
            CreateTestFile("Contact");
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

            IUploader upl = new FTPSimulator();

            // Create remote stage folder
            //upl.CreateFolder(_TraxDIFTPServer, _TraxDIFTPUser, _TraxDIFTPPwd, _TraxDIRemoteFTPStagePath);

            //logger.Trace("FTP files...{0}", _TraxDIFTPTestMode);
            foreach (var f in files)
            {
                logger.Trace(f);

                // ftp success
                if (upl.Upload("","","","","",f))
                {
                    // move to archive
                    File.Move(f, Path.Combine(ETLUtil._TraxDILocalArchivePath, Path.GetFileName(f)));
                    logger.Trace("Archived file {0}", f);
                }

            }
        }

        //bool FTPDataLocal(string f)
        //{
        //    try
        //    {
        //        var tmpFtpFileName = ETLUtil.GetTempFileName(f);
        //        File.Copy(f, Path.Combine(ETLUtil._TraxDILocalFTPPath, tmpFtpFileName));
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Trace("Error Occured {0} ", e.Message);
        //        return false;
        //    }
        //    return true;
        //}

        //static bool FTPDataTestRemote(string f)
        //{

        //    var tmpFtpFileName = GetTempFileName(f);
        //    return FTP.Upload(_TraxDIFTPServer, _TraxDIFTPUser, _TraxDIFTPPwd, _TraxDIRemoteFTPStagePath, tmpFtpFileName, f);

        //}

        void CreateTestFile(string DataType)
        {
            var file = String.Format("{0}_{0}ID_{1:yyyyMMddhhmmssfff}.txt", DataType, DateTime.Now);
            var path = Path.Combine(ETLUtil._TraxDILocalStagePath, file);

            using (TextWriter tw = new StreamWriter(path))
            {
                tw.WriteLine("Test data file " + file);
                tw.Close();
            }
        }

    }
}
