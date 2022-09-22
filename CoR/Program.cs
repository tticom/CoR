using System;

namespace CoR
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Enter q to terminate");
            var fileWatcherObserver = new FileWatcherObserver();
            var fileWatcherSubject = new FileWatcherSubject();
            fileWatcherSubject.Attach(fileWatcherObserver);
            fileWatcherSubject.StartWatching();

            while(Console.Read()!='q');

            fileWatcherSubject.Detach(fileWatcherObserver);
        }
    }
}
