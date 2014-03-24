﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text;
using NLog;
using System.Configuration;
using System.IO;

namespace specp.DataIntegration
{
    public class DataIntegrator
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private static string _TraxDIStagePath;
        private static string _TraxDILocalFTPPath;
        private static string _TraxDIFTPTestMode;
        private static string _TraxDIRemoteFTPStagePath;
        private static string _TraxDILocalArchivePath;
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
            logger.Trace("reading configuration...");

            _TraxDIStagePath = ConfigurationManager.AppSettings["TraxDIStagePath"].ToString();
            _TraxDILocalFTPPath = ConfigurationManager.AppSettings["TraxDILocalFTPPath"].ToString();
            _TraxDIFTPTestMode = ConfigurationManager.AppSettings["TraxDIFTPTestMode"].ToString();
            _TraxDIRemoteFTPStagePath = ConfigurationManager.AppSettings["TraxDIRemoteFTPStagePath"].ToString();
            _TraxDILocalArchivePath = ConfigurationManager.AppSettings["TraxDILocalArchivePath"].ToString();

            logger.Trace("TraxDIStagePath={0} _TraxDILocalFTPPath={1} ", _TraxDIStagePath, _TraxDILocalFTPPath);
        }

        public static void ServiceController()
        {


            logger.Trace("1.	On <scheduled time to> exchange data with Tencia");

            logger.Trace("1.1	Create Data Exchange Run Identifier <DXCHGRID>");

            logger.Trace("1.2	Create log file for Data Exchange Session/Run with < DXCHGRID>");

            logger.Trace("1.3	Process Destination/Tencia Backlog");
            ProcessDestination();

            logger.Trace("1.4	Extract Transform and Stage (ETS)");
            ETS();

            logger.Trace("1.5	Upload Staged Data");
            UploadStagedData();

            logger.Trace("1.6	Notify Problems");
            NotifyProblem();

        }

        static void ProcessDestination()
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

        static void ETS()
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

        static void CreateTestFile(string DataType)
        {
            var file = String.Format("{0}_{0}ID_{1:yyyyMMddhhmmssfff}.txt", DataType, DateTime.Now);
            var path = Path.Combine(_TraxDIStagePath, file);

            using (TextWriter tw = new StreamWriter(path))
            {
                tw.WriteLine("Test data file " + file);
                tw.Close();
            }
        }

        static void UploadStagedData()
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
            FTPTest();
        }

        static void FTPTest()
        {
            var files = Directory.GetFiles(_TraxDIStagePath);

            // Create remote stage folder
            FTP.CreateFolder(_TraxDIRemoteFTPStagePath);

            logger.Trace("FTP files...{0}", _TraxDIFTPTestMode);
            foreach (var f in files)
            {
                logger.Trace(f);

                // ftp success
                if (_TraxDIFTPTestMode.ToUpper() == "LOCAL" ? FTPLocal(f) : FTPRemote(f))
                {
                    // move to archive
                    File.Move(f, Path.Combine(_TraxDILocalArchivePath, Path.GetFileName(f)));
                    logger.Trace("Archived file {0}", f);
                }

            }
            logger.Trace("FTP files...{0} Done", _TraxDIFTPTestMode);
        }

        static bool FTPLocal(string f)
        {
            try
            {

                File.Move(f, Path.Combine(_TraxDILocalFTPPath, Path.GetFileName(f)));
            }
            catch (Exception e)
            {
                logger.Trace("Error Occured {0} ", e.Message);
                return false;
            }
            return true;
        }

        static bool FTPRemote(string f)
        {
            return FTP.Upload(f, _TraxDIRemoteFTPStagePath);

        }

        static void NotifyProblem()
        {
            logger.Trace("2.2.5	Notify Problems");
            //1.	For each log entry in <TransferData> Log with status in (STAGED-PENDING, SENT-PENDING, SENT-REJECTED)
            //1.1	Add entry to Notification Body
            //1.2	Log entry to Log of current <DXCHGRID>
            //2.	Send Notification 

        }
    }
}