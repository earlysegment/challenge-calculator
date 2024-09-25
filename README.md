# challenge-calculator

## String Calculator Challenge

### Overview

This project implements a String Calculator as part of a coding challenge. The calculator performs addition operations based on a formatted input string, with support for various delimiters and customizations. The implementation is in **C#** using **Visual Studio Code** as the IDE. The project follows the separation of concerns principle, with the main application code in one project and unit tests in a separate test project. 

### Requirements

The calculator supports the following operations:

1. Add up to two numbers using a comma delimiter. Invalid or missing numbers are treated as `0`, and an exception is thrown if more than two numbers are provided.
2. No limit to the number of numbers that can be added.
3. Support for newline characters as an alternative delimiter.
4. Deny negative numbers by throwing an exception with all negative numbers listed.
5. Ignore any value greater than 1000.
6. Support a single custom delimiter in the format `//{delimiter}\n{numbers}`.
7. Support a custom delimiter of any length using the format `//[{delimiter}]\n{numbers}`.
8. Support multiple custom delimiters of any length using the format `//[{delimiter1}][{delimiter2}]...\n{numbers}`.

### Stretch Goals

- Display the formula used for the calculation.
- Allow continuous input processing until the user interrupts with Ctrl+C.
- Support command-line arguments to customize behavior.
- Use Dependency Injection (DI) for improved modularity.
- Extend the calculator to support subtraction, multiplication, and division.

---

### How to Run the Application

#### Prerequisites

- Ensure you have the **.NET Core SDK** installed. You can download it from [here](https://dotnet.microsoft.com/download).

#### Steps to Run the Application

1. **Clone the repository** to your local machine:

   ```bash
   git clone https://github.com/<your-username>/challenge-calculator.git
   ```

2. **Navigate to the main project directory**:

   ```bash
   cd challenge-calculator/StringCalculator
   ```

3. **Build the application**:

   ```bash
   dotnet build
   ```

4. **Run the application**:

   ```bash
   dotnet run
   ```

---

### How to Run the Unit Tests

The unit tests are located in a separate project.

#### Steps to Run the Tests

1. **Navigate to the test project directory**:

   ```bash
   cd ../StringCalculator.Tests
   ```

2. **Build the test project**:

   ```bash
   dotnet build
   ```

3. **Run the tests**:

   ```bash
   dotnet test
   ```
## Project Structure

The project is organized into two separate projects:

```
challenge-calculator/
├── StringCalculator/           # Main application code
│   ├── Program.cs              # Entry point for the application
│   ├── StringCalculator.cs     # Core logic for the calculator
│   └── StringCalculator.csproj # Project file for the application
└── StringCalculator.Tests/     # Test project
    ├── StringCalculatorTests.cs# Unit tests for the StringCalculator class
    └── StringCalculator.Tests.csproj # Project file for the tests
```

### Dependencies

#### Main Project (`StringCalculator`)

- **.NET Core 7.0**: The project is built on the .NET 7.0 framework.

#### Test Project (`StringCalculator.Tests`)

- **NUnit**: Used for unit testing.
- **NUnit3TestAdapter**: Enables running NUnit tests with `dotnet test`.
- **Microsoft.NET.Test.Sdk**: Required for running the tests within the .NET Core testing environment.



