#include <vector>
#include <string.h>
#include <map>
#include <arpa/inet.h>
#include "Struct.h"
#include <fstream>
#include <nanomsg/nn.h>
#include <nanomsg/pubsub.h>
//#include "INICPP/INIReader.h"
#include <iostream>
#include <stdlib.h>
#include <boost/thread/thread.hpp>
#include <boost/bind.hpp>
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/ini_parser.hpp>

//#include <boost/thread.hpp>
#include <mutex>

#include <boost/multi_index_container.hpp>
#include <boost/multi_index/member.hpp>
#include <boost/multi_index/ordered_index.hpp>
#include <boost/multi_index/hashed_index.hpp>
#include <boost/multi_index/tag.hpp>
#include <boost/multi_index/mem_fun.hpp>

//#ifndef EVENTCLASS_H
#define	EVENTCLASS_H
using namespace boost;

using boost::multi_index::multi_index_container;
using boost::multi_index::ordered_non_unique;
using boost::multi_index::ordered_unique;
using boost::multi_index::indexed_by;
using boost::multi_index::member;
using boost::multi_index::nth_index;
using boost::multi_index::get;
using boost::multi_index::hashed_unique;
using boost::multi_index::tag;
using boost::multi_index::const_mem_fun;

extern "C"
{
        typedef void ( * ProgressCallback)( char*,int);
        void PassRefOfMethod(ProgressCallback progressCallback,long ClientID);
        void DisposeCPP(long ClientID);
}

namespace AutoClient
{


struct Rec {
	//int Token, Bid, Ask, LTP;
	FinalPrice FP;
	int _Token() const { return FP.Token; }
	struct ByToken {};

	struct PriceChange : public std::unary_function<Rec,void> {
		FinalPrice p; PriceChange(const FinalPrice &_p) : p(_p) {}
		//void operator()(Rec & r) { r.Bid = p.MAXBID;r.Ask=p.MINASK;r.LTP= p.LTP; }
		void operator()(Rec & r) { r.FP = p; }
	};

};

typedef boost::multi_index_container
<Rec,
	indexed_by
	<
		hashed_unique<
                        tag<Rec::ByToken>, const_mem_fun<Rec,int,&Rec::_Token>
                     >
    >
> Store;



class DataCheck
{


 Store _store;


typedef Store::index<Rec::ByToken>::type TokenList;
TokenList & ns;// = _store.get<Rec::ByToken>();
TokenList::const_iterator nit;

public:

DataCheck():ns(_store.get<Rec::ByToken>())
{

}
 void InsertRecord(int Token)
{
   // Price_View& InsertRec = get<0>(_store);
    //InsertRec.insert(Record(Token,0,0,0));
    FinalPrice _tempFP;
    memset(&_tempFP,0,sizeof(_tempFP));
    _tempFP.Token=Token;
    _tempFP.subToken=Token;

    Rec r1= {_tempFP};
    _store.insert(r1);
}

void UpdatePrice(FinalPrice FP)
{

	//PList & ps = store.get<Rec::ByPhone>();
	nit = ns.find(FP.Token);


	if ( nit != ns.end() )
	{
        ns.modify(nit, Rec::PriceChange(FP));
	}
}

void CleanRecord(int Token)
{
   //TokenList & ns = _store.get<Rec::ByToken>();
	//PList & ps = store.get<Rec::ByPhone>();
	nit = ns.find(Token);


	if ( nit != ns.end() )
	{
        ns.erase(nit);
	}
}

FinalPrice GetRecord(int Token)
{
    FinalPrice _fp;
    memset(&_fp,0,sizeof(FinalPrice));
   // TokenList & ns = _store.get<Rec::ByToken>();
	//PList & ps = store.get<Rec::ByPhone>();
	nit = ns.find(Token);


	if ( nit != ns.end() )
	{
         /*_fp.Token = Token;
         _fp.MAXBID= nit->Bid;
        _fp.MINASK=nit->Ask;*/

        _fp = nit->FP;
	}



    return _fp;
}


void ClearAllRecords()
{
   //TokenList & ns = _store.get<Rec::ByToken>();
    for(TokenList::iterator _it= ns.begin();_it!=ns.end();_it++)
    {
        ns.erase(_it);
    }
}

};


class AutoClientFoFo
{
    DataCheck _dataHolder;
	//map<int, FinalPrice> _Datadict;
	map<int, NFToken> SymbolDictionary;
	map<int,struct FOPAIRDIFF> _FOPAIRDIFF ;

	map<int, OrderDetails> _OrderDetailsBuy;
	map<int, OrderDetails> _OrderDetailsSell;
//	map<int, map<char, OrderDetails>> _OrderDetails
	map<int,  NearMonthPacket> _NMPACK;
    map<int , bool> _SubStatus;
    //map<int , std::mutex> _mutex;
std::array<std::mutex, 30> _mutex;

//#define CancelCode 20070
//#define ModificationCode 20070
//*************************************************
   // boost::mutex mutex;

    //std::mutex _lock;

    boost::thread_group producer_threads;

	short  CancelCode;
	short  ModificationCode;
	long   UserId;//Trader
	short  BranchId;
    string SECTION;
    string CONTRACTFILEPATH;
    string DATANANOPATH;
    boost::thread* _dataThread;
    boost::thread* _eventThread;

	int BrokerId;

	MS_OE_REQUEST_TR OEObj;
	MS_OM_REQUEST_TR OMObj;

    //INIReader reader;

public:    bool IsExit;

    ProgressCallback ProcessToEnqueue;

    long ClientIdAuto;

    map<int,struct Contract_File> cimlist;

public:

	AutoClientFoFo()
	{


	}

	void InItClass(map<int,struct Contract_File> AllCimlist)
	{

        boost::property_tree::ptree pt;
        boost::property_tree::ini_parser::read_ini("settings.ini", pt);
        SECTION = pt.get<std::string>("SECTION.ID");
        CONTRACTFILEPATH =pt.get<std::string>(SECTION +".CONTRACTFILEPATH");
        DATANANOPATH = pt.get<std::string>(SECTION +".DATANANOPATH");

        BrokerId= pt.get<int>(SECTION +".BROKERID");
        BranchId=pt.get<int>(SECTION +".BRANCHID");
        UserId=pt.get<int>(SECTION +".USERID");

        IsExit=false;
		CancelCode=ntohs(20070);
		ModificationCode=ntohs(20040);

    //cout << "SECTION : " << SECTION << " CONTRACTFILEPATH : " << CONTRACTFILEPATH << " DATANANOPATH : " << DATANANOPATH << " BrokerId : "
    //<< BrokerId << " BranchId : " << BranchId << " UserId : " << UserId << endl;

    cimlist.insert(AllCimlist.begin(),AllCimlist.end());
    //Contract_Filefun();

    _dataThread = new boost::thread(&AutoClientFoFo::Datasubscriber, this);

    producer_threads.add_thread(_dataThread);

    cout << "Thread Started for Datasubscriber"<< endl;

    _eventThread=new boost::thread(&AutoClientFoFo::Eventsubscriber, this);
    producer_threads.add_thread(_eventThread);

    cout << "Thread Started for Eventsubscriber"<< endl;
    //Eventsubscriber

	}

	 void PadRight(char *string, int padded_len, char *pad)
	 {
            int len = (int) strlen(string);
            if (len >= padded_len) {
            //return string;
            }
            int i;
            for (i = 0; i < padded_len - len; i++) {
            strcat(string, pad);
            }
        //  return string;

    }

void toUpper(char* pArray, int arrayLength)
{
    for(int i = 0; i < arrayLength; i++)
    {
        if(pArray[i] >= 'a' && pArray[i] <= 'z')
            pArray[i] -= ' ';
    }
}



		///Contract File Loading....begin



		////contract File Loading.....End

		///here InitTokenDetails Call..../////////


		TokenPartnerDetails  InitTokenDetails( int FirstLeg, int alternateLeg, int PortfolioName)
	{

		 TokenPartnerDetails tpdobj;

		/*if (TokenPartner.find(FirstLeg) != TokenPartner.end())
		{
			cout << "Token already exists" << endl;
			return TokenPartner;

		}*/


	//	vector<Contract_File> san_contract = cimlist; //CSV_Class.cimlist.Where (a => a.Token == FirstLeg).ToList ();

		if (cimlist.empty())
		{
			cout << "Contract holder empty" << endl;
			return tpdobj;

		}


		//for (vector<Contract_File>::iterator it = cimlist.begin(); it != cimlist.end(); it++)
		if(cimlist.find(FirstLeg)!=cimlist.end())
		{
                struct Contract_File *cf =new Contract_File();
				tpdobj.CF.Token = cimlist[FirstLeg].Token;
				tpdobj.CF.AssetToken = cimlist[FirstLeg].AssetToken;
				tpdobj.CF.InstrumentName = cimlist[FirstLeg].InstrumentName;
				tpdobj.CF.Symbol = cimlist[FirstLeg].Symbol;
				tpdobj.CF.Series = cimlist[FirstLeg].Series;
				tpdobj.CF.ExpiryDate = cimlist[FirstLeg].ExpiryDate;
				tpdobj.CF.OptionType =cimlist[FirstLeg].OptionType;
				tpdobj.CF.BoardLotQuantity=cimlist[FirstLeg].BoardLotQuantity;
				tpdobj.PartnerLeg = alternateLeg;
            	tpdobj.PortfolioName = PortfolioName;
		}
		else
		{
            cout << "Your subscribed token not found in Contract list "<< endl;
		}

		return tpdobj;

	}

///Start ReturnPack................................



public:

//char *
MS_OE_REQUEST_TR ReturnNearPack(int Token,BUYSELL BS,int Qty,int FMBlq =0)
{

    MS_OE_REQUEST_TR obj2;

    //for (vector<Contract_File>::iterator it2 = cimlist.begin(); it2 != cimlist.end(); it2++)
    if(cimlist.find(Token)!=cimlist.end())
		{

			strncpy(obj2.InstrumentName,cimlist[Token].InstrumentName.c_str(),sizeof(obj2.InstrumentName));
			//obj2.InstrumentName=it2->InstrumentName;
			strncpy(obj2.Symbol,cimlist[Token].Symbol.c_str(),sizeof(obj2.Symbol));
			//obj2.Symbol=it2->Symbol;
			obj2.ExpiryDate=cimlist[Token].ExpiryDate;
			obj2.StrikePrice=cimlist[Token].StrikePrice;
			strncpy(obj2.OptionType,cimlist[Token].OptionType.c_str(),sizeof(obj2.OptionType));
			//obj2.OptionType=it2->OptionType;
		//	obj2.DisclosedVolume=it2->BoardLotQuantity;
			if(FMBlq<=0)
			{
				obj2.Volume=cimlist[Token].BoardLotQuantity;
			}
			else
			{
				cout<< "Near BLQ set to FAR BLQ" << FMBlq << endl;
				obj2.Volume = FMBlq;
			}
			obj2.TokenNo=cimlist[Token].Token;


        }


                MS_OE_REQUEST_TR obj;

				obj.TransactionCode =ntohs((short)20000);
				obj.ReasonCode =ntohs((short)0);
				obj.BookType = ntohs((short)1);
				obj.GoodTillDate =ntohl(0);
/*
                obj.Oflag.AON=0;
                obj.Oflag.IOC=0;
                obj.Oflag.GTC=0;
                obj.Oflag.DAY=1;
                obj.Oflag.MIT=0;
                obj.Oflag.SL=0;
                obj.Oflag.MARKET=0;
                obj.Oflag.ATO=0;

                obj.Oflag.Reserved=0;
                obj.Oflag.Frozen=0;
                obj.Oflag.Modified=0;
                obj.Oflag.Traded=0;
                obj.Oflag.MatchedInd=0;
                obj.Oflag.MF=0;

*/
                obj.FlagIn=8;
                obj.FlagOut=0;

				obj.Reserved1 =2;
                obj.TokenNo = ntohl(Token);

                 strcpy(obj.InstrumentName,obj2.InstrumentName);
                 PadRight(obj.InstrumentName,sizeof(obj.InstrumentName)," ");
                 toUpper(obj.InstrumentName,sizeof(obj.InstrumentName));
                strcpy(obj.Symbol,obj2.Symbol);
                PadRight(obj.Symbol,sizeof(obj.Symbol)," ");
                toUpper(obj.Symbol,sizeof(obj.Symbol));
				obj.ExpiryDate =ntohl(obj2.ExpiryDate);
				obj.StrikePrice =ntohl(obj2.StrikePrice);
				strcpy(obj.OptionType,obj2.OptionType);
                PadRight(obj.OptionType,sizeof(obj.OptionType)," ");
                toUpper(obj.OptionType,sizeof(obj.OptionType));
                strcpy(obj.AccountNumber,"");//obj2.AccountNumber);
                PadRight(obj.AccountNumber,sizeof(obj.AccountNumber)," ");
                toUpper(obj.AccountNumber,sizeof(obj.AccountNumber));



				obj.Buy_SellIndicator =ntohs((short)BS);
				obj.DisclosedVolume = ntohl(Qty*obj2.Volume);
				//cout << "Volume set to " << Qty*obj2.Volume << " Qty "<< Qty << " LOTSIZE " << obj2.Volume << endl;
				obj.Volume =ntohl(Qty*obj2.Volume);
				obj.Price =ntohl(0);    //

                obj.Open_Close='O';

            //    printf("\nobj.Open_Close== %c",obj.Open_Close);

				//sprintf(&obj.Open_Close,"%c",'O');

				obj.UserId =ntohl(UserId);
				obj.BranchId =ntohs(BranchId);
				obj.TraderId =ntohl(UserId);

                sprintf(obj.BrokerId,"%d",BrokerId);
				//strcpy(obj.BrokerId,BrokerId.c);
                PadRight(obj.BrokerId,sizeof(obj.BrokerId)," ");
                toUpper(obj.BrokerId,sizeof(obj.BrokerId));

                strcpy(obj.Settlor,"");
                PadRight(obj.Settlor,sizeof(obj.Settlor)," ");
                toUpper(obj.Settlor,sizeof(obj.Settlor));
				obj.Pro_ClientIndicator = ntohs(2);
				double dbl=ClientIdAuto;
				htond(dbl) ;
				obj.nnffield =dbl;

				obj.Length =ntohs(sizeof(MS_OE_REQUEST_TR));
			//	obj.SequenceNumber =ntohs(0);
				obj.MsgCount =ntohs(1);


            //cout<<"--->Token. :"<< Token <<" BS :"<<BS<<" InstrumentName :"<<obj2.InstrumentName<<" obj2.TokenNo :"<<obj2.TokenNo<<endl;
                return obj;
		}



