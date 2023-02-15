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

                var interpreter = CommandInterpreter.GetInterpreter();
                interpreter.Interpret(command);
            }
        }
    }
}