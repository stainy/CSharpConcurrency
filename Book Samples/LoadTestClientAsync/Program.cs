using System.Diagnostics;
using System.Net;
using System.Net.Sockets;


var count = int.Parse(args[0]);

Console.WriteLine($"Running with {count} connections");

var tasks = new Task[count];
var failCount = 0;
var failCountLock = new object();

var sw = Stopwatch.StartNew();

for (var i = 0; i < count; ++i)
{
	// 2 runs individual test
	tasks[i] = RunTest(i);
}

// 3 wait for all tests to complete
Task.WaitAll(tasks);

sw.Stop();

lock (failCountLock)
	if (failCount > 0) Console.WriteLine($"{failCount} failures");

Console.WriteLine($"time: {sw.ElapsedMilliseconds}ms");
return;

Task RunTest(int currentTask)
{
	// 5 run all tests in parallel
	return Task.Run(async () =>
	{
		var rng = new Random(currentTask);
		
		await Task.Delay(rng.Next(2 * count));
		using var clientSocket =
			 new Socket(SocketType.Stream, ProtocolType.Tcp);
		try
		{
			// 6 connect to server
			await clientSocket.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 7777));
			
			var buffer = new byte[1024 * 1024];
			while (clientSocket.Connected)
			{
				// 7 reads data
				var read = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
				if (read == 0) break;
			}
		}
		catch
		{
			lock (failCountLock)
				++failCount; // 8 count failures
		}
	});
}