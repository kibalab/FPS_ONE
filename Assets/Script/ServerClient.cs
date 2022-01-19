using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class ServerClient : MonoBehaviour {
    TcpClient clientSocket = new TcpClient(); // 소켓

    public NetworkStream stream = default(NetworkStream);

    public Text ContentText;
    public string message = string.Empty;
    public string pID;
    // Use this for initialization
    void Start () {
        try

        {

            clientSocket.Connect("203.228.7.159", 11000); // 접속 IP 및 포트

            stream = clientSocket.GetStream();

        }

        catch (Exception e2)

        {

            Debug.Log("연결 실패!");

        }



        message = "채팅 서버에 연결 되었습니다.";

        DisplayText(message);
        pID = this.GetComponent<UserID>().GetRandomID(15);
        Debug.Log(pID);
        try
        {
            SendMessage(pID);
        }
        catch
        {
            ContentText.text = "Connection timed out.";
        }



        Thread t_handler = new Thread(GetMessage);

        t_handler.IsBackground = true;

        t_handler.Start();

    }
    public void Send(string str) // 보내기 버튼

    {

        Debug.Log("Me : " + str + "\r\n");
        byte[] buffer = Encoding.Unicode.GetBytes(str);
        stream.Write(buffer, 0, buffer.Length); // 보내버리기

        stream.Flush();
    }
    private void Receive() // 서버로 부터 값 받아오기

    {

        while (stream.CanRead)

        {

            Thread.Sleep(1);

            if (stream.CanRead)

            {

                byte[] buffer = new byte[1024]; // 버퍼

                int bytes = stream.ReadByte();
                string tempStr = Encoding.Unicode.GetString(buffer, 0, bytes);
                if (tempStr.Length > 0)

                {

                    Send("You : " + tempStr + "\r\n");

                }

            }

        }

    }
    public void SendMessage(string str)
    {
        byte[] buffer = Encoding.Unicode.GetBytes(str + "$");

        stream.Write(buffer, 0, buffer.Length);

        stream.Flush();
    }
    private void GetMessage() // 메세지 받기

    {

        while (true)

        {

            stream = clientSocket.GetStream();

            int BUFFERSIZE = clientSocket.ReceiveBufferSize;

            byte[] buffer = new byte[BUFFERSIZE];

            int bytes = stream.Read(buffer, 0, buffer.Length);



            string message = Encoding.Unicode.GetString(buffer, 0, bytes);
            //this.GetComponent<NetworkManager>().playerPositionUpdate(message);
            DisplayText(message);

        }

    }



    private void DisplayText(string text) // Server에 메세지 출력

    {
        message = text;
        Debug.Log(text);

    }
    public void Close()
    {
        SendMessage("leaveChat" + "$");
    }
}
