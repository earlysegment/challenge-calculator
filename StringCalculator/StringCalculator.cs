using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class StringCalculator
{
    private readonly bool _denyNegativeNumbers;
    private readonly int _upperBound;
    private readonly string[] _initialDelimiters;
    private string[] _delimiters;

    // Constructor with IStringCalculatorSettings
    public StringCalculator(IStringCalculatorSettings settings)
    {
        _denyNegativeNumbers = settings.DenyNegativeNumbers;
        _upperBound = settings.UpperBound;
        _initialDelimiters = settings.CustomDelimiters ?? new[] { ",", "\n" };
        _delimiters = _initialDelimiters;
    }

    public int Add(string? input)
    {
        // Reset delimiters to initial state for each call
        _delimiters = _initialDelimiters;

        if (string.IsNullOrWhiteSpace(input)) return 0;

        // Replace literal "\n" with actual newline character
        input = input.Replace(@"\n", "\n");

        // Parse delimiters and get the updated input string
        input = ParseDelimiters(input, ref _delimiters);

        // Replace delimiters with '+'
        foreach (var delimiter in _delimiters)
        {
            var escapedDelimiter = Regex.Escape(delimiter);
            input = Regex.Replace(input, escapedDelimiter, "+");
        }

        // Collect negative numbers from input tokens
        var negativeNumbers = new List<int>();

        // Tokenize the input string
        var tokens = Tokenize(input, negativeNumbers);

        if (_denyNegativeNumbers && negativeNumbers.Any())
        {
            var uniqueNegatives = negativeNumbers.Distinct();
            throw new ArgumentException($"Negatives not allowed: {string.Join(", ", uniqueNegatives)}");
        }

        // Evaluate the expression
        int result = EvaluateExpression(tokens);

        return result;
    }

    // Parse the custom delimiters and update the input string
    private string ParseDelimiters(string input, ref string[] delimiters)
    {
        if (input.StartsWith("//"))
        {
            int newlineIndex = input.IndexOf('\n');
            if (newlineIndex == -1)
            {
                throw new ArgumentException("Input format is incorrect. Expected newline after custom delimiter declaration.");
            }

            var delimiterSection = input.Substring(2, newlineIndex - 2);

            if (string.IsNullOrEmpty(delimiterSection))
            {
                throw new ArgumentException("Custom delimiter cannot be empty.");
            }

            // Check for mismatched brackets
            if ((delimiterSection.StartsWith("[") && !delimiterSection.EndsWith("]")) ||
                (!delimiterSection.StartsWith("[") && delimiterSection.EndsWith("]")))
            {
                throw new ArgumentException("Custom delimiter format is incorrect. Mismatched brackets.");
            }

            if (delimiterSection.StartsWith("[") && delimiterSection.EndsWith("]"))
            {
                // Handle multiple or single custom delimiters of any length
                delimiters = ParseMultipleDelimiters(delimiterSection);
            }
            else
            {
                // Ensure single-character delimiter does not contain brackets
                if (delimiterSection.Contains("[") || delimiterSection.Contains("]"))
                {
                    throw new ArgumentException("Custom delimiter format is incorrect.");
                }
                // Handle single-character custom delimiter
                delimiters = new[] { delimiterSection };
            }

            return input.Substring(newlineIndex + 1); // Remove delimiter part
        }
        return input;
    }

    // Parse multiple custom delimiters enclosed in []
    private string[] ParseMultipleDelimiters(string delimiterSection)
    {
        var delimiterList = new List<string>();
        var matches = Regex.Matches(delimiterSection, @"\[(.*?)\]");

        if (matches.Count == 0)
        {
            throw new ArgumentException("Custom delimiter format is incorrect. Expected delimiters enclosed in square brackets.");
        }

        foreach (Match match in matches)
        {
            var delimiter = match.Groups[1].Value;
            if (string.IsNullOrEmpty(delimiter))
            {
                throw new ArgumentException("Custom delimiter cannot be empty.");
            }
            delimiterList.Add(delimiter);
        }

        return delimiterList.ToArray();
    }

    // Define operator precedence
    private static readonly Dictionary<string, int> precedence = new Dictionary<string, int>
    {
        { "+", 1 },
        { "-", 1 },
        { "*", 2 },
        { "/", 2 }
    };

    // Check if operator is left-associative
    private bool IsLeftAssociative(string op)
    {
        // All operators in this calculator are left-associative
        return true;
    }

    // Tokenize the input string into numbers and operators
    private List<string> Tokenize(string input, List<int> negativeNumbers)
    {
        var tokens = new List<string>();
        bool expectOperator = false;
        var matches = Regex.Matches(input, @"(\d+)|([\+\-\*/\(\)])|(\S+)");

        foreach (Match match in matches)
        {
            if (match.Groups[1].Success) // Number
            {
                int num = int.Parse(match.Groups[1].Value);

                // Apply unary minus if previous token was '_'
                if (tokens.Count > 0 && tokens.Last() == "_")
                {
                    tokens.RemoveAt(tokens.Count - 1); // Remove the placeholder '_'
                    num = -num;
                }

                if (_denyNegativeNumbers && num < 0)
                {
                    negativeNumbers.Add(num);
                }

                // Replace numbers exceeding the upper bound with zero
                if (num > _upperBound)
                {
                    num = 0;
                }

                tokens.Add(num.ToString());
                expectOperator = true;
            }
            else if (match.Groups[2].Success) // Operator or Parenthesis
            {
                var token = match.Groups[2].Value;

                if (token == "(")
                {
                    tokens.Add(token);
                    expectOperator = false;
                }
                else if (token == ")")
                {
                    tokens.Add(token);
                    expectOperator = true;
                }
                else if (token == "-")
                {
                    if (expectOperator)
                    {
                        tokens.Add(token);
                        expectOperator = false;
                    }
                    else
                    {
                        // Unary minus
                        tokens.Add("_"); // Placeholder for unary minus
                    }
                }
                else if (token == "+")
                {
                    if (!expectOperator)
                    {
                        // Unary plus, ignore it
                        continue;
                    }
                    tokens.Add(token);
                    expectOperator = false;
                }
                else
                {
                    if (!expectOperator)
                    {
                        throw new ArgumentException("Invalid expression.");
                    }
                    tokens.Add(token);
                    expectOperator = false;
                }
            }
            else if (match.Groups[3].Success) // Invalid token
            {
                // Treat invalid tokens as zero
                tokens.Add("0");
                expectOperator = true;
            }
        }

        // If the expression ends with an operator, append zero
        if (tokens.Count > 0 && precedence.ContainsKey(tokens.Last()))
        {
            tokens.Add("0");
        }

        return tokens;
    }

    // Evaluate the expression using the Shunting Yard algorithm
    private int EvaluateExpression(List<string> tokens)
    {
        var outputQueue = new Queue<string>();
        var operatorStack = new Stack<string>();

        // Process tokens using the Shunting Yard algorithm
        foreach (var token in tokens)
        {
            if (int.TryParse(token, out _))
            {
                outputQueue.Enqueue(token);
            }
            else if (token == "_") // Unary minus placeholder
            {
                outputQueue.Enqueue("0");
                operatorStack.Push("-");
            }
            else if (token == "(")
            {
                operatorStack.Push(token);
            }
            else if (token == ")")
            {
                while (operatorStack.Count > 0 && operatorStack.Peek() != "(")
                {
                    outputQueue.Enqueue(operatorStack.Pop());
                }
                if (operatorStack.Count == 0 || operatorStack.Pop() != "(")
                {
                    throw new ArgumentException("Mismatched parentheses.");
                }
            }
            else if (precedence.ContainsKey(token))
            {
                while (operatorStack.Count > 0 && operatorStack.Peek() != "(" &&
                       (precedence[operatorStack.Peek()] > precedence[token] ||
                       (precedence[operatorStack.Peek()] == precedence[token] && IsLeftAssociative(token))))
                {
                    outputQueue.Enqueue(operatorStack.Pop());
                }
                operatorStack.Push(token);
            }
            else
            {
                throw new ArgumentException("Invalid token encountered.");
            }
        }

        while (operatorStack.Count > 0)
        {
            var op = operatorStack.Pop();
            if (op == "(" || op == ")")
            {
                throw new ArgumentException("Mismatched parentheses.");
            }
            outputQueue.Enqueue(op);
        }

        // Evaluate the output queue (Reverse Polish Notation)
        var evaluationStack = new Stack<int>();
        var formulaStack = new Stack<string>();

        while (outputQueue.Count > 0)
        {
            var currentToken = outputQueue.Dequeue();

            if (int.TryParse(currentToken, out int number))
            {
                evaluationStack.Push(number);
                formulaStack.Push(number.ToString());
            }
            else if (precedence.ContainsKey(currentToken))
            {
                if (evaluationStack.Count < 2)
                {
                    throw new ArgumentException("Invalid expression.");
                }

                int b = evaluationStack.Pop();
                int a = evaluationStack.Pop();

                string fb = formulaStack.Pop();
                string fa = formulaStack.Pop();

                int result = 0;
                string formulaPart = "";

                // Determine if parentheses are needed in formula
                bool needParensA = false;
                bool needParensB = false;

                if (formulaStack.Count > 0)
                {
                    var prevOp = currentToken;
                    needParensA = precedence.ContainsKey(prevOp) && precedence[prevOp] > precedence[currentToken];
                    needParensB = precedence.ContainsKey(prevOp) && precedence[prevOp] > precedence[currentToken];
                }

                fa = needParensA ? $"({fa})" : fa;
                fb = needParensB ? $"({fb})" : fb;

                switch (currentToken)
                {
                    case "+":
                        result = a + b;
                        formulaPart = $"{fa} + {fb}";
                        break;
                    case "-":
                        result = a - b;
                        formulaPart = $"{fa} - {fb}";
                        break;
                    case "*":
                        result = a * b;
                        formulaPart = $"{fa} * {fb}";
                        break;
                    case "/":
                        if (b == 0)
                        {
                            throw new DivideByZeroException("Division by zero is not allowed.");
                        }
                        result = a / b;
                        formulaPart = $"{fa} / {fb}";
                        break;
                }

                evaluationStack.Push(result);
                formulaStack.Push($"({formulaPart})");
            }
            else
            {
                throw new ArgumentException("Invalid token encountered.");
            }
        }

        if (evaluationStack.Count != 1)
        {
            throw new ArgumentException("Invalid expression.");
        }

        int sum = evaluationStack.Pop();
        string formulaString = formulaStack.Pop();

        // Display the formula used for the calculation
        Console.WriteLine($"Formula: {formulaString} = {sum}");

        return sum;
    }
}