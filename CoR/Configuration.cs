
using System.Configuration;

namespace CoR
{
    public static class Configuration
    {
        public static string InputFolder => ConfigurationManager.AppSettings["InputFolder"];
        public static string OutputFolder => ConfigurationManager.AppSettings["OutputFolder"];
        public static string ProcessingFolder => ConfigurationManager.AppSettings["ProcessingFolder"];
        public static string ProcessedFolder => ConfigurationManager.AppSettings["ProcessedFolder"];
        public static string ErrorFolder => ConfigurationManager.AppSettings["ErrorFolder"];
        public static string ReferenceDataFile => ConfigurationManager.AppSettings["ReferenceDataFile"];
        public static string OutputFilename => ConfigurationManager.AppSettings["OutputFilename"];
    }
}