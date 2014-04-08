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
            //ProcessDestination();
            //ETS();
            //UploadStagedData();
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

       

    }
}
