using System.Formats.Asn1;
using System.Xml;
using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class Calculator
    {
        public List<double> Results;
        public int Counter = 0;
        JsonWriter writer;
        public Calculator()
        {
            Results = new List<double>();
            StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true; //every time the write method is called, the text will be immediately written into the file, instead of waiting for the writer to be closed
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Newtonsoft.Json.Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }


        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");


            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    break;
                case "s":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    break;
                case "m":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    writer.WriteValue("Divide");
                    break;
                case "sqrt":
                    if (num1 >= 0)
                    {
                        result = Math.Sqrt(num1);
                        writer.WriteValue("SquareRoot");
                    }
                    else
                    {
                        Console.WriteLine("Cannot calculate the square root of a negative number.");
                    }
                    break;
                case "pow":
                    result = Math.Pow(num1, num2);
                    writer.WriteValue("Power");
                    break;
                case "sin":
                    result = Math.Sin(num1);
                    writer.WriteValue("Sin");
                    break;
                case "cos":
                    result = Math.Cos(num1);
                    writer.WriteValue("Cos");
                    break;
                case "tan":
                    result = Math.Tan(num1);
                    writer.WriteValue("Tan");
                    break;
                // Return text for an incorrect option entry.
                default:
                    Console.WriteLine("Invalid operation.");
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();

            Counter++;
            Results.Add(result);
            return result;
        }


        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
}
