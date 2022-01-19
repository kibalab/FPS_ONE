using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System;
using System.Net;
using System.Threading;
public class tcpCilent : MonoBehaviour {
	// Use this for initialization
	private TcpClient tc;
	private NetworkStream stream;
	void Start () {
		//TcpClient tc = new TcpClient("localhost", 7000);
		tcpConnect();
		tcpSend("Connected");
	}
	public void tcpConnect (){
		tc = new TcpClient("127.0.0.1", 7000);
		stream = tc.GetStream();
	}
	// Update is called once per frame
	public string tcpSend (string msg) {
		
		byte[] buff = Encoding.Unicode.GetBytes(msg);

		// (3) 스트림에 바이트 데이타 전송
		stream.Write(buff, 0, buff.Length);
		byte[] outbuf = new byte[1024];
		int nbytes = stream.Read(outbuf, 0, outbuf.Length);
		string output = Encoding.ASCII.GetString(outbuf, 0, nbytes);
		return output;
	}

	public void tcpClose(){
		stream.Close();
		tc.Close();
	}

}