 void htond (double &x)
{
  int *Double_Overlay;
  int Holding_Buffer;
  Double_Overlay = (int *) &x;
  Holding_Buffer = Double_Overlay [0];
  Double_Overlay [0] = htonl (Double_Overlay [1]);
  Double_Overlay [1] = htonl (Holding_Buffer);
}



 MS_OM_REQUEST_TR ReturnModificationPack(int Token,BUYSELL BS)
		{


             //****we use dll for below line..................
		    //var _contract = CSV_Class.cimlist.Where (a => a.Token == Token).ToList ();
                MS_OM_REQUEST_TR obj2;
        //for (vector<Contract_File>::iterator it2 = cimlist.begin(); it2 != cimlist.end(); it2++)
        if(cimlist.find(Token)!=cimlist.end())

		{

			strncpy(obj2.InstrumentName,cimlist[Token].InstrumentName.c_str(),sizeof(obj2.InstrumentName));
			//obj2.InstrumentName=it2->InstrumentName;
			strncpy(obj2.Symbol,cimlist[Token].Symbol.c_str(),sizeof(obj2.Symbol));
			//obj2.Symbol=it2->Symbol;
			obj2.ExpiryDate=cimlist[Token].ExpiryDate;
			obj2.StrikePrice=cimlist[Token].StrikePrice;
			strncpy(obj2.OptionType,cimlist[Token].OptionType.c_str(),sizeof(obj2.OptionType));
			//obj2.OptionType=it2->OptionType;
		//	obj2.DisclosedVolume=it2->BoardLotQuantity;
			obj2.Volume=cimlist[Token].BoardLotQuantity;

        }
           MS_OM_REQUEST_TR obj;

				//obj.TransactionCode =20000;
                obj.UserId =ntohl(UserId);
				obj.TokenNo = ntohl(Token);

                strcpy(obj.InstrumentName,obj2.InstrumentName);
                PadRight(obj.InstrumentName,sizeof(obj.InstrumentName)," ");
				toUpper(obj.InstrumentName,sizeof(obj.InstrumentName));



                strcpy(obj.Symbol,obj2.Symbol);
                PadRight(obj.Symbol,sizeof(obj.Symbol)," ");
                toUpper(obj.Symbol,sizeof(obj.Symbol));



                obj.ExpiryDate =ntohl(obj2.ExpiryDate);
                obj.StrikePrice = ntohl(obj2.StrikePrice);

                strcpy(obj.OptionType,obj2.OptionType);
                PadRight(obj.OptionType,sizeof(obj.OptionType)," ");
                toUpper(obj.OptionType,sizeof(obj.OptionType));

				obj.BookType = ntohs(1);
				obj.Buy_SellIndicator =ntohs((short) BS);
				obj.GoodTillDate =ntohl(0);

				obj.BranchId =ntohs(BranchId);
				obj.TraderId =ntohl(UserId);

                sprintf(obj.BrokerId,"%d",BrokerId);
                //strcpy(obj.BrokerId,"12468");
                PadRight(obj.BrokerId,sizeof(obj.BrokerId)," ");
				toUpper(obj.BrokerId,sizeof(obj.BrokerId));



                obj.Open_Close = 'O';
                obj.Pro_ClientIndicator =ntohs(2);

                double dbl=ClientIdAuto;
				htond(dbl) ;
				obj.nnffield =dbl;
                obj.Modified_CancelledBy='T';
                /*
                obj.AON=0;
                obj.IOC=0;
                obj.GTC=0;
                obj.DAY=1;
                obj.MIT=0;
                obj.SL=0;
                obj.MARKET=0;
                obj.ATO=0;

                obj.Reserved=0;
                obj.Frozen=0;
                obj.Modified=0;
                obj.Traded=0;
                obj.MatchedInd=0;
                obj.MF=0;
*/
                //st_ord_flg_obj=Logic.Instance.OrderTypeFlag (OrderType.DAY),
				//st_ord_flg_obj = new ST_ORDER_FLAGS
				//{
					//STOrderFlagIn = Logic.Instance.GetBitsToByteValue(0, 0, 0, 1, 0, 0, 0, 0),
					//STOrderFlagOut = Logic.Instance.GetBitsToByteValue(0, 0, 0, 0, 0, 0, 0, 0),
				//}
                obj.FlagIn=8;
                obj.FlagOut=16;

                strcpy(obj.AccountNumber,"");
                PadRight(obj.AccountNumber,sizeof(obj.AccountNumber)," ");
                toUpper(obj.AccountNumber,sizeof(obj.AccountNumber));


                strcpy(obj.Settlor,"");
                PadRight(obj.Settlor,sizeof(obj.Settlor)," ");
                toUpper(obj.Settlor,sizeof(obj.Settlor));

				obj.Length =ntohs(sizeof(MS_OM_REQUEST_TR));
				obj.MsgCount =ntohs(1);
                //char buffer[sizeof(obj)];
                //memcpy(buffer,&obj,sizeof(obj));
                //return buffer;
            return obj;
		}



///end ReturnPack................................



public:
  char *HandleOnFOPairSubscription (char* buffer)
{
     strFOPAIR _FOpairObj;

	 memcpy(&_FOpairObj,buffer,sizeof(_FOpairObj));

    map<int,TokenPartnerDetails>Primeleg;

    if(Primeleg.find(_FOpairObj.TokenNear)==Primeleg.end())
    {
        Primeleg[_FOpairObj.TokenNear]=InitTokenDetails(_FOpairObj.TokenNear, _FOpairObj.TokenFar, _FOpairObj.PORTFOLIONAME);
    }
if(Primeleg.find(_FOpairObj.TokenFar)==Primeleg.end())
{
Primeleg[_FOpairObj.TokenFar]=InitTokenDetails(_FOpairObj.TokenFar, _FOpairObj.TokenNear, _FOpairObj.PORTFOLIONAME);
}
			SymbolDictionary[_FOpairObj.TokenNear].FARTOKEN = _FOpairObj.TokenFar;
			SymbolDictionary[_FOpairObj.TokenNear].NEARTOKEN = _FOpairObj.TokenNear;
			SymbolDictionary[_FOpairObj.TokenNear].PFNUMBER = _FOpairObj.PORTFOLIONAME ;
			SymbolDictionary[_FOpairObj.TokenNear].BLQ= Primeleg[_FOpairObj.TokenFar].CF.BoardLotQuantity;

			SymbolDictionary[_FOpairObj.TokenFar].FARTOKEN = _FOpairObj.TokenFar;
			SymbolDictionary[_FOpairObj.TokenFar].NEARTOKEN = _FOpairObj.TokenNear ;
			SymbolDictionary[_FOpairObj.TokenFar].PFNUMBER = _FOpairObj.PORTFOLIONAME;
			SymbolDictionary[_FOpairObj.TokenFar].BLQ= Primeleg[_FOpairObj.TokenFar].CF.BoardLotQuantity;


//  Packet
	if (_NMPACK.find(_FOpairObj.TokenFar) ==_NMPACK.end())
	{
        memset(&_NMPACK[_FOpairObj.TokenFar],0,sizeof(MS_OE_REQUEST_TR));

		memcpy(&_NMPACK [_FOpairObj.TokenFar].NEARMONTHBUYMARKET,&ReturnNearPack(_FOpairObj.TokenNear, BUY, 1,SymbolDictionary[_FOpairObj.TokenFar].BLQ),sizeof(MS_OE_REQUEST_TR));
        memcpy(&_NMPACK [_FOpairObj.TokenFar].NEARMONTHSELLMARKET,&ReturnNearPack(_FOpairObj.TokenNear, SELL, 1,SymbolDictionary[_FOpairObj.TokenFar].BLQ),sizeof(MS_OE_REQUEST_TR));


		memcpy(&_NMPACK [_FOpairObj.TokenFar].FARMONTHBUY,&ReturnNearPack(_FOpairObj.TokenFar, BUY, 1),sizeof(MS_OE_REQUEST_TR));
		memcpy(&_NMPACK [_FOpairObj.TokenFar].FARMONTHSELL,&ReturnNearPack(_FOpairObj.TokenFar, SELL, 1),sizeof(MS_OE_REQUEST_TR));


        memcpy(&_NMPACK [_FOpairObj.TokenFar].FARMONTHMODBUY,&ReturnModificationPack(_FOpairObj.TokenFar,BUY),sizeof(MS_OM_REQUEST_TR));
        memcpy(&_NMPACK [_FOpairObj.TokenFar].FARMONTHMODSELL,&ReturnModificationPack(_FOpairObj.TokenFar, SELL),sizeof(MS_OM_REQUEST_TR));


        memset(&_NMPACK[_FOpairObj.TokenNear],0,sizeof(MS_OE_REQUEST_TR));


	}

	_FOPAIRDIFF [_FOpairObj.PORTFOLIONAME].BNSFMNQ=0;
	_FOPAIRDIFF [_FOpairObj.PORTFOLIONAME].BFSNMNQ=0;
	_FOPAIRDIFF [_FOpairObj.PORTFOLIONAME].BNSFMXQ=0;
	_FOPAIRDIFF [_FOpairObj.PORTFOLIONAME].BFSNMXQ=0;

        _dataHolder.CleanRecord(_FOpairObj.TokenFar);
        _dataHolder.CleanRecord(_FOpairObj.TokenNear);


        SubscribeTokenOnFalse(_FOpairObj.TokenFar);
        SubscribeTokenOnFalse(_FOpairObj.TokenNear);


// SubsCription



 //  BOARD_LOT_IN_TRBuy(_FOpairObj.TokenFar,ntohl(25),ntohl(1828795));
 //cout << "Subscrition Done for PORTFOLIONAME " << _FOpairObj.PORTFOLIONAME<<" NEARTOKEN " << _FOpairObj.TokenNear<< " FARTOKEN " << _FOpairObj.TokenFar << endl;

//**************
/*

 MS_OE_REQUEST_TR objA=_NMPACK [_FOpairObj.TokenFar].NEARMONTHBUYMARKET;

 cout<<"NEARMONTHBUYMARKET -->>>size: "<<sizeof(buffer)<<"TokenNo "<<htonl(objA.TokenNo)<<" ExpiryDate "<<htonl(objA.ExpiryDate)
                <<"obj.AccountNumber-"<<objA.AccountNumber
                  <<"obj.InstrumentName-"<<objA.InstrumentName
                   <<"BUYSELL-"<<htons(objA.Buy_SellIndicator)
                    <<"Length-"<<htons(objA.Length)
                <<":"<<sizeof(objA.AccountNumber)
                <<"--> FlagIn: "<< objA.FlagIn<<" openClose: "<<objA.Open_Close
                <<" BuySell "<<htons(objA.Buy_SellIndicator)
                <<" Volume "<<htonl(objA.Volume)
                 <<" Price "<<htonl(objA.Price)
                <<" DisclosedVolume: "<<htonl(objA.DisclosedVolume)
                <<endl;

                MS_OM_REQUEST_TR obj=_NMPACK [_FOpairObj.TokenFar].FARMONTHMODBUY;
                cout<<"Struct -->>>size: "<<sizeof(buffer)<<"TokenNo "<<htonl(obj.TokenNo)<<" ExpiryDate "<<htonl(obj.ExpiryDate)
                <<"obj.AccountNumber-"<<obj.AccountNumber
                  <<"obj.InstrumentName-"<<obj.InstrumentName
                   <<"BUYSELL-"<<htons(obj.Buy_SellIndicator)
                    <<"Length-"<<htons(obj.Length)
                <<":"<<sizeof(obj.AccountNumber)
                <<"--> FlagIn: "<< obj.FlagIn<<" openClose: "<<obj.Open_Close

                <<" Volume "<<htonl(obj.Volume)
                     <<" Price "<<htonl(obj.Price)
                <<" DisclosedVolume: "<<htonl(obj.DisclosedVolume)
                <<endl;



                MS_OM_REQUEST_TR obj2=_NMPACK [_FOpairObj.TokenFar].FARMONTHMODSELL;
                cout<<"FARMONTHMODBUY -->>>size: "<<sizeof(buffer)<<"TokenNo "<<htonl(obj2.TokenNo)<<" ExpiryDate "<<htonl(obj2.ExpiryDate)
                <<"AccountNumber-"<<obj2.AccountNumber
                  <<".InstrumentName-"<<obj2.InstrumentName
                   <<"BUYSELL-"<<htons(obj2.Buy_SellIndicator)
                    <<"Length-"<<htons(obj2.Length)
                <<":"<<sizeof(obj2.AccountNumber)
                <<"--> FlagIn: "<< obj2.FlagIn<<" openClose: "<<obj2.Open_Close
 <<" BuySell "<<htons(obj2.Buy_SellIndicator)
                <<" Volume "<<htonl(obj2.Volume)
                 <<" Price "<<htonl(obj2.Price)
                <<" DisclosedVolume: "<<htonl(obj2.DisclosedVolume)
                <<endl;


*/

//*****************


}

int count;

