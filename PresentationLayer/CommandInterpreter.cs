using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace PaymentTransactionProcessService.PresentationLayer
{
    public static class CommandInterpreter
    {
        private static EventController _eventController;
        public static void Interpret(string command)
        {
            switch (command.ToLower())
            {
                case "start":
                    Start();
                    break;

                case "stop": 
                    Stop();
                    break;

                case "reset":
                    Reset();
                    break;
                default:
                    WrongCommandInvoke();
                    break;
            }
        }

        private static void Start()
        {
            if (_eventController != null)
            {
                Console.WriteLine("The service has already been started");
            }
            else
            {
                _eventController = new EventController();

                Console.WriteLine("The service has been started");
                Console.WriteLine("Data within one field must be separated with ';' character." +
                                  "\r\nFields must be separated with a ',' character" +
                                  "\r\nOtherwise, the data will be considered invalid.");
            }
        }        
        
        private static void Stop()
        {
            if (_eventController != null)
            {
                _eventController.StopWatching();
                _eventController = null;

                Console.WriteLine("The service is stopped");
            }
            else
            {
                Console.WriteLine("The service has not started working yet");
            }
        }        
        
        private static void Reset()
        {
            if (_eventController != null)
            {
                _eventController.StopWatching();
                _eventController = new EventController();

                Console.WriteLine("The service has been restarted");
            }
            else
            {
                Console.WriteLine("The service has not started working yet");
            }
        }

        private static void WrongCommandInvoke()
        {
            var wrongMessage = "Invalid command, please try again";
            Console.WriteLine(wrongMessage);
        }
    }
}
