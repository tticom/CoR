using System;
using System.IO;

namespace CoR
{
    public static class FileError
    {
        public static void MoveErrorFile(string filepath)
        {
            var start = filepath.LastIndexOf(@"\", StringComparison.Ordinal) + 1;
            var errorFolder = Configuration.ErrorFolder + @"\" + filepath.Substring(start);
            File.Copy(filepath, errorFolder, true);
            File.Delete(filepath);
        }
    }
}