 void HandleOnFOPairUnSubscription (char* buffer)
		{

		 strFOPAIR _FOpairObj;

	 	 	 memcpy(&_FOpairObj,buffer,sizeof(_FOpairObj));


            UnSubscribeTokenOnTrue(_FOpairObj.TokenFar);
            UnSubscribeTokenOnTrue(_FOpairObj.TokenNear);


            _FOPAIRDIFF [_FOpairObj.PORTFOLIONAME].BNSFMNQ=0;
            _FOPAIRDIFF [_FOpairObj.PORTFOLIONAME].BFSNMNQ=0;
            _FOPAIRDIFF [_FOpairObj.PORTFOLIONAME].BNSFMXQ=0;
            _FOPAIRDIFF [_FOpairObj.PORTFOLIONAME].BFSNMXQ=0;

	 	 	 //_udpObj.UnSubscribe = _FOpairObj.TokenNear;
	 	 	 //_udpObj.UnSubscribe = _FOpairObj.TokenFar;

                CancelBuyOrder(_FOpairObj.TokenFar);
                CancelSellOrder(_FOpairObj.TokenFar);


    //cout << "FoPairUnsubscription is completed for PORTFOLIONAME " << _FOpairObj.PORTFOLIONAME<<" NEARTOKEN " << _FOpairObj.TokenNear<< " FARTOKEN " << _FOpairObj.TokenFar << endl;

		}


void HandleOnFOPairDiff (char* buffer)
	{
			struct FOPAIRDIFF _INpairDiff;

            memcpy(&_INpairDiff,buffer,sizeof(_INpairDiff));

			_FOPAIRDIFF [_INpairDiff.PORTFOLIONAME] = _INpairDiff;

            CancelSellOrder(_INpairDiff.TokenFar);
            CancelBuyOrder(_INpairDiff.TokenFar);

			cout<<"Pairdiff Recieved...Far "<<_INpairDiff.TokenFar<<" Near "<<_INpairDiff.TokenNear<<" PFNUMBER "<<SymbolDictionary[_INpairDiff.TokenNear].PFNUMBER<<" BFSNDIFF "<<_INpairDiff.BFSNDIFF<<" BNSFDIFF "<<_INpairDiff.BNSFDIFF<<" BNSFMNQ "<<_INpairDiff.BNSFMNQ<<" BFSNMNQ "<<
			_INpairDiff.BFSNMNQ<<" BNSFMXQ "<<_INpairDiff.BNSFMXQ<<" BFSNMXQ "<<_INpairDiff.BFSNMXQ<<" TickCount "<<_INpairDiff.TickCount<<
			"  Depth Or Bid " << _INpairDiff.Depth_Best<< endl;

		}


void Datasubscriber ()
{
    const char* addr = DATANANOPATH.c_str();

    cout << "Datasubscriber Start: "<<addr<<"  ClientIdAuto: "<<ClientIdAuto<<endl;

  Datasock = nn_socket(AF_SP, NN_SUB);
  assert(Datasock >= 0);
  int msg =111;

  // int setBufSize= 100 * 1024 * 1024 ;

   // nn_setsockopt(Datasock,NN_SOL_SOCKET,NN_RCVBUF,&setBufSize,sizeof(setBufSize));


  assert(nn_setsockopt(Datasock, NN_SUB, NN_SUB_SUBSCRIBE,&msg , sizeof(msg)) >= 0);

  assert(nn_connect(Datasock, addr) >= 0);

  FinalPrice* buf;

  buf =(FinalPrice *) std::malloc(sizeof(FinalPrice));

  int fpSize= sizeof(FinalPrice);

  while (!IsExit)
  {

    int bytes = nn_recv(Datasock, buf,fpSize, 0);

    if(bytes > 0)
    {

       // cout << "Data arrived "<< endl<< endl;
        OnDataArrived(buf);

    }

    /*nn_freemsg(&buf);*/
    //memset(&buf, 0, 200);
  }
 cout << "Datasubscriber End" << endl;
 // delete(buf);
 //   pthread_mutex_destroy(&mutex2);
  }
void SubscribeTokenOnFalse(int Token)
{
            if(_SubStatus.find(Token)==_SubStatus.end())
            {
                _SubStatus[Token]= false;
            }

            if(!_SubStatus[Token])
			{
			//_udpObj.Subscribe = _FOpairObj.TokenFar;
                _SubStatus[Token]=true;
                _dataHolder.InsertRecord(Token);
                SubscribeToken(Token);
                cout << "Symbol " << Token << " subscribed successfully "<< endl;
			}

}

void UnSubscribeTokenOnTrue(int Token)
{
     if(_SubStatus.find(Token)!=_SubStatus.end())
     {
        if(_SubStatus[Token])
        {
			//_udpObj.Subscribe = _FOpairObj.TokenFar;
            _SubStatus[Token]=false;
            UnSubscribeToken(Token);
            cout << "Symbol " << Token << " UnSubscribed successfully "<< endl;

           _dataHolder.CleanRecord(Token);
        }
    }
}



void OnDataArrived(FinalPrice* _fp)
{

 /*if(_fp->subToken==111)
    {

        if(SymbolDictionary.find(_fp->Token)!= SymbolDictionary.end())
        {
            if(SymbolDictionary[_fp->Token].FARTOKEN== _fp->Token)
            {
                // cout << "Cancel order for "<<  _fp->Token << endl;
                CancelBuyOrder(_fp->Token);
                CancelSellOrder(_fp->Token);
            }

        }
        return;
    }*/

 _dataHolder.UpdatePrice(*_fp);

//cout<<"Data Recieved .."<<"Token :"<<_fp->Token<<" MAXBID :"<<_fp->MAXBID<<" MINASK :"<<_fp->MINASK<<"--"<<endl;

//cout << " Data 1" << endl<<endl;
    int Token = _fp->Token;


//cout << " Data 2 Token " <<Token<< endl<<endl;

	int NearMonth;
	int FarMonth;
	int PFNumber;
    NFToken _NFToken = SymbolDictionary[Token];

	NearMonth=_NFToken.NEARTOKEN;

	FarMonth= _NFToken.FARTOKEN;

//cout << " Data 3 NearMonth " <<NearMonth<< " FarMonth "<<FarMonth<< endl<<endl;

	FinalPrice NearFP;// = _dataHolder.GetRecord(NearMonth);

	FinalPrice FarFP;// = _dataHolder.GetRecord(FarMonth);

    if(Token == NearMonth)
    {
        NearFP = *_fp;
        FarFP = _dataHolder.GetRecord(FarMonth);
        //cout << " Data 4A NearMonth BID " <<NearFP.MAXBID << " ASK " << NearFP.MINASK << " FarMonth BID "<<FarFP.MAXBID << " ASK "<< FarFP.MINASK <<  endl<<endl;
    }
    else if(Token == FarMonth)
    {
        FarFP = *_fp;
        NearFP = _dataHolder.GetRecord(NearMonth);
        //cout << " Data 4B NearMonth BID " <<NearFP.MAXBID << " ASK " << NearFP.MINASK << " FarMonth BID "<<FarFP.MAXBID << " ASK "<< FarFP.MINASK <<  endl<<endl;
    }

	if(NearFP.Token <=0 || FarFP.Token<=0)
	{
           // cout << "INside NearFP  FarFPQ " << NearFP.Token << " " << FarFP.Token<< endl <<endl;
            return;
    }


   // cout << "Token Near BID " << NearFP.MAXBID << " ASK " << NearFP.MINASK << " Token Far BID " << FarFP.MAXBID << " ASK " << FarFP.MINASK << endl<< endl;

	PFNumber = _NFToken.PFNUMBER;

	int _BNSFMNQ = 0;
	int _BFSNMNQ = 0;
	int _BNSFMXQ = 0;
	int _BFSNMXQ = 0;
	int BFSNDIFF = 0;
	int BNSFDIFF = 0;

    bool DEPTHORBEST = false;

	_BNSFMNQ = _FOPAIRDIFF [PFNumber].BNSFMNQ;
	_BFSNMNQ = _FOPAIRDIFF [PFNumber].BFSNMNQ;
	_BNSFMXQ = _FOPAIRDIFF [PFNumber].BNSFMXQ;
	_BFSNMXQ = _FOPAIRDIFF [PFNumber].BFSNMXQ;
//	DEPTHORBEST = _FOPAIRDIFF [PFNumber].Depth_Best ==Depth ? true  :false ;

	BFSNDIFF = (int)_FOPAIRDIFF [PFNumber].BFSNDIFF;
	BNSFDIFF = (int)_FOPAIRDIFF [PFNumber].BNSFDIFF;


	int dFarBestBuyRate = FarFP.MAXBID ;
	int dNearBestBuyRate = NearFP.MAXBID;
	int dFarBestSellRate = FarFP.MINASK;
	int dNearBestSellRate = NearFP.MINASK;

//cout<<"BFSNDIFF: "<<BFSNDIFF<<" BNSFDIFF: "<<BNSFDIFF<<endl;

//	=======================================================BuyNear SellFar============================

	int dAsksDifference = dFarBestSellRate - dNearBestSellRate;

//cout<<"dAsksDifference: "<<dAsksDifference<<" dFarBestSellRate: "<<dFarBestSellRate<<" dNearBestSellRate: "<<dNearBestSellRate<<endl;

	OrderDetails OrdDetailSell=_OrderDetailsSell[FarMonth] ;


//	cout << "DEPTHORBEST " << DEPTHORBEST << endl<<endl;
//cout<<"Data Recieved_1 .."<<endl;
   cout<<"PFNumber "<< PFNumber <<" Sell orderstat : "<<(OrderStatus)OrdDetailSell.orderstat<<" BNSFDIFF :"<<BNSFDIFF<<" _BNSFMNQ :"<<_BNSFMNQ <<" OrdDetailSell.TotalTraded "<<OrdDetailSell.TotalTraded <<""<<endl;
//cout<<"BFSNDIFF "<<BFSNDIFF<<" BNSFDIFF :"<<BNSFDIFF<<" _BNSFMNQ :"<<_BNSFMNQ<<"_BFSNMNQ "<<_BFSNMNQ<<"_BNSFMXQ"<<_BNSFMXQ <<endl;

	if((OrdDetailSell.orderstat == (OrderStatus)TRADE || OrdDetailSell.orderstat == (OrderStatus)CANCEL ||
						OrdDetailSell.orderstat == (OrderStatus)REJECTED || OrdDetailSell.orderstat == (OrderStatus)NONE)
						&& BNSFDIFF != 0 && _BNSFMNQ > 0 && _BNSFMXQ > 0 && OrdDetailSell.TotalTraded < _BNSFMXQ && OrdDetailSell.TotalTraded >= 0
						&& dFarBestSellRate > 0 && dNearBestSellRate > 0 )
						// dAsksDifference > dHitsDifference
				 {		//	 ( FAROPENSELLQTY ==0 && BNSFDIFF != 0 && dAsksDifference > dHitsDifference && _BNSFMNQ > 0 && _BNSFMXQ > 0 && FARTRADEDSELLQTY < _BNSFMXQ && FARTRADEDSELLQTY >= 0 )

                        //boost::mutex::scoped_lock lock(mutex,boost::try_to_lock);
                        //_mutex[PFNumber].lock();
                        {
                            //cout << " Inside Sell side " << endl<< endl;

                            int dFarMonthSellRate = 0;

                        if(DEPTHORBEST)
                        {
                            dFarMonthSellRate = dNearBestSellRate + BNSFDIFF;
                          //  cout << " DEPTHORBEST dFarMonthSellRate " << dFarMonthSellRate << endl<<endl;
                        }
                        else if(BNSFDIFF <= dAsksDifference)
                        {
                            if (BNSFDIFF < dAsksDifference) {
                                dFarMonthSellRate = (dFarBestSellRate) - 5;

                            } else if (BNSFDIFF == dAsksDifference) {
                                dFarMonthSellRate = (dNearBestSellRate) + (BNSFDIFF);
                            }
                           // cout << " BEST dFarMonthSellRate " << dFarMonthSellRate << endl<<endl;
                        }

                          //  cout <<"dFarMonthSellRate " << dFarMonthSellRate << "  OrdDetailSell.orderstat  " <<  OrdDetailSell.orderstat  << " PENDING "<< PENDING<<endl<<endl;
                            if (dFarMonthSellRate > 0 && OrdDetailSell.orderstat != (OrderStatus)PENDING && dFarBestSellRate > 0 && dNearBestSellRate > 0)
                            {

                        	//cout << " Inside dFarMonthSellRate > 0 " << endl<< endl;

                                OrdDetailSell.orderstat = (OrderStatus)PENDING;
                            //	OrdDetailSell.orderstat = (OrderStatus)OPEN;
                                OrdDetailSell.Token = FarMonth;
                                OrdDetailSell.OrderNumber = -1;
                                OrdDetailSell.ReplaceCount = 1;

                                OrdDetailSell.OrderType = (_orderType)LIMIT;
                                OrdDetailSell.Price = dFarMonthSellRate;
                                OrdDetailSell.Qty = _FOPAIRDIFF [PFNumber].BNSFMNQ;
                                OrdDetailSell.TotalTraded += 1;
                                OrdDetailSell.side = (BUYSELL)SELL;

                                _OrderDetailsSell[FarMonth] = OrdDetailSell;

                                BOARD_LOT_IN_TRSell (
                                    FarMonth,
                                //	ntohl(OrdDetailSell.Qty * BLQ),
                                    ntohl(OrdDetailSell.Price)
                                );
                            cout << " ================================================================================================================================="<<endl<<endl;
                            cout <<"New Sell Order Placed PFNumber "<< PFNumber << " dAsksDifference "<< dAsksDifference <<  " BNSFDIFF " << BNSFDIFF << " dFarMonthSellRate "<< dFarMonthSellRate<<endl<<endl;
                            cout << " Near Bid "<< NearFP.MAXBID <<" Near Ask "<< NearFP.MINASK << " Far Bid " << FarFP.MAXBID << " Far Ask" << FarFP.MINASK<<endl<<endl;
                            cout << " ================================================================================================================================="<<endl<<endl;
                            }

                        }
                        //_mutex[PFNumber].unlock();
//cout<<"Data Recieved_2 .."<<endl;
					}

	 else if(OrdDetailSell.orderstat == (OrderStatus)OPEN || OrdDetailSell.orderstat == (OrderStatus)REPLACED)

					 {


                        if(DEPTHORBEST)
                        {
                            if(dNearBestSellRate > 0 )
                            {
                            int dQuoteRateDepth = dNearBestSellRate + BNSFDIFF;
                            int Prevprice = OrdDetailSell.Price;
								OrdDetailSell.Price = dQuoteRateDepth;
								if (Prevprice != dQuoteRateDepth) {
									OrdDetailSell.orderstat = (OrderStatus)PENDING;
                                //    OrdDetailSell.orderstat = (OrderStatus)REPLACED;

									ORDER_MOD_IN_TR (
									                FarMonth,
									            //    ntohl( OrdDetailSell.Qty * BLQ),
									                ntohl(dQuoteRateDepth),(BUYSELL)SELL

									);
 //cout << " Inside sell modify order sent  > " << endl<< endl;

									OrdDetailSell.ReplaceCount += 1;

									_OrderDetailsSell[FarMonth] = OrdDetailSell;

                              //  cout << " Depth Mod Sell "<<dQuoteRateDepth<<"  dNearBestSellRate "<<dNearBestSellRate <<endl<<endl;
                                }
                            }

                        }
                        else
                        {

                           // cout << " Best Mod Sell "<<endl<<endl;
                            if (dAsksDifference > BNSFDIFF && dFarBestSellRate > 0)
                            {
								 int dQuoteRate= (dFarBestSellRate) - 5;

								int Prevprice = 0;
								Prevprice = OrdDetailSell.Price;
								OrdDetailSell.Price = dQuoteRate;
								if (Prevprice != dFarBestSellRate && Prevprice != dQuoteRate) {
									OrdDetailSell.orderstat = (OrderStatus)PENDING;
                                //    OrdDetailSell.orderstat = (OrderStatus)REPLACED;

									ORDER_MOD_IN_TR (
									                FarMonth,
									            //    ntohl( OrdDetailSell.Qty * BLQ),
									                ntohl(dQuoteRate),(BUYSELL)SELL

									);
 //cout << " Inside sell modify order sent  > " << endl<< endl;

									OrdDetailSell.ReplaceCount += 1;

									_OrderDetailsSell[FarMonth] = OrdDetailSell;

                      //          cout<<"step 2 "<<endl;



								}
							} else if (dAsksDifference == BNSFDIFF && dNearBestSellRate > 0 ) {

								int dQuoteRateM = (dNearBestSellRate) + BNSFDIFF;
								int Prevprice = 0;
								Prevprice = OrdDetailSell.Price;
								OrdDetailSell.Price = dQuoteRateM;
						//		  cout<<"step 3 "<<Prevprice <<" "<< dQuoteRateM<<endl;
								if (Prevprice != dQuoteRateM) {

									OrdDetailSell.orderstat = (OrderStatus)PENDING;
                                //	OrdDetailSell.orderstat = (OrderStatus)REPLACED;
									ORDER_MOD_IN_TR (
									                OrdDetailSell.Token,
									          //      ntohl( OrdDetailSell.Qty *BLQ),
									                		ntohl(dQuoteRateM),(BUYSELL)SELL
									);

 //cout << " Inside sell modify order sent  == " << endl<< endl;
									OrdDetailSell.ReplaceCount += 1;

									_OrderDetailsSell[FarMonth] = OrdDetailSell;

								}
							} else {


//							 cout << " Inside sell cancel 1 order sent  > " << endl<< endl;
								OrdDetailSell.orderstat = (OrderStatus)PENDING;
								ORDER_CANCEL_IN_TR ( OrdDetailSell.Token, (BUYSELL)SELL);
								//	OrdDetailSell.orderstat = (OrderStatus)CANCEL;
								OrdDetailSell.ReplaceCount += 1;
								OrdDetailSell.Price = 0;
								//	OrdDetailSell.TotalTraded-=1;

								_OrderDetailsSell[FarMonth] = OrdDetailSell;
						//		 cout<<"step 4 "<<endl;
							}
                        }

						} else if ((OrdDetailSell.orderstat == (OrderStatus)OPEN || OrdDetailSell.orderstat == (OrderStatus)REPLACED) && dAsksDifference < BNSFDIFF) { //Double check to cancel

 //cout << " Inside sell cancel 2 order sent  > " << endl<< endl;

							OrdDetailSell.orderstat = (OrderStatus)PENDING;
							ORDER_CANCEL_IN_TR ( OrdDetailSell.Token, (BUYSELL)SELL);
							//	OrdDetailSell.orderstat = (OrderStatus)CANCEL;
							OrdDetailSell.ReplaceCount += 1;
							OrdDetailSell.Price = 0;
							//	OrdDetailSell.TotalTraded-=1;

							_OrderDetailsSell[FarMonth] = OrdDetailSell;
                    //    cout<<"step 5 "<<endl;


						}



	//	=======================================================BuyFar SellNear============================

		//=================================================================BUY FAR SELL NEAR SECTION STARTS HERE =======================================


			int dBidsDifference = dFarBestBuyRate - dNearBestBuyRate;
			OrderDetails OrdDetailBuy =_OrderDetailsBuy[FarMonth] ;
//cout<<"Data Recieved_3 .."<<endl;
   cout<<"PFNumber " <<PFNumber <<" Buy orderstat : "<<(OrderStatus)OrdDetailBuy.orderstat<<" BNSFDIFF :"<<BNSFDIFF<<" _BNSFMNQ :"<<_BNSFMNQ <<" OrdDetailBuy.TotalTraded "<<OrdDetailBuy.TotalTraded <<""<<endl;

			if
			((OrdDetailBuy.orderstat == (OrderStatus)TRADE || OrdDetailBuy.orderstat == (OrderStatus)CANCEL ||
				OrdDetailBuy.orderstat == (OrderStatus)REJECTED || OrdDetailBuy.orderstat == (OrderStatus)NONE)
				&& BFSNDIFF != 0 && _BFSNMNQ > 0 && _BFSNMXQ > 0 && OrdDetailBuy.TotalTraded < _BFSNMXQ && OrdDetailBuy.TotalTraded >= 0
				&& dFarBestBuyRate > 0 && dNearBestBuyRate > 0)

				//dBidsDifference < dHitsDifference
{		//	 (FAROPENBUYQTY ==0   && BFSNDIFF != 0 && dBidsDifference < dHitsDifference && _BFSNMNQ > 0 && _BFSNMXQ > 0 && FARTRADEDBUYQTY < _BFSNMXQ && FARTRADEDBUYQTY >= 0)

                        //boost::mutex::scoped_lock lock(mutex,boost::try_to_lock);
                        //_mutex[PFNumber].lock();
                        {
                            //cout << " Inside Buy side " << endl<< endl;

                            int dFarMonthBuyRate = 0;
                        if(DEPTHORBEST)
                        {
                            dFarMonthBuyRate = dNearBestBuyRate + BFSNDIFF;
                           // cout << " DEPTHORBEST dFarMonthBuyRate " << dFarMonthBuyRate << endl<<endl;
                        }
                        else if(BFSNDIFF >= dBidsDifference)
                        {
                            if (BFSNDIFF > dBidsDifference) {
                                dFarMonthBuyRate = (dFarBestBuyRate) + 5;

                            } else if (BFSNDIFF == dBidsDifference) {
                                dFarMonthBuyRate = (dNearBestBuyRate) + (BFSNDIFF);
                            }
                          // cout << " BEST dFarMonthBuyRate " << dFarMonthBuyRate << endl<<endl;
//
                        }
                        //cout <<"dFarMonthBuyRate " << dFarMonthBuyRate << "  OrdDetailBuy.orderstat  " <<  OrdDetailBuy.orderstat  << " PENDING "<< PENDING<<endl<<endl;
                            if (dFarMonthBuyRate > 0  &&  OrdDetailBuy.orderstat != (OrderStatus)PENDING &&  dFarBestBuyRate > 0 && dNearBestBuyRate > 0) {

                                OrdDetailBuy.orderstat = (OrderStatus)PENDING;
                        //	    OrdDetailBuy.orderstat = (OrderStatus)OPEN;
                                OrdDetailBuy.Token = FarMonth;
                                OrdDetailBuy.OrderNumber = -1;
                                OrdDetailBuy.ReplaceCount = 1;
                                OrdDetailBuy.OrderType = (_orderType)LIMIT;
                                OrdDetailBuy.Price = dFarMonthBuyRate;
                                OrdDetailBuy.Qty = _FOPAIRDIFF [PFNumber].BFSNMNQ;
                                OrdDetailBuy.TotalTraded += 1;
                                OrdDetailBuy.side = (BUYSELL)BUY;

                                _OrderDetailsBuy[FarMonth] = OrdDetailBuy;


                                BOARD_LOT_IN_TRBuy (
                                    FarMonth,
                                    //	ntohl(OrdDetailBuy.Qty *BLQ),
                                    ntohl(OrdDetailBuy.Price)
                                );
                            cout << " ================================================================================================================================= "<<endl<<endl;
                            cout << " New Buy Order Placed PFNumber "<< PFNumber <<" dBidsDifference "<< dBidsDifference <<  " BFSNDIFF " << BFSNDIFF << " dFarMonthBuyRate "<< dFarMonthBuyRate<<endl<<endl;
                            cout << " Near Bid "<< NearFP.MAXBID <<" Near Ask "<< NearFP.MINASK << " Far Bid " << FarFP.MAXBID << " Far Ask" << FarFP.MINASK<<endl<<endl;
                            cout << " ================================================================================================================================= "<<endl<<endl;
					return;
                    }
				}
                //_mutex[PFNumber].unlock();
//cout<<"Data Recieved_4 .."<<endl;
			} else if
			(OrdDetailBuy.orderstat == (OrderStatus)OPEN || OrdDetailBuy.orderstat == (OrderStatus)REPLACED)
            {
                if(DEPTHORBEST)
                {
                    if(dNearBestBuyRate>0)
                    {
                    int dQuoteRateDepth = dNearBestBuyRate + BFSNDIFF;
                    int Prevprice = 0;
					Prevprice = OrdDetailBuy.Price;
					OrdDetailBuy.Price = dQuoteRateDepth;

					if (Prevprice != dQuoteRateDepth) {

						OrdDetailBuy.orderstat = (OrderStatus)PENDING;


						ORDER_MOD_IN_TR (
						                OrdDetailBuy.Token,
						         //       ntohl(OrdDetailBuy.Qty *BLQ),
						                		ntohl(dQuoteRateDepth),(BUYSELL)BUY
						);

						//	OrdDetailBuy.orderstat = (OrderStatus)REPLACED;
						OrdDetailBuy.ReplaceCount += 1;

						_OrderDetailsBuy[FarMonth] = OrdDetailBuy;

                        //cout << " Depth Mod Buy "<<dQuoteRateDepth<<" dNearBestBuyRate " << dNearBestBuyRate <<endl<<endl;
					}

					}

                }
                else
                {

               // cout <<"Best Mod Buy"<<endl<<endl;

				if (dBidsDifference < BFSNDIFF && dFarBestBuyRate > 0)
				{
					int dQuoteRate = (dFarBestBuyRate) + 5;
					int Prevprice = 0;
					Prevprice = OrdDetailBuy.Price;
					OrdDetailBuy.Price = dQuoteRate;

					if (Prevprice != dFarBestBuyRate && Prevprice != dQuoteRate) {

						OrdDetailBuy.orderstat = (OrderStatus)PENDING;


						ORDER_MOD_IN_TR (
						                OrdDetailBuy.Token,
						         //       ntohl(OrdDetailBuy.Qty *BLQ),
						                		ntohl(dQuoteRate),(BUYSELL)BUY
						);

						//	OrdDetailBuy.orderstat = (OrderStatus)REPLACED;
						OrdDetailBuy.ReplaceCount += 1;

						_OrderDetailsBuy[FarMonth] = OrdDetailBuy;

					}

//cout<<"Data Recieved_5 .."<<endl;
				} else if (dBidsDifference == BFSNDIFF && dNearBestBuyRate > 0) {

					int dQuoteRateM = (dNearBestBuyRate) + BFSNDIFF;
					int Prevprice = 0;
					Prevprice = OrdDetailBuy.Price;
					OrdDetailBuy.Price = dQuoteRateM;
					if (Prevprice != dQuoteRateM) {

						OrdDetailBuy.orderstat = (OrderStatus)PENDING;
						ORDER_MOD_IN_TR (
						                OrdDetailBuy.Token,
						           //     ntohl( OrdDetailBuy.Qty *BLQ),
						                		ntohl( dQuoteRateM),(BUYSELL)BUY

						);

						//	OrdDetailBuy.orderstat = (OrderStatus)REPLACED;
						OrdDetailBuy.ReplaceCount += 1;

						_OrderDetailsBuy [FarMonth]  = OrdDetailBuy;

					}
				} else {
					OrdDetailBuy.orderstat = (OrderStatus)PENDING;
					ORDER_CANCEL_IN_TR ( OrdDetailBuy.Token, (BUYSELL)BUY);

					//	OrdDetailBuy.orderstat = (OrderStatus)CANCEL;
					OrdDetailBuy.ReplaceCount += 1;
					OrdDetailBuy.Price = 0;
					//	OrdDetailBuy.TotalTraded-=1;

					_OrderDetailsBuy [FarMonth] = OrdDetailBuy;


				}
            }
			} else if((OrdDetailBuy.orderstat == (OrderStatus)OPEN || OrdDetailBuy.orderstat == (OrderStatus)REPLACED) &&
				dBidsDifference > BFSNDIFF) {

				OrdDetailBuy.orderstat = (OrderStatus)PENDING;

				ORDER_CANCEL_IN_TR (OrdDetailBuy.Token, (BUYSELL)BUY);
				//	OrdDetailBuy.orderstat = (OrderStatus)CANCEL;
				OrdDetailBuy.ReplaceCount += 1;
				OrdDetailBuy.Price = 0;
				//	OrdDetailBuy.TotalTraded-=1;

				_OrderDetailsBuy [FarMonth] = OrdDetailBuy;


			}


//cout<<"Data Recieved_end .."<<endl;

			//=================================================================BUY FAR SELL NEAR SECTION ENDS HERE =======================================



    //cout << "Data reciver exitted successfully" << endl;
}// function

 void BOARD_LOT_IN_TRBuy(int FarTokenNo, int price)  //-- 20000
		{// MS_OE_REQUEST_TR 110+26

          //  cout<<"Buy: "<<FarTokenNo<<" price "<<htonl(price)<<" size "<<sizeof (_NMPACK [FarTokenNo].FARMONTHBUY) <<endl;



                    //cout<<"BOARD_LOT_IN_TRBuy  : 1 ";
                    MS_OE_REQUEST_TR obj;
                   // cout<<"BOARD_LOT_IN_TRBuy  : 2 ";
                    memset(&obj,0,136);
                    //cout<<"BOARD_LOT_IN_TRBuy  : 3 ";
                    obj=_NMPACK [FarTokenNo].FARMONTHBUY;
                   // cout<<"BOARD_LOT_IN_TRBuy  : 4 ";
	 			//	obj.DisclosedVolume=obj.Volume=volume;
	 				obj.Price=price;
	 				//cout<<"BOARD_LOT_IN_TRBuy  : 5 ";
	 				ProcessToEnqueue((char*)&obj,136);
	 				//cout<<"BOARD_LOT_IN_TRBuy  : 6 ";

		}


 void BOARD_LOT_IN_TRSell(int FarTokenNo, int price)  //-- 20000
		{// MS_OE_REQUEST_TR 110+26

//cout<<"Sell: "<<FarTokenNo<<" price "<<htonl(price)<<" size "<<sizeof (_NMPACK [FarTokenNo].FARMONTHSELL) <<endl;

                   // cout<<"BOARD_LOT_IN_TRSell  : 1 ";
                    MS_OE_REQUEST_TR obj;
                  // cout<<"BOARD_LOT_IN_TRSell  : 2 ";
                    memset(&obj,0,136);
                  // cout<<"BOARD_LOT_IN_TRSell  : 3 ";
                    obj=_NMPACK [FarTokenNo].FARMONTHSELL;
	 			//	obj.DisclosedVolume=obj.Volume=volume;
	 				//cout<<"BOARD_LOT_IN_TRSell  : 4 ";
	 				obj.Price=price;
	 				//cout<<"BOARD_LOT_IN_TRSell  : 5 ";
	 				ProcessToEnqueue((char*)&obj,136);
	 				//cout<<"BOARD_LOT_IN_TRSell  : 6 ";
		}


