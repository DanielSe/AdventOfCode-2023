namespace App;

internal class Day3 : IPuzzle
{
    public void Solve()
    {
        var data = File.ReadAllLines("day3.txt")
            .Select(x => x.ToCharArray())
            .ToArray();
        
        var numRows = data.Length;
        var numCols = data[0].Length;

        var grid = new Grid(numCols, numRows);
        grid.LoadData(data);

        var sum = grid.Numbers
            .Where(x => x.IsPartNumber)
            .Sum(x => x.Value);
        
        Console.WriteLine($"Part 1: {sum}");

        var sum2 = grid.Gears
            .Sum(x => x.Ratio);
        
        Console.WriteLine($"Part 2: {sum2}");
    }

    internal class Grid(int width, int height)
    {
        public int Width { get; } = width;
        public int Height { get; } = height;

        private readonly Cell[] _cells = new Cell[width * height];

        public void LoadData(char[][] data)
        {
            for(var row = 0; row < Height; row++)
            for (var col = 0; col < Width; col++)
                _cells[row * Width + col] = new Cell(this, row, col, data[row][col]);
        }

        public Cell? GetCell(int row, int col)
        {
            if (row < 0 || row >= Height) return null;
            if (col < 0 || col >= Width) return null;

            return _cells[row * Width + col];
        }

        public IEnumerable<Number> Numbers =>
            _cells
                .Where(cell => cell.IsNumber)
                .Select(cell => new Number(cell));

        public IEnumerable<Gear> Gears =>
            _cells
                .Where(cell => cell.Ch == '*')
                .Select(cell => new Gear(cell))
                .Where(gear => gear.IsValid);
    }
    internal class Cell(Grid grid, int row, int col, char ch)
    {
        private static readonly char[] Symbols = ['/', '@', '*', '$', '=', '&', '#', '-', '+', '%'];
        
        public Grid Grid { get; } = grid ?? throw new ArgumentNullException(nameof(grid), "Grid needs to be set");
        public int Row { get; } = row;
        public int Col { get; } = col;
        public char Ch { get; } = ch;

        public bool IsSymbol => Symbols.Contains(Ch);
        public bool IsDigit => char.IsDigit(Ch);
        public int Value => IsDigit 
            ? Ch - '0' 
            : throw new InvalidOperationException("Value is only valid for digits");
        public Cell? Prev => Grid.GetCell(Row, Col - 1);
        public Cell? Next => Grid.GetCell(Row, Col + 1);
        public bool IsNumber => IsDigit && Prev is not { IsDigit: true };

        public IEnumerable<Cell> Adjacent
        {
            get
            {
                for (var rowOfs = - 1; rowOfs < 2; rowOfs++)
                for (var colOfs = -1; colOfs < 2; colOfs++)
                {
                    if (rowOfs == 0 && colOfs == 0) continue;

                    var cell = Grid.GetCell(Row + rowOfs, Col + colOfs);
                    if (cell == null) continue;

                    yield return cell;
                }
            }
        }
    }

    internal class Number
    {
        public string Id => $"{Origin.Row}x{Origin.Col}";
        public Cell Origin { get; }

        public Number(Cell origin)
        {
            if (!origin.IsDigit)
                throw new ArgumentException("Invalid origin cell");

            while (origin.Prev is { IsDigit: true })
                origin = origin.Prev;
            
            Origin = origin;
        }

        public IEnumerable<Cell> Cells
        {
            get
            {
                var current = Origin;
                while (current is { IsDigit: true })
                {
                    yield return current;
                    current = current.Next;
                }
            }
        }

        public int Value => Cells.Aggregate(0, (current, cell) => current * 10 + cell.Value);

        public bool IsPartNumber => Cells.SelectMany(x => x.Adjacent).Any(x => x.IsSymbol);
    }

    internal class Gear
    {
        public Cell Origin { get; }

        public Gear(Cell origin)
        {
            if (origin.Ch != '*')
                throw new ArgumentException("Invalid origin cell for Gear");

            Origin = origin;
        }

        public bool IsValid
        {
            get
            {
                var numbers = AdjacentNumbers
                    .ToArray();

                return numbers.Length == 2;
            }
        }

        public int Ratio => AdjacentNumbers.Aggregate(1, (current, num) => current * num.Value);

        private IEnumerable<Number> AdjacentNumbers =>
            Origin.Adjacent
                .Where(x => x.IsDigit)
                .Select(x => new Number(x))
                .DistinctBy(x => x.Id);
    }
}