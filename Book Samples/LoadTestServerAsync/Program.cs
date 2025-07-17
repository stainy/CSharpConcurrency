using System.Net;
using System.Net.Sockets;

var listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

listenSocket.Bind(new IPEndPoint(IPAddress.Any, 7777));
listenSocket.Listen(100);

while (true)
{
    // ❶ AcceptAsync instead of Accept
    var connection = await listenSocket.AcceptAsync();

    // ❷ Task.Run instead of new Thread
    _ = Task.Run(async () =>
    {
        using (var file = new FileStream("somefile.bin", FileMode.Open, FileAccess.Read))
        {
            var buffer = new byte[1024 * 1024];

            while (true)
            {
                // ❸ File.ReadAsync instead of Read
                var read = await file.ReadAsync(buffer, 0, buffer.Length);

                if (read != 0)
                {
                    // ❹ SendAsync instead of Send
                    await connection.SendAsync(
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
        }
    });
}