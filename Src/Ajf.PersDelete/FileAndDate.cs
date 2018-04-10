using System;

namespace PersDelete
{
    internal class FileAndDate
    {
        private string fileName;
        private DateTime dateTime;

        public FileAndDate(string x, DateTime dateTime)
        {
            FileName = x;
            DateTime = dateTime;
        }

        public string FileName { get => fileName; set => fileName = value; }
        public DateTime DateTime { get => dateTime; set => dateTime = value; }
    }
}