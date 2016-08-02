using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FileBrowser.Models
{
    public class CurrentDirectory
    {
        public string ParentPath { get; set; }
        public string CurrentPath { get; set; }
        public List<Folder> Folders { get; set; }
        public List<File> Files { get; set; }
        public int CountFileLess10Mb { get; set; }
        public int CountFileBetween10And50Mb { get; set; }
        public int CountFileMore100Md { get; set; }
        public int AccessDenied { get; set; }
    }
}