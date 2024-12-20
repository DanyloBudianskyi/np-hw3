using System.Net;
using System.Net.Sockets;
using System.Text;

namespace np_hw3_server
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 5000);
            server.Start();
            Console.WriteLine("Server has started and working...");
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("User connected...");
                Thread thread = new Thread(HandleClient);
                thread.Start(client);

            }
            static void HandleClient(object obj)
            {
                TcpClient client = (TcpClient)obj;
                NetworkStream stream = client.GetStream();
                try
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"User: {message}");
                        if (message.ToUpper() == "TURN_ON" || message.ToUpper() == "TURN ON")
                        {
                            buffer = Encoding.UTF8.GetBytes("LIGTH ON");
                            stream.Write(buffer, 0, buffer.Length);
                        }
                        else if (message.ToUpper() == "TURN_OFF" || message.ToUpper() == "TURN OFF")
                        {
                            buffer = Encoding.UTF8.GetBytes("LIGTH OFF");
                            stream.Write(buffer, 0, buffer.Length);
                        }
                        else
                        {
                            buffer = Encoding.UTF8.GetBytes("UNKNOWM COMMAND");
                            stream.Write(buffer, 0, buffer.Length);
                        }
                    }

                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                

                
            }
        }

    }
}
