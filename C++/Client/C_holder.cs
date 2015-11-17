using System;
using System.Collections.Concurrent;
using Structure;
namespace Client
{
	public class Holder
	{

		public static ConcurrentDictionary<Int16,object> holderDownload=new ConcurrentDictionary<Int16,object>(); //transjection No
		public static ConcurrentDictionary<double,MS_OE_REQUEST> holderOrder=new ConcurrentDictionary<double,MS_OE_REQUEST>(); //Order Number
		public static ConcurrentDictionary<double,MS_OE_RESPONSE_TR> holderOrderTRres=new ConcurrentDictionary<double,MS_OE_RESPONSE_TR>(); //Order Number		
		public static ConcurrentDictionary<string,object> holder_Exercise_and_Position_Liquidation=new ConcurrentDictionary<string,object>();
		public static ConcurrentDictionary<double,MS_SPD_OE_REQUEST> holderspreadOrderTRres=new ConcurrentDictionary<double,MS_SPD_OE_REQUEST>(); //Order Number		
		public static ConcurrentDictionary<double,order> holderGlobal=new ConcurrentDictionary<double,order>(); //Order Number
	}
	public class order
	{

		internal MS_OE_REQUEST objMS;
		internal MS_OE_RESPONSE_TR objTr;
		internal MS_SPD_OE_REQUEST objTrSPD;


	}
}

