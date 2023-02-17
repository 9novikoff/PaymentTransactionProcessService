using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentTransactionProcessService.BusinessLayer;
using PaymentTransactionProcessService.Models;

namespace PaymentTransactionProcessService
{
    public class EventController
    {
        private readonly ErrorRepository _repository;
        private readonly FileSystemWatcher _watcher;
        public EventController()
        {
            _repository = ErrorRepository.GetErrorRepository();
            _watcher = new FileSystemWatcher();

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
            else if (txtExtension.Equals(csvExtension))
                reader = new CsvReader();
            else
            {
                _repository.AddInvalidPath(e.FullPath);
                return;
            }

            var service = new TransactionService(reader);

            service.HandleTransaction(e.FullPath, _repository);
        }


    }
}
