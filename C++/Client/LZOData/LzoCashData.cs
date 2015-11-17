using LzoNseFO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using Structure;
using Client;


namespace CashData
{
    class LzoCashData
    {
        public event EventHandler<Structure.ReadOnlyEventArgs<MS_INDICES_7207>> OSpotnIndexChange;
        

        #region FO LZO

        private LzoNseFO.xSocket sock = new xSocket();
        private Data IncomingData = new Data();
        private MESSAGE_HEADER msgheader = new MESSAGE_HEADER();
        private BROADCAST_ONLY_MBP BroadCastData_7208 = new BROADCAST_ONLY_MBP();
        private MS_TICKER_TRADE_DATA_7202 TickerTrade_7202 = new MS_TICKER_TRADE_DATA_7202();
        private MS_BCAST_INDICES_7207 Bcast_Indices = new MS_BCAST_INDICES_7207();
        private MS_SPD_MKT_INFO_7211 SpreadData_7211 = new MS_SPD_MKT_INFO_7211();

        private MS_BCAST_VCT_MSGS BcastOpenCloseStatus = new MS_BCAST_VCT_MSGS();

        private short NoPacket;
        private int icomplen = 0;
        private int DecompreddedLen = 0;
        private byte[] decompressedData;
        private byte[] tempBuffer;
        private byte[] src = new byte[0];
        private cCompData tempRemainingData;
        private int StartPont;
        private int endPoint;
        private int error_code;
        private byte[] OrigData;

        SendData dataSend = new SendData();

        [DllImport("TS.dll", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern int lzo1z_decompress(byte[] src, int icomplen, byte[] decompressedData, ref int DecompreddedLen);

        xSocket BroadcastSock = new xSocket();
        List<string> _iSpotSubscribe = new List<string>();

        internal string SubscribeSpot
        {
            //	get { return mSeqNumber; }

            set
            {
                if (!_iSpotSubscribe.Contains(value))
                {
                    _iSpotSubscribe.Add(value);
                }
            }
        }

        #endregion

        public void OnDataArrival(byte[] data)
        {
            IncomingData = (Data)sock.RawDeserialize(data, typeof(Data));
            NoPacket = Convert.ToInt16(IPAddress.NetworkToHostOrder(IncomingData.iNoPackets));
            StartPont = 4;
            endPoint = 0;
            try
            {
                for (short ai = 0; ai < NoPacket; ai++)
                {
                    tempBuffer = data.Skip(StartPont).Take(512).ToArray();
                    tempRemainingData = (cCompData)sock.RawDeserialize(tempBuffer, typeof(cCompData));
                    tempBuffer = null;
                    icomplen = Convert.ToInt32(IPAddress.NetworkToHostOrder(tempRemainingData.iCompLen));
                    StartPont = StartPont + 2 + icomplen;
                    endPoint = endPoint + 2 + icomplen;
                    decompressedData = new byte[1024];
                    src = tempRemainingData.buffer.Skip(0).Take(icomplen).ToArray();
                    /*
                     if (icomplen > 0 && icomplen < 512)
                     {
                         error_code = lzo1z_decompress(src, icomplen, decompressedData, ref DecompreddedLen);

                         if (error_code == 0)
                         {
                             OrigData = decompressedData.Skip(8).Take(512).ToArray();
                             decompressedData = null;
                             DataProcessing(OrigData);
                         }
                         else Console.WriteLine("Data Issue ....");
                     }
                    
                     else
                     * * */

                    if (icomplen == 0)
                    {
                        OrigData = tempRemainingData.buffer.Skip(8).ToArray();
                        DataProcessing(OrigData);
                    }
                    OrigData = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("OnDataArrival  " + ex.ToString());
            }
        }

        private void DataProcessing(byte[] byteMsg)
        {
            try
            {
                msgheader = (MESSAGE_HEADER)sock.RawDeserialize(byteMsg, typeof(MESSAGE_HEADER));
                int Tcode = Convert.ToInt32(IPAddress.NetworkToHostOrder(msgheader.TransactionCode));
                int msgLen = Convert.ToInt32(IPAddress.NetworkToHostOrder(msgheader.MessageLength));

                // File.AppendAllText(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "testData.txt", Tcode.ToString() +"_"+ "\n");

                switch (Tcode)
                {
                    case 7207: 
                        DataProcessing_7207(byteMsg);
                        break;
                    case 7208:
                        //DataProcessing_7208(byteMsg);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataProcessing  " + ex.ToString());
            }
        }
        private void DataProcessing_7207(byte[] byteMsg)
        {
            try
            {
                Bcast_Indices = (MS_BCAST_INDICES_7207)sock.RawDeserialize(byteMsg, typeof(MS_BCAST_INDICES_7207));
                int TotalRecord = IPAddress.NetworkToHostOrder(Bcast_Indices.NumberOfRecords);
                for (int noRec = 0; noRec < TotalRecord; noRec++)
                {
                    OSpotnIndexChange.Raise(OSpotnIndexChange, OSpotnIndexChange.CreateReadOnlyArgs(Bcast_Indices.Indices[noRec]));
                    if (Global.Instance.Child_Index_Dict.ContainsKey(Bcast_Indices.Indices[noRec].IndexName.Trim()))
                    {
                        Child_Index.Instance.UpdateRecord_child_Index(OSpotnIndexChange.CreateReadOnlyArgs(Bcast_Indices.Indices[noRec]));
                    }
                    //Console.WriteLine(Bcast_Indices.Indices[noRec].IndexName.Trim() + "," + IPAddress.NetworkToHostOrder(Bcast_Indices.Indices[noRec].IndexValue).ToString());
                   //if(_iSpotSubscribe.Contains(Bcast_Indices.Indices[noRec].IndexName.Trim()))
                  //{
                 //OSpotnIndexChange.Raise(OSpotnIndexChange, OSpotnIndexChange.CreateReadOnlyArgs(Bcast_Indices.Indices[noRec]));
                //}
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataProcessing_7207  " + ex.ToString());
            }
        }

    }
}
