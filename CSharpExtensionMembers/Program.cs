using System.Numerics;


var list = new List<int> { 1, 2, 3, 4, 5 };
var large = list.WhereGreaterThan3(3);

if (large.IsEmpty)
{
    Console.WriteLine("No large numbers");
}
else
{
    Console.WriteLine("Found large numbers");
}

_ = List<int>.Create();


public static class Extensions1
{
    public static IEnumerable<int> WhereGreaterThan1(this IEnumerable<int> source, int threshold)
        => source.Where(x => x > threshold);
}

public static class Extensions2
{
    extension(IEnumerable<int> source)
    {
        public IEnumerable<int> WhereGreaterThan2(int threshold)
            => source.Where(x => x > threshold);

        public bool IsEmpty
            => !source.Any();
    }

    extension<T>(IEnumerable<T> source) where T : INumber<T>
    {
        public IEnumerable<T> WhereGreaterThan3(T threshold)
            => source.Where(x => x > threshold);

        public bool IsEmpty
            => !source.Any();
    }

    extension<T>(List<T>)
    {
        public static List<T> Create()
            => [];
    }
}