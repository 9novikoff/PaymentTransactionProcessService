using PaymentTransactionProcessService.PresentationLayer;

namespace PaymentTransactionProcessService
{
    internal class Program
    {
        private static void Main()
        {
            Console.WriteLine("Available commands: start, reset, stop");
            while (true)
            {
                var command = Console.ReadLine();

                CommandInterpreter.Interpret(command);
            }
        }
    }
}