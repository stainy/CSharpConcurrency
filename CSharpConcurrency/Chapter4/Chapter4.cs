
using System.Diagnostics;

internal class Chapter4
{
    internal static void DoWork()
    {
        GetIncorrectValue();
        GetCorrectValue();
    }
    private static void GetIncorrectValue()
    {
        var theValue = 0;
        var threads = new Thread[2];

        for (var i = 0; i < 2; ++i)
        {
            threads[i] = new Thread(() =>
            {
                for (var j = 0; j < 5000000; ++j)
                {
                    ++theValue;
                }
            });

            threads[i].Start();
        }

        foreach (var current in threads)
        {
            current.Join();
        }

        Console.WriteLine(theValue);
        }
    
    private static void GetCorrectValue()
    {
        var theValue = 0;
        var threads = new Thread[2];
        var theLock = new object();

        for (var i = 0; i < 2; ++i)
        {
            threads[i] = new Thread(() =>
            {
                for (var j = 0; j < 5000000; ++j)
                {
                    lock (theLock)
                    {
                        ++theValue;
                    }
                }
            });

            threads[i].Start();
        }

        foreach (var current in threads)
        {
            current.Join();
        }

        Console.WriteLine(theValue);
        }
}

