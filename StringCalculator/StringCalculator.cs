using System;
using System.Linq;

public class StringCalculator
{
    public int Add(string numbers)
    {
        if (string.IsNullOrEmpty(numbers)) return 0;

        // Split the numbers by comma
        var numberArray = numbers.Split(',');

        // Check if there are more than 2 numbers
        if (numberArray.Length > 2)
        {
            throw new ArgumentException("Only two numbers are allowed.");
        }

        // Convert each valid number, invalid numbers are treated as 0
        return numberArray.Sum(x => int.TryParse(x, out int result) ? result : 0);
    }
}