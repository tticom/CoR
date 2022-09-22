using System;
using System.Collections.Generic;
using System.IO;
using CoR.Input;
using CoR.Output;

namespace CoR
{
    public interface IObserver
    {
        void Update(string filepath);
    }

    public class FileWatcherObserver : IObserver
    {
        public ReferenceData ReferenceData { get; set; }

        public FileWatcherObserver()
        {
            ReferenceData = new ReferenceDataRepository().ReferenceData;
        }

        public void Update(string filepath)
        {
            Console.WriteLine(filepath);
            var generationReportRespository = new GenerationReportRepository(filepath);
            var generation = new Generation(ReferenceData, generationReportRespository.GeneratorReportData);
            generation.Calculate();
            var writer = new Writer(generation.GenerationData, filepath);
            writer.WriteToFile();
        }
    }

    public abstract class Subject
    {
        internal string _fileToProcess;
        private readonly List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var o in _observers) o.Update(_fileToProcess);
        }
    }

    public class FileWatcherSubject : Subject
    {
        public string FileToProcess
        {
            set
            {
                _fileToProcess = value;
                Notify();
            }
        }

        public void StartWatching()
        {
            var watcher =
                new FileSystemWatcher(Configuration.InputFolder)
                {
                    NotifyFilter =  NotifyFilters.LastWrite, // | NotifyFilters.LastAccess | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    Filter = "*.xml",
                    EnableRaisingEvents = true
                };

            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            try
            {
                Console.WriteLine("File: " + e.FullPath + " " + e.ChangeType);

                if (!File.Exists(e.FullPath))
                    throw new FileNotFoundException($"{e.Name} has been moved from {e.FullPath.Substring(0, e.FullPath.Length - e.Name.Length)}");

                var targetPath = e.FullPath.Substring(0, e.FullPath.Length - ".xml".Length) + TimeBasedName() + ".xml";
                targetPath = targetPath.Replace(Configuration.InputFolder, Configuration.ProcessingFolder);
                if (!Directory.Exists(Configuration.ProcessingFolder))
                    Directory.CreateDirectory(Configuration.ProcessingFolder);

                File.Copy(e.FullPath, targetPath, true);
                File.Delete(e.FullPath);
                FileToProcess = targetPath;
            }
            catch (FileNotFoundException fnfe)
            {
                Console.WriteLine(fnfe.Message + " " + fnfe.FileName);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                FileError.MoveErrorFile(_fileToProcess);
            }
        }

        private string TimeBasedName()
        {
            return DateTime.Now.Year.ToString() + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + DateTime.Now.Millisecond;
        }
    }
}
