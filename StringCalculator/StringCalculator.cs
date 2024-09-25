using System;
using System.Linq;
using System.Collections.Generic;

public class StringCalculator
{
    public int Add(string? numbers)
    {
        // Handle null or empty input
        if (string.IsNullOrWhiteSpace(numbers)) return 0;

        // Define the delimiters: comma and newline
        var delimiters = new[] { ',', '\n' };

        // Split the numbers by both comma and newline delimiters
        var numberArray = numbers.Split(delimiters, StringSplitOptions.None);

        // List to track negative numbers
        var negativeNumbers = new List<int>();

        // Sum the valid numbers and collect negative numbers
        int sum = 0;
        foreach (var num in numberArray)
        {
            if (int.TryParse(num, out int result))
            {
                if (result < 0)
                {
                    // Collect negative numbers
                    negativeNumbers.Add(result);
                }
                else if (result <= 1000)
                {
                    // Sum numbers less than or equal to 1000
                    sum += result;
                }
            }
        }

        // If there are negative numbers, throw an exception
        if (negativeNumbers.Any())
        {
            throw new ArgumentException($"Negatives not allowed: {string.Join(", ", negativeNumbers)}");
        }

        return sum;
    }
}