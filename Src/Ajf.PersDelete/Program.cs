using System;
using System.IO;

namespace PersDelete
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = GetArg(args, "-p", "c:\\temp");
            var pattern = GetArg(args, "-m", "*.*");
            var age = Convert.ToInt32(GetArg(args, "-a", "7"));

            var threshold = DateTime.Now.AddDays(-age);

            var files = Directory.EnumerateFiles(path, pattern);

            foreach (var file in files)
            {
                var creationTIme = File.GetCreationTime(file);

                if (creationTIme < threshold)
                {
                    Console.WriteLine("Deleting " + file);
                    File.Delete(file);
                }
                else
                {
                    Console.WriteLine("NOT Deleting " + file);
                }
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
