using App;

var puzzles = new IPuzzle[]
{
    new Day1(), new Day2(), new Day3()
};

foreach (var puzzle in puzzles)
{
    puzzle.Solve();
}