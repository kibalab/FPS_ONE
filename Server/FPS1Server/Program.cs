using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace FPS1Server
{
    class Program
    {
        static void Main(string[] args)
        {
            // (1) 로컬 포트 7000 을 Listen
            TcpListener listener = new TcpListener(IPAddress.Any, 7000);
            listener.Start();

            byte[] buff = new byte[1024];

            while (true)
            {
                // (2) TcpClient Connection 요청을 받아들여
                //     서버에서 새 TcpClient 객체를 생성하여 리턴
                TcpClient tc = listener.AcceptTcpClient();

                // (3) TcpClient 객체에서 NetworkStream을 얻어옴 
                NetworkStream stream = tc.GetStream();

                // (4) 클라이언트가 연결을 끊을 때까지 데이타 수신
                int nbytes;
                while ((nbytes = stream.Read(buff, 0, buff.Length)) > 0)
                {
                    // (5) 데이타 그대로 송신
                    Console.WriteLine(Encoding.Unicode.GetString(buff));
                    stream.Write(buff, 0, nbytes);
                }

                // (6) 스트림과 TcpClient 객체 
                stream.Close();
                tc.Close();

                // (7) 계속 반복
            }
        }
    }
}
