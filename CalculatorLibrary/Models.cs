﻿// CalculatorLibrary/Models.cs
namespace CalculatorLibrary
{
    public class OperationInfo
    {
        public string? Operation { get; set; }
        public double Operand1 { get; set; }
        public double Operand2 { get; set; }
        public double Result { get; set; }
    }

    public class CalculatorLog
    {
        public List<OperationInfo>? Operations { get; set; }
    }
}
