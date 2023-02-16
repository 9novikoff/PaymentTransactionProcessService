using PaymentTransactionProcessService.PresentationLayer;

namespace PaymentTransactionProcessService
{
    internal class Program
    {
        private static void Main()
        {
            while (true)
            {
                var command = Console.ReadLine();

                var interpreter = new CommandInterpreter();
                interpreter.Interpret(command);
            }
        }
    }
}