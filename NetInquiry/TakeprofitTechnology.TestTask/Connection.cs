using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace TakeprofitTechnology.TestTask
{
    class Connection
    {
        //Функция запроса, в резульатате работы возвращает массив данных
        public static byte[] tcpRequest(String request, String server, int port)
        {
            //Создаем подключение и формируем запрос
            TcpClient client = new TcpClient(server, port);
            NetworkStream stream = client.GetStream();
            byte[] dataWrite = Encoding.UTF8.GetBytes(request + "\n");
            //Отправляем запрос
            stream.Write(dataWrite, 0, dataWrite.Length);
            //Считываем ответ
            List<byte> byteList = new List<byte>(); //Количество символов в потоке неизвестно, использую список для считывания всех, до конца потока
            int answSymbol; //Переменная для посимвольного считввания
            while ((answSymbol = stream.ReadByte()) != -1)
            {
                byteList.Add((byte)answSymbol);
            }
            byteList.Add((byte)answSymbol);
            stream.Close();
            client.Close();
            //Формируем массив из списка
            byte[] dataRead = new byte[byteList.Count];
            for (int i = 0; i < dataRead.Length; i++)
            {
                dataRead[i] = byteList[i];
            }
            return dataRead;
        }
    }
}
