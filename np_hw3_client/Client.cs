using System.Net.Sockets;
using System.Text;

namespace np_hw3_client
{
    class Client
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("127.0.0.1", 5000);
            NetworkStream stream = client.GetStream();
            Console.WriteLine("All command list:\nTURN_ON\nTURN_OFF\nBRIGHTNESS_UP\nBRIGHTNESS_DOWN\nTURN_ON_ALL\nTURN_OFF_ALL\nCHANGE_COLOR");
            Thread writeThread = new Thread(() => WriteToServer(stream));
            Thread listenThread = new Thread(() => ListenServer(stream));
            writeThread.Start();
            listenThread.Start();

        }
        static void WriteToServer(NetworkStream stream)
        {
            while (true)
            {
                string message = Console.ReadLine();
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                stream.Write(buffer, 0, buffer.Length);
            }
        }
        static void ListenServer(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            while (true)
            {
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Server: {response}");
            }
        }
    }
}
