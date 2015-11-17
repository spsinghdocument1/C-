//INSTANT C# NOTE: Formerly VB project-level imports:
using System;
using System.Collections;
using System.Diagnostics;
//using System.Linq;


using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace LzoNseFO
{
    public class xSocket
    {

        #region Declares
        private UdpClient UDP_Client = new UdpClient();
        private int UDP_Server_Port = 0;
        private Thread thdUdp;
        #endregion
        private UdpClient UDP_Server;
        private Socket s;

        #region Events
        public event DataArrivalEventHandler DataArrival;
        public delegate void DataArrivalEventHandler(byte[] Data);
        public event Sock_ErrorEventHandler Sock_Error;
        public delegate void Sock_ErrorEventHandler(string Description);
        #endregion

        public struct IpDetails
        {
            public int Port;
            public string LocalIp;
            public string McastIp;
        }
        public IpDetails ipdet = new IpDetails();

        #region UDP_Multicast

        Socket mSendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint ipepSend;
        public void McastSendSettings(string localIp = "127.0.0.1", string mcast = "233.1.2.5", int _port = 35098)
        {
            // foreach (IPAddress localIp in  Dns.GetHostAddresses(Dns.GetHostName()).Where(i => i.AddressFamily == AddressFamily.InterNetwork))
            {
                IPAddress ipToUse, _multicastIp;
                IPAddress.TryParse(localIp, out ipToUse);
                IPAddress.TryParse(mcast, out _multicastIp);
                ipepSend = new IPEndPoint(_multicastIp, _port);

                // using (var mSendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    mSendSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(_multicastIp, IPAddress.Any));
                    //mSendSocket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, 255);
                    mSendSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    //mSendSocket.MulticastLoopback = true;
                    mSendSocket.Bind(new IPEndPoint(ipToUse, _port));
                    //byte[] bytes = Encoding.ASCII.GetBytes("This is my welcome message");
                    //var ipep = new IPEndPoint(_multicastIp, _port);
                    //mSendSocket.SendTo(bytes, ipep);
                }
            }
        }
        public int sendMcastData(byte[] data)
        {
            // var ipep = new IPEndPoint(_multicastIp, _port);
            return mSendSocket.SendTo(data, ipepSend);
        }

        public bool ListenMcastData(int Port, string LocalIp)
        {
            return ListenMcastData(Port, LocalIp, "233.1.2.5");
        }
        public bool ListenMcastData(int Port)
        {
            return ListenMcastData(Port, "127.0.0.1", "233.1.2.5");
        }
        public bool ListenMcastData()
        {
            return ListenMcastData(35098, "127.0.0.1", "233.1.2.5");
        }
        public bool ListenMcastData(int Port, string LocalIp, string McastIp)
        {
            ipdet.Port = Port;
            ipdet.LocalIp = LocalIp;
            ipdet.McastIp = McastIp;
            try
            {
                // Start the thread with a ParameterizedThreadStart.
                Thread thread = new Thread(new ParameterizedThreadStart(GetMcastData));
                thread.Start(ipdet);
                return true;
            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(e.Message.ToString());
                return false;
            }

        }
        public void GetMcastData(object ipd)
        {
            try
            {
                IpDetails ipConf = (IpDetails)ipd;
                byte[] byt = new byte[1024];
                //Socket s;
                s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                s.ExclusiveAddressUse = false;
                s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, Convert.ToInt32(ipConf.Port));
                s.Bind(ipep);
                IPAddress ip = IPAddress.Parse(ipConf.McastIp.Trim());
                s.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(ip, IPAddress.Parse(ipConf.LocalIp)));


                while (true)
                {
                    try
                    {
                        int size = s.Receive(byt);
                        // MessageBox.Show("DAta Recv " + size.ToString());
                        //break;
                        if (size > 0)
                        {
                            if (DataArrival != null)
                                DataArrival(byt);
                        }
                    }
                    catch (Exception e)
                    {
                        if (Sock_Error != null)
                            Sock_Error(e.Message.ToString());
                    }
                }




            }
            catch (Exception ex)
            {
                if (Sock_Error != null)
                    Sock_Error(ex.Message.ToString());
            }
        }
        #endregion
        public void UdpSendSettings(string Host, int Port) // Connect Socket for Specific IP and Port 
        {
            UDP_Client.Connect(Host, Port);
        }
        public int sendUdpByte(byte[] data)
        {
            return UDP_Client.Send(data, data.Length);
        }
        public void UDP_Send(string Host, int Port, string Data)
        {
            try
            {
                UDP_Client.Connect(Host, Port);

                byte[] sendBytes = System.Text.Encoding.Unicode.GetBytes(Data);

                UDP_Client.Send(sendBytes, sendBytes.Length);
            }
            catch (Exception e)
            {
                if (Sock_Error != null)
                    Sock_Error(e.Message.ToString());
            }

        }
        public void CloseSock()
        {
            UDP_Send("127.0.0.1", UDP_Server_Port, "CloseMe");
            Thread.Sleep(30);
            UDP_Server.Close();
            thdUdp.Abort();
        }

        #region Function

        public object RawDeserialize(byte[] rawdatas, Type anytype)
        {
            int rawsize = Marshal.SizeOf(anytype);
            if (rawsize > rawdatas.Length)
            {
                return null;
            }
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.Copy(rawdatas, 0, buffer, rawsize);
            object retobj = Marshal.PtrToStructure(buffer, anytype);
            Marshal.FreeHGlobal(buffer);
            return retobj;
        }
        public byte[] rawserialize(object anything)
        {
            int rawsize = Marshal.SizeOf(anything);
            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.StructureToPtr(anything, buffer, false);
            byte[] rawdatas = new byte[rawsize];
            Marshal.Copy(buffer, rawdatas, 0, rawsize);
            Marshal.FreeHGlobal(buffer);
            return rawdatas;
        }



        #endregion
    }
}