using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class Calculator
    {
        public List<double> Results;
        public int Counter = 0;
       
        public Calculator()
        {
            Results = new List<double>();
        }


        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.

            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    break;

                case "s":
                    result = num1 - num2;
                    break;

                case "m":
                    result = num1 * num2;
                    break;

                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0) result = num1 / num2;
                    break;

                case "v":
                    result = (num1 + num2) / 2;
                    break;

                case "w":
                    if (num1 < 0 && num2 % 1 != 0)
                    {
                        Console.WriteLine("Cannot calculate the power of a negative base with a fractional exponent.");
                    }
                    else
                    {
                        result = Math.Pow(num1, num2);
                    }
                    break;

                case "r":
                    if (num1 >= 0)
                    {
                        result = Math.Sqrt(num1);
                    }
                    else
                    {
                        Console.WriteLine("Cannot calculate the square root of a negative number.");
                    }
                    break;

                case "n":
                    result = Math.Sin(num1);
                    break;

                case "c":
                    result = Math.Cos(num1);
                    break;

                case "t":
                    result = Math.Tan(num1);
                    break;
                case "l":
                    if (num1 > 0)
                    {
                        result = Math.Log(num1);
                    }
                    else
                    {
                        Console.WriteLine("Cannot calculate the logarithm of a non-positive number.");
                    }
                    break;

                default:
                    Console.WriteLine("Invalid operation.");
                    break;
            }
            // Log the operation to the JSON file
            LogOperation(num1, num2, op, result);

            Counter++;
            Results.Add(result);
            return result;
        }

        private void LogOperation(double num1, double num2, string op, double result)
        {
            var operation = new
            {
                Operand1 = num1,
                Operand2 = num2,
                Operation = op,
                Result = result
            };

            // Write to the JSON file
            string filePath = "calculatorlog.json";
            List<dynamic> operations;

            if (File.Exists(filePath))
            {
                try
                {
                    string json = File.ReadAllText(filePath);
                    operations = JsonConvert.DeserializeObject<List<dynamic>>(json) ?? new List<dynamic>();
                }
                catch (JsonException)
                {
                    Console.WriteLine("Error reading history: The JSON file is corrupted. Starting with a new history.");
                    operations = new List<dynamic>();
                }
            }
            else
            {
                operations = new List<dynamic>();
            }

            operations.Add(operation);
            File.WriteAllText(filePath, JsonConvert.SerializeObject(operations, Formatting.Indented));
        }
    }
}
