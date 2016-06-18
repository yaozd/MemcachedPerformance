using System;
using System.IO;

namespace MemcachedPerformance
{
    public class MyLog
    {
        private string _fileName;
        private readonly string _filePath;

        public MyLog()
            : this("log.txt", "D:\\temp-test")
        {

        }
        public MyLog(string fileName, string filePath)
        {
            this._fileName = fileName;
            this._filePath = filePath;
        }
        public void Write(string content)
        {
            try
            {
                if (!Directory.Exists(_filePath))
                {
                    Directory.CreateDirectory(_filePath);
                }
                if (_fileName.IndexOf("/", StringComparison.Ordinal) != -1)
                {
                    _fileName = _fileName.Replace("/", "-");
                }
                string strLogName = _filePath + "\\" + _fileName;
                var logFile = new FileInfo(strLogName);
                var textFile = logFile.AppendText();
                //如果日志文件大于50M
                if (logFile.Length < 50000000)
                {
                    textFile.WriteLine(content);
                }
                textFile.Close();
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}