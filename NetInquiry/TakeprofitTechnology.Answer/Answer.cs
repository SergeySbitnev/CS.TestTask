using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TakeprofitTechnology.Answer
{
    class Answer
    {
        static void Main(string[] args)
        {

            double answ = 4966487.5;
            Console.WriteLine(answ);
            //Console.WriteLine(answ * 2018);
            Console.WriteLine("Check " + answ.ToString() + "\n");

            

            byte[] dataWrite = Encoding.UTF8.GetBytes("Check " + answ.ToString() + "\n");
            String strNum = String.Empty;
            List<byte> parts = new List<byte>();
            try
            {
                int ch = 0; //Переменная для проверки байта из потока
                int count = 0;
                TcpClient client = new TcpClient("88.212.241.115", 2013);

                //Отправляем запрос
                NetworkStream stream = client.GetStream();
                stream.Write(dataWrite, 0, dataWrite.Length);

                //получаем ответ
                List<int> numbers = new List<int>();
                //Побайтно ищем первое вхожение цифры
                while ((ch = stream.ReadByte()) != -1)
                {
                    parts.Add((byte)ch);
                    Console.Write(Convert.ToChar(ch));
                }
                stream.Close();
                client.Close();

                byte[] dataRead = new byte[parts.Count];
                foreach (byte a in parts)
                {
                    dataRead[count] = a;
                    count++;
                }

                //Console.Write(Encoding.GetEncoding("koi8r").GetString(dataRead));
            }
            catch
            {
                Console.WriteLine("Ошибка");
            }


            Console.ReadKey();

        }
    }
}
