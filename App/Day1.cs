namespace App;


internal class Day1 : IPuzzle
{
    public void Solve()
    {
        var sum = File.ReadAllLines("day1.txt")
            .Select(FindDigits)
            .Sum();
        
        Console.WriteLine($"Sum is: {sum}");
    }

    private static int FindDigits(string input) => FindDigit(input) * 10 + FindDigit(input, true);
    
    private static int FindDigit(string input, bool fromEnd = false)
    {
        var replacements = new[]
        {
            new { Search = "one", Value = 1 },
            new { Search = "two", Value = 2 },
            new { Search = "three", Value = 3 },
            new { Search = "four", Value = 4 },
            new { Search = "five", Value = 5 },
            new { Search = "six", Value = 6 },
            new { Search = "seven", Value = 7 },
            new { Search = "eight", Value = 8 },
            new { Search = "nine", Value = 9 },
            new { Search = "1", Value = 1 },
            new { Search = "2", Value = 2 },
            new { Search = "3", Value = 3 },
            new { Search = "4", Value = 4 },
            new { Search = "5", Value = 5 },
            new { Search = "6", Value = 6 },
            new { Search = "7", Value = 7 },
            new { Search = "8", Value = 8 },
            new { Search = "9", Value = 9 },
        };
        
        var index = fromEnd ? -1 : int.MaxValue;
        var value = 0;
        
        foreach (var r in replacements)
        {
            var i = fromEnd
                ? input.LastIndexOf(r.Search, StringComparison.InvariantCulture)
                : input.IndexOf(r.Search, StringComparison.InvariantCulture);
            
            if (i == -1)
                continue;
            
            if ((!fromEnd && i < index) || (fromEnd && i > index))
            {
                index = i;
                value = r.Value;
            }
        }

        return value;
    }
}