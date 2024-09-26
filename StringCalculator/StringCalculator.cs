using System;
using System.Linq;
using System.Collections.Generic;

public class StringCalculator
{
    private readonly bool _denyNegativeNumbers;
    private readonly int _upperBound;
    private string[] _delimiters;

    // Constructor with command-line argument options
    public StringCalculator(bool denyNegativeNumbers = true, int upperBound = 1000, string[]? customDelimiters = null)
    {
        _denyNegativeNumbers = denyNegativeNumbers;
        _upperBound = upperBound;
        _delimiters = customDelimiters ?? new[] { ",", "\n" };
    }

    public int Add(string? numbers)
    {
        if (string.IsNullOrWhiteSpace(numbers)) return 0;

        // Parse delimiters and get the updated number string
        numbers = ParseDelimiters(numbers, ref _delimiters);

        // Split and sum the numbers
        var numberArray = SplitNumbers(numbers, _delimiters);
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
                    if (_denyNegativeNumbers)
                    {
                        negativeNumbers.Add(result);
                        formulaParts.Add($"({result})");  // Negative numbers in parentheses
                    }
                    else
                    {
                        formulaParts.Add(result.ToString());
                        sum += result;
                    }
                }
                else if (result > _upperBound)
                {
                    formulaParts.Add("0");  // Numbers greater than upper bound treated as 0
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

        // If there are negative numbers and they are denied, throw an exception
        if (_denyNegativeNumbers && negativeNumbers.Any())
        {
            throw new ArgumentException($"Negatives not allowed: {string.Join(", ", negativeNumbers)}");
        }

        // Display the formula used for the calculation
        Console.WriteLine($"Formula: {string.Join(" + ", formulaParts)} = {sum}");

        return sum;
    }
}