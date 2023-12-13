namespace advent_of_code.aoc_2024.aoc_2024_01;

public abstract class WordAutomaton : IWordAutomaton
{
    protected string Word;

    private int _currentIndex;

    protected WordAutomaton()
    {
        Word = string.Empty;
        _currentIndex = 0;
    }

    protected WordAutomaton(string word)
    {
        Word = word;
        _currentIndex = 0;
    }
    
    public virtual string GetWord()
    {
        return Word;
    }

    /// <summary>
    /// Check next char in this checker. 
    /// </summary>
    /// <param name="c">Char to check.</param>
    /// <returns>2 if the word has been matched, 1 if this matcher is still active but not yet matched fully, 0 if we can discard this matcher.</returns>
    public virtual int CheckChar(char c)
    {
        if (c != Word[_currentIndex]) return 0;
        
        if (_currentIndex == Word.Length - 1)
        {
            return 2;
        }
        _currentIndex++;
        return 1;
    }
    
}

public class FrontWordAutomaton : WordAutomaton
{
    public FrontWordAutomaton(string word) : base(word)
    {
    }
}

public class BackWordAutomaton : WordAutomaton
{
    private readonly string _originalWord;
    
    public BackWordAutomaton(string word)
    {
        var charArray = word.ToCharArray();
        Array.Reverse(charArray);
        Word = new string(charArray);
        _originalWord = word;
    }

    public override string GetWord()
    {
        return _originalWord;
    }
}