 void ORDER_MOD_IN_TR(int TokenNo, int price,BUYSELL buySell)// 20040
		{//MS_OM_REQUEST_TR 138+26=164

                    //cout<<"ORDER_MOD_IN_TR  : 1 ";
                    MS_OM_REQUEST_TR obj;
                  //  cout<<"ORDER_MOD_IN_TR  : 2 ";
                    memset(&obj,0,164);
                  //  cout<<"ORDER_MOD_IN_TR  : 3 ";
  	 				switch ((short)buySell) {
	 				case 1:
	 					obj=_NMPACK [TokenNo].FARMONTHMODBUY;
	 				//	cout<<"ORDER_MOD_IN_TR  : 4 ";

	 					break;
	 				case 2:
	 					obj=_NMPACK [TokenNo].FARMONTHMODSELL;
                      //  cout<<"ORDER_MOD_IN_TR  : 5 ";
	 					break;
	 				}
	 				char TransCodeBytes[0];
                    //cout<<"ORDER_MOD_IN_TR  : 6 ";
	 				obj.TransactionCode=ModificationCode;
	 			//	obj.DisclosedVolume=obj.Volume=volume;
	 				//cout<<"ORDER_MOD_IN_TR  : 7 ";
	 				obj.Price=price;
	 				//cout<<"ORDER_MOD_IN_TR  : 8 ";

                    ProcessToEnqueue((char*)&obj,164);
                  //  cout<<"ORDER_MOD_IN_TR  : 9 ";

		}


 void ORDER_CANCEL_IN_TR(int TokenNo, BUYSELL buySell)  //-- 20070
		{// MS_OM_REQUEST_TR 138+26

        //cout<<"ORDER_CANCEL_IN_TR  : 1 ";
		MS_OM_REQUEST_TR obj;
		//cout<<"ORDER_CANCEL_IN_TR  : 2 ";
        memset(&obj,0,164);
        //cout<<"ORDER_CANCEL_IN_TR  : 3 ";
  	 				switch ((short)buySell) {
	 				case 1:
	 					obj=_NMPACK [TokenNo].FARMONTHMODBUY;
	 					//cout<<"ORDER_CANCEL_IN_TR  : 4 ";
	 					break;
	 				case 2:
	 					obj=_NMPACK [TokenNo].FARMONTHMODSELL;
                    //    cout<<"ORDER_CANCEL_IN_TR  : 5 ";
	 					break;
	 				}
	 			//	cout<<"ORDER_CANCEL_IN_TR  : 6 ";
	 				char TransCodeBytes[0];
	 				obj.TransactionCode=CancelCode;
	 				//cout<<"ORDER_CANCEL_IN_TR  : 7 ";
	 				if(obj.OrderNumber==0 || obj.OrderNumber== -1)
	 				{
                        cout << " OrderNumber invalid. Cancellation not sent " << endl;
                        return;
	 				}
                    ProcessToEnqueue((char*)&obj,164);
                  //  cout<<"ORDER_CANCEL_IN_TR  : 8 ";
		}

