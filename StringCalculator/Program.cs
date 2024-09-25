using System;

class Program
{
    static void Main(string[] args)
    {
        var calculator = new StringCalculator();

        // Example test cases
        Console.WriteLine(calculator.Add("1,2"));    // Output: 3
        Console.WriteLine(calculator.Add("4,-3"));   // Output: 1
        Console.WriteLine(calculator.Add("5,tytyt")); // Output: 5
        Console.WriteLine(calculator.Add(""));       // Output: 0
    }
}