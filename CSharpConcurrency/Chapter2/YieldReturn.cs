namespace CSharpConcurrency.Chapter2;

public class YieldReturn
{
    private IEnumerable<int> NoYieldDemo()
    {
        var result = new List<int>();

        result.Add(1);
        result.Add(2);
        return result;
    }

    // Returns a collection with two items, the numbers 1 and 2
    public void UseNoYieldDemo()
    {
        foreach (var current in NoYieldDemo())
        {
            Console.WriteLine($"Got {current}");
        }
    }
      
    private IEnumerable<int> YieldDemo()
    {
        yield return 1;
        yield return 2;
    }

    public void UseYieldDemo()
    {
        foreach (var current in YieldDemo())
        {
            Console.WriteLine($"Got {current}");
        }
    }
}