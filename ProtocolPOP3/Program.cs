using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;

namespace ProtocolPOP3 {
    internal class Program {
        public static void Main(string[] args) {
            TcpClient tcpClient = new TcpClient();               
            tcpClient.Connect("pop.mail.ru", 995);
            SslStream sslStream = new SslStream(tcpClient.GetStream());
            sslStream.AuthenticateAsClient("pop.mail.ru");

            StreamWriter streamWriter = new StreamWriter(sslStream);
            StreamReader streamReader = new StreamReader(sslStream);
            
            streamWriter.WriteLine("USER YOUR_EMAIL");
            streamWriter.Flush();
            
            streamWriter.WriteLine("PASS YOUR_PASSWORD");
            streamWriter.Flush();
            
            streamWriter.WriteLine("STAT");
            streamWriter.Flush();
            
            streamWriter.WriteLine("LIST 1");
            streamWriter.Flush();

            streamWriter.WriteLine("RETR 4");
            streamWriter.Flush();
            
            streamWriter.WriteLine("QUIT");
            streamWriter.Flush();

            string receivedInfo = ""; 
            string line = "";
            
            while ((line = streamReader.ReadLine()) != null) {
                receivedInfo += line + "\n";
            }

            Console.Write(receivedInfo);
            Console.Read();
        }
    }
}
