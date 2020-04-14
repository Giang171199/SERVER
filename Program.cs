using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

class TCPServer
{

    const int MAX_CONNECTION = 10;
    const int PORT_NUMBER = 9999;

    static TcpListener listener;

    static Dictionary<string, string> _data =
        new Dictionary<string, string>
    {
        {"0","Zero"},
        {"1","One"},
        {"2","Two"},
        {"3","Three"},
        {"4","Four"},
        {"5","Five"},
        {"6","Six"},
        {"7","Seven"},
        {"8","Eight"},
        {"9","Nine"},
    };

    public static void Main()
    {
        IPAddress address = IPAddress.Parse("127.0.0.1");

        listener = new TcpListener(address, PORT_NUMBER);
        Console.WriteLine("Doi ket noi ...");
        listener.Start();

        for (int i = 0; i < MAX_CONNECTION; i++)
        {
            new Thread(DoWork).Start();
        }
    }

    static void DoWork()
    {
        while (true)
        {
            Socket soc = listener.AcceptSocket();

            Console.WriteLine("Da nhan duoc ket noi tu: {0}",
                              soc.RemoteEndPoint);

            try
            {
                var stream = new NetworkStream(soc);
                var reader = new StreamReader(stream);
                var writer = new StreamWriter(stream);
                writer.AutoFlush = true;
                
                while (true)
                {
                    string id = reader.ReadLine();

                    if (String.IsNullOrEmpty(id))
                        break; // disconnect

                    if (_data.ContainsKey(id))
                        writer.WriteLine("So ban da nhap la: '{0}'", _data[id]);
                    else
                        writer.WriteLine("So ban da nhap khong hop le !");
                }
                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Loi: " + ex);
            }

            Console.WriteLine("Da ngat ket noi : {0}",
                              soc.RemoteEndPoint);
            soc.Close();
        }
    }
}