 //==========================================================   OUT



 void ORDER_CONFIRMATION_TR (char *buffer) //-- 20073
		{

			 MS_OE_RESPONSE_TR obj;//156
			// cout<<"ORDER_CONFIRMATION_TR  : 1 ";
            memset(&obj,0,156);
          //  cout<<"ORDER_CONFIRMATION_TR  : 2 ";
            memcpy(&obj,buffer,156);

       //cout<<"ORDER_CONFIRMATION_TR  : 3 ";
		short _BS = htons(obj.Buy_SellIndicator);
		//cout<<"ORDER_CONFIRMATION_TR  : 4 ";
		int _TKN = htonl(obj.TokenNo);
		double _OrderNo =obj.OrderNumber ;
		//cout<<"ORDER_CONFIRMATION_TR  : 5 ";
		int _price = htonl(obj.Price);
        int _Volume = htonl(obj.Volume);
       // cout<<"ORDER_CONFIRMATION_TR  : 6 ";


       // cout << "Token " << _TKN << " _OrderNo " << _OrderNo << " ORDER_CONFIRMATION_TR In" << endl;
		switch (_BS)
		{
		case  (BUYSELL)BUY:
			{
			//cout<<"ORDER_CONFIRMATION_TR  : 7 ";
            MS_OM_REQUEST_TR _obj;
            memset(&_obj,0,164);
            _obj=_NMPACK [_TKN].FARMONTHMODBUY;
            _obj.OrderNumber=obj.OrderNumber;
            _obj.DisclosedVolume =obj.DisclosedVolume ;
             _obj.DisclosedVolumeRemaining =obj.DisclosedVolumeRemaining ;
              _obj.TotalVolumeRemaining =obj.TotalVolumeRemaining ;
               _obj.Volume =obj.Volume ;
                _obj.VolumeFilledToday =obj.VolumeFilledToday ;
                 _obj.Price =obj.Price ;
                  _obj.EntryDateTime =obj.EntryDateTime ;
                   _obj.LastModified =obj.LastModified ;
                    _obj.filler =obj.filler ;

     _NMPACK [_TKN].FARMONTHMODBUY=_obj;
//cout<<"ORDER_CONFIRMATION_TR  : 8 ";
       _OrderDetailsBuy[_TKN].orderstat = (OrderStatus)OPEN;
     	_OrderDetailsBuy[_TKN].OrderNumber = _OrderNo;
     		_OrderDetailsBuy[_TKN].Price = _price;

     	//	cout << "Token " << _TKN << "  obj.OrderNumber " <<  obj.OrderNumber<< " ORDER_CONFIRMATION_TR In" << endl;
     	//	cout << "Token " << _TKN << "  _obj.OrderNumber " <<  _obj.OrderNumber << " ORDER_CONFIRMATION_TR In" << endl;
     	//	cout << "Token " << _TKN << " _NMPACK [_TKN].FARMONTHMODBUY " <<  _NMPACK [_TKN].FARMONTHMODBUY.OrderNumber << " ORDER_CONFIRMATION_TR In" << endl;
     		//cout<<"ORDER_CONFIRMATION_TR  : 9 ";
				break;
			}
		case  (BUYSELL)SELL:

			{
			//cout<<"ORDER_CONFIRMATION_TR  : 10 ";
			    MS_OM_REQUEST_TR _obj;
            memset(&_obj,0,164);
            _obj=_NMPACK [_TKN].FARMONTHMODSELL;
            _obj.OrderNumber=obj.OrderNumber;
            _obj.DisclosedVolume =obj.DisclosedVolume ;
             _obj.DisclosedVolumeRemaining =obj.DisclosedVolumeRemaining ;
              _obj.TotalVolumeRemaining =obj.TotalVolumeRemaining ;
               _obj.Volume =obj.Volume ;
                _obj.VolumeFilledToday =obj.VolumeFilledToday ;
                 _obj.Price =obj.Price ;
                  _obj.EntryDateTime =obj.EntryDateTime ;
                   _obj.LastModified =obj.LastModified ;
                    _obj.filler =obj.filler ;

//cout<<"ORDER_CONFIRMATION_TR  : 11 ";
     _NMPACK [_TKN].FARMONTHMODSELL=_obj;
     _OrderDetailsSell[_TKN].orderstat = (OrderStatus)OPEN;
     	_OrderDetailsSell[_TKN].OrderNumber = _OrderNo;
     		_OrderDetailsSell[_TKN].Price = _price;
//cout<<"ORDER_CONFIRMATION_TR  : 12 ";

     	break;
			}
		}

     //   ORDER_MOD_IN_TR(_TKN, 50,ntohl( _price+100),(BUYSELL)_BS);
	}

		 void ORDER_CXL_CONFIRMATION_TR (char *buffer) //-- 20075
	{

			 MS_OE_RESPONSE_TR obj;//156
			//  cout<<"ORDER_CONFIRMATION_TR  : 1 ";
			 memset(&obj,0,156);
			 memcpy(&obj,buffer,156);
			//  cout<<"ORDER_CONFIRMATION_TR  : 2 ";
        short _BS = htons(obj.Buy_SellIndicator);
		int _TKN = htonl(obj.TokenNo);
		// cout<<"ORDER_CONFIRMATION_TR  : 3 ";
        switch(_BS)
		{
            case (BUYSELL)BUY:
            {
                _OrderDetailsBuy[_TKN].orderstat = (OrderStatus)CANCEL;
                _OrderDetailsBuy[_TKN].TotalTraded -=1 ;
                   /* map<int,OrderDetails>::iterator iterase = _OrderDetailsBuy.find(_TKN);
                    if(iterase!=_OrderDetailsBuy.end())
                    {
                        _OrderDetailsBuy.erase(iterase);
                    }*/
                    //cout<<"ORDER_CONFIRMATION_TR  : 4 ";

              }
                break;

            case (BUYSELL)SELL:
                    _OrderDetailsSell[_TKN].orderstat = (OrderStatus)CANCEL;
                    _OrderDetailsSell[_TKN].TotalTraded -=1 ;
                    //cout<<"ORDER_CONFIRMATION_TR  : 5 ";
                    /*{
                        map<int,OrderDetails>::iterator iterase = _OrderDetailsSell.find(_TKN);
                        if(iterase!=_OrderDetailsSell.end())
                        {
                            _OrderDetailsSell.erase(iterase);
                        }
                    }*/
                break;

		}



	}
//***********************



		 void ORDER_MOD_CONFIRMATION_TR (char *buffer) //-- 20074
		{
//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step1"<<endl;

       MS_OE_RESPONSE_TR obj;//156
//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step2"<<endl;
	   memset(&obj,0,156);
	//   cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step3"<<endl;
		memcpy(&obj,buffer,156);
		//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step4"<<endl;
		short _BS = htons(obj.Buy_SellIndicator);
		//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step5"<<endl;
		int _TKN = htonl(obj.TokenNo);
		//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step6"<<endl;
		double _OrderNo =obj.OrderNumber ;
		//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step7"<<endl;
		int _price = htonl(obj.Price);
		//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step8"<<endl;

		switch (_BS) {
		case  (BUYSELL)BUY:
			{
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step9"<<endl;
            MS_OM_REQUEST_TR _obj;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step10"<<endl;
            memset(&_obj,0,164);
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step11"<<endl;
            _obj=_NMPACK [_TKN].FARMONTHMODBUY;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step12"<<endl;
            _obj.OrderNumber=obj.OrderNumber;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step13"<<endl;
            _obj.DisclosedVolume =obj.DisclosedVolume ;
			// cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step14"<<endl;
             _obj.DisclosedVolumeRemaining =obj.DisclosedVolumeRemaining ;
			// cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step15"<<endl;
              _obj.TotalVolumeRemaining =obj.TotalVolumeRemaining ;
			  //cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step16"<<endl;
               _obj.Volume =obj.Volume ;
			   //cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step17"<<endl;
                _obj.VolumeFilledToday =obj.VolumeFilledToday ;
				//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step18"<<endl;
                 _obj.Price =obj.Price ;
				 //cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step19"<<endl;
                  _obj.EntryDateTime =obj.EntryDateTime ;
				  //cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step20"<<endl;
                   _obj.LastModified =obj.LastModified ;
				   //cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step21"<<endl;
                    _obj.filler =obj.filler ;
					//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step22"<<endl;

            _NMPACK [_TKN].FARMONTHMODBUY=_obj;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step23"<<endl;

            _OrderDetailsBuy[_TKN].orderstat = (OrderStatus)REPLACED;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step24"<<endl;
     		_OrderDetailsBuy[_TKN].Price = _price;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step25"<<endl;
            break;
			}
		case  (BUYSELL)SELL:
			{
			    MS_OM_REQUEST_TR _obj;
			//	cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step26"<<endl;
            memset(&_obj,0,164);
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step27"<<endl;
            _obj=_NMPACK [_TKN].FARMONTHMODSELL;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step28"<<endl;
            _obj.OrderNumber=obj.OrderNumber;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step29"<<endl;
            _obj.DisclosedVolume =obj.DisclosedVolume ;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step30"<<endl;
             _obj.DisclosedVolumeRemaining =obj.DisclosedVolumeRemaining ;
			 //cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step31"<<endl;
              _obj.TotalVolumeRemaining =obj.TotalVolumeRemaining ;
			  //cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step32"<<endl;
               _obj.Volume =obj.Volume ;
			   //cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step33"<<endl;
                _obj.VolumeFilledToday =obj.VolumeFilledToday ;
				//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step34"<<endl;
                 _obj.Price =obj.Price ;
				 //cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step35"<<endl;
                  _obj.EntryDateTime =obj.EntryDateTime ;
				  //cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step36"<<endl;
                   _obj.LastModified =obj.LastModified ;
				   //cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step37"<<endl;
                    _obj.filler =obj.filler ;
					//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step38"<<endl;

                _NMPACK [_TKN].FARMONTHMODSELL=_obj;
				//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step39"<<endl;
                _OrderDetailsSell[_TKN].orderstat = (OrderStatus)REPLACED;
				//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step40"<<endl;
                _OrderDetailsSell[_TKN].Price = _price;
				//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step41"<<endl;

              //  cout<<_TKN<<" bs "<<_BS<<" price "<<_price<<endl;
                break;
			}
		}


	//	ORDER_CANCEL_IN_TR(_TKN,(BUYSELL)_BS);

	}


