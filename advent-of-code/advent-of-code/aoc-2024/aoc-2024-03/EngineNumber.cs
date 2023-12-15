using System.Text;

namespace advent_of_code.aoc_2024.aoc2024_03;

public class EngineNumber
{
    private StringBuilder _currentNumber;
    private bool _isValid;

    public EngineNumber()
    {
        _currentNumber = new StringBuilder();
        _isValid = false;
    }

    public int GetNumber()
    {
        return _currentNumber.Length == 0 ? 0 : int.Parse(_currentNumber.ToString());
    }

    public void AddDigit(char c)
    {
        if (char.IsDigit(c)) _currentNumber.Append(c);
    }

    public void MarkAsValid()
    {
        _isValid = true;
    }

    public bool IsValid()
    {
        return _isValid;
    }
}