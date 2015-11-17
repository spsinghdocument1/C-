using System;
using System.Runtime.InteropServices;
using Structure;
using System.Linq;

namespace HashKey
{
	class MainClass
	{
		[DllImport("libMd5CheckSum.so", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern void GenerateHash(byte[] Buffer, short PFNumber, int CID);

		[DllImport("libMd5CheckSum.so", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public static extern PFHolder GetPFOnConfirmation(byte[] Buffer);

		[DllImport("libMd5CheckSum.so", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
		public  static extern PFHolder GetPFfromOrderNumber(double OrderNumber);


		[StructLayout (LayoutKind.Sequential, Pack = 2)]
		public struct PFHolder
		{
			public	short PF;
			public int CID;
			public long HashKey;
			public int _size;
			public double OrderNo;	

		}
		[StructLayout (LayoutKind.Sequential, Pack = 2)]
		public struct Packetheader
		{
			double i1;
			double i2;
			double i3;
			short i4;
		}

		public static void Main (string[] args)
		{

			// ========================Before placing new Order =============================

			MS_OE_REQUEST_TR _oetr= new MS_OE_REQUEST_TR();


			_oetr.TokenNo = 12546;
			_oetr.Volume = 1245;
			_oetr.Buy_SellIndicator = 256;
			_oetr.Price = 78965;

			Packetheader _pkt = new Packetheader ();

			GenerateHash( DataPacket.RawSerialize(_pkt).Concat(DataPacket.RawSerialize(_oetr)).ToArray(),1,123456);


			// =====================================================


			//^^^^^^^^^^^^^^^^^^^^^^ After Order Confirmation  ============

			double OrderNumber = 8974563214587;

			MS_OE_RESPONSE_TR _oertr = new MS_OE_RESPONSE_TR ();

			_oertr.TokenNo = 12546;
			_oertr.Volume = 1245;
			_oertr.Buy_SellIndicator = 256;
			_oertr.Price = 78965;
			_oertr.OrderNumber = OrderNumber;


			PFHolder _pf =	GetPFOnConfirmation (DataPacket.RawSerialize (_oertr));

			Console.WriteLine ("From Confirmation PFNumber " + _pf.PF + " CID " + _pf.CID );
			//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^=======================


			//^^^^^^^^^^^^^^^^^^^^^^ After any order event other than Order Confirmation  ============



			PFHolder _pf1 =	GetPFfromOrderNumber (OrderNumber);

			Console.WriteLine ("From other PFNumber " + _pf1.PF + " CID " + _pf1.CID );
			//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^=======================











		}
	}
}
