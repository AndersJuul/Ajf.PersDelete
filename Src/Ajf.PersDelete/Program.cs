using Ajf.Nuget.Logging;
using Serilog;
using System;
using System.IO;
using System.Linq;

namespace PersDelete
{
    class Program
    {
        static void Main(string[] args)
        {
            Log.Logger = StandardLoggerConfigurator
                .GetLoggerConfig().MinimumLevel
                .Debug()
                .CreateLogger();

            Log.Logger.Debug("Started...");

            var path = GetArg(args, "-p", "c:\\temp");
            var pattern = GetArg(args, "-m", "*.*");
            var age = Convert.ToInt32(GetArg(args, "-a", "7"));
            var keep = Convert.ToInt32(GetArg(args, "-k", "0"));

            Log.Logger.Debug("Args: ... ",path);
            Log.Logger.Debug("Args: ... ",pattern);
            Log.Logger.Debug("Args: ... ",age);
            Log.Logger.Debug("Args: ... ",keep);


            var threshold = DateTime.Now.AddDays(-age);

            var files = Directory
                .EnumerateFiles(path, pattern)
                .Select(x=> new FileAndDate(x, File.GetCreationTime(x)))
                .OrderBy(fd=>fd.DateTime)
                .ToArray();

            Log.Logger.Debug("Found files: ... ", files.Length);

            while (files.Length>0)
            {
                if (files.Length < keep)
                {
                    Log.Logger.Debug("Less files than required to keep.");
                        return;
                }

                var file = files.First();

                if (file.DateTime< threshold)
                {
                    Log.Logger.Debug("Deleting " + file.FileName);
                    File.Delete(file.FileName);
                }
                else
                {
                    Log.Logger.Debug("NOT Deleting " + file.FileName);
                    Log.Logger.Debug("Oldest file is not old enough to be deleted.");
                    return;
                }
                files = files.Skip(1).ToArray();
            }
        }

        private static string GetArg(string[] args, string arg, string defaultValue)
        {
            var index = Array.IndexOf(args, arg);
            if (index == -1 || index >= args.Length)
                return defaultValue;

            return args[index + 1];
        }
    }
}
