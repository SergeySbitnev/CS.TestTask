using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace NetInquiry
{
    class Test
    {
        private const string server = "88.212.241.115"; //Сервер
        private const int port = 2013; //Порт
        private const int flow = 500; //Количество потоков
        public static double[] arrayAnswer = new double[2018]; //Массив ответов сервера

        public static void Inquiry(object str)
        {
            //Формируем запрос
            byte[] dataWrite = Encoding.UTF8.GetBytes(str.ToString() + "\n");
            
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
                    if ((ch >= 48) && (ch <= 57))
                    {
                        num = num * 10 + (ch - 48);
                    }
                    //Проверка сбоя
                    if (ch == -1) break;
                }
                stream.Close();
                client.Close();

                if (ch == -1)
                {
                    Thread.Sleep(10000);
                    Inquiry(str);
                }
                else
                {
                    arrayAnswer[(int)str - 1] = num;
                    Console.WriteLine("\nОтвет сервера на число {0}: {1}", str, num);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Thread.Sleep(20000);
                Inquiry(str);
            }
        }

        static void Main(string[] args)
        {
            //Массив потоков
            Thread[] myThread = new Thread[flow];
            for (int i = 0; i < myThread.Length; i++)
            {
                myThread[i] = new Thread(new ParameterizedThreadStart(Inquiry));
            }

            //Счетчик потоков
            int numFlow = 0;
            //Запуск потоков на отправку запросов
            for (int i = 1; i <= arrayAnswer.Length; i++)
            {
                while (myThread[numFlow].IsAlive)
                {
                    if (numFlow < myThread.Length - 1) numFlow++; else numFlow = 0;
                }
                Console.WriteLine("Число запроса: {0}\nНомер потока: {1}", i, numFlow);
                myThread[numFlow] = new Thread(new ParameterizedThreadStart(Inquiry));
                myThread[numFlow].Start(i);
                //Thread.Sleep(10000);
            }

            //Проверяем что все потоки отработали
            bool flowStop = true;
            while (flowStop)
            {
                flowStop = false;
                for (int i = 0; i < myThread.Length; i++)
                {
                    if (myThread[i].IsAlive) flowStop = true;
                }
                Thread.Sleep(5000);
                Console.WriteLine("Состояние потоков: {0}", flowStop);

            }

            double tg = 0;
            for(int i = 0; i < arrayAnswer.Length; i++)
            {
                Console.WriteLine("{0} : {1}", i+1, arrayAnswer[i]);
                tg += arrayAnswer[i];
            }
            Console.WriteLine("\nСумма запросов: {0}", tg);
            Console.ReadKey();
        }
    }
}
