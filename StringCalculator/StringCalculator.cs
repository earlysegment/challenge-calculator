using System;
using System.Linq;
using System.Collections.Generic;

public class StringCalculator
{
    public int Add(string? numbers)
    {
        // Handle null or empty input
        if (string.IsNullOrWhiteSpace(numbers)) return 0;

        string[] delimiters = { ",", "\n" }; // Default delimiters

        // Check if a custom delimiter is provided
        if (numbers.StartsWith("//"))
        {
            int newlineIndex = numbers.IndexOf('\n');
            if (newlineIndex == -1)
            {
                throw new ArgumentException("Input format is incorrect. Missing newline after custom delimiter.");
            }

            // Extract the custom delimiter between the // and \n
            var delimiterSection = numbers.Substring(2, newlineIndex - 2);
            delimiters = new[] { delimiterSection };
            numbers = numbers.Substring(newlineIndex + 1); // Remove the delimiter part
        }

        // Split the numbers using the delimiters
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