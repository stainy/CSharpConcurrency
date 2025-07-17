using System.Net;
using System.Net.Sockets;

var listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

listenSocket.Bind(new IPEndPoint(IPAddress.Any, 7777));
listenSocket.Listen(50);

while (true)
{
	var connection = listenSocket.Accept();

    // ❶ Handles connection in a new thread
    var thread = new Thread(() =>
	{
		using var file = new FileStream(@"somefile.bin", FileMode.Open, FileAccess.Read);
		
        var buffer = new byte[1024 * 1024];
		
        while (true)
		{
			var read = file.Read(buffer, 0, buffer.Length);
			if ((read) != 0)
            {
                // ❷ Sends file content to client
                connection.Send(
				  new ArraySegment<byte>(buffer, 0, read),
				  SocketFlags.None);
			}
			else
			{
				connection.Shutdown(SocketShutdown.Both);
				connection.Dispose();
				return;
			}
		}
	});
	
    // ❸ Don’t forget to start the thread.
    thread.Start();
}