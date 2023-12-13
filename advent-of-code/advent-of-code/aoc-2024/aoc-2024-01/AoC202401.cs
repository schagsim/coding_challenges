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

    private static List<IWordAutomaton> _frontWordCheckers;
    private static List<IWordAutomaton> _backWordCheckers;

    private static readonly Dictionary<char, HashSet<string>> CharWordsInitFrontPairing;
    private static readonly Dictionary<char, HashSet<string>> CharWordsInitBackPairing;

    static AoC202401()
    {
        _frontWordCheckers = new List<IWordAutomaton>();
        _backWordCheckers = new List<IWordAutomaton>();
        CharWordsInitFrontPairing = new Dictionary<char, HashSet<string>>();
        CharWordsInitBackPairing = new Dictionary<char, HashSet<string>>();
        
        var digitsKeys = DigitsMap.Keys;
        foreach (var key in digitsKeys)
        {
            var firstChar = key[0];
            var lastChar = key[^1];
            if (!CharWordsInitFrontPairing.ContainsKey(firstChar))
            {
                CharWordsInitFrontPairing[firstChar] = new HashSet<string>();
            }
            CharWordsInitFrontPairing[firstChar].Add(key);

            if (!CharWordsInitBackPairing.ContainsKey(lastChar))
            {
                CharWordsInitBackPairing[lastChar] = new HashSet<string>();
            }
            CharWordsInitBackPairing[lastChar].Add(key);
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

    private static void AddCheckers(char frontChar, char backChar)
    {
        if (CharWordsInitFrontPairing.TryGetValue(frontChar, out var wordCheckersToCreate))
        {
            foreach (var wordCheckerToCreate in wordCheckersToCreate)
            {
                _frontWordCheckers.Add(new FrontWordAutomaton(wordCheckerToCreate));
            }
        }
        
        if (CharWordsInitBackPairing.TryGetValue(backChar, out wordCheckersToCreate))
        {
            foreach (var wordCheckerToCreate in wordCheckersToCreate)
            {
                _backWordCheckers.Add(new BackWordAutomaton(wordCheckerToCreate));
            }
        }
    }

    private static bool CheckCheckers(in List<IWordAutomaton> wordAutomata, char frontChar, out string matchedWord)
    {
        matchedWord = string.Empty;
        for (var i = wordAutomata.Count - 1; i >= 0; i--)
        {
            var matchResult = wordAutomata[i].CheckChar(frontChar);
            if (matchResult == 0)
            {
                wordAutomata.RemoveAt(i);
            }

            if (matchResult == 2)
            {
                matchedWord = wordAutomata[i].GetWord();
                return true;
            }
        }

        return false;
    }

    private static void ClearCheckers()
    {
        for (var i = _frontWordCheckers.Count - 1; i >= 0; i--)
        {
            _frontWordCheckers.RemoveAt(i);
        }
        
        for (var i = _backWordCheckers.Count - 1; i >= 0; i--)
        {
            _backWordCheckers.RemoveAt(i);
        }
    }

    private static bool FindFirstAndLastDigitV2(string line, out int firstDigit, out int lastDigit)
    {
        firstDigit = 0;
        lastDigit = 0;
        var firstFound = false;
        var lastFound = false;
        ClearCheckers();
        
        var currentIndex = 0;
        while (currentIndex != line.Length && !(firstFound && lastFound))
        {
            var currentChar = line[currentIndex];
            var reverseChar = line[line.Length - currentIndex - 1];
            AddCheckers(currentChar, reverseChar);
            if
            (
                !firstFound &&
                CheckCheckers(_frontWordCheckers, currentChar, out var matchedWord) &&
                ConvertWordToDigit(matchedWord, out firstDigit)
            )
            {
                firstFound = true;
            }
            
            if
            (
                !lastFound &&
                CheckCheckers(_backWordCheckers, reverseChar, out matchedWord) &&
                ConvertWordToDigit(matchedWord, out lastDigit)
            )
            {
                lastFound = true;
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
                Console.WriteLine($"Line {i}: {currentLine} - found digits {currentFirstDigit} and {currentLastDigit}, sum: {currentSum}");
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
                Console.WriteLine($"Line {i}: {currentLine} - found digits {currentFirstDigit} and {currentLastDigit}, sum: {currentSum}");
            }

            sum += currentSum;
        }
        
        Console.WriteLine($"Sum: {sum}");
    }
}
