using System.Net;
using System.Net.Sockets;
using System.Text;

namespace np_hw3_server
{
    public class Server
    {
        static public Dictionary<string, string> Commands = new Dictionary<string, string> {
            {"TURN_ON", "LIGTH ON" },
            {"TURN_OFF", "LIGTH OFF"},
            {"BRIGHTNESS_UP", "BRIGHTNESS INCREASED"}, 
            {"BRIGHTNESS_DOWN", "BRIGHTNESS DECREASED"},
            {"TURN_ON_ALL", "ALL DEVICES ON"}, 
            {"TURN_OFF_ALL", "ALL DEVICES OFF"},
            {"CHANGE_COLOR", "COLOR CHANGED"}
        };
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
                        if(Commands.TryGetValue(message.ToUpper(), out string value))
                        {
                            buffer = Encoding.UTF8.GetBytes(value);
                        }
                        else
                        {
                            buffer = Encoding.UTF8.GetBytes("UNKNOWM COMMAND");
                        }
                        stream.Write(buffer, 0, buffer.Length);
                    }

                    client.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    client.Close();
                    Console.WriteLine("User disconnected...");
                }

                
            }
        }

    }
}
