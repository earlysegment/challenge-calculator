using System;
using System.Linq;
using System.Collections.Generic;

public class StringCalculator
{
    public int Add(string? numbers)
    {
        if (string.IsNullOrWhiteSpace(numbers)) return 0;

        // Parse delimiters and get the updated number string
        string[] delimiters = { ",", "\n" };
        numbers = ParseDelimiters(numbers, ref delimiters);

        // Split and sum the numbers
        var numberArray = SplitNumbers(numbers, delimiters);
        return CalculateSumWithFormula(numberArray);
    }

    // Parse the custom delimiters and update the number string
    private string ParseDelimiters(string numbers, ref string[] delimiters)
    {
        if (numbers.StartsWith("//"))
        {
            int newlineIndex = numbers.IndexOf('\n');
            if (newlineIndex == -1)
            {
                throw new ArgumentException("Input format is incorrect. Missing newline after custom delimiter.");
            }

            if (numbers.StartsWith("//[") && numbers.Contains("]\n"))
            {
                // Handle multiple custom delimiters
                string delimiterSection = numbers.Substring(2, newlineIndex - 2);
                delimiters = ParseMultipleDelimiters(delimiterSection);
                return numbers.Substring(newlineIndex + 1); // Remove delimiter part
            }
            else
            {
                // Handle single custom delimiter
                var delimiterSection = numbers.Substring(2, newlineIndex - 2);
                delimiters = new[] { delimiterSection };
                return numbers.Substring(newlineIndex + 1); // Remove delimiter part
            }
        }
        return numbers;
    }

    // Parse multiple custom delimiters enclosed in []
    private string[] ParseMultipleDelimiters(string delimiterSection)
    {
        var delimiterList = new List<string>();
        var matches = System.Text.RegularExpressions.Regex.Matches(delimiterSection, @"\[(.*?)\]");

        foreach (System.Text.RegularExpressions.Match match in matches)
        {
            delimiterList.Add(match.Groups[1].Value);
        }

        return delimiterList.ToArray();
    }

    // Split the numbers based on the delimiters
    private string[] SplitNumbers(string numbers, string[] delimiters)
    {
        return numbers.Split(delimiters, StringSplitOptions.None);
    }

    // Calculate the sum of the numbers, display the formula, and handle negatives and large numbers
    private int CalculateSumWithFormula(string[] numberArray)
    {
        var negativeNumbers = new List<int>();
        var formulaParts = new List<string>();  // To store each part of the formula
        int sum = 0;

        foreach (var num in numberArray)
        {
            if (int.TryParse(num, out int result))
            {
                if (result < 0)
                {
                    negativeNumbers.Add(result);
                    formulaParts.Add($"({result})");  // Negative numbers in parentheses
                }
                else if (result > 1000)
                {
                    formulaParts.Add("0");  // Numbers greater than 1000 treated as 0
                }
                else
                {
                    formulaParts.Add(result.ToString());
                    sum += result;
                }
            }
            else
            {
                formulaParts.Add("0");  // Invalid numbers treated as 0
            }
        }

        // If there are negative numbers, throw an exception
        if (negativeNumbers.Any())
        {
            throw new ArgumentException($"Negatives not allowed: {string.Join(", ", negativeNumbers)}");
        }

        // Display the formula used for the calculation
        Console.WriteLine($"Formula: {string.Join(" + ", formulaParts)} = {sum}");

        return sum;
    }
}