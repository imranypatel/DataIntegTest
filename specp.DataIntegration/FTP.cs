﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Configuration;
using NLog;

namespace specp.DataIntegration
{
    public class FTP : specp.DataIntegration.IUploader
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //static string _TraxDIFTPServer;
        //static string _TraxDIFTPUser;
        //static string _TraxDIFTPPwd;

        public FTP()
        {
            //_TraxDIFTPServer = ConfigurationManager.AppSettings["TraxDIFTPServer"].ToString();
            //_TraxDIFTPUser = ConfigurationManager.AppSettings["TraxDIFTPUser"].ToString();
            //_TraxDIFTPPwd = ConfigurationManager.AppSettings["TraxDIFTPPwd"].ToString();

        }

       

         FtpWebRequest CreateRequest(string ftpServer, string ftpUser, string ftpPassword, string ftpFolderName, string ftpFileName)
        {

            FtpWebRequest request;
            //string ftpFileName;
            string uri = "";
            Uri objUri;

            try
            {
                //string folderName;
                //string fileName;
                //ftpFileName = Path.GetFileName(srcFileName);
                if(ftpFileName==null || ftpFileName=="")
                    uri = string.Format(@"{0}/{1}", ftpServer, ftpFolderName);
                else
                    uri = string.Format(@"{0}/{1}/{2}", ftpServer, ftpFolderName, ftpFileName);

                objUri = new Uri(uri);

                //request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", _TraxDIFTPServer, folderName, fileName))) as FtpWebRequest;
                request = WebRequest.Create(objUri) as FtpWebRequest;
                //request = WebRequest.Create(new Uri(string.Format(@"{0}/{1}", _TraxDIFTPServer, absoluteFileName))) as FtpWebRequest;
                //request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = true;
                request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                request.ConnectionGroupName = "group";

                logger.Trace("FTP request created {0} ", uri);

            }
            catch (Exception e)
            {
                logger.Trace("Error occured {0} while creating FTP Request for {1} ", e.Message, uri);
                return null;
            }
            return request;
        }

        //public static void Dir()
        //{
        //    try
        //    {

        //        // Get the object used to communicate with the server.
        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(_TraxDIFTPServer);
        //        request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

        //        // This example assumes the FTP site uses anonymous logon.
        //        request.Credentials = new NetworkCredential(_TraxDIFTPUser, _TraxDIFTPPwd);

        //        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        //        Stream responseStream = response.GetResponseStream();
        //        StreamReader reader = new StreamReader(responseStream);
        //        Console.WriteLine(reader.ReadToEnd());

        //        Console.WriteLine("Directory List Complete, status {0}", response.StatusDescription);
        //        logger.Trace("Directory List Complete, status {0}", response.StatusDescription);

        //        reader.Close();
        //        response.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Trace("Error occured {0}", e.Message);
        //    }

        //}

        public  string[] Dir(string ftpServer, string ftpUser, string ftpPassword, string ftpFolderName)
        {
            FtpWebRequest request;
            try
            {

                request = CreateRequest(ftpServer, ftpUser, ftpPassword, ftpFolderName,null);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;

               
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                var resp = reader.ReadToEnd();
                Console.WriteLine(resp);

                Console.WriteLine("Directory List Complete, status {0}", response.StatusDescription);
                logger.Trace("Directory List Complete, status {0}", response.StatusDescription);

                reader.Close();
                response.Close();

                // list of file names
                return GetFileNames(resp);
            }
            catch (Exception e)
            {
                logger.Trace("Error occured {0}", e.Message);
                return null;
            }

        }

