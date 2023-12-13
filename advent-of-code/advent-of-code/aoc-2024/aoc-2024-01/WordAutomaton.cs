namespace advent_of_code.aoc_2024.aoc_2024_01;

public class WordAutomaton : IWordAutomaton
{
    private readonly string _word;
    private readonly string _reverseWord;

    private int _currentIndex;
    private int _currentBackIndex;
    
    public WordAutomaton(string word)
    {
        _word = word;
        var charArray = word.ToCharArray();
        Array.Reverse(charArray);
        _reverseWord = new string(charArray);
        _currentIndex = 0;
        _currentBackIndex = _word.Length - 1;
    }
    
    public string GetWord()
    {
        return _word;
    }

    public bool CheckCharForward(char c)
    {
        var searchSuccessful = false;
        if (c == _word[_currentIndex])
        {
            if (_currentIndex == _word.Length - 1)
            {
                searchSuccessful = true;
                _currentIndex = 0;
            }
            else
            {
                _currentIndex++;
            }
        }
        else
        {
            _currentIndex = 0;
        }

        return searchSuccessful;
    }

    public bool CheckCharBackward(char c)
    {
        var searchSuccessful = false;
        if (c == _reverseWord[_currentBackIndex])
        {
            if (_currentBackIndex == _reverseWord.Length - 1)
            {
                searchSuccessful = true;
                _currentBackIndex = 0;
            }
            else
            {
                _currentBackIndex++;
            }
        }
        else
        {
            _currentBackIndex = 0;
        }

        return searchSuccessful;
    }
}