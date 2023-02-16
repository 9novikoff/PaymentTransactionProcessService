using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTransactionProcessService
{
    public class ErrorRepository
    {
        private ErrorRepository _instance;
        private int _errorsCounter = 0;
        private int _parsedLines = 0;
        private int _parsedFiles = 0;
        private List<string> _invalidPathes = new List<string>();

        private ErrorRepository(){}

        public ErrorRepository GetErrorRepository()
        {
            _instance ??= new ErrorRepository();
            return _instance;
        }
        public void AddInvalidPath(string path)
        {
            _invalidPathes.Add(path);
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

        public int GetParsedNumber()
        {
            return _parsedFiles;
        }
    }
}
