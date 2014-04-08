using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using NLog;
namespace specp.DataIntegration
{
    public class ETLUtil
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //mode
        public static string _TraxDIFTPTestMode;

        //local
        public static string _TraxDILocalStagePath;
        public static string _TraxDILocalUploadPath;
        public static string _TraxDILocalSuccessPath;
        public static string _TraxDILocalRejectedPath;
        public static string _TraxDILocalArchivePath;


        //ftp
        public static string _TraxDIFTPUploadPath;
        public static string _TraxDIFTPSuccessPath;
        public static string _TraxDIFTPRejectedPath;

        public static string _TraxDIFTPServer;
        public static string _TraxDIFTPUser;
        public static string _TraxDIFTPPwd;

        static ETLUtil()
        {
            logger.Trace("Initializing...");
            _TraxDIFTPTestMode = ConfigurationManager.AppSettings["TraxDIFTPTestMode"].ToString();

            _TraxDILocalStagePath = ConfigurationManager.AppSettings["TraxDILocalStagePath"].ToString();
            _TraxDILocalUploadPath = ConfigurationManager.AppSettings["TraxDILocalUploadPath"].ToString();
            _TraxDILocalSuccessPath = ConfigurationManager.AppSettings["TraxDILocalSuccessPath"].ToString();
            _TraxDILocalRejectedPath = ConfigurationManager.AppSettings["TraxDILocalRejectedPath"].ToString();  
            _TraxDILocalArchivePath = ConfigurationManager.AppSettings["TraxDILocalArchivePath"].ToString();

            _TraxDIFTPUploadPath = ConfigurationManager.AppSettings["TraxDIFTPUploadPath"].ToString();
            _TraxDIFTPSuccessPath = ConfigurationManager.AppSettings["TraxDIFTPSuccessPath"].ToString();
            _TraxDIFTPRejectedPath = ConfigurationManager.AppSettings["TraxDIFTPRejectedPath"].ToString();
            _TraxDIFTPServer = ConfigurationManager.AppSettings["TraxDIFTPServer"].ToString();
            _TraxDIFTPUser = ConfigurationManager.AppSettings["TraxDIFTPUser"].ToString();
            _TraxDIFTPPwd = ConfigurationManager.AppSettings["TraxDIFTPPwd"].ToString();

            logger.Trace("_TraxDILocalUploadPath={0} _TraxDIFTPUploadPath={1} ", ETLUtil._TraxDILocalUploadPath, ETLUtil._TraxDIFTPUploadPath);
            logger.Trace("Initializing...Done.");
        }

        public static string GetTempFileName(string f)
        {
            return Path.GetFileNameWithoutExtension(f) + ".tmp";

        }
    }
}
