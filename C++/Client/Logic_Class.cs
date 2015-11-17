using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
namespace Client
{
	public class LogicClass
	{
		


		public static IPAddress TapIp =IPAddress.Parse ("192.168.168.252");
		public static int UserId=32865;
		public static short SettlementPeriod=10;
		public static short TapPort=9602;
		public static int InvitationCount=0,SeqNo=0;
		public static string password="11111111";
		public static TcpClient TapSocket=null;
		public static Int16 WarningPercentLimit=90,VolumeFreezePercentLimit=90;

	/*	public static byte[] BufferCopy(byte[] buffer,int srcOffset,int dstOffset,int noOfElement)
		{
			byte[] buffLoc = null;
			if (buffer.Length > 0)
			{
				buffLoc = new byte[noOfElement];
				Buffer.BlockCopy (buffer, srcOffset, buffLoc, dstOffset, noOfElement);	
			}
			return buffLoc;
		}
*/
		public static double DoubleEndianChange(double value)
		{
			return BitConverter.ToDouble(BitConverter.GetBytes(value).Reverse ().ToArray(), 0);
		}

		public static double ConvertToTimestamp(DateTime date)
		{
			return Math.Floor((date-new DateTime(1980,1,1, 0, 0, 0, 0)).TotalSeconds);
		}

		public static DateTime ConvertFromTimestamp(double timestamp)
		{
			return new DateTime(1980, 1, 1, 0, 0, 0, 0).AddSeconds(timestamp);
		}
		public static byte GetBitsToByteValue(byte bit0, byte bit1, byte bit2, byte bit3, byte bit4, byte bit5, byte bit6, byte bit7)
		{
			byte[] numArray1;
			//if (!cynpYinO2Y3VZVnYT8.XIXJKT59x(4))
			numArray1 = new byte[8]
			{
				bit0,
				bit1, 
				bit2,
				bit3,
				bit4,
				bit5,
				bit6,
				bit7
			};
			byte[] numArray2 = numArray1;
			byte num = (byte)0;
			for (byte index = (byte)0; (int)index < 8; ++index)
				num |= (int)numArray2[(int)index] == 1 ? (byte)(1 << (int)index) : (byte)0;
			return num;
		}

		public  byte[] BufferMerge(byte[] buffer1,byte[] buffer2)
		{
			return buffer1.Concat (buffer2).ToArray();
		}

		}
	}


