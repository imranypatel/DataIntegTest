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
        public static string _TraxDILocalFTPPath;
        public static string _TraxDILocalArchivePath;


        //ftp
        public static string _TraxDIFTPStagePath;
        public static string _TraxDIFTPServer;
        public static string _TraxDIFTPUser;
        public static string _TraxDIFTPPwd;

        static ETLUtil()
        {
            logger.Trace("Initializing...");
            _TraxDIFTPTestMode = ConfigurationManager.AppSettings["TraxDIFTPTestMode"].ToString();

            _TraxDILocalStagePath = ConfigurationManager.AppSettings["TraxDILocalStagePath"].ToString();
            _TraxDILocalFTPPath = ConfigurationManager.AppSettings["TraxDILocalFTPPath"].ToString();                   
            _TraxDILocalArchivePath = ConfigurationManager.AppSettings["TraxDILocalArchivePath"].ToString();

            _TraxDIFTPStagePath = ConfigurationManager.AppSettings["TraxDIFTPStagePath"].ToString();
            _TraxDIFTPServer = ConfigurationManager.AppSettings["TraxDIFTPServer"].ToString();
            _TraxDIFTPUser = ConfigurationManager.AppSettings["TraxDIFTPUser"].ToString();
            _TraxDIFTPPwd = ConfigurationManager.AppSettings["TraxDIFTPPwd"].ToString();

            logger.Trace("TraxDIStagePath={0} _TraxDILocalFTPPath={1} ", ETLUtil._TraxDILocalStagePath, ETLUtil._TraxDILocalFTPPath);
            logger.Trace("Initializing...Done.");
        }

        public static string GetTempFileName(string f)
        {
            return Path.GetFileNameWithoutExtension(f) + ".tmp";

        }
    }
}