        public  string[] Dir(string ftpServer, string ftpUser, string ftpPassword, string ftpFolderName, string srcFileName)
        {
            FtpWebRequest request;
            try
            {

                request = CreateRequest(ftpServer, ftpUser, ftpPassword, ftpFolderName, srcFileName);
                request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;


                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);

                var resp = reader.ReadToEnd();
                Console.WriteLine(resp);

                Console.WriteLine("Directory List Complete, status {0}", response.StatusDescription);
                logger.Trace("Directory List Complete, status {0}", response.StatusDescription);

                reader.Close();
                response.Close();

                // list of file names
                return GetFileNames(resp);
            }
            catch (Exception e)
            {
                logger.Trace("Error occured {0}", e.Message);
                return null;
            }

        }

        //public static bool CreateFolder0(string ftpServer, string ftpUser, string ftpPassword, string ftpFolderName)
        //{
        //    FtpWebRequest request;
        //    try
        //    {
        //        //request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/", _TraxDIFTPServer, folder))) as FtpWebRequest;
        //        string uri = string.Format(@"{0}/{1}/", _TraxDIFTPServer, folder);
        //        request = WebRequest.Create(new Uri(uri)) as FtpWebRequest;
        //        request.Method = WebRequestMethods.Ftp.MakeDirectory;
        //        request.UseBinary = true;
        //        request.UsePassive = true;
        //        request.KeepAlive = true;
        //        request.Credentials = new NetworkCredential(_TraxDIFTPUser, _TraxDIFTPPwd);
        //        request.ConnectionGroupName = "group";
        //        FtpWebResponse ftpResponse = (FtpWebResponse)request.GetResponse();

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Trace("Error occured {0} while creating folder {1} ", e.Message, folder);
        //        return false;
        //    }
        //    return true;
        //}
        public  bool CreateFolder(string ftpServer, string ftpUser, string ftpPassword, string ftpFolderName)
        {
            FtpWebRequest request;
            try
            {
               
                request = CreateRequest(ftpServer, ftpUser, ftpPassword, ftpFolderName, null);
                request.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse ftpResponse = (FtpWebResponse)request.GetResponse();

            }
            catch (Exception e)
            {
                logger.Trace("Error occured {0} while creating folder {1} ", e.Message, ftpFolderName);
                return false;
            }
            return true;
        }

        //public static void Upload0(string file, string folder)
        //{
        //    try
        //    {
        //        var destServerFile = _TraxDIFTPServer + "/" + file;
        //        // Get the object used to communicate with the server.
        //        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(destServerFile);
        //        request.Method = WebRequestMethods.Ftp.UploadFile;

        //        // This example assumes the FTP site uses anonymous logon.
        //        request.Credentials = new NetworkCredential(_TraxDIFTPUser, _TraxDIFTPPwd);

        //        // Copy the contents of the file to the request stream.
        //        StreamReader sourceStream = new StreamReader(file);
        //        byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
        //        sourceStream.Close();
        //        request.ContentLength = fileContents.Length;

        //        Stream requestStream = request.GetRequestStream();
        //        requestStream.Write(fileContents, 0, fileContents.Length);
        //        requestStream.Close();

        //        FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        //        Console.WriteLine("Upload File Complete, status {0}", response.StatusDescription);

        //        response.Close();
        //    }
        //    catch (Exception e)
        //    {
        //        logger.Trace("Error occured {0}", e.Message);
        //    }
        //}


        //public static bool Upload1(string fileName, string folderName)
        //{

        //    FtpWebRequest request;
        //    try
        //    {
        //        //string folderName;
        //        //string fileName;
        //        string absoluteFileName = Path.GetFileName(fileName);

        //        //request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", _TraxDIFTPServer, folderName, fileName))) as FtpWebRequest;
        //        string uri = string.Format(@"{0}/{1}/{2}", _TraxDIFTPServer, folderName, absoluteFileName);
        //        request = WebRequest.Create(new Uri(uri)) as FtpWebRequest;
        //        //request = WebRequest.Create(new Uri(string.Format(@"{0}/{1}", _TraxDIFTPServer, absoluteFileName))) as FtpWebRequest;
        //        request.Method = WebRequestMethods.Ftp.UploadFile;
        //        request.UseBinary = true;
        //        request.UsePassive = true;
        //        request.KeepAlive = true;
        //        request.Credentials = new NetworkCredential(_TraxDIFTPUser, _TraxDIFTPPwd);
        //        request.ConnectionGroupName = "group";

        //        using (FileStream fs = File.OpenRead(fileName))
        //        {
        //            byte[] buffer = new byte[fs.Length];
        //            fs.Read(buffer, 0, buffer.Length);
        //            fs.Close();
        //            Stream requestStream = request.GetRequestStream();
        //            requestStream.Write(buffer, 0, buffer.Length);
        //            requestStream.Close();
        //            requestStream.Flush();
        //        }

        //        logger.Trace("FTP done for {0} ", fileName);

        //    }
        //    catch (Exception e)
        //    {
        //        logger.Trace("Error occured {0} while upload {1} ",e.Message, fileName);
        //        return false;
        //    }
        //    return true;
        //}

        public  bool Upload(string ftpServer, string ftpUser, string ftpPassword, string ftpFolderName, string ftpFileName, string srcFileName)
        {

            FtpWebRequest request;
            try
            {
                var tmpFtpFileName = ETLUtil.GetTempFileName(srcFileName);
                request = CreateRequest(ftpServer, ftpUser, ftpPassword, ftpFolderName, tmpFtpFileName);
                //request = WebRequest.Create(new Uri(string.Format(@"{0}/{1}", _TraxDIFTPServer, absoluteFileName))) as FtpWebRequest;
                
                // Upload
                request.Method = WebRequestMethods.Ftp.UploadFile;
                using (FileStream fs = File.OpenRead(srcFileName))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Close();
                    requestStream.Flush();
                }

                // rename
                request = CreateRequest(ftpServer, ftpUser, ftpPassword, ftpFolderName, tmpFtpFileName);
                request.Method = WebRequestMethods.Ftp.Rename;
                request.RenameTo = ftpFileName;
                request.GetResponse();

                logger.Trace("FTP upload done for {0} ", srcFileName);

            }
            catch (Exception e)
            {
                logger.Trace("Error occured {0} while uploading {1} ", e.Message, srcFileName);
                return false;
            }
            return true;
        }

        private static string[] GetFileNames(string ftpResp)
        {
            var lst = ftpResp.Split(new string[] { "\n", "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            return lst;
        }

    }
}

