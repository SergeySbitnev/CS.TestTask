using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace TakeprofitTechnology.TestTask.NumInquiry
{
    class NumInquiry
    {
        private const string server = "88.212.241.115"; //Сервер
        private const int port = 2013; //Порт
        public static double sumAnswers = 0; //Сумма полученных ответов
        private const int flow = 2018; //Количество потоков

        public static void Inquiry(object str)
        {
            //Формируем запрос
            byte[] dataWrite = Encoding.UTF8.GetBytes(str.ToString() + "\n");
            String strNum = String.Empty;
            try
            {
                int ch = 0; //Переменная для проверки байта из потока
                TcpClient client = new TcpClient(server, port);

                //Отправляем запрос
                NetworkStream stream = client.GetStream();
                stream.Write(dataWrite, 0, dataWrite.Length);

                //получаем ответ
                List<int> numbers = new List<int>();
                //Побайтно ищем первое вхожение цифры
                while ((ch = stream.ReadByte()) != -1)
                {
                    if ((ch >= 48) && (ch <= 56))
                    {
                        while ((ch >= 48) && (ch <= 56))
                        {
                            numbers.Add(ch);
                            ch = stream.ReadByte();
                        }
                        break;
                    }
                }
                stream.Close();
                client.Close();

                //Если список пуст или после цифры идет конец ответа (-1), запрос необходимо повторить
                if ((numbers.Count == 0) || (ch == -1))
                {
                    Inquiry(str);
                }
                else
                {
                    foreach (byte a in numbers)
                    {
                        strNum += Convert.ToChar(a);
                    }
                    Console.WriteLine("\nОтвет сервера на число {0}: {1}", str, strNum);
                    sumAnswers += Convert.ToDouble(strNum);
                }
            }
            catch
            {
                //Console.WriteLine(ex);
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

            int numFlow = 0; //Счетчик потоков
            //Запуск потоков на отправку запросов
            for (int i = 1; i <= 2018; i++)
            {
                while (myThread[numFlow].IsAlive)
                {
                    if (numFlow < myThread.Length - 1) numFlow++; else numFlow = 0;
                }
                Console.WriteLine("Число запроса: {0}\nНомер потока: {1}", i, numFlow);
                myThread[numFlow] = new Thread(new ParameterizedThreadStart(Inquiry));
                myThread[numFlow].Start(i);
                Thread.Sleep(100);
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

            }
            Console.WriteLine("\nСумма запросов: {0}", sumAnswers);
            Console.ReadKey();
        }
    }
}
