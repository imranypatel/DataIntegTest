using System;
namespace specp.DataIntegration
{
    interface IUploader
    {
        bool CreateFolder(string ftpServer, string ftpUser, string ftpPassword, string ftpFolderName);
        string[] Dir(string ftpServer, string ftpUser, string ftpPassword, string ftpFolderName);
        string[] Dir(string ftpServer, string ftpUser, string ftpPassword, string ftpFolderName, string srcFileName);
        bool Upload(string ftpServer, string ftpUser, string ftpPassword, string ftpFolderName, string ftpFileName, string srcFileName);
    }
}
