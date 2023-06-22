using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DirectoryScanner
{
    public class DirectoryNode
    {
        public string Name { get; }
        public long Size { get => size; }
        public string MimeType { get; }
        public bool IsDirectory { get; }

        public List<DirectoryNode> Children { get; } = new List<DirectoryNode>();

        private string fullName;
        private long size;

        public DirectoryNode(string path)
        {
            IsDirectory = GetIsDirectory(path);
            Name = GetName(path);
            fullName = path;
            MimeType = GetMimeType(path);
        }

        public void ScanDirectory()
        {
            DirectoryInfo currentDirectoryInfo = new DirectoryInfo(fullName);

            DirectoryInfo[] directoryInfos = currentDirectoryInfo.GetDirectories();
            foreach (DirectoryInfo directoryInfo in directoryInfos)
            {
                DirectoryNode newNode = new DirectoryNode(directoryInfo.FullName);
                Children.Add(newNode);
                newNode.ScanDirectory();
            }

            FileInfo[] fileInfos = currentDirectoryInfo.GetFiles();
            foreach (FileInfo fileInfo in fileInfos)
            {
                Children.Add(new DirectoryNode(fileInfo.FullName) { size = fileInfo.Length });
            }

            size = Children.Sum(x => x.Size);
        }

        public void PrintDirectory()
        {
            Console.WriteLine($"{Name} - {Size} - {MimeType}");
            foreach (DirectoryNode childNode in Children)
            {
                childNode.PrintDirectory();
            }
        }

        private bool GetIsDirectory(string path)
        {
            if (File.Exists(path))
            {
                return false;
            }
            else if (Directory.Exists(path))
            {
                return true;
            }
            else
            {
                throw new FileNotFoundException(path);
            }
        }

        private string GetName(string path)
        {
            return IsDirectory ? new DirectoryInfo(path).Name : Path.GetFileName(path);
        }

        private string GetMimeType(string path)
        {
            return IsDirectory ? "directory" : MimeTypes.GetMimeType(path);
        }
    }
}
