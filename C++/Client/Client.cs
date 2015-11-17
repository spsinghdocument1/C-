using System;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Runtime.InteropServices;
namespace Client
{
	public class clientCls
	{
		public static  TcpClient GetSocket(IPAddress remoteHost,int port)
		{
			TcpClient clientSocket= new TcpClient();

			try
			{
				clientSocket.Connect(remoteHost,port);
				Console.WriteLine ("Socket Connect");
				return clientSocket;
			}
			catch (Exception ex) 
			{
				Console.WriteLine (ex.Message);
				return null;
			}
		}


		public  void WriteOnNStream(byte[] buffer,TcpClient clientSocket)
			{       
			try
			{   
				NetworkStream networkStream = clientSocket.GetStream();
				networkStream.Write(buffer,0,buffer.Length);
				networkStream.Flush();
				Console.WriteLine("Srtream write Completed : " +buffer.Length);
			}
			catch(Exception ex)
			{
				Console.WriteLine (ex.Message);
			}
		}


		public  byte[] ReadFromNStream(TcpClient clientSocket)
		{
			try
			{
				NetworkStream networkStream = clientSocket.GetStream();
				byte[] buffer = new byte[clientSocket.ReceiveBufferSize];				
				int RecLength=networkStream.Read(buffer, 0, (int)clientSocket.ReceiveBufferSize);
			//	Console.WriteLine("Srtream Read Completed");
				return LogicClass.BufferCopy(buffer,0,0,RecLength);
			}
			catch(Exception ex)
			{
				Console.WriteLine (ex.Message);
				return null;
			}
		}
	}
}

