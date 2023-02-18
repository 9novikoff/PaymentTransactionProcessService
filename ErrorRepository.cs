using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTransactionProcessService
{
    public class ErrorRepository
    {
        private static ErrorRepository _instance;
        private int _errorsCounter = 0;
        private int _parsedLines = 0;
        private int _parsedFiles = 0;
        private List<string> _invalidFiles = new List<string>();

        private ErrorRepository(){}

        public static ErrorRepository GetErrorRepository()
        {
            _instance ??= new ErrorRepository();
            return _instance;
        }
        public void AddInvalidFile(string path)
        {
            _invalidFiles.Add(path);
        }

        public void AddError()
        {
            _errorsCounter++;
        }

        public void AddParsedLine()
        {
            _parsedLines++;
        }

        public void AddParsedFile()
        {
            _parsedFiles++;
        }

        public int GetParsedFiles()
        {
            return _parsedFiles;
        }

        public int GetFoundErrors()
        {
            return _errorsCounter;
        }

        public List<string> GetInvalidFiles()
        {
            return _invalidFiles;
        }

        public int GetParsedLines()
        {
            return _parsedLines;
        }

        public void Clear()
        {
            _errorsCounter = 0;
            _parsedLines = 0;
            _parsedFiles = 0;
            _invalidFiles.Clear();
        }
    }
}
