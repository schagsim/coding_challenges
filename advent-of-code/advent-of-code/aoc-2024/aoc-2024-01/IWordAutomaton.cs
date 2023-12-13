namespace advent_of_code.aoc_2024.aoc_2024_01;

public interface IWordAutomaton
{
    public string GetWord();
    public int CheckChar(char c);
}