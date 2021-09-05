using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;

namespace TakeprofitTechnology.TestTask
{
    class TestTask
    {
        private const string server = "88.212.241.115"; //Сервер
        private const int port = 2013; //Порт
        private const int flow = 500; //Количество потоков
        public static double[] arrayAnswer = new double[2018]; //Массив ответов сервера

        private static void numRequest(object numReq)
        {
            try
            {
                byte[] data = Connection.tcpRequest(numReq.ToString(), server, port);
                double numAnsw = 0;
                int order = 0;
                while ((data[order] != 10) && (order < data.Length))
                {
                    //Console.Write((char) data[order]);
                    if ((data[order] >= 48) && (data[order] <= 57))
                    {
                        numAnsw = numAnsw * 10 + (data[order] - 48);
                    }
                    //Проверка сбоя, если вдруг сервер разорвал соединение до появления новой строки
                    if (data[order] == 255) break;
                    order++;
                }

                if (data[order] == 255)
                {
                    Thread.Sleep(10000);
                    numRequest(numReq);
                }
                else
                {
                    arrayAnswer[(int)numReq - 1] = numAnsw;
                    Console.WriteLine("\nОтвет сервера на число {0}: {1}", numReq, numAnsw);
                }
            }
            catch
            {
                //Console.WriteLine(ex);
                Thread.Sleep(20000);
                numRequest(numReq);
            }
        }
        
        static void Main(string[] args)
        {


            //Массив потоков
            Thread[] myThread = new Thread[flow];
            for (int i = 0; i < myThread.Length; i++)
            {
                myThread[i] = new Thread(new ParameterizedThreadStart(numRequest));
            }

            //Счетчик потоков
            int numFlow = 0;
            //Запуск потоков на отправку запросов
            for (int i = 1; i <= arrayAnswer.Length; i++)
            {
                while (myThread[numFlow].IsAlive)
                {
                    if (numFlow < myThread.Length - 1) numFlow++;
                    else
                    {
                        numFlow = 0;
                        Thread.Sleep(30000);
                    }
                }
                Console.WriteLine("Число запроса: {0}\nНомер потока: {1}", i, numFlow);
                myThread[numFlow] = new Thread(new ParameterizedThreadStart(numRequest));
                myThread[numFlow].Start(i);
            }

            //Ожидаем завершения всех потоков
            bool flowStop = true;
            while (flowStop)
            {
                flowStop = false;
                for (int i = 0; i < myThread.Length; i++)
                {
                    if (myThread[i].IsAlive) flowStop = true;
                }
                if (flowStop)
                {
                    Console.WriteLine("Работа продолжается...");
                }
                else
                {
                    Console.WriteLine("Работа окончена...");
                }
                Thread.Sleep(10000);
            }

            //Вычисление медианы и проверка значения на сервере
            double med = MedianaCalc.Mediana(arrayAnswer);
            Console.WriteLine("Check " + med.ToString());
            Console.WriteLine(Encoding.GetEncoding("koi8r").GetString(Connection.tcpRequest("Check " + med.ToString(), server, port)));
            //Console.WriteLine(Encoding.GetEncoding("koi8r").GetString(Connection.tcpRequest("1", server, port)));

            Console.ReadKey();



        }
    }
}
