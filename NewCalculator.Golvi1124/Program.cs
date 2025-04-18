/* 
 To improve:
   * make code nicer/cleaner
   * extra coments for more difficult parts
   * get history direcly from the Json file
    
    Improvements:
    * Updated Regex
    * Changed that menu is at the begining of the program and only then numbers are asked for. 
    * Handled big/small letters for input
    * Changed n to Q for ending the program
    * Changed a bit Json structure how it is written and read......NOOOOT
    * Handled better getting previous results
    * Handled possible null reference exceptions
 */

using System.Text.RegularExpressions;
using CalculatorLibrary;
using Newtonsoft.Json;

namespace CalculatorProgram
{
    class Program
    {
        static void Main()
        {
            bool endApp = false;
            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine("-----------------------------------------\n");

            Calculator calculator = new Calculator();

            while (!endApp)
            {
              
                Console.WriteLine($"This calculator was used {calculator.Counter} times.");

                Console.WriteLine(@"
Choose an operator from the following list:

Standard Operations:
    A - Add
    S - Subtract
    M - Multiply
    D - Divide
    V - Average
Advanced Operations:
    W - Power
    R - Square Root
    N - Sine
    C - Cosine
    T - Tangent
    L - Logarithm
Other Operations:
    H - History
-----------------------------------------
");


                Console.Write("Your option? ");
                string? opInput = Console.ReadLine()?.Trim().ToLower();

                if (opInput == "h")
                {
                    ShowHistory();
                    continue;
                }

                if (string.IsNullOrEmpty(opInput) || !Regex.IsMatch(opInput, "^[asmvdwrnctlh]$"))
                {
                    Console.WriteLine("Error: Unrecognized input. Press any key to try again.");
                    Console.ReadKey();
                    continue;
                }

                double cleanNum1 = 0;
                double cleanNum2 = 0;

                // Only need one number for these operations
                bool singleInputOp = new[] { "r", "n", "c", "t", "l" }.Contains(opInput);

                var firstPrompt = calculator.Results.Count() == 0
                    ? "Type the number for the operation and press Enter: "
                    : "Type a number or 'p' to use a previous result, then press Enter: ";

                var validationError = calculator.Results.Count() == 0
                    ? "This is not valid input. Please enter a number: "
                    : "Invalid input. Please enter a number or 'p': ";

                Console.Write(firstPrompt);
                string? numInput1 = Console.ReadLine();

                while (numInput1?.ToLower() != "p" && !double.TryParse(numInput1, out cleanNum1))
                {
                    Console.Write(validationError);
                    numInput1 = Console.ReadLine();
                }

                if (numInput1?.ToLower() == "p")
                {
                    cleanNum1 = GetPreviousResult(calculator.Results);
                }

                if (!singleInputOp)
                {
                    Console.Write("Type another number for the operation and press Enter: ");
                    string? numInput2 = Console.ReadLine();

                    while (numInput2?.ToLower() != "p" && !double.TryParse(numInput2, out cleanNum2))
                    {
                        Console.Write(validationError);
                        numInput2 = Console.ReadLine();
                    }

                    if (numInput2?.ToLower() == "p")
                    {
                        cleanNum2 = GetPreviousResult(calculator.Results);
                    }
                }

                try
                {
                    double result = calculator.DoOperation(cleanNum1, cleanNum2, opInput);
                    if (double.IsNaN(result))
                    {
                        Console.WriteLine("This operation resulted in a mathematical error.");
                    }
                    else
                    {
                        Console.WriteLine($"Your result: {result:0.##}\n");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("An exception occurred: " + e.Message);
                }

                Console.WriteLine("------------------------\n");
                if (Console.ReadLine()?.Trim().ToLower() == "q") endApp = true;
            }
        }


        private static double GetPreviousResult(List<double> previousResults)
        {
            if (previousResults == null || !previousResults.Any())
            {
                Console.WriteLine("No previous results available.");
                return 0; // Default value
            }

            Console.WriteLine("Type the index of the previous result:");
            for (int index = 1; index <= previousResults.Count; index++)
            {
                Console.WriteLine($"{index}: {previousResults[index - 1]}");
            }

            while (true)
            {
                string? userChoice = Console.ReadLine();
                if (int.TryParse(userChoice, out int index) && index > 0 && index <= previousResults.Count)
                {
                    return previousResults[index - 1];
                }
                Console.WriteLine("Invalid index. Please try again.");
            }
        }


        private static void ShowHistory()
        {
            try
            {
                string json = File.ReadAllText("calculatorlog.json");
                var data = JsonConvert.DeserializeObject<CalculatorLog>(json);

                if (data?.Operations != null && data.Operations.Any())
                {
                    Console.WriteLine("\n--- Operation History ---");
                    foreach (var op in data.Operations)
                    {
                        Console.WriteLine($"{op.Operation} | {op.Operand1} & {op.Operand2} => {op.Result}");
                    }
                    Console.WriteLine("-------------------------\n");
                }
                else
                {
                    Console.WriteLine("No operations found in history.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading history: " + ex.Message);
            }
        }

        // Define a class to map the JSON structure
        public class CalculatorLog
        {
            public List<OperationInfo>? Operations { get; set; }
        }

        public class OperationInfo
        {
            public string? Operation { get; set; }
            public double Operand1 { get; set; }
            public double Operand2 { get; set; }
            public double Result { get; set; }
        }

    }
}