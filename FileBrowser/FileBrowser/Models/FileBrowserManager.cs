using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace FileBrowser.Models
{
    public class FileBrowserManager
    {
        
        private const int TenMegabytes = 10485760;
        private const int FiftyMegabytes = 52428800;
        private const int HundredMegabytes = 104857600;

        private readonly StringCollection _loggerAccessDenied = new StringCollection();
        private readonly StringCollection _loggerDirectoryNotFound = new StringCollection();
        private readonly DirectoryInfo _directoryInfo;
        private readonly CurrentDirectory _currentDirectory = new CurrentDirectory();

        public StringCollection LoggerAccessDenied
        {
            get { return _loggerAccessDenied; }
        }
        public StringCollection LoggerDirectoryNotFound
        {
            get { return _loggerDirectoryNotFound; }
        }

        public FileBrowserManager(string path)
        {
            if (string.IsNullOrEmpty(path) || path =="null")
            {
                _directoryInfo = null;
            }
            else
            {
                _directoryInfo = new DirectoryInfo(@path);        
            }
        }

        public CurrentDirectory GetInformationAboutDirectory(bool isRecursionFind)
        {
            try
            {
                if (_directoryInfo == null)
                {
                    _currentDirectory.Folders = GetLogicalDriversList();
                    _currentDirectory.CurrentPath = "Root";
                }
                else
                {
                    int countFileLess10Mb = 0, countFileBetween10And50Mb = 0, countFileMore100Md = 0;

                    _currentDirectory.CurrentPath = _directoryInfo.FullName;
                    _currentDirectory.ParentPath = _directoryInfo.Parent != null ? _directoryInfo.Parent.FullName : string.Empty;
                    _currentDirectory.Folders = GetFoldersInCurrentDirectory();
                    _currentDirectory.Files = GetFilesInCurrentDirectory();

                    GetCountFileForDefiniteSize(isRecursionFind, ref countFileLess10Mb, ref countFileBetween10And50Mb, ref countFileMore100Md);

                    _currentDirectory.CountFileLess10Mb = countFileLess10Mb;
                    _currentDirectory.CountFileBetween10And50Mb = countFileBetween10And50Mb;
                    _currentDirectory.CountFileMore100Md = countFileMore100Md;

                }
                _currentDirectory.AccessDenied = _loggerAccessDenied.Count;
            }
            catch (Exception)
            {
                throw;
            }
            return _currentDirectory;
        }

        private List<Folder> GetLogicalDriversList()
        {
            var drives = Environment.GetLogicalDrives().ToList();
            return drives.Select(item => new Folder() { Name = item, Path = item }).ToList();
            
        }

        private List<Folder> GetFoldersInCurrentDirectory()
        {
            List<Folder> folderslist = null;
            try
            {
                DirectoryInfo[] directoriesInfo = _directoryInfo.GetDirectories();
                folderslist = directoriesInfo.Select(item => new Folder() { Name = item.Name, Path = item.FullName }).ToList();
            }
            catch (UnauthorizedAccessException e)
            {
                _loggerAccessDenied.Add(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                _loggerDirectoryNotFound.Add(e.Message);
            }
            return folderslist;

        }

        private List<File> GetFilesInCurrentDirectory()
        {
            List<File> filesList = null;
            try
            {
                FileInfo[] filesInfo = _directoryInfo.GetFiles();
                filesList =  filesInfo.Select(item => new File() { Name = item.Name }).ToList();
            }
            catch (UnauthorizedAccessException e)
            {
                _loggerAccessDenied.Add(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                _loggerDirectoryNotFound.Add(e.Message);
            }
            return filesList;
        }

        private void GetCountFileForDefiniteSize(bool isRecursionFind, ref int countFileLess10Mb, ref int countFileBetween10And50Mb, ref int countFileMore100Md)
        {
            if (isRecursionFind)
                FindDirectoryTreeRecursive(_directoryInfo, ref countFileLess10Mb,
                    ref countFileBetween10And50Mb, ref countFileMore100Md);
            else
               FindDirectoryTree(ref countFileLess10Mb, ref countFileBetween10And50Mb, ref countFileMore100Md);
        }

        //for folder and only subfolders
        private void FindDirectoryTree(ref int countFileLess10Mb,
            ref int countFileBetween10And50Mb, ref int countFileMore100Md)
        {
            FileInfo[] files = null;
            DirectoryInfo[] subDirectories = null;
            try
            {
                files = _directoryInfo.GetFiles("*.*");
                foreach (FileInfo file in files)
                {
                    if (file.Length < TenMegabytes)
                        countFileLess10Mb++;
                    if (file.Length > TenMegabytes && file.Length <= FiftyMegabytes)
                        countFileBetween10And50Mb++;
                    if (file.Length > HundredMegabytes)
                        countFileMore100Md++;
                }
                subDirectories = _directoryInfo.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirectories)
                {
                    FileInfo[] subfolderFiles = null;
                    subfolderFiles = dirInfo.GetFiles("*.*");
                    foreach (FileInfo file in subfolderFiles)
                    {
                        if (file.Length < TenMegabytes)
                            countFileLess10Mb++;
                        if (file.Length > TenMegabytes && file.Length <= FiftyMegabytes)
                            countFileBetween10And50Mb++;
                        if (file.Length > HundredMegabytes)
                            countFileMore100Md++;
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                _loggerAccessDenied.Add(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                _loggerDirectoryNotFound.Add(e.Message);
            }
        }

        // for folder and all subfolders
        private void FindDirectoryTreeRecursive(DirectoryInfo root, ref int countFileLess10Mb,
            ref int countFileBetween10And50Mb, ref int countFileMore100Md)
        {
            try
            {
                FileInfo[] files = null;
                DirectoryInfo[] subDirectories = null;
                files = root.GetFiles("*.*");

                foreach (FileInfo file in files)
                {
                    if (file.Length < TenMegabytes)
                        countFileLess10Mb++;
                    if (file.Length > TenMegabytes && file.Length <= FiftyMegabytes)
                        countFileBetween10And50Mb++;
                    if (file.Length > HundredMegabytes)
                        countFileMore100Md++;
                }

                subDirectories = root.GetDirectories();

                foreach (DirectoryInfo dirInfo in subDirectories)
                {
                    // Resursive call for each subdirectory.
                    FindDirectoryTreeRecursive(dirInfo, ref countFileLess10Mb, ref countFileBetween10And50Mb,
                        ref countFileMore100Md);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                _loggerAccessDenied.Add(e.Message);
            }
            catch (DirectoryNotFoundException e)
            {
                _loggerDirectoryNotFound.Add(e.Message);
            }
        }
    }
}