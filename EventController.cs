using System.Configuration;
using PaymentTransactionProcessService.BusinessLayer;

namespace PaymentTransactionProcessService
{
    public class EventController
    {
        private readonly ErrorRepository _repository;
        private readonly FileSystemWatcher _watcher;
        private readonly System.Timers.Timer _timer;
        public EventController()
        {
            _repository = ErrorRepository.GetErrorRepository();
            _watcher = new FileSystemWatcher();
            _timer = new System.Timers.Timer();
            _timer.Enabled = true;
            _timer.Interval = (new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 23, 59, 999) - DateTime.Now).TotalMilliseconds;
            _timer.Elapsed += OnTimerElapsed;

            var path = ConfigurationManager.AppSettings["TransactionsSource"];
            Directory.CreateDirectory(path);

            _watcher.Path = path;
            _watcher.Created += OnCreated;
            _watcher.EnableRaisingEvents = true;
        }

        public void StopWatching()
        {
            _watcher.Created -= OnCreated;
            _watcher.EnableRaisingEvents = false;
        }
        private void OnCreated(object sender, FileSystemEventArgs e)
        {
            var fileExtension = Path.GetExtension(e.FullPath);
            var txtExtension = ".txt";
            var csvExtension = ".csv";

            IFormatReader reader;

            if (fileExtension.Equals(txtExtension))
                reader = new TxtReader();
            else if (fileExtension.Equals(csvExtension))
                reader = new CsvReader();
            else
            {
                _repository.AddInvalidFile(e.Name);
                return;
            }

            var service = new TransactionService(reader);

            service.HandleTransaction(e.FullPath, _repository);
        }

        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var directory = ConfigurationManager.AppSettings["TransactionsResult"] + @"\" +
                            e.SignalTime.ToString("MM-dd-yyyy");
            Directory.CreateDirectory(directory);
            var resPath = directory + @"\meta" + ".log";

            File.AppendAllText(resPath, "parsed_files: " + _repository.GetParsedFiles() + Environment.NewLine);
            File.AppendAllText(resPath, "parsed_lines: " + _repository.GetParsedLines() + Environment.NewLine);
            File.AppendAllText(resPath, "found_errors: " + _repository.GetFoundErrors() + Environment.NewLine);
            File.AppendAllText(resPath, "invalid_files: ");
            File.AppendAllLines(resPath, _repository.GetInvalidFiles());

            _repository.Clear();
        }


    }
}
