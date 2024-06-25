using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace WebServer0x01
{
    internal class Program
    {
        const int PORT = 6789;
        const int BUFFSIZE = 1024;
        static void Main(string[] args)
        {

            TcpListener server = new TcpListener(PORT);
            server.Start();

            Console.WriteLine("Server bat dau ! " + server.LocalEndpoint);

            int i = 0;
            while (i < 1000)
            {
                Socket client = server.AcceptSocket();
                Thread thread = new Thread(
                    new ParameterizedThreadStart(serveClient)
                    );
                thread.Start(client);
            }

            server.Stop();
        }

        static void serveClient(object xclient)
        {
            Socket client = xclient as Socket;

            // ... nhan request 
            byte[] buffer = new byte[BUFFSIZE];
            string str = "";
            while (client.Available > 0)
            {
                client.Receive(buffer);
                str = System.Text.Encoding.UTF8.GetString(buffer);
            }
            // ... xu ly request 
            Console.WriteLine("\n---------------\n... nhan ket noi ! "
                + client.RemoteEndPoint
                + "\n DATA: \t"
                + str
                + "---------------------\n\n"
                );
            string[] strlist = str.Split("\n");
            if(strlist != null && (strlist.Length > 1))
            {
                string[] general = strlist[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
                string[] requestLineParts = strlist[0].Split(' ');
                string requestedPath = requestLineParts[1];
                // ... tra ve http response 
                string response;
                if(requestedPath == "/contact")
                {
                    response = response = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\nContent-Length: 1000\r\nConnection: keep-alive\r\nCache-Control: s-maxage=300, public, max-age=0\r\nContent-Language: en-US\r\n\r\n<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n  <meta charset=\"utf-8\">\r\n</head>\r\n<body>\r\n  <p>Hello, world!</p>\r\n" + "</p><p> Local: "
                    + client.LocalEndPoint
                    + "</p><p> Client - Remote: " + client.RemoteEndPoint
                    + "</p><p> HTTP request line 0: " + strlist[0]
                    +"</ html > ";
                }  
                else if(requestedPath == "/about")
                {
                    response = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\nContent-Length: 1000\r\nConnection: keep-alive\r\nCache-Control: s-maxage=300, public, max-age=0\r\nContent-Language: en-US\r\n\r\n<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n  <meta charset=\"utf-8\">\r\n</head>\r\n<body>\r\n  <p>About: Nguyen Tan Phat</p></ html > ";
                } 
                else if(requestedPath == "/index.html")
                {
                    response = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\nContent-Length: 1000\r\nConnection: keep-alive\r\nCache-Control: s-maxage=300, public, max-age=0\r\nContent-Language: en-US\r\n\r\n<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n  <meta charset=\"utf-8\">\r\n</head>\r\n<body>\r\n  <p>Hello, world!</p>\r\n" 
                + "</p><img width='500px' src='https://bizweb.dktcdn.net/100/438/408/files/anh-luffy-yody-vn-17.jpg?v=1688806228708' /><p><body></html>";
                }    
                else if(requestedPath == "/")
                {
                response = "HTTP/1.1 200 OK\r\nContent-Type: text/html; charset=utf-8\r\nContent-Length: 1000\r\nConnection: keep-alive\r\nCache-Control: s-maxage=300, public, max-age=0\r\nContent-Language: en-US\r\n\r\n<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n  <meta charset=\"utf-8\">\r\n</head>\r\n<body>\r\n  <p>Hello, world!</p>\r\n"+ "</p><p> Local: "
                + client.LocalEndPoint
                + "</p><p> Client - Remote: " + client.RemoteEndPoint
                + "</p><p> HTTP request line 0: " + strlist[0]
                + "</p><p> PATH request: " + general[1]
                + "</p><img width='500px' src='https://bizweb.dktcdn.net/100/438/408/files/anh-luffy-yody-vn-17.jpg?v=1688806228708' /><p><body></html>";
                }
                else
                {
                    response = "HTTP/1.1 404 Not Found\r\nContent-Type: text/plain; charset=utf-8\r\n\r\n404 Not Found";
                }
                client.Send(System.Text.Encoding.UTF8.GetBytes(response));
            }
            client.Close();
        }
    }
}
