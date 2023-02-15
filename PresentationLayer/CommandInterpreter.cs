using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace PaymentTransactionProcessService.PresentationLayer
{
    public class CommandInterpreter
    {
        private static CommandInterpreter _interpreter;
        private CommandInterpreter(){}

        public static CommandInterpreter GetInstance()
        {
            _interpreter ??= new CommandInterpreter();
            return _interpreter;
        }
        public void Interpret(string command)
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

        private void Start()
        {

        }        
        
        private void Stop()
        {

        }        
        
        private void Reset()
        {

        }

        private void WrongCommandInvoke()
        {
            var wrongMessage = "Invalid command, please try again";
            Console.WriteLine(wrongMessage);
        }
    }
}