		void TRADE_CONFIRMATION_TR (char *buffer) //-- 20222
		{
		//cout<<"TRADE_CONFIRMATION_TR =>"<<"step1"<<endl;
          MS_TRADE_CONFIRM_TR obj_Trade;
		//  cout<<"TRADE_CONFIRMATION_TR =>"<<"step2"<<endl;
          memset(&obj_Trade,0,153);
		//  cout<<"TRADE_CONFIRMATION_TR =>"<<"step3"<<endl;
          memcpy(&obj_Trade,buffer,153);
		//  cout<<"TRADE_CONFIRMATION_TR =>"<<"step4"<<endl;
				int _TKN =htonl(obj_Trade.Token);
		//		cout<<"TRADE_CONFIRMATION_TR =>"<<"step5"<<endl;
				short _BS = htons(obj_Trade.Buy_SellIndicator);
		//		cout<<"TRADE_CONFIRMATION_TR =>"<<"step6"<<endl;
          if(SymbolDictionary[_TKN].FARTOKEN==_TKN)
          {
		  //cout<<"TRADE_CONFIRMATION_TR =>"<<"step7"<<endl;

			//cout<<"TRADE_CONFIRMATION_TR =>"<<"step8"<<endl;
			switch (_BS)
			{
				case (BUYSELL)BUY:
				{
				//cout<<"TRADE_CONFIRMATION_TR =>"<<"step9"<<endl;
                    MS_OE_REQUEST_TR obj_New;
				//	cout<<"TRADE_CONFIRMATION_TR =>"<<"step10"<<endl;
                    memset(&obj_New,0,136);
				//	cout<<"TRADE_CONFIRMATION_TR =>"<<"step11"<<endl;
                    obj_New = _NMPACK [_TKN].NEARMONTHSELLMARKET;
				//	cout<<"TRADE_CONFIRMATION_TR =>"<<"step12"<<endl;
					//cout << "Near Month Pack placed" << endl<< endl;
	 				ProcessToEnqueue((char*)&obj_New,136);
				//	cout<<"TRADE_CONFIRMATION_TR =>"<<"step13"<<endl;
                    _OrderDetailsBuy[_TKN].orderstat = (OrderStatus)TRADE;
				//	cout<<"TRADE_CONFIRMATION_TR =>"<<"step14"<<endl;
                }

				break;
				case (BUYSELL)SELL:
				{
			//	cout<<"TRADE_CONFIRMATION_TR =>"<<"step15"<<endl;
                    MS_OE_REQUEST_TR obj_New;
			//		cout<<"TRADE_CONFIRMATION_TR =>"<<"step16"<<endl;
                    memset(&obj_New,0,136);
			//		cout<<"TRADE_CONFIRMATION_TR =>"<<"step17"<<endl;
                    obj_New=_NMPACK [_TKN].NEARMONTHBUYMARKET;
			//		cout<<"TRADE_CONFIRMATION_TR =>"<<"step18"<<endl;
					//cout << " NearMonth Pack placed " << endl<<endl;
                    ProcessToEnqueue((char*)&obj_New,136);
			//		cout<<"TRADE_CONFIRMATION_TR =>"<<"step19"<<endl;
                    _OrderDetailsSell[_TKN].orderstat = (OrderStatus)TRADE;
			//		cout<<"TRADE_CONFIRMATION_TR =>"<<"step20"<<endl;
                }
				break;
			}
			// cout<<"Trade Far  _TKN: "<<_TKN<<" _BS: "<<_BS<<endl;
			// cout<<"TRADE_CONFIRMATION_TR =>"<<"step21"<<endl;
			}
			 else
			 {
			 // cout<<"Trade Near  _TKN: "<<_TKN<<" _BS: "<<_BS<<endl;

			  }


		}




		 void ORDER_CXL_REJ_OUT (char *buffer) //-- 2072
		{//MS_OE_REQUEST 240
//cout<<"ORDER_CXL_REJ_OUT =>"<<"step1"<<endl;
			 MS_OE_REQUEST obj;
	//		 cout<<"ORDER_CXL_REJ_OUT =>"<<"step2"<<endl;
			 memset(&obj,0,240);
		//	 cout<<"ORDER_CXL_REJ_OUT =>"<<"step3"<<endl;
             memcpy(&obj,buffer,240);
			// cout<<"ORDER_CXL_REJ_OUT =>"<<"step4"<<endl;
						    short _BS =htons(obj.Buy_SellIndicator);
				//					cout<<"ORDER_CXL_REJ_OUT =>"<<"step5"<<endl;
							int _TKN =htonl(obj.TokenNo);
					//		cout<<"ORDER_CXL_REJ_OUT =>"<<"step6"<<endl;
							short _Error = htons(obj.ErrorCode);
						//	cout<<"ORDER_CXL_REJ_OUT =>"<<"step7"<<endl;
							switch(_BS)
							{
							case (BUYSELL)BUY:
							//cout<<"ORDER_CXL_REJ_OUT =>"<<"step8"<<endl;
							if (_Error == 16273)

							_OrderDetailsBuy[_TKN].orderstat = (OrderStatus)CANCEL;

							else if(_OrderDetailsBuy[_TKN].orderstat != (OrderStatus)TRADE)
							_OrderDetailsBuy[_TKN].orderstat = (OrderStatus)REPLACED;
							break;
							case (BUYSELL)SELL:
							if (_Error == 16273)
							_OrderDetailsSell[_TKN].orderstat = (OrderStatus)CANCEL;
							else if(_OrderDetailsSell[_TKN].orderstat != (OrderStatus)TRADE)
							_OrderDetailsSell[_TKN].orderstat = (OrderStatus)REPLACED;
							break;

							}
						//	cout<<"ORDER_CXL_REJ_OUT =>"<<"step9"<<endl;
		}


		void ORDER_MOD_REJ_OUT (char *buffer) //-- 2042
		{//MS_OE_REQUEST 240
//cout<<"ORDER_MOD_REJ_OUT =>"<<"step1"<<endl;
			 MS_OE_REQUEST obj;
	//		 cout<<"ORDER_MOD_REJ_OUT =>"<<"step2"<<endl;
			 memset(&obj,0,240);
		//	 cout<<"ORDER_MOD_REJ_OUT =>"<<"step3"<<endl;
			  memcpy(&obj,buffer,240);
			//  cout<<"ORDER_MOD_REJ_OUT =>"<<"step4"<<endl;

                short _BS =htons(obj.Buy_SellIndicator);
				//cout<<"ORDER_MOD_REJ_OUT =>"<<"step5"<<endl;
				int _TKN =htonl(obj.TokenNo);
				//cout<<"ORDER_MOD_REJ_OUT =>"<<"step6"<<endl;
				short _Error = htons(obj.ErrorCode);
				//cout<<"ORDER_MOD_REJ_OUT =>"<<"step7"<<endl;


				switch(_BS)
				{
				case (BUYSELL)BUY:

				if (_Error == 16273)
				_OrderDetailsBuy[_TKN].orderstat = (OrderStatus)CANCEL;
				else if(_OrderDetailsBuy[_TKN].orderstat != (OrderStatus)TRADE)
				_OrderDetailsBuy[_TKN].orderstat = (OrderStatus)REPLACED;
				break;
				case (BUYSELL)SELL:
				if (_Error == 16273)
				_OrderDetailsSell[_TKN].orderstat = (OrderStatus)CANCEL;
				else if(_OrderDetailsSell[_TKN].orderstat != (OrderStatus)TRADE)
				_OrderDetailsSell[_TKN].orderstat = (OrderStatus)REPLACED;
				break;
				}
				//cout<<"ORDER_MOD_REJ_OUT =>"<<"step8"<<endl;
	}

    void ORDER_ERROR_OUT (char *buffer) //-- 2231
		{//MS_OE_REQUEST 240
//cout<<"ORDER_ERROR_OUT =>"<<"step1"<<endl;
		MS_OE_REQUEST obj;
	//	cout<<"ORDER_ERROR_OUT =>"<<"step2"<<endl;
		memset(&obj,0,240);
		//cout<<"ORDER_ERROR_OUT =>"<<"step3"<<endl;
        memcpy(&obj,buffer,240);
//cout<<"ORDER_ERROR_OUT =>"<<"step4"<<endl;
        short _BS =htons(obj.Buy_SellIndicator);
	//	cout<<"ORDER_ERROR_OUT =>"<<"step5"<<endl;
		int _TKN =htonl(obj.TokenNo);
		//cout<<"ORDER_ERROR_OUT =>"<<"step6"<<endl;

		switch(_BS)
		{
		case (BUYSELL)BUY:
		_OrderDetailsBuy[_TKN].orderstat = (OrderStatus)REJECTED;
		_OrderDetailsBuy[_TKN].TotalTraded -=1 ;
     //   cout<<"ORDER_ERROR_OUT BUY\n";
		break;
		case (BUYSELL)SELL:
		_OrderDetailsSell[_TKN].orderstat = (OrderStatus)REJECTED;
		_OrderDetailsSell[_TKN].TotalTraded -=1 ;
	//	cout<<"ORDER_ERROR_OUT SELL\n";
		break;
		}
cout<<"ORDER_ERROR_OUT =>"<<"step7"<<endl;
		}




  int Datasock;// = nn_socket(AF_SP, NN_SUB);
  int sock ;
void SubscribeToken (int _token)
{
    nn_setsockopt(Datasock, NN_SUB, NN_SUB_SUBSCRIBE,&_token , 4);
}
void UnSubscribeToken (int _token)
{
    nn_setsockopt(Datasock, NN_SUB, NN_SUB_UNSUBSCRIBE,&_token , 4);
}

//void Datasubscriber (void *data)
/*
void Datasubscriber ()
{
  const char* addr = DATANANOPATH.c_str();//"tcp://192.168.100.227:7070";     //For Seperate DataServer IP:Port
  // const char* addr = "inproc://DataSubPub";       //For Inbuit DataServer


cout << "Datasubscriber Start: "<<addr<<"  ClientIdAuto: "<<ClientIdAuto<<endl;

  Datasock = nn_socket(AF_SP, NN_SUB);

  int msg =111;
  nn_setsockopt(Datasock, NN_SUB, NN_SUB_SUBSCRIBE,&msg , sizeof(msg));


  nn_connect(Datasock, addr) ;
//std::mutex lock;



  FinalPrice* buf;


  buf =(FinalPrice *) malloc(sizeof(FinalPrice));


  int fpSize= sizeof(FinalPrice);


  while (!IsExit) {

    int bytes = nn_recv(Datasock, buf,fpSize, 0);

    if(bytes > 0)
    {


     //   cout<< "Recv Token : "<< buf->Token<<endl;
        OnDataArrived(buf);

    }


    //memset(&buf, 0, 200);
  }
 cout << "Datasubscriber End" << endl;
 // delete(buf);
 //   pthread_mutex_destroy(&mutex2);
  }*/

long long concat(long long x, long long y)
{
    long long temp = y;
    while (y != 0) {
        x *= 10;
        y /= 10;
    }
    return x + temp;
}

struct getTrans
{
short getTransCode;
};

 void Eventsubscriber ()
    {

    //const char* addr = "tcp://127.0.0.1:7071";
 const char* addr = "ipc:///tmp/eventpubsub.ipc";//"inproc://eventpubsub";

cout << "Eventsubscriber Start: "<<addr<<"  ClientIdAuto: "<<ClientIdAuto<<endl;

 int* msg1;
//cout << "Eventsubscriber Step:  1 "<<endl;
  sock = nn_socket(AF_SP, NN_SUB);
//  cout << "Eventsubscriber Step:  2 "<<endl;
int setBufSize= 32 * 1024 * 1024 ;

nn_setsockopt(sock,NN_SOL_SOCKET,NN_RCVBUF,&setBufSize,sizeof(setBufSize));

  //  *msg1=123;
  long lng=concat((short)(MessageType)eORDER,ClientIdAuto);
   nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));

  lng=concat((short)(MessageType)eFOPAIR,ClientIdAuto);

   nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));

   lng=concat((short)(MessageType)eFOPAIRDIFF,ClientIdAuto);
   nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));

   lng=concat((short)(MessageType)eFOPAIRUNSUB,ClientIdAuto);
   nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));

	lng=concat((short)(MessageType)eCANCELALL,ClientIdAuto);
   nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));

	lng=concat((short)(MessageType)eSTOPALL,ClientIdAuto);
   nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));

    nn_connect(sock, addr) ;

