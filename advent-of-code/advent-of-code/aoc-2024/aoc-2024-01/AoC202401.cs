namespace advent_of_code.aoc_2024.aoc_2024_01;

/*
 * --- Day 1: Trebuchet?! ---
Something is wrong with global snow production, and you've been selected to take a look.
The Elves have even given you a map;
on it, they've used stars to mark the top fifty locations that are likely to be having problems.

You've been doing this long enough to know that to restore snow operations,
you need to check all fifty stars by December 25th.

Collect stars by solving puzzles.
Two puzzles will be made available on each day in the Advent calendar;
the second puzzle is unlocked when you complete the first. Each puzzle grants one star.
Good luck!

You try to ask why they can't just use a weather machine <https://adventofcode.com/2015/day/1> ("not powerful enough") and
where they're even sending you ("the sky") and why your map looks mostly blank ("you sure ask a lot of questions") and
hang on did you just say the sky ("of course, where do you think snow comes from")
when you realize that the Elves are already loading you into a trebuchet <https://en.wikipedia.org/wiki/Trebuchet> ("please hold still, we need to strap you in").

As they're making the final adjustments,
they discover that their calibration document (your puzzle input) has been amended by a very young Elf
who was apparently just excited to show off her art skills.
Consequently, the Elves are having trouble reading the values on the document.

The newly-improved calibration document consists of lines of text;
each line originally contained a specific calibration value that the Elves now need to recover.
On each line, the calibration value can be found by combining the first digit and the last digit (in that order) to form a single two-digit number.

For example:

1abc2
pqr3stu8vwx
a1b2c3d4e5f
treb7uchet

In this example, the calibration values of these four lines are 12, 38, 15, and 77. Adding these together produces 142.

Consider your entire calibration document. What is the sum of all of the calibration values?
 */

public static class AoC202401
{
    private static readonly Dictionary<string, int> DigitsMap = new()
    {
        {"1", 1},
        {"2", 2},
        {"3", 3},
        {"4", 4},
        {"5", 5},
        {"6", 6},
        {"7", 7},
        {"8", 8},
        {"9", 9},
        {"0", 0},
        {"one", 1},
        {"two", 2},
        {"three", 3},
        {"four", 4},
        {"five", 5},
        {"six", 6},
        {"seven", 7},
        {"eight", 8},
        {"nine", 9},
        {"zero", 0},
    };

    private static readonly List<IWordAutomaton> WordCheckers;

    static AoC202401()
    {
        WordCheckers = new List<IWordAutomaton>();
        var digitsKeys = DigitsMap.Keys;
        foreach (var key in digitsKeys)
        {
            WordCheckers.Add(new WordAutomaton(key));
        }
    }
    
    private static string[] ReadInputFile()
    {
        Console.WriteLine($"Current folder: {AppDomain.CurrentDomain.BaseDirectory}");
        var inputFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "aoc-2024", "aoc-2024-01", "input.txt");
        return File.ReadAllLines(inputFilePath);
    }
    
    private static bool ConvertWordToDigit(string word, out int digit)
    {
        var containsWord = DigitsMap.ContainsKey(word);
        digit = containsWord ? DigitsMap[word] : 0;
        return containsWord;
    }

    private static bool FindFirstAndLastDigit(string line, out int firstDigit, out int lastDigit)
    {
        firstDigit = 0;
        lastDigit = 0;
        var firstFound = false;
        var lastFound = false;

        var currentIndex = 0;
        while (currentIndex != line.Length && !(firstFound && lastFound))
        {
            var reverseIndex = line.Length - currentIndex - 1;
            if (!firstFound && char.IsDigit(line[currentIndex]))
            {
                firstDigit = line[currentIndex] - '0';
                firstFound = true;
            }
            
            if (!lastFound && char.IsDigit(line[reverseIndex]))
            {
                lastDigit = line[reverseIndex] - '0';
                lastFound = true;
            }

            currentIndex++;
        }

        return firstFound && lastFound;
    }

    private static bool FindFirstAndLastDigitV2(string line, out int firstDigit, out int lastDigit)
    {
        firstDigit = 0;
        lastDigit = 0;
        var firstFound = false;
        var lastFound = false;
        
        var currentIndex = 0;
        while (currentIndex != line.Length && !(firstFound && lastFound))
        {
            var currentChar = line[currentIndex];
            var reverseChar = line[line.Length - currentIndex - 1];
            foreach (var wordChecker in WordCheckers)
            {
                if (!firstFound && wordChecker.CheckCharForward(currentChar))
                {
                    ConvertWordToDigit(wordChecker.GetWord(), out firstDigit);
                    firstFound = true;
                }

                if (!lastFound && wordChecker.CheckCharBackward(reverseChar))
                {
                    ConvertWordToDigit(wordChecker.GetWord(), out lastDigit);
                    lastFound = true;
                }
            }

            currentIndex++;
        }

        return firstFound && lastFound;
    }
    
    public static void ExecuteFirst()
    {
        var content = ReadInputFile();

        var sum = 0;
        for (var i = 0; i < content.Length; i++)
        {
            var currentLine = content[i];
            var digitsFound = FindFirstAndLastDigit(currentLine, out var currentFirstDigit, out var currentLastDigit);
            var currentSum = 10 * currentFirstDigit + currentLastDigit;
            
            if (!digitsFound)
            {
                Console.WriteLine($"Did not find both digits at line {i} ({currentLine})");
            }
            else
            {
                Console.WriteLine($"Line: {currentLine} - found digits {currentFirstDigit} and {currentLastDigit}, sum: {currentSum}");
            }

            sum += currentSum;
        }
        
        Console.WriteLine($"Sum: {sum}");
    }

    public static void ExecuteSecond()
    {
        var content = ReadInputFile();

        var sum = 0;
        for (var i = 0; i < content.Length; i++)
        {
            var currentLine = content[i];
            var digitsFound = FindFirstAndLastDigitV2(currentLine, out var currentFirstDigit, out var currentLastDigit);
            var currentSum = 10 * currentFirstDigit + currentLastDigit;
            
            if (!digitsFound)
            {
                Console.WriteLine($"Did not find both digits at line {i} ({currentLine})");
            }
            else
            {
                Console.WriteLine($"Line: {currentLine} - found digits {currentFirstDigit} and {currentLastDigit}, sum: {currentSum}");
            }

            sum += currentSum;
        }
        
        Console.WriteLine($"Sum: {sum}");
    }
}
