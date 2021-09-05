using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace TestOneNumber
{
    class TestOneNumber
    {
        private const string server = "88.212.241.115"; //Сервер
        private const int port = 2013; //Порт
        private const int flow = 200; //Количество потоков
        public static double[] arrayAnswer = new double[2018]; //Массив ответов сервера

        static void Main(string[] args)
        {
            String str = "1850";
            byte[] dataWrite = Encoding.UTF8.GetBytes(str + "\n");

            try
            {
                int ch = 0; //Переменная для проверки байта из потока
                TcpClient client = new TcpClient(server, port);

                //Отправляем запрос
                NetworkStream stream = client.GetStream();
                stream.Write(dataWrite, 0, dataWrite.Length);

                //Получаем ответ
                //Побайтно ищем первое вхожение цифры пока не будет символа новой строки
                double num = 0;
                while ((ch = stream.ReadByte()) != 10)
                {
                    Console.WriteLine("byte {0} - char {1}", ch, Convert.ToChar(ch));
                    if ((ch >= 48) && (ch <= 57))
                    {
                        num = num * 10 + (ch - 48);
                    }
                    //Проверка сбоя
                    if (ch == -1) break;
                }
                stream.Close();
                client.Close();

                //arrayAnswer[(int)str - 1] = num;
                Console.WriteLine("\nОтвет сервера на число {0}: {1}", str, num);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }



            //byte[] data = Connection.tcpRequest("Register", server, port);
            //byte[] data2 = new byte[data.Length - 8];
            //for (int i = 0; i < data2.Length; i++)
            //{
            //    data2[i] = data[i + 3];
            //    Console.WriteLine("{0} - {1}", data2[i], (char)data[i]);
            //}

            //string str = Encoding.GetEncoding("koi8r").GetString(data2) + "|1";
            //Console.WriteLine("\n" + str);


            Console.ReadKey();

        }
    }
}