char _buffer[1024];
char buffer[1024];
 //   _buffer =(char *) std::malloc(1024);
	short TransCode;




 while (!IsExit)
 {

      //  cout << "While Loop Eventsubscriber step 1"<< endl;
        memset(&_buffer,0,1024);
       // cout << "While Loop Eventsubscriber step 1_A"<< endl;
        int size = nn_recv(sock, _buffer,1024, 0);

        if(size<1)
		{
			cout << "Some error occured in eventubsub"<<endl<<endl;
			        continue;
		}


        //cout << "While Loop Eventsubscriber step 3"<< endl;
	 	  long suId;
       //     memcpy(&suId,_buffer,sizeof(suId));
       		suId=*(long*)_buffer;
       		//cout << "While Loop Eventsubscriber step 4"<< endl;
            short MsgType =suId/1000000000000000;
            //cout << "While Loop Eventsubscriber step 5"<< endl;
	// 	 cout<< "->MsgType "<<MsgType<<" size: "<<size<<" suId "<<suId<<" MsgType "<<(char)MsgType<<" "<<MsgType<<endl;

	 	 memset(buffer,0,1024);
	 	 memcpy(buffer,_buffer+8,size-8);
	 	// cout << "While Loop Eventsubscriber step 6"<< endl;
				switch ((MessageType)MsgType)//(MessageType)BitConverter.ToInt16(_IncomingData, 0))
								{
									case (MessageType)eCANCELALL:

                                      // cout << "Cancel All called b4" <<endl;
                                        CancelAllOrder();
                                       // cout << "Cancel All called ftr" <<endl;

                                    break;

                                    case (MessageType)eSTOPALL:

                                      // cout << "Cancel All called b4" <<endl;
                                        StopAllOrder();
                                       // cout << "Cancel All called ftr" <<endl;

                                    break;
									case (MessageType)eFOPAIR:

									HandleOnFOPairSubscription(buffer);
										//	OnFOPairSubscription.Raise (OnFOPairSubscription, OnFOPairSubscription.CreateReadOnlyArgs (buffer));
											break;
									case (MessageType)eFOPAIRDIFF:
									HandleOnFOPairDiff(buffer);
										//	OnFOPairDiff.Raise (OnFOPairDiff, OnFOPairDiff.CreateReadOnlyArgs (buffer));
											break;
									case (MessageType)eFOPAIRUNSUB:
									HandleOnFOPairUnSubscription(buffer);
										//	OnFOPairUnSubscription.Raise (OnFOPairUnSubscription, OnFOPairUnSubscription.CreateReadOnlyArgs (buffer));
											break;
									case (MessageType)eORDER:
                                        TransCode=0;
										TransCode=*(short*)buffer;
										switch (htons(TransCode))
										{
											case 20073:
											ORDER_CONFIRMATION_TR (buffer);
											break;
											case 20075:
											ORDER_CXL_CONFIRMATION_TR (buffer);
											break;
											case 20074:
											ORDER_MOD_CONFIRMATION_TR (buffer);
											break;
											case 20222:
											TRADE_CONFIRMATION_TR (buffer);
											break;
											case 2072:
											ORDER_CXL_REJ_OUT (buffer);
											break;
											case 2042:
											ORDER_MOD_REJ_OUT (buffer);
											break;
											case 2231:
											ORDER_ERROR_OUT (buffer);
											break;
										}

								break;

									default:
									break;

								}
                           // cout << "While Loop Eventsubscriber step 7"<< endl;
						}
						if(IsExit)
						{
						 cout << "Eventsubscriber End" << endl;
                        cout << "Unsubscribing PubSub Event" << endl;

                        lng=concat((short)(MessageType)eORDER,ClientIdAuto);
                        nn_setsockopt(sock, NN_SUB,NN_SUB_UNSUBSCRIBE,&lng , sizeof(lng));

                        lng=concat((short)(MessageType)eFOPAIR,ClientIdAuto);

                        nn_setsockopt(sock, NN_SUB,NN_SUB_UNSUBSCRIBE,&lng , sizeof(lng));

                        lng=concat((short)(MessageType)eFOPAIRDIFF,ClientIdAuto);
                        nn_setsockopt(sock, NN_SUB,NN_SUB_UNSUBSCRIBE,&lng , sizeof(lng));

                        lng=concat((short)(MessageType)eFOPAIRUNSUB,ClientIdAuto);
                        nn_setsockopt(sock, NN_SUB,NN_SUB_UNSUBSCRIBE,&lng , sizeof(lng));

                        lng=concat((short)(MessageType)eCANCELALL,ClientIdAuto);
                        nn_setsockopt(sock, NN_SUB,NN_SUB_UNSUBSCRIBE,&lng , sizeof(lng));

                         lng=concat((short)(MessageType)eSTOPALL,ClientIdAuto);
                        nn_setsockopt(sock, NN_SUB,NN_SUB_UNSUBSCRIBE,&lng , sizeof(lng));
                          cout << "PubSub Event Unsubscribed Successfully" << endl;
                        }
//delete (_buffer);
		}


void CancelAllOrder()
	{

		for(map<int, NFToken>::iterator _it = SymbolDictionary.begin(); _it!= SymbolDictionary.end();_it++)
        {

            int FarToken = _it->second.FARTOKEN ;

            cout << "Cancellation Token Number "<< FarToken << endl<<endl;

            CancelBuyOrder(FarToken);
            CancelSellOrder(FarToken);

        }

	 cout<<"Cancel All Order SuccessFully........."<<endl;

	}


void StopAllOrder()
	{

		for(map<int, NFToken>::iterator _it = SymbolDictionary.begin(); _it!= SymbolDictionary.end();_it++)
        {

            int FarToken = _it->second.FARTOKEN ;
            int NearToken = _it->second.NEARTOKEN;

            cout << "Cancellation Token Number "<< FarToken << endl<<endl;

            UnSubscribeTokenOnTrue(FarToken);
            UnSubscribeTokenOnTrue(NearToken);

            _FOPAIRDIFF [SymbolDictionary[FarToken].PFNUMBER].BNSFMNQ=0;
            _FOPAIRDIFF [SymbolDictionary[FarToken].PFNUMBER].BFSNMNQ=0;
            _FOPAIRDIFF [SymbolDictionary[FarToken].PFNUMBER].BNSFMXQ=0;
            _FOPAIRDIFF [SymbolDictionary[FarToken].PFNUMBER].BFSNMXQ=0;

            _FOPAIRDIFF [SymbolDictionary[NearToken].PFNUMBER].BNSFMNQ=0;
            _FOPAIRDIFF [SymbolDictionary[NearToken].PFNUMBER].BFSNMNQ=0;
            _FOPAIRDIFF [SymbolDictionary[NearToken].PFNUMBER].BNSFMXQ=0;
            _FOPAIRDIFF [SymbolDictionary[NearToken].PFNUMBER].BFSNMXQ=0;



            //UnSubscribeToken(NearToken);
            //UnSubscribeToken(FarToken);




			CancelBuyOrder(FarToken);
			CancelSellOrder(FarToken);

        }


	 cout<<"Stopped All Order SuccessFully........."<<endl;
	}

       void Dispose()
	{

        cout << "Shutting Down Datasocket "<< endl;
        nn_shutdown(Datasock,0);

        cout << "Shutting Down Tradesocket "<< endl;
        nn_shutdown(sock,0);


        cout << "Calling StopAllOrder/Token Unsubscription "<<endl;
        StopAllOrder();

        cout<<"Looking for open orders"<<endl;
        CancelAllOrder();


        cout << " Waiting for 30 secs to see if any open order traded before cancelling" << endl<<endl;
       // boost::this_thread::sleep( boost::posix_time::seconds(30) );
       sleep(30);
        cout << " Time over. Time to release used resources"<<endl<<endl;
        cout << "Unsubscribing Data for Tokens and clearing SymbolDictionary" << endl;
         while (!SymbolDictionary.empty())
        {
            /*int TokenNear=0;
            int TokenFar =0;

            TokenNear = SymbolDictionary.begin()->second.NEARTOKEN;
            TokenFar = SymbolDictionary.begin()->second.FARTOKEN;
            cout << " 2 Unsubscribing Data for Token Near " << TokenNear << " Token Far " << TokenFar<< endl;
            cout << "Is Token Subscribed value "<< SymbolDictionary.begin()->second.IsSubscribe << " Value for true " << true << endl;

            UnSubscribeTokenOnTrue(TokenFar);
            UnSubscribeTokenOnTrue(TokenNear);*/

            SymbolDictionary.erase(SymbolDictionary.begin());
            cout << "SymbolDictionary Erased" << endl << endl;
        }




        cout << "Clearing OrderDetailsBuy" << endl;
        while (!_OrderDetailsBuy.empty())
        {
            _OrderDetailsBuy.erase(_OrderDetailsBuy.begin());
        }

        cout << "Clearing _OrderDetailsSell" << endl;
        while (!_OrderDetailsSell.empty())
        {
            _OrderDetailsSell.erase(_OrderDetailsSell.begin());
        }

        cout << "Clearing _NMPACK" << endl;
         while (! _NMPACK.empty())
         {
            _NMPACK.erase(_NMPACK.begin());
         }

        cout << "Clearing Dataholder" << endl;
        _dataHolder.ClearAllRecords();


        cout << "Clearing _FOPAIRDIFF" << endl;
        while (!_FOPAIRDIFF.empty())
        {
            _FOPAIRDIFF.erase(_FOPAIRDIFF.begin());
        }


        sleep(1);
        IsExit=true;
        sleep(1);

        if(producer_threads.is_thread_in(_dataThread))
        {

            cout << "Stopping running Data thread "<< producer_threads.is_thread_in(_dataThread)  << endl;
            producer_threads.remove_thread(_dataThread);
            cout << "Data Thread released from group"<< endl;
             _dataThread->~thread();
             cout << "Data Thread disposed "<< endl;
        }

        if(producer_threads.is_thread_in(_eventThread))
        {

            cout << "Stopping running Event thread "<< producer_threads.is_thread_in(_eventThread)  << endl;
            producer_threads.remove_thread(_eventThread);
             cout << "Event Thread released from group"<< endl;
             _eventThread->~thread();
              cout << "Data Thread disposed "<< endl;

        }




        cout<<"Dispose Of class Called "<<endl;

        cout<<"DisposeCPP End SuccessFully........."<<endl;
	}



void CancelBuyOrder(int Token)
{
    if (_OrderDetailsBuy [Token].orderstat == (OrderStatus)OPEN || _OrderDetailsBuy [Token].orderstat == (OrderStatus)REPLACED || _OrderDetailsBuy [Token].orderstat == (OrderStatus)PENDING)
    {
		ORDER_CANCEL_IN_TR ( Token, (BUYSELL)BUY);
		cout <<"Buy Cancelation Send Order No: " << _OrderDetailsBuy [Token].OrderNumber<<endl;
	}
}

void CancelSellOrder(int Token)
{
    if (_OrderDetailsSell [Token].orderstat == (OrderStatus)OPEN || _OrderDetailsSell[Token].orderstat == (OrderStatus)REPLACED || _OrderDetailsSell [Token].orderstat == (OrderStatus)PENDING)
    {
        ORDER_CANCEL_IN_TR (Token, (BUYSELL)SELL);
		cout <<"Sell Cancelation Send Order No: " << _OrderDetailsSell [Token].OrderNumber<<endl;
	}
}


};

static class LoadContract
{

public :
    map<int,struct Contract_File> cimlist;
		//vector<struct Contract_File> cimlist;
	//	vector<struct Contract_File> first;
	void Contract_Filefun(string CONTRACTFILEPATH)
	{

		Contract_File obj;
		char delimiter = '|';
		string line;
		string row[100];
		string acc = "";
		int cnt = 0;
		ifstream myfile;

		//myfile.open("/root/Desktop/contract.txt");                                   //36
        //myfile.open("/root/Documents/Pradeep/NanoMQ/NNFServerTest/contract.txt");  //227
        //myfile.open("/root/Nitin/NNFServerTest/contract.txt");  //226

        myfile.open(CONTRACTFILEPATH.c_str());

		while (getline(myfile, line))
		{

			int c = 0;
			for (int i = 0; i < line.length(); i++)
			{
				if (line[i] == ',' || line[i] == '|'||i+1==line.length())
				{

					row[c] = acc;
					c++;


					acc = "";

				}
				else
				{
					acc += line[i];

				}
			}
			row[c] = acc;

			if (cnt == 0)
			{

				obj.NEATFO = row[0];

				obj.VersionNumber = row[1];
				//cimlist.push_back(obj);

                cimlist[obj.Token]= obj;


			}
			else
			{
				int j = 0;


				obj.Token = atoi(row[j++].c_str());
				obj.AssetToken = atol(row[j++].c_str());

				obj.InstrumentName = row[j++];
				obj.Symbol = row[j++];
				obj.Series = row[j++];
				j++;
				obj.ExpiryDate = atoi(row[j++].c_str());
				obj.StrikePrice = atoi(row[j++].c_str());
				obj.OptionType = row[j++];
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				j++;
				obj.BoardLotQuantity  = atoi(row[j++].c_str());

				//cimlist.push_back(obj);
				cimlist[obj.Token]= obj;
			}
			cnt++;
			line = "";
		}


		myfile.close();


        cout<<"Contract_Filefun Loaded"<<": cnt="<<cnt<<endl;

	}
}_ldContract;
}
