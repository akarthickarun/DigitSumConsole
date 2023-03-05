// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;

var value = 456789;

// Splitting the numbers into array
// 12345 => 1,2,3,4,5

///
/// Regular looping based implementation
///

//IEnumerable<int> SplitDigits(int value)
//{
//    var sum = value;

//    while (sum > 9)
//    {
//        var lastDigit = sum % 10;
//        sum /= 10;

//        yield return lastDigit;
//    }

//    yield return sum;
//}

/// Recursive with enumerables
IEnumerable<int> Split(int value)
{
    if (value > 9)
    {
        foreach (var digit in Split(value / 10))
            yield return digit;
    }

    yield return value % 10;
}

int Demo(int value, IExplain? explain)
{
    if (value > 9)
    {
        var values = Split(value);
        var sum = values.Sum();

        /// Explain will keep the intermediate results
        /// based on the explanation instance passed through. 
        /// It can be even null reference
        explain?.Add(new IntermediateResult
        {
            Value = value,
            Values = values.ToArray(),
            Sum = sum
        });

        return Demo(sum, explain);
    }

    return value;
}

var explain = new DetailedExplanation();
var result = Demo(value, explain);
Console.WriteLine(result);

Console.WriteLine("Explanations");
explain.Show();

public interface IExplain
{
    void Add(IntermediateResult result);
    void Show();
}

public class DefaultExplanation : IExplain
{
    public void Show() => Console.WriteLine("No explanations available");

    public void Add(IntermediateResult result) { }
}

public class DetailedExplanation : IExplain
{
    private List<IntermediateResult> _results = new();

    public void Show() => _results.ForEach(result => System.Console.WriteLine(result));

    public void Add(IntermediateResult result) => _results.Add(result);
}

public record IntermediateResult
{
    public int Value { get; init; }
    public int[]? Values { get; init; }
    public int Sum { get; init; }

    public override string ToString()
    {
        var expression = Values != null ? string.Join(" + ", Values) : string.Empty;
        return $"{Value} => [{expression}] => {Sum}";
    }
}