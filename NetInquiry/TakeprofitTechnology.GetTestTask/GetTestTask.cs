using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TakeprofitTechnology.GetTestTask
{
    class GetTestTask
    {
        private const string server = "88.212.241.115"; //Сервер
        private const int port = 2013; //Порт

        static void Main(string[] args)
        {
            //Формируем запрос
            //byte[] dataWrite = Encoding.UTF8.GetBytes("Check 4966487.5\n");
            byte[] dataWrite = Encoding.UTF8.GetBytes("Greetings\n");

            String inq = String.Empty;
            String inq2 = String.Empty;
            
            try
            {
                int ch = 0; //Переменная для проверки байта из потока
                TcpClient client = new TcpClient(server, port);

                //Отправляем запрос
                NetworkStream stream = client.GetStream();
                stream.Write(dataWrite, 0, dataWrite.Length);

                //получаем ответ
                List<byte> byteList = new List<byte>();
                while ((ch = stream.ReadByte()) != -1)
                {
                    Console.WriteLine("{0} - {1}", ch, Convert.ToChar(ch));
                    
                    byteList.Add((byte)ch);
                }
                stream.Close();
                client.Close();
                
                byte[] dataRead = new byte[byteList.Count-4];
                int count = 0;
                for (int i = 0; i < byteList.Count - 4; i++)
                {
                    dataRead[i] = byteList[i + 3];
                }
                
                inq = Encoding.GetEncoding("koi8r").GetString(dataRead);

                Console.WriteLine("\n");
                Console.WriteLine(inq + "|1\n");






                ch = 0;
                TcpClient client2 = new TcpClient(server, port);
                dataWrite = Encoding.UTF8.GetBytes(inq + "|1\n");
                stream = client2.GetStream();
                stream.Write(dataWrite, 0, dataWrite.Length);

                List<byte> byteList2 = new List<byte>();
                while ((ch = stream.ReadByte()) != -1)
                {
                    Console.Write("{0} ", Convert.ToChar(ch));
                    byteList2.Add((byte)ch);
                }
                stream.Close();
                client.Close();

                dataRead = new byte[byteList2.Count];
                count = 0;
                foreach (byte a in byteList2)
                {
                    dataRead[count] = a;
                    count++;
                }
                inq = Encoding.GetEncoding("koi8r").GetString(dataRead);
                Console.WriteLine(inq + "\n");

                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }
    }
}
