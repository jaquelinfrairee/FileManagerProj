using System;
using System.IO;

namespace FileManagerP3
{
    // Handles all file system operations (separation from GUI)
    public static class FileOperations
    {
        public const string FolderPrefix = "[Folder] ";
        public const string DefaultFileContent = "This is my new file for Project 3.";

        public static void CreateFile(string path)
        {
            File.WriteAllText(path, DefaultFileContent);
        }

        public static string ReadFile(string path)
        {
            return File.ReadAllText(path);
        }

        public static void UpdateFile(string path, string content)
        {
            File.WriteAllText(path, content);
        }

        public static void DeleteItem(string path)
        {
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }
            else if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static void RenameItem(string oldPath, string newPath)
        {
            if (Directory.Exists(oldPath))
            {
                Directory.Move(oldPath, newPath);
            }
            else if (File.Exists(oldPath))
            {
                File.Move(oldPath, newPath);
            }
        }

        public static void CreateFolder(string path)
        {
            Directory.CreateDirectory(path);
        }

        public static string[] GetFiles(string path)
        {
            return Directory.GetFiles(path);
        }

        public static string[] GetFolders(string path)
        {
            return Directory.GetDirectories(path);
        }

        public static FileInfo GetFileInfo(string path)
        {
            return new FileInfo(path);
        }

        public static void CopyItem(string oldPath, string newPath)
        {
            if (File.Exists(oldPath))
            {
                File.Copy(oldPath, newPath, true);
            }
        }

        public static void MoveItem(string oldPath, string newPath)
        {
            if (File.Exists(oldPath))
            {
                File.Move(oldPath, newPath);
            }
        }
    }
}
