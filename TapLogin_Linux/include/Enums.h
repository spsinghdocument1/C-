#ifndef ENUMS_H_INCLUDED
#define ENUMS_H_INCLUDED

namespace Enums
{


    enum STGTYPE
	{
        FOFO =1,
		TWOLEGOPT =2
	};

    enum MessageType
	{
		eORDER=65,
		eORDERRej=66,
		eLOGIN=67,
		eMESSAGE=68,
		eHEARTBEAT=69,
		eFOPAIR=70,
		eFOPAIRDIFF=71,
		eFOPAIRUNSUB=72,
		eIOCPAIR=73,
		eIOCPAIRUNSUB=74,
		eIOCPAIRDIFF=75,
		eA_LOGIN=76,	//LogIn of Admin
		eA_UPDATE=77,
		eA_UPDATEClient=78,
		eA_UPDATEOrder=79,
		eA_UPDATEComplete=80,
		eA_MESSAGE=81,
		eCANCELALL=82,
		eSTOP_ALL=83,
        eDelete = 84
	};

    enum LogInStatus
	{
		LogIn=201,
		PwdError=202,
		UserAlreadyLogIn=203,
		PwdExpire=204,
		LogOutStatus=205,
		LogOutbyAdmin=206,
		LogOutNoheartbeat=207,
		LogOut=208,
		UserAlreadyLogOut=209,
	};

	enum BP
	{
		PROD = 1,
		BASE =2
	};

    struct InHeader
    {
        short TransectionCode;
		double ClintId;
    };

#pragma pack(2)
    struct  C_SignIn
	{

        short TransectionCode;
		double ClintId;
		char Password[9];
		short Status;
		STGTYPE StgType;
	};
	#pragma pack(2)
struct ClientUpdateMsg
    {
         short TransectionCode;
		 double ClintId;
         C_SignIn _csign;
    };

	struct C_Contract_Desc
	{

        char InstrumentName[7];
		char	Symbol[11];
        int ExpiryDate;
		int StrikePrice;
		char OptionType[3];
		short CALevel;
	};

    struct  C_LotIN
	{
		 short TransectionCode;
		 double OrderNo;
		 double ClintId;
		 int TokenNo;
		 C_Contract_Desc contract_obj;
		 char AccountNumber[11];
		 short Buy_SellIndicator;
		 int DisclosedVolume;
		 int Volume;
		 int Price;
		 int TriggerPrice;
		 char Open_Close;
		 short Reasoncode;
	};

    struct  HeartBeatInfo//size not match
	{
		 int tapStatus;
		 int dataStatus;
		 short tapQueue;
		 short ClientQueue;
	};

    struct  C_OrderReject
	{
		 double OrderNo;
		 short Reasoncode;
	};



}


#endif // ENUMS_H_INCLUDED
