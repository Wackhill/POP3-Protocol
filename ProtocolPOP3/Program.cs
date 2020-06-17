using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;

namespace ProtocolPOP3 {
    internal class Program {
        public static void Main(string[] args) {
            TcpClient tcpClient = new TcpClient();               
            tcpClient.Connect("pop.mail.ru", 995);        //Подключаем клиента к порту 995
            SslStream sslStream = new SslStream(tcpClient.GetStream());
            sslStream.AuthenticateAsClient("pop.mail.ru");

            StreamWriter streamWriter = new StreamWriter(sslStream);    //Настраиваем поток воода и вывода
            StreamReader streamReader = new StreamReader(sslStream);
            
            streamWriter.WriteLine("USER andrei.shpakovskiy@mail.ru");  //Отправляем на сервер адрес почты.  
            streamWriter.Flush();                                       //Так как оно там все такое умное и кеширует,
                                                                        //чтобы отправить большим куском, принудительно
                                                                        //чистим этот буфер.
            
            streamWriter.WriteLine("PASS password");                    //То же самое с паролем   
            streamWriter.Flush();
            
            streamWriter.WriteLine("STAT");                             //Запрос на количество сообщений в ящике 
            streamWriter.Flush();                                       //и их размер ящика в октетах 
            
            streamWriter.WriteLine("LIST 1");                           //Запрашиваем инфу по сообщению с номером 1 
            streamWriter.Flush();
            
            streamWriter.WriteLine("RETR 4");                           //Запрашиваем сообщение с номером 4
            streamWriter.Flush();
            
            streamWriter.WriteLine("QUIT");                             //Закрываем соединение
            streamWriter.Flush();

            string receivedInfo = ""; 
            string line = "";
            
            while ((line = streamReader.ReadLine()) != null) {          //Получаем ответы на то, что отослали серверу выше  
                receivedInfo += line + "\n";
            }

            Console.Write(receivedInfo);                                //И выводим на консоль
            Console.Read();
        }
    }
}
