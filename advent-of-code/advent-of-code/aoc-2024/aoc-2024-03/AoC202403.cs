using System.Text;
using advent_of_code.aoc_2024.aoc2024_03;

namespace advent_of_code.aoc_2024.aoc_2024_03;

public static class AoC202403
{
    private static readonly HashSet<char> EngineSymbols = new HashSet<char>()
    {
        '*',
        '-',
        '$',
        '%',
        '+',
        '&',
        '/',
        '@',
        '=',
        '#'
    };
    
    //Dict<yCoord, Dict<xCoord, ref EngineNumber>>
    private static Dictionary<int, Dictionary<int, HashSet<EngineNumber>>> GearsEngineNumbers = new();
    
    private static string[] ReadInputFile()
    {
        Console.WriteLine($"Current folder: {AppDomain.CurrentDomain.BaseDirectory}");
        var inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "aoc-2024", "aoc-2024-03", "input.txt");
        return File.ReadAllLines(inputFilePath);
    }

    private static void AddEngineNumberToGearMarkerCoords(int yCoord, int xCoord, EngineNumber engineNumber)
    {
        var gearsKeys = GearsEngineNumbers.Keys;
        if (!gearsKeys.Contains(yCoord))
        {
            GearsEngineNumbers[yCoord] = new Dictionary<int, HashSet<EngineNumber>>();
        }

        var yCoordsGearsKeys = GearsEngineNumbers[yCoord].Keys;
        if (!yCoordsGearsKeys.Contains(xCoord))
        {
            GearsEngineNumbers[yCoord][xCoord] = new HashSet<EngineNumber>();
        }

        GearsEngineNumbers[yCoord][xCoord].Add(engineNumber);
    }

    private static bool IsPositionSymbol(int indexY, int indexX, in string[] input, out bool isStar)
    {
        isStar = input[indexY][indexX] == '*';
        return EngineSymbols.Contains(input[indexY][indexX]);
    }

    private static bool CheckDigitPosition(int indexY, int indexX, in string[] input, out bool isStar)
    {
        Console.WriteLine($"DEBUG - Checking index [{indexY}, {indexX}], digit {input[indexY][indexX]}");
        isStar = false;
        if (indexX - 1 >= 0)
        {
            //Console.WriteLine($"DEBUG - Checking index [{indexY}, {indexX - 1}], digit {input[indexX][indexY]}");
            if (IsPositionSymbol(indexY, indexX - 1, input, out isStar))
            {
                return true;
            }
            
            if (indexY - 1 >= 0)
            {
                if (IsPositionSymbol(indexY - 1, indexX - 1, input, out isStar))
                {
                    return true;
                }
            }

            if (indexY + 1 <= input.Length - 1)
            {
                if (IsPositionSymbol(indexY + 1, indexX - 1, input, out isStar))
                {
                    return true;
                }
            }
        }

        if (indexX + 1 <= input.Length - 1)
        {
            if (IsPositionSymbol(indexY, indexX + 1, input, out isStar))
            {
                return true;
            }
            
            if (indexY - 1 >= 0)
            {
                if (IsPositionSymbol(indexY - 1, indexX + 1, input, out isStar))
                {
                    return true;
                }
            }

            if (indexY + 1 <= input.Length - 1)
            {
                if (IsPositionSymbol(indexY + 1, indexX + 1, input, out isStar))
                {
                    return true;
                }
            }
        }

        if (indexY - 1 >= 0)
        {
            if (IsPositionSymbol(indexY - 1, indexX, input, out isStar))
            {
                return true;
            }
        }

        if (indexY + 1 <= input.Length - 1)
        {
            if (IsPositionSymbol(indexY + 1, indexX, input, out isStar))
            {
                return true;
            }
        }

        return false;
    }

    public static void ExecuteFirst()
    {
        var input = ReadInputFile();

        var symbols = new HashSet<char>();

        bool readingNumber = false;
        var currentEngineNumber = new EngineNumber(); // How do I not initialize this here? I get an error this might not get initialized later
        var validSum = 0;
        for (var i = 0; i < input.Length; i++)
        {
            readingNumber = false;
            for (var j = 0; j < input[i].Length; j++)
            {
                var c = input[i][j];
                if (!char.IsDigit(c) && c != '.')
                {
                    symbols.Add(c);
                }

                if (char.IsDigit(c))
                {
                    if (!readingNumber)
                    {
                        currentEngineNumber = new EngineNumber();
                    }

                    readingNumber = true;
                    currentEngineNumber.AddDigit(c);
                    if (CheckDigitPosition(i, j, in input, out var isStar))
                    {
                        currentEngineNumber.MarkAsValid();
                        if (isStar)
                        {
                            AddEngineNumberToGearMarkerCoords(i, j, currentEngineNumber);
                        }
                    }
                }
                else
                {
                    if (readingNumber)
                    {
                        var newNumber = currentEngineNumber.GetNumber();
                        Console.WriteLine(
                            $"Line {i + 1} new number: {newNumber}, is valid: {currentEngineNumber.IsValid()}"
                        );
                        if (currentEngineNumber.IsValid())
                        {
                            validSum += newNumber;
                        }
                    }

                    readingNumber = false;
                }

                if (readingNumber && j == input[i].Length - 1)
                {
                    var newNumber = currentEngineNumber.GetNumber();
                    Console.WriteLine(
                        $"Line {i + 1} new number: {newNumber}, is valid: {currentEngineNumber.IsValid()}"
                    );
                    if (currentEngineNumber.IsValid())
                    {
                        validSum += newNumber;
                    }
                }
            }
        }

        var sumGears = 0;
        foreach (var gearsEngineNumber in GearsEngineNumbers)
        {
            foreach (var xPositions in gearsEngineNumber.Value)
            {
                var currentEngineSet = xPositions.Value;
                if (currentEngineSet.Count == 2)
                {
                    foreach (var engineNumber in currentEngineSet)
                    {
                        sumGears += engineNumber.GetNumber();
                    }
                }
            }
        }
        Console.WriteLine($"Sum of gear numbers: {sumGears}");

        var symbolsOutput = new StringBuilder("Symbols: ");
        foreach (var symbol in symbols)
        {
            symbolsOutput.Append($"{symbol} ");
        }
        Console.WriteLine($"Used symbols: {symbolsOutput.ToString()}");
        
        Console.WriteLine($"Sum of valid numbers: {validSum}");
    }
}