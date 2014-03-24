using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using NLog;

namespace specp.DataIntegration
{
    public class FTP
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        static string _TraxDIFTPServer;
        static string _TraxDIFTPUser;
        static string _TraxDIFTPPwd;

        static FTP()
        {
            _TraxDIFTPServer = ConfigurationManager.AppSettings["TraxDIFTPServer"].ToString();
            _TraxDIFTPUser = ConfigurationManager.AppSettings["TraxDIFTPUser"].ToString();
            _TraxDIFTPPwd = ConfigurationManager.AppSettings["TraxDIFTPPwd"].ToString();

        }

        public static void Dir()
        {
            try
            {

                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_TraxDIFTPServer);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(_TraxDIFTPUser, _TraxDIFTPPwd);

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                Console.WriteLine(reader.ReadToEnd());

                Console.WriteLine("Directory List Complete, status {0}", response.StatusDescription);
                logger.Trace("Directory List Complete, status {0}", response.StatusDescription);

                reader.Close();
                response.Close();
            }
            catch (Exception e)
            {
                logger.Trace("Error occured {0}", e.Message);
            }

        }
        public static void Upload0(string file, string folder)
        {
            try
            {
                var destServerFile = _TraxDIFTPServer + "/" + file;
                // Get the object used to communicate with the server.
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(destServerFile);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                // This example assumes the FTP site uses anonymous logon.
                request.Credentials = new NetworkCredential(_TraxDIFTPUser, _TraxDIFTPPwd);

                // Copy the contents of the file to the request stream.
                StreamReader sourceStream = new StreamReader(file);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

                response.Close();
            }
            catch (Exception e)
            {
                logger.Trace("Error occured {0}", e.Message);
            }
        }

        public static bool CreateFolder(string folder)
        {
            FtpWebRequest request;
            try
            {
                //request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/", _TraxDIFTPServer, folder))) as FtpWebRequest;
                string uri = string.Format(@"{0}/{1}/", _TraxDIFTPServer, folder);
                request = WebRequest.Create(new Uri(uri)) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = true;
                request.Credentials = new NetworkCredential(_TraxDIFTPUser, _TraxDIFTPPwd);
                request.ConnectionGroupName = "group";
                FtpWebResponse ftpResponse = (FtpWebResponse)request.GetResponse();
                
            }
            catch (Exception e)
            {
                logger.Trace("Error occured {0} while creating folder {1} ", e.Message, folder);
                return false;
            }
            return true;
        }

        public static bool Upload(string fileName, string folderName)
        {

            FtpWebRequest request;
            try
            {
                //string folderName;
                //string fileName;
                string absoluteFileName = Path.GetFileName(fileName);

                //request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", _TraxDIFTPServer, folderName, fileName))) as FtpWebRequest;
                string uri = string.Format(@"{0}/{1}/{2}", _TraxDIFTPServer, folderName, absoluteFileName);
                request = WebRequest.Create(new Uri(uri)) as FtpWebRequest;
                //request = WebRequest.Create(new Uri(string.Format(@"{0}/{1}", _TraxDIFTPServer, absoluteFileName))) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = true;
                request.Credentials = new NetworkCredential(_TraxDIFTPUser, _TraxDIFTPPwd);
                request.ConnectionGroupName = "group";

                using (FileStream fs = File.OpenRead(fileName))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                    requestStream.Flush();
                }

                logger.Trace("FTP done for {0} ", fileName);
               
            }
            catch (Exception e)
            {
                logger.Trace("Error occured {0} while upload {1} ",e.Message, fileName);
                return false;
            }
            return true;
        }
    }
}

