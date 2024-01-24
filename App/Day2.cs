namespace App;

internal class Day2 : IPuzzle
{
    public void Solve()
    {
        var limits = new Example(12, 13, 14);
        
        var score =
            File.ReadAllLines("day2.txt")
                .Select(Game.Parse)
                .Where(x => x.Examples.All(y =>
                    y.Red <= limits.Red && y.Green <= limits.Green && y.Blue <= limits.Blue))
                .Sum(x => x.Id);
        
        Console.WriteLine($"Day 2 Part 1: {score}");
        
        var powers = 
            File.ReadAllLines("day2.txt")
                .Select(Game.Parse)
                .Select(x => new Example(
                    x.Examples.Max(y => y.Red),
                    x.Examples.Max(y => y.Green),
                    x.Examples.Max(y => y.Blue)))
                .Select(x => x.Red * x.Green * x.Blue)
                .Sum();
        
        Console.WriteLine($"Day 2 Part 2: {powers}");
    }


    internal record Game(int Id, Example[] Examples)
    {
        public static Game Parse(string data)
        {
            var parts = data.Split(":", StringSplitOptions.TrimEntries);
            var idPart = int.Parse(parts[0].Substring("Game ".Length));
            var examples = parts[1]
                .Split(";", StringSplitOptions.TrimEntries)
                .Select(Example.Parse)
                .ToArray();

            return new Game(idPart, examples);
        }
    };

    internal record Example(int Red, int Green, int Blue)
    {
        public static Example Parse(string data)
        {
            var red = 0;
            var green = 0;
            var blue = 0;

            foreach (var e in data.Split(",", StringSplitOptions.TrimEntries))
            {
                var p = e.Split(" ", StringSplitOptions.TrimEntries);
                var v = int.Parse(p[0]);

                if (p[1] == "red")
                    red = v;
                else if (p[1] == "green")
                    green = v;
                else if (p[1] == "blue")
                    blue = v;
            }

            return new Example(red, green, blue);
        }
    }
}