/// <summary>
/// //////////////////------HOLDER CLASS CONTAINS ALL HOLDERS(WHICH IS USED TO HOLD IN MEMORY DATA).
/// PRADEEP
/// </summary>




using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
//using System.Collections.Generic;

using Structure;


namespace Client
{
    public  struct Csv_Struct
    {
      internal  int lotsize;
    }


	public class Holder
	{
        internal static List<Structure.Contract_File> clliest_contractfile = new List<Contract_File>();
        public static ConcurrentDictionary<double, Order> holderOrder = new ConcurrentDictionary<double, Order>();
        public static ConcurrentDictionary<int, FinalPrice> holderData = new ConcurrentDictionary<int, FinalPrice>();

        public static ConcurrentDictionary<int, Csv_Struct> _DictLotSize = new ConcurrentDictionary<int, Csv_Struct>();
	
	}


	public class Order
	{
		int _type;
		public Order(int iType)
		{
			_type = iType;
		}

		internal MS_OE_REQUEST mS_OE_REQUEST;
		internal MS_OE_RESPONSE_TR mS_OE_RESPONSE_TR;
		internal MS_SPD_OE_REQUEST mS_SPD_OE_REQUEST;

		internal int GetType()
		{
			return _type;
		}

		internal string OrderTypeName(int ival)
		{
			string retval="None";
			switch (this.GetType ()) {
			case  (int)_Type.MS_OE_REQUEST :
				retval ="ORDER ENTRY REQUEST";
				break;
			case (int)_Type.MS_OE_RESPONSE_TR:
				retval ="ORDER a REQUEST";
				break;
			case (int)_Type.MS_SPD_OE_REQUEST:
				retval ="ORDER d REQUEST";
				break;
			}
			return retval;
		}
	}

	enum _Type
	{
		MS_OE_REQUEST=1,
		MS_OE_RESPONSE_TR=2,
		MS_SPD_OE_REQUEST=3,

	}



}

