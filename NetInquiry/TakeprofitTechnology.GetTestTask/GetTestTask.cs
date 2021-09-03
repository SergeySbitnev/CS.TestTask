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
            byte[] dataWrite = Encoding.UTF8.GetBytes("Check 4966487.5\n");
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
                    byteList.Add((byte)ch);
                }
                stream.Close();
                client.Close();

                byte[] dataRead = new byte[byteList.Count];
                int count = 0;
                foreach (byte a in byteList)
                {
                    dataRead[count] = a;
                    count++;
                }
                Console.WriteLine(Encoding.GetEncoding("koi8r").GetString(dataRead));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            Console.ReadKey();
        }
    }
}
