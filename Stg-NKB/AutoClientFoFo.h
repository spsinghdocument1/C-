#include <vector>
#include <stdio.h>
#include <string.h>
#include <vector>
#include <map>
#include <arpa/inet.h>
#include "Struct.h"

#include "log.h"

#include <fstream>
#include <assert.h>
#include <nanomsg/nn.h>
#include <nanomsg/pubsub.h>
#include<iostream>
#include <unistd.h>
#include <pthread.h>
#include <cstdlib>
#include<stdio.h>
#include <sstream>


#include <boost/multi_index_container.hpp>
#include <boost/multi_index/member.hpp>
#include <boost/multi_index/ordered_index.hpp>
#include <boost/multi_index/hashed_index.hpp>
#include <boost/multi_index/tag.hpp>
#include <boost/multi_index/mem_fun.hpp>

using namespace std;
using boost::multi_index::multi_index_container;
using boost::multi_index::ordered_non_unique;

using boost::multi_index::indexed_by;
using boost::multi_index::member;
using boost::multi_index::nth_index;
using boost::multi_index::get;
using boost::multi_index::hashed_unique;
using boost::multi_index::tag;
using boost::multi_index::const_mem_fun;

//#ifndef EVENTCLASS_H
#define	EVENTCLASS_H

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
TokenList & ns;
TokenList::const_iterator nit;


public:
DataCheck():ns( _store.get<Rec::ByToken>())
{
}
 void InsertRecord(int Token)
{
   // Price_View& InsertRec = get<0>(_store);
    //InsertRec.insert(Record(Token,0,0,0));
    FinalPrice _tempFP;
    memset(&_tempFP,0,sizeof(FinalPrice));
    _tempFP.Token = Token;
    _tempFP.subToken = Token;
    Rec r1= { _tempFP};
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

	//PList & ps = store.get<Rec::ByPhone>();
	nit = ns.find(Token);


	if ( nit != ns.end() )
	{
         _fp = nit->FP;
	}


    return _fp;
}


void ClearAllRecords()
{

    for(TokenList::iterator _it= ns.begin();_it!=ns.end();_it++)
    {
        ns.erase(_it);
    }
}

};

class AutoClientFoFo
{

    DataCheck _dataHolder;
    map<int ,TokenPartnerDetails> Primeleg;

	//map<int, FinalPrice> _Datadict;
	map<int, NFToken> SymbolDictionary;
	map<int,struct FOPAIRDIFF> _FOPAIRDIFF ;

	//map<int, OptOrderPacket> _OrderDetailsCreate;
	//map<int, OptOrderPacket> _OrderDetailsReverse;

//	map<int, map<char, OrderDetails>> _OrderDetails
	//map<int,  NearMonthPacket> _NMPACK;

    map<int,_innerpack> _OptOrderPacket;

    typedef map<BUYSELL, OrderDetails> _innerOrderPack;
    map<int, _innerOrderPack> _OrderDetails;
    map<int , bool> _SubStatus;

   // boost::asio::io_service io_service;
	//UDPClient client(io_service, "localhost", "1337");
	//UDPClient client;//(io_service, "localhost", "5565");

    //map<int, map<BUYSELL, OrderDetails>> _OrderDetails;

//#define CancelCode 20070
//#define ModificationCode 20070
//*************************************************

	short  CancelCode;
	short  ModificationCode;
	long UserId;//Trader
	short BranchId;

	int BrokerId;


	MS_OE_REQUEST_TR OEObj;
	MS_OM_REQUEST_TR OMObj;

public:
 bool IsExit;
    ProgressCallback ProcessToEnqueue;
    long ClientIdAuto;
public:


	AutoClientFoFo()
	{
       // cout <<"Loaded --------------------------" << endl;


	}

	void InItClass()
	{


    start(params);

        IsExit=false;
		CancelCode=ntohs(20070);
		ModificationCode=ntohs(20040);

        BrokerId=12468;

	  // BranchId=1;    //252
      // UserId=32865;

       BranchId=4;    //100.36
       UserId=28823;
      //BranchId=1;    //sim
 //UserId=32865;
        Contract_Filefun();

       // client.Init(io_service, "localhost", "5565");
      //write_text_to_log_file("Init class called from Stg");
      cout << "Init called from stg"<< endl;
	}



	/////////

//pthread_mutex_t mutex2;


//	public:

private:
  static void *runDatasubscriber(void *my_object)
  {
   static_cast<AutoClientFoFo*>(my_object)->Datasubscriber(my_object);
  }
   static void *runEventsubscriber(void *my_object)
  {
    static_cast<AutoClientFoFo*>(my_object)->Eventsubscriber(my_object);
  }



private:
    pthread_t threadId;
    void *params;


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
		vector<struct Contract_File> cimlist;
	//	vector<struct Contract_File> first;
	void Contract_Filefun()
	{

		Contract_File obj;
		char delimiter = '|';
		string line;
		string row[100];
		string acc = "";
		int cnt = 0;

		ifstream myfile;
        bool isFileLoaded= false;

		//myfile.open("/root/Desktop/contract.txt");                                   //36
       myfile.open("/root/NKB/NNFServer/contract.txt");  //100.226


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
				cimlist.push_back(obj);




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
				//cout<<"Token= "<<obj.Token<<"StrikeP="<<obj.StrikePrice;
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

				cimlist.push_back(obj);

				isFileLoaded = true;
			}
			cnt++;
			line = "";
		}


		myfile.close();

		cout << "Contract file found " << (isFileLoaded == 1 ? "true" : "false") << endl;

        cout<<"Contract_Filefun Loaded"<<": cnt="<<cnt<<endl;
         FILE_LOG(logDEBUG) << "Contract loaded " ;

	}

		////contract File Loading.....End

		///here InitTokenDetails Call..../////////


    TokenPartnerDetails  InitTokenDetails( int FirstLeg, int alternateLeg, int PortfolioName )
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


		for (vector<Contract_File>::iterator it = cimlist.begin(); it != cimlist.end(); it++)
		{


			struct Contract_File *cf =new Contract_File();
//cout<<"Token Test=> "<<it->Token<<endl;

			if (it->Token == FirstLeg)
			{

				tpdobj.CF.Token = it->Token;
				tpdobj.CF.AssetToken = it->AssetToken;
				tpdobj.CF.InstrumentName = it->InstrumentName;
				tpdobj.CF.Symbol = it->Symbol;
				tpdobj.CF.Series = it->Series;
				tpdobj.CF.ExpiryDate = it->ExpiryDate;
				tpdobj.CF.OptionType = it->OptionType;
				tpdobj.CF.BoardLotQuantity=it->BoardLotQuantity;
				tpdobj.PartnerLeg = alternateLeg;
            	tpdobj.PortfolioName = PortfolioName;


			}


		}

		return tpdobj;

	}

///Start ReturnPack................................



public:

//char *
MS_OE_REQUEST_TR ReturnNearPack(int Token,BUYSELL BS,int Qty)
{
            //MS_OE_REQUEST_TR obj2;
		    //****we use dll for below line..................
		    //var _contract = CSV_Class.cimlist.Where (a => a.Token == Token).ToList ();


 MS_OE_REQUEST_TR obj2;
   for (vector<Contract_File>::iterator it2 = cimlist.begin(); it2 != cimlist.end(); it2++)
		{
			if (it2->Token == Token)
			{
			strncpy(obj2.InstrumentName,it2->InstrumentName.c_str(),sizeof(obj2.InstrumentName));
			//obj2.InstrumentName=it2->InstrumentName;
			strncpy(obj2.Symbol,it2->Symbol.c_str(),sizeof(obj2.Symbol));
			//obj2.Symbol=it2->Symbol;
			obj2.ExpiryDate=it2->ExpiryDate;
			obj2.StrikePrice=it2->StrikePrice;
			strncpy(obj2.OptionType,it2->OptionType.c_str(),sizeof(obj2.OptionType));
			//obj2.OptionType=it2->OptionType;
		//	obj2.DisclosedVolume=it2->BoardLotQuantity;
			obj2.Volume=it2->BoardLotQuantity;
			obj2.TokenNo=it2->Token;

			//cout<<"Read from contract file Token =>"<<it2->Token<<"StrikePrice =>"<<it2->StrikePrice<<endl;
			}
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

				//cout << "Disclosed Volume set to " << Qty*obj2.Volume << " Qty "<< Qty << " LOTSIZE " << obj2.Volume << "Disclosed "<<obj.DisclosedVolume << "Converted " << htonl(obj.DisclosedVolume)<< endl;
				obj.Volume =ntohl(Qty*obj2.Volume);
				//cout << "Volume set to " << Qty*obj2.Volume << " Qty "<< Qty << " LOTSIZE " << obj2.Volume << " Volume " << obj.Volume<< "Converted " << htonl(obj.Volume)<<endl;
				obj.Price =ntohl(0);    //

                obj.Open_Close='O';

            //    printf("\nobj.Open_Close== %c",obj.Open_Close);

				//sprintf(&obj.Open_Close,"%c",'O');

				obj.UserId =ntohl(UserId);
				obj.BranchId =ntohs(BranchId);
				obj.TraderId =ntohl(UserId);

				strcpy(obj.BrokerId,"12468");
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


           // cout<<" ReturnNearPack --->Token. :"<< Token <<" BS :"<<BS<<" InstrumentName :"<<obj2.InstrumentName<<" obj2.TokenNo :"<<obj2.TokenNo<<endl;
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
        for (vector<Contract_File>::iterator it2 = cimlist.begin(); it2 != cimlist.end(); it2++)
		{
			if (it2->Token == Token)
			{
			strncpy(obj2.InstrumentName,it2->InstrumentName.c_str(),sizeof(obj2.InstrumentName));
			//obj2.InstrumentName=it2->InstrumentName;
			strncpy(obj2.Symbol,it2->Symbol.c_str(),sizeof(obj2.Symbol));
			//obj2.Symbol=it2->Symbol;
			obj2.ExpiryDate=it2->ExpiryDate;
			obj2.StrikePrice=it2->StrikePrice;
			strncpy(obj2.OptionType,it2->OptionType.c_str(),sizeof(obj2.OptionType));
			//obj2.OptionType=it2->OptionType;
		//	obj2.DisclosedVolume=it2->BoardLotQuantity;
			obj2.Volume=it2->BoardLotQuantity;
			}
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

                strcpy(obj.BrokerId,"12468");
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
                //<<" ReturnModificationPack --->Token. :"<< Token <<" BS :"<<BS<<" InstrumentName :"<<obj2.InstrumentName<<" obj2.TokenNo :"<<obj2.TokenNo<<endl;
            return obj;
		}



///end ReturnPack................................



public:


int count;



char *HandleOnIOCPairSubscription (char* buffer)
{
    FOPAIRLEG2 _OptpairObj;

    // strFOPAIR _FOpairObj;
	 memcpy(&_OptpairObj,buffer,sizeof(_OptpairObj));


	//cout << "Strategy Type set to " << (short)Two_Leg << " Leg" <<  " Token 1  " << _OptpairObj.Token1 << " Token 2 " << _OptpairObj.Token2 <<endl;

    //if(Primeleg.find(_OptpairObj.Token1)==Primeleg.end())
    if(_OptpairObj.Token1>0)
    {
        Primeleg[_OptpairObj.Token1]=InitTokenDetails(_OptpairObj.Token1, _OptpairObj.Token2, _OptpairObj.PORTFOLIONAME);

        Primeleg[_OptpairObj.Token1].PortfolioName = _OptpairObj.PORTFOLIONAME;
    }

   // if(Primeleg.find(_OptpairObj.Token2)==Primeleg.end())
   if(_OptpairObj.Token2>0)
    {
        Primeleg[_OptpairObj.Token2]=InitTokenDetails(_OptpairObj.Token2, _OptpairObj.Token1, _OptpairObj.PORTFOLIONAME);
        Primeleg[_OptpairObj.Token2].PortfolioName = _OptpairObj.PORTFOLIONAME;
      //  cout << " 2  Inside HandleOnFOPairSubscription Token  " << _OptpairObj.Token2 << " Portfolio Number "<<  Primeleg[_OptpairObj.Token2].PortfolioName <<   "  " << _OptpairObj.PORTFOLIONAME <<endl;
    }

    if(_OptpairObj.Token3>0)
    {
        Primeleg[_OptpairObj.Token3]=InitTokenDetails(_OptpairObj.Token3, _OptpairObj.Token1, _OptpairObj.PORTFOLIONAME);
        Primeleg[_OptpairObj.Token3].PortfolioName = _OptpairObj.PORTFOLIONAME;
    }

    if(_OptpairObj.Token4>0)
    {
        Primeleg[_OptpairObj.Token4]=InitTokenDetails(_OptpairObj.Token4, _OptpairObj.Token1, _OptpairObj.PORTFOLIONAME);
        Primeleg[_OptpairObj.Token4].PortfolioName = _OptpairObj.PORTFOLIONAME;
    }

    SymbolDictionary[_OptpairObj.PORTFOLIONAME]._OptTokens = _OptpairObj;
    SymbolDictionary[_OptpairObj.PORTFOLIONAME].BLQ = Primeleg[_OptpairObj.Token1].CF.BoardLotQuantity;


    //cout << " Token1Ratio " << _OptpairObj.Token1Ratio << " Token2Ratio " << _OptpairObj.Token2Ratio << "  Token3Ratio " << _OptpairObj.Token3Ratio << endl<<endl;


        int Token = _OptpairObj.Token1;

           // cout << "Location PairDIFF Token 1" << Token << endl;

            if(_OrderDetails.find( Token)==_OrderDetails.end())
			    {
			 //   cout << "Location PairDIFF OrderDetails "<< Token << endl;
                   _innerOrderPack InnerOrderPack;

                    OrderDetails obj_order_Create;
                    OrderDetails obj_order_Reverse;

                    memset(&obj_order_Create,0,sizeof(OrderDetails));
                    memset(&obj_order_Reverse,0,sizeof(OrderDetails));

                    InnerOrderPack[_OptpairObj.Token1side==(short)BUY?BUY:SELL]= obj_order_Create;
                    InnerOrderPack[_OptpairObj.Token1side==(short)BUY?SELL:BUY]= obj_order_Reverse;

                    _OrderDetails[Token]=InnerOrderPack;




//Creating OrderPacket for tokens-----------------------

               // FOPAIRLEG2 _OptpairObj =SymbolDictionary[_OptpairObj.PORTFOLIONAME]._OptTokens ;

                _innerpack InnerPack; // This inner pack will contain Create side stg as true and reverse side as false

                //==================================== Create Side Initialization starts ==================================

                OptOrderPacket obj_Order_Pack;
                memset(&obj_Order_Pack,0,sizeof(OptOrderPacket));

               // SymbolDictionary[_OptpairObj.PORTFOLIONAME]._OptTokenDets.Token1Ratio = 1;
               // SymbolDictionary[_OptpairObj.PORTFOLIONAME]._OptTokenDets.Token2Ratio = 2;
             //   cout << "Token 1 Init Token " <<SymbolDictionary[_INpairDiff.PORTFOLIONAME]._OptTokens.Token1<<
             //    "  Side " << (BUYSELL)SymbolDictionary[_INpairDiff.PORTFOLIONAME]._OptTokens.Token1side <<
             //    "  Ratio " << SymbolDictionary[_INpairDiff.PORTFOLIONAME]._OptTokenDets.Token1Ratio<< endl;

                //Token 1 init
                memcpy(&obj_Order_Pack.FirstToken, &ReturnNearPack(_OptpairObj.Token1,
                (BUYSELL)_OptpairObj.Token1side,
                _OptpairObj.Token1Ratio),sizeof(MS_OE_REQUEST_TR));

                memcpy(&obj_Order_Pack.FirstTokenMod, &ReturnModificationPack(_OptpairObj.Token1,
                (BUYSELL)_OptpairObj.Token1side),sizeof(MS_OM_REQUEST_TR));


             //   cout << "Token 2 Init Token " <<SymbolDictionary[_INpairDiff.PORTFOLIONAME]._OptTokens.Token2<<
              //   "  Side " << (BUYSELL)SymbolDictionary[_INpairDiff.PORTFOLIONAME]._OptTokens.Token2side <<
             //    "  Ratio " << SymbolDictionary[_INpairDiff.PORTFOLIONAME]._OptTokenDets.Token2Ratio<< endl;
                 //Token 2 init
                 if(_OptpairObj.Token2>0)
                 {
                memcpy(&obj_Order_Pack.SecondToken, &ReturnNearPack(_OptpairObj.Token2,
                (BUYSELL)_OptpairObj.Token2side,
                _OptpairObj.Token2Ratio),sizeof(MS_OE_REQUEST_TR));

                memcpy(&obj_Order_Pack.SecondTokenMod, &ReturnModificationPack(_OptpairObj.Token2,
                (BUYSELL)_OptpairObj.Token2side),sizeof(MS_OM_REQUEST_TR));
                }

                //Token 3 init
                if(_OptpairObj.Token3>0)
                memcpy(&obj_Order_Pack.ThirdToken, &ReturnNearPack(_OptpairObj.Token3,
                (BUYSELL)_OptpairObj.Token3side,
                _OptpairObj.Token3Ratio),sizeof(MS_OE_REQUEST_TR));

                 //Token 4 init
                 if(_OptpairObj.Token4>0)
                memcpy(&obj_Order_Pack.FourthToken, &ReturnNearPack(_OptpairObj.Token4,
                (BUYSELL)_OptpairObj.Token4side,
                _OptpairObj.Token4Ratio),sizeof(MS_OE_REQUEST_TR));


                InnerPack[_OptpairObj.Token1side==(short)BUY?BUY:SELL]= obj_Order_Pack;
                //==================================== Create Side Initialization ends ==================================


                //==================================== Reverse Side Initialization starts ==================================

                OptOrderPacket obj_Order_PackRev;
                memset(&obj_Order_PackRev,0,sizeof(OptOrderPacket));

                //Token 1 init
                memcpy(&obj_Order_PackRev.FirstToken, &ReturnNearPack(_OptpairObj.Token1,
                (BUYSELL)(_OptpairObj.Token1side== BUY ? SELL : BUY),
                _OptpairObj.Token1Ratio),sizeof(MS_OE_REQUEST_TR));

                memcpy(&obj_Order_PackRev.FirstTokenMod, &ReturnModificationPack(_OptpairObj.Token1,
                (BUYSELL)(_OptpairObj.Token1side== BUY ? SELL : BUY)),sizeof(MS_OM_REQUEST_TR));

                 //Token 2 init
                 if(_OptpairObj.Token2>0)
                 {
                    memcpy(&obj_Order_PackRev.SecondToken, &ReturnNearPack(_OptpairObj.Token2,
                    (BUYSELL)(_OptpairObj.Token2side== BUY ? SELL : BUY),
                    _OptpairObj.Token2Ratio),sizeof(MS_OE_REQUEST_TR));

                     memcpy(&obj_Order_PackRev.SecondTokenMod, &ReturnModificationPack(_OptpairObj.Token2,
                (BUYSELL)(_OptpairObj.Token2side== BUY ? SELL : BUY)),sizeof(MS_OM_REQUEST_TR));
                }
                //Token 3 init
                if(_OptpairObj.Token3>0)
                memcpy(&obj_Order_PackRev.ThirdToken, &ReturnNearPack(_OptpairObj.Token3,
                (BUYSELL)(_OptpairObj.Token3side== BUY ? SELL : BUY),
                _OptpairObj.Token3Ratio),sizeof(MS_OE_REQUEST_TR));

                 //Token 4 init
                 if(_OptpairObj.Token4>0)
                memcpy(&obj_Order_PackRev.FourthToken, &ReturnNearPack(_OptpairObj.Token4,
                (BUYSELL)(_OptpairObj.Token4side== BUY ? SELL : BUY),
                _OptpairObj.Token4Ratio),sizeof(MS_OE_REQUEST_TR));


                InnerPack[_OptpairObj.Token1side==(short)BUY?SELL:BUY]= obj_Order_PackRev;
                //==================================== Reverse Side Initialization ends ==================================

                _OptOrderPacket[_OptpairObj.PORTFOLIONAME] = InnerPack;
                SymbolDictionary[_OptpairObj.PORTFOLIONAME]._InnerPack = InnerPack;
                cout << "Reached innerpack token subscription 1 "<< _OptpairObj.Token1<< endl;
                SubscribeTokenOnFalse(_OptpairObj.Token1);

                cout << "Reached innerpack token subscription 2 "<< _OptpairObj.Token2<< endl;
                SubscribeTokenOnFalse(_OptpairObj.Token2);

                if(_OptpairObj.Token3 > 0)
                  SubscribeTokenOnFalse(_OptpairObj.Token3);

                if(_OptpairObj.Token4 > 0)
                  SubscribeTokenOnFalse(_OptpairObj.Token4);
    }

   // cout << " SymbolDictionary Token 1 " << SymbolDictionary[_OptpairObj.PORTFOLIONAME]._OptTokens.Token1 << " Token 2 "<<
    //SymbolDictionary[_OptpairObj.PORTFOLIONAME]._OptTokens.Token2<< endl;

 if(_OrderDetails.find(_OptpairObj.Token1)!=_OrderDetails.end())
{
    SubscribeTokenOnFalse(_OptpairObj.Token1);
    SubscribeTokenOnFalse(_OptpairObj.Token2);
    if(_OptpairObj.Token3)
    SubscribeTokenOnFalse(_OptpairObj.Token3);
    if(_OptpairObj.Token4)
    SubscribeTokenOnFalse(_OptpairObj.Token4);

// cout << "Token subscribed successfully Token 1"<< _OptpairObj.Token1 << " Token 2 " << _OptpairObj.Token2<< endl;
}
else
{
    cout << "Location 1 Please apply parameters first for the Portfolio " << _OptpairObj.PORTFOLIONAME << endl;
}



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
            _dataHolder.CleanRecord(Token);
        }
    }
}

void HandleOnIOCPairUnSubscription (char* buffer)
		{

		 FOPAIRLEG2 _OptpairObj;
 	 	 memcpy(&_OptpairObj,buffer,sizeof(_OptpairObj));

         if(_OptpairObj.Token1> 0)
         UnSubscribeTokenOnTrue(_OptpairObj.Token1);
          if(_OptpairObj.Token2> 0)
         UnSubscribeTokenOnTrue(_OptpairObj.Token2);
         if(_OptpairObj.Token3> 0)
         UnSubscribeTokenOnTrue(_OptpairObj.Token3);
        if(_OptpairObj.Token4> 0)
         UnSubscribeTokenOnTrue(_OptpairObj.Token2);


        //_Datadict[SecondToken]





        SymbolDictionary[_OptpairObj.PORTFOLIONAME]._OptTokenDets.BuyMin=0;
        SymbolDictionary[_OptpairObj.PORTFOLIONAME]._OptTokenDets.SellMin=0;
        SymbolDictionary[_OptpairObj.PORTFOLIONAME]._OptTokenDets.BuyMax=0;
        SymbolDictionary[_OptpairObj.PORTFOLIONAME]._OptTokenDets.SellMax=0;

      if(_OrderDetails.find(_OptpairObj.Token1)!=_OrderDetails.end())
      {
       // cout << " Token Found in OrderDetails checking side" << endl << endl;

        if(_OrderDetails[_OptpairObj.Token1].find(BUY)!=_OrderDetails[_OptpairObj.Token1].end())
        {
          //  cout << "Buy Side Found for token in OrderDetails placing cancel" << endl << endl;

            OrderStatus Stat= (OrderStatus)_OrderDetails[_OptpairObj.Token1][BUY].orderstat;

          //  cout << "OrderStat Found " << (OrderStatus)Stat << endl << endl;

            if(Stat == (OrderStatus)OPEN || Stat == (OrderStatus)REPLACED || Stat == (OrderStatus)PENDING)
            {
          //      cout << "Stat Found placing cancel for OrderNumber " << _OrderDetails[_OptpairObj.Token1][true].OrderNumber << endl<<endl;
                ORDER_CANCEL_IN_TR ( _OptpairObj.Token1, (BUYSELL)BUY,_OptpairObj.PORTFOLIONAME);
            }
        }
        if(_OrderDetails[_OptpairObj.Token1].find(SELL)!=_OrderDetails[_OptpairObj.Token1].end())
        {
          //  cout << "Sell Side Found for token in OrderDetails placing cancel" << endl << endl;

            OrderStatus Stat= (OrderStatus)_OrderDetails[_OptpairObj.Token1][SELL].orderstat;

         //   cout << "OrderStat Found " << (OrderStatus)Stat << endl << endl;

            if(Stat == (OrderStatus)OPEN || Stat == (OrderStatus)REPLACED || Stat == (OrderStatus)PENDING)
            {
               // cout << "Stat Found placing cancel for OrderNumber " << _OrderDetails[_OptpairObj.Token1][false].OrderNumber << endl<<endl;
                ORDER_CANCEL_IN_TR ( _OptpairObj.Token1, (BUYSELL)SELL,_OptpairObj.PORTFOLIONAME);
            }
        }
      }
        cout << "OptPairUnsubscription is completed for PORTFOLIONAME " << _OptpairObj.PORTFOLIONAME<<" PrimeToken " << _OptpairObj.Token1<< endl;



    }

        TradableQty UpdateQty(NFToken _NFTokn,bool BS)
        {

            TradableQty _nfToken;
            if(BS==true)//buy
            {
                _nfToken = _NFTokn._BuyTQty;
            }
            else
            {
                 _nfToken = _NFTokn._SellTQty;
            }

            _nfToken.MINHITTK2 =
                (short)
                    (_nfToken.CurTradedTK1 < _nfToken.MINTK1BIDQTY
                        ? (_nfToken.CurTradedTK2 < _nfToken.MINTK2BIDQTY
                            ? min((_nfToken.MINTK2BIDQTY - _nfToken.CurTradedTK2),
                                (_nfToken.CurTradedTK1 - _nfToken.CurTradedTK2))
                            : 0)
                        : (_nfToken.MINTK2BIDQTY - _nfToken.CurTradedTK2));

            _nfToken.MINHITTK3 =
    (short)
        (_nfToken.CurTradedTK1 < _nfToken.MINTK1BIDQTY
            ? (_nfToken.CurTradedTK3 < _nfToken.MINTK3BIDQTY
                ? min((_nfToken.MINTK3BIDQTY - _nfToken.CurTradedTK3),
                    (_nfToken.CurTradedTK1 - _nfToken.CurTradedTK3))
                : 0)
            : (_nfToken.MINTK3BIDQTY - _nfToken.CurTradedTK3));

            return _nfToken;
        }

void HandleOnIOCPairDiff (char* buffer)
	{
			FOPAIRDIFFLEG2 _INpairDiff;
            memcpy(&_INpairDiff,buffer,sizeof(_INpairDiff));

		//	cout<< "AttemptRaised OnOptPairDIFFArrived"<<endl;


        //    cout << " Bidding_Range  " << _INpairDiff.Bidding_Range<< " firstbid  " << _INpairDiff.firstbid << " Opt_Tick  " << _INpairDiff.Opt_Tick
        //    << " Order_Depth  " << _INpairDiff.Order_Depth << " Order_Type  " << _INpairDiff.Order_Type << " Req_count  " << _INpairDiff.Req_count<<
        //    " second_leg  " << _INpairDiff.second_leg<<endl<<endl;
        //cout << " _INpairDiff " << " PayUpTicks "<< _INpairDiff.PayUpTicks<<endl<<endl;




            NFToken _NFTkn = SymbolDictionary[_INpairDiff.PORTFOLIONAME];

            CancelBuyOrder(_NFTkn._OptTokens.Token1);
            CancelSellOrder(_NFTkn._OptTokens.Token1);

            short Ratio1 = _INpairDiff.Token1Ratio;
            short Ratio2 =  _INpairDiff.Token2Ratio;
            short Ratio3 =  _INpairDiff.Token3Ratio;

            short BuyMinQty =  _INpairDiff.BuyMin;
            short BuyMaxQty = _INpairDiff.BuyMax;

            short SellMinQty =  _INpairDiff.SellMin;
            short SellMaxQty = _INpairDiff.SellMax;

            short LastBuyMax = _NFTkn._OptTokenDets.BuyMax;
            short LastSellMax = _NFTkn._OptTokenDets.SellMax;

            _NFTkn._BuyTQty.TK1MIN = (short)(BuyMinQty * Ratio1);
            _NFTkn._BuyTQty.TK2MIN = (short)(BuyMinQty * Ratio2);
            _NFTkn._BuyTQty.TK3MIN = (short)(BuyMinQty * Ratio3);

            _NFTkn._BuyTQty.TK1MAX = (short)(BuyMaxQty * Ratio1);
            _NFTkn._BuyTQty.TK2MAX = (short)(BuyMaxQty * Ratio2);
            _NFTkn._BuyTQty.TK3MAX = (short)(BuyMaxQty * Ratio3);

            _NFTkn._SellTQty.TK1MIN = (short)(SellMinQty * Ratio1);
            _NFTkn._SellTQty.TK2MIN = (short)(SellMinQty * Ratio2);
            _NFTkn._SellTQty.TK3MIN = (short)(SellMinQty * Ratio3);

            _NFTkn._SellTQty.TK1MAX = (short)(SellMaxQty * Ratio1);
            _NFTkn._SellTQty.TK2MAX = (short)(SellMaxQty * Ratio2);
            _NFTkn._SellTQty.TK3MAX = (short)(SellMaxQty * Ratio3);

            if(LastBuyMax != _INpairDiff.BuyMax)
            {
                _NFTkn._BuyTQty = BidQtyInit(_NFTkn,true);
                _NFTkn._BuyTQty =  UpdateQty(_NFTkn,true);
                cout << "Inside LastBuyMax "<< LastBuyMax << " PairDiff BuyMax " << _INpairDiff.BuyMax<<endl<<endl;
            }
            if(LastSellMax != _INpairDiff.SellMax)
            {
                _NFTkn._SellTQty = BidQtyInit(_NFTkn,false);
                _NFTkn._SellTQty = UpdateQty(_NFTkn,false);
                cout << "Inside LastSellMax "<< LastSellMax << " PairDiff SellMax " << _INpairDiff.SellMax<<endl<<endl;
            }

            cout << "Outside LastBuyMax "<< LastBuyMax << " PairDiff BuyMax " << _INpairDiff.BuyMax<<" LastSellMax "<< LastSellMax << " PairDiff SellMax " << _INpairDiff.SellMax<<endl<<endl;

            _NFTkn._OptTokenDets = _INpairDiff;

            SymbolDictionary[_INpairDiff.PORTFOLIONAME] = _NFTkn;

		}

  TradableQty BidQtyInit(NFToken _NFToken,bool BS)
        {

            TradableQty _nfToken;
            if(BS==true)//buy
            {
                _nfToken = _NFToken._BuyTQty;
            }
            else
            {
                 _nfToken = _NFToken._SellTQty;
            }
          //  int i = min((short)1,(short)_nfToken.TK1MIN);
            _nfToken.MINTK1BIDQTY = min((_nfToken.TK1MAX - _nfToken.NetTradedTK1), _nfToken.TK1MIN);
            _nfToken.MINTK2BIDQTY =(_nfToken.MINTK1BIDQTY < _nfToken.TK1MIN ? (_nfToken.TK2MAX - _nfToken.NetTradedTK2) : min((_nfToken.TK2MAX - _nfToken.NetTradedTK2), _nfToken.TK2MIN));
            _nfToken.MINTK3BIDQTY = (_nfToken.MINTK1BIDQTY < _nfToken.TK1MIN ? (_nfToken.TK3MAX - _nfToken.NetTradedTK3) : min((_nfToken.TK3MAX - _nfToken.NetTradedTK3), _nfToken.TK3MIN));
        return _nfToken;
        }

	 int GetExpectedProdPrice(  BUYSELL BS, FinalPrice FP, int Ratio,bool reverse = false)
		{

			int RetVal = 0;

			if (!reverse)
			{
				// THis case calculates the price to generate buy spread

				RetVal = BS== (BUYSELL)BUY ? (FP.MINASK*Ratio * -1) : (FP.MAXBID*Ratio);
			}
			else
			{
				// Here in case of sale actual stg with buy mode token will be sold just to make a complete trade
				RetVal = BS == (BUYSELL)BUY ? (FP.MAXBID*Ratio) : (FP.MINASK*Ratio * -1);
			}
			return RetVal;

		}

		 int GetExpectedBasePrice(BUYSELL BS, FinalPrice FP, bool reverse = false)
		{

			int RetVal = 0;

			if (!reverse)
			{
				// This case calculates the price to generate buy spread

				RetVal = BS == (BUYSELL)BUY ? (FP.MINASK * -1) : FP.MAXBID  ;
			}
			else
			{
				// Here in case of sale actual stg with buy mode token will be sold just to make a complete cycle
				// The Token with B will be sold and token with S will be bought just to reverse the Trade.
				RetVal = BS == (BUYSELL)BUY ? FP.MAXBID  : (FP.MINASK * -1);
			}

			return RetVal;

		}
		 int GetBaseRatio(int Ratio1, int Ratio2)
		{
			return Ratio1 > Ratio2 ? (int) Ratio1/(int) Ratio2 : (int)Ratio2/(int) Ratio1;

		}
		 BUYSELL GetReverse(BUYSELL BS)
		{
			return BS == (BUYSELL)BUY?(BUYSELL)SELL:(BUYSELL)BUY;
		}

std::stringstream msglog ;


int GetUpperLimit(int Value)
{
    return Value + ((0.2 * Value)/100);
}
int GetLowerLimit(int Value)
{
    return Value - ((0.2 * Value)/100);
}

/*void CancelBuyOrder(int Token)
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
}*/

void OnDataArrived(FinalPrice* _fp)
{
   if(_fp->subToken==111)
    {

     /*   if(Primeleg.find(_fp->Token)!= Primeleg.end())
        {
            if(Primeleg[_fp->Token].PrimeToken== _fp->Token)
            {
                // cout << "Cancel order for "<<  _fp->Token << endl;
               CancelBuyOrder(_fp->Token);
               CancelSellOrder(_fp->Token);
            }

        }*/
        //cout << "Cancel called for Token " << _fp->Token<< endl;
        return;
    }

 _dataHolder.UpdatePrice(*_fp);


//cout << "Token Data " << _fp->Token << endl<< endl;

 //_Datadict[_fp->Token]=*_fp;
	int FirstToken;
	int SecondToken;
	int ThirdToken;
	int FourthToken;
	int PFNumber;
	int BLQ;
    bool PhaseCreateCancel;
    bool PhaseReverseCancel;

    PFNumber = Primeleg[_fp->Token].PortfolioName;

    NFToken _NFToken= SymbolDictionary[PFNumber];

	FirstToken = _NFToken._OptTokens.Token1;
	SecondToken = _NFToken._OptTokens.Token2;
    ThirdToken = _NFToken._OptTokens.Token3;
    FourthToken = _NFToken._OptTokens.Token4;

 int PayUpTicks=0;

    PayUpTicks= _NFToken._OptTokenDets.PayUpTicks;

	FinalPrice FirstFP;
	FinalPrice SecondFP;
	FinalPrice ThirdFP;
	FinalPrice FourthFP;

   // cout << " First Token " << FirstToken << " Second Token  "<< SecondToken << " Third Token " << ThirdToken << " Fourth Token " << FourthToken<< endl;

	FirstFP = _dataHolder.GetRecord(FirstToken);
	if(SecondToken>0)
	{
        SecondFP = _dataHolder.GetRecord(SecondToken);
        SecondFP.MAXBID = SecondFP.MAXBID - PayUpTicks;
        SecondFP.MINASK = SecondFP.MINASK + PayUpTicks;
	}
	if(ThirdToken >0)
	{
        ThirdFP = _dataHolder.GetRecord(ThirdToken);
        ThirdFP.MAXBID = ThirdFP.MAXBID - PayUpTicks;
        ThirdFP.MINASK = ThirdFP.MINASK + PayUpTicks;

    }
    if(FourthToken>0)
    {
        FourthFP = _dataHolder.GetRecord(FourthToken);
        FourthFP.MAXBID = FourthFP.MAXBID - PayUpTicks;
        FourthFP.MINASK = FourthFP.MINASK + PayUpTicks;
    }


   // cout << " First Bid " << FirstFP.MAXBID << " First Ask " << FirstFP.MINASK << " Second Bid " << SecondFP.MAXBID << " Second Ask " << SecondFP.MINASK<<
   // "  Third Bid "<< ThirdFP.MAXBID << " Third Ask " << ThirdFP.MINASK<< endl<< endl;



if(SecondFP.Token <=0 || FirstFP.Token<=0 || ( ThirdToken > 0 && ThirdFP.Token<=0 )|| ( FourthToken > 0 && FourthFP.Token<=0 ))
{
 //cout << "Incomplete Data recieved "<< endl;
 return;
}



	BLQ = _NFToken.BLQ;

	//msglog.str("");
  //  msglog << "PFNumber " << PFNumber << " BLQ " << BLQ ;

   // write_text_to_log_file( msglog.str() );

//cout<<"blq "<<BLQ<<endl;

	int _CREATEMNQ = 0;
	int _REVERSEMNQ = 0;
	int _CREATEMXQ = 0;
	int _REVERSEMXQ = 0;

	int CREATEDIFF = 0;
	int REVERSEDIFF = 0;


	_CREATEMNQ = _NFToken._OptTokenDets.BuyMin;
	_REVERSEMNQ = _NFToken._OptTokenDets.SellMin;
	_CREATEMXQ = _NFToken._OptTokenDets.BuyMax;
	_REVERSEMXQ = _NFToken._OptTokenDets.SellMax;


//cout<<"_CREATEMNQ "<<_CREATEMNQ<<endl;
//cout<<"_REVERSEMNQ "<<_REVERSEMNQ<<endl;
//cout<<"_CREATEMXQ "<<_CREATEMXQ<<endl;
//cout<<"_REVERSEMXQ "<<_REVERSEMXQ<<endl;

	CREATEDIFF = (int)_NFToken._OptTokenDets.SPREADBUY;
	REVERSEDIFF = (int)_NFToken._OptTokenDets.SPREADSELL;

    int firstbuyProd =GetExpectedProdPrice((BUYSELL)_NFToken._OptTokens.Token1side, FirstFP, _NFToken._OptTokenDets.Token1Ratio);
    int secondbuyProd = GetExpectedProdPrice((BUYSELL)_NFToken._OptTokens.Token2side, SecondFP, _NFToken._OptTokenDets.Token2Ratio);
    int thirdbuyProd = ThirdToken > 0 ? GetExpectedProdPrice((BUYSELL)_NFToken._OptTokens.Token3side, ThirdFP, _NFToken._OptTokenDets.Token3Ratio)  : 0;
    int fourthbuyProd = FourthToken > 0 ? GetExpectedProdPrice((BUYSELL)_NFToken._OptTokens.Token4side, FourthFP, _NFToken._OptTokenDets.Token4Ratio)  : 0;


    //cout << " firstbuyProd " << firstbuyProd << " secondbuyProd " << secondbuyProd << " thirdbuyProd " << thirdbuyProd <<  " fourthbuyProd " << fourthbuyProd<< endl<<endl;


    int _ProdBuySpread = firstbuyProd + secondbuyProd + thirdbuyProd + fourthbuyProd;

    int firstsellProd = GetExpectedProdPrice((BUYSELL)_NFToken._OptTokens.Token1side, FirstFP, _NFToken._OptTokenDets.Token1Ratio,true);
    int secondsellProd = GetExpectedProdPrice((BUYSELL)_NFToken._OptTokens.Token2side, SecondFP, _NFToken._OptTokenDets.Token2Ratio,true);
    int thirdsellProd = ThirdToken > 0 ? GetExpectedProdPrice((BUYSELL)_NFToken._OptTokens.Token3side, ThirdFP, _NFToken._OptTokenDets.Token3Ratio,true) : 0;
    int fourthsellProd = FourthToken > 0 ? GetExpectedProdPrice((BUYSELL)_NFToken._OptTokens.Token4side, FourthFP, _NFToken._OptTokenDets.Token4Ratio,true) : 0;

  	int _ProdSellSpread = firstsellProd + secondsellProd + thirdsellProd + fourthsellProd;

	int dFirstBestBuyRate =FirstFP.MAXBID ;

	int dFirstBestSellRate = FirstFP.MINASK;

    int CreateSpreadDiff = abs(abs( _ProdBuySpread) - abs(CREATEDIFF));
    int ReverseSpreadDiff = abs(abs( _ProdSellSpread) - abs(REVERSEDIFF));

    int CreateSpreadPhase = abs( _ProdBuySpread) >= abs(CREATEDIFF);
    int ReverseSpreadPhase = abs( _ProdSellSpread) <= abs(REVERSEDIFF);


	OrderDetails OrdDetailSell=_OrderDetails[FirstToken] [_NFToken._OptTokens.Token1side  == BUY ?  SELL : BUY];//Reverse Section
	OrdDetailSell.side = _NFToken._OptTokens.Token1side  == BUY ?  SELL : BUY;
    OrdDetailSell.SIDE2 = _NFToken._OptTokens.Token2side  == BUY ?  SELL : BUY;

    //int dFirstSellRate = (OrdDetailSell.side == BUY ? (dFirstBestBuyRate - ReverseSpreadDiff) : (dFirstBestSellRate  + ReverseSpreadDiff));

    int dFirstSellRate =0;


    dFirstSellRate = (OrdDetailSell.side == BUY ? (dFirstBestSellRate- ReverseSpreadDiff) : (dFirstBestBuyRate  + ReverseSpreadDiff));


    PhaseReverseCancel =  OrdDetailSell.ReplaceCount  <= 100 ? true:false;


    //FILE_LOG(logDEBUG1) << "  Price recieved Token "<< FirstFP.Token <<" _ProdBuySpread " << _ProdBuySpread << " _ProdSellSpread " << _ProdSellSpread <<"  " ;


	if((OrdDetailSell.orderstat == (OrderStatus)TRADE || OrdDetailSell.orderstat == (OrderStatus)CANCEL ||
    OrdDetailSell.orderstat == (OrderStatus)REJECTED || OrdDetailSell.orderstat == (OrderStatus)NONE)
	&& REVERSEDIFF != 0 && _REVERSEMNQ > 0 && _REVERSEMXQ > 0 && OrdDetailSell.TotalTraded < (_REVERSEMXQ * _REVERSEMNQ)
	&& OrdDetailSell.TotalTraded >= 0 && dFirstBestSellRate>0 && dFirstBestBuyRate > 0 && dFirstSellRate > 0 )
						// dAsksDifference > dHitsDifference
	{


                if (dFirstSellRate > 0 && dFirstSellRate != abs(REVERSEDIFF) && dFirstSellRate != abs(_ProdSellSpread ))
				{

                    cout <<"Buy 1 PF "<< _NFToken._OptTokens.PORTFOLIONAME <<" -- " << __TIMESTAMP__ << endl<<endl;

                    _NFToken._SellTQty.CurTradedTK1=0;
                    _NFToken._SellTQty.CurTradedTK2=0;
                    _NFToken._SellTQty.CurTradedTK3=0;

                    _NFToken._SellTQty = BidQtyInit(_NFToken,false);

                    int TradableQty = _NFToken._SellTQty.MINTK1BIDQTY;



                    if(TradableQty > 0 && OrdDetailSell.orderstat != (OrderStatus)PENDING)
                    {
                        //cout <<"Buy 2 PF "<< _NFToken._OptTokens.PORTFOLIONAME <<" -- " << __TIMESTAMP__ << endl<<endl;
							OrdDetailSell.orderstat = (OrderStatus)PENDING;
							//OrdDetailSell.orderstat = (OrderStatus)OPEN;
							OrdDetailSell.Token = FirstToken;
							OrdDetailSell.OrderNumber = -1;
							OrdDetailSell.ReplaceCount = 1;

							OrdDetailSell.OrderType = (_orderType)LIMIT;
							OrdDetailSell.Price = dFirstSellRate;
							OrdDetailSell.Qty = TradableQty;
							//OrdDetailSell.TotalTraded += 1;
							OrdDetailSell.side = (BUYSELL)(_NFToken._OptTokens.Token1side == BUY ? SELL : BUY);
                            OrdDetailSell.SecondLegTradeCounter=0;
                            OrdDetailSell.FirstLegTradeCounter=0;
                            OrdDetailSell.ThirdLegTradeCounter=0;
                            OrdDetailSell.FourthLegTradeCounter=0;

							_OrderDetails[FirstToken][OrdDetailSell.side] = OrdDetailSell;
                           // cout << " Inside OrdDetailSell before BOARD_LOT_IN_TRSell " << dFirstSellRate  << endl;

                          //  cout << "Sell Price New " << OrdDetailSell.Price << endl<<endl;

                       // cout << " OrdDetailSell.Qty " << OrdDetailSell.Qty << " OrdDetailSell.side " << OrdDetailSell.side << " BUY " << BUY << " SELL "<< SELL<< endl;
							BOARD_LOT_IN_TRSell (
								FirstToken,
								TradableQty * BLQ,
								OrdDetailSell.Price, PFNumber,OrdDetailSell.side
							);


							cout << " Tradable Qty Sell " << TradableQty << " NetTraded "<< _NFToken._SellTQty.NetTradedTK1 << "  Max " << _NFToken._SellTQty.TK1MAX << " Left "<<(_NFToken._SellTQty.TK1MAX - _NFToken._SellTQty.NetTradedTK1)<<  endl<<endl;
                    }

                        SymbolDictionary[PFNumber] = _NFToken;
						}



					}

	 else if( (OrdDetailSell.orderstat == (OrderStatus)OPEN || OrdDetailSell.orderstat == (OrderStatus)REPLACED) && PhaseReverseCancel && dFirstSellRate > 0 && ReverseSpreadPhase)

					 {
				//	 cout<<"step 1 State:"<<dAsksDifference <<" : "<< BNSFDIFF<<" OrderStatus "<<OrdDetailSell.orderstat<<endl;
                    if(dFirstSellRate > 0)
                    {

								int dQuoteRate = dFirstSellRate;
								int Prevprice = 0;
								Prevprice = OrdDetailSell.Price;
								OrdDetailSell.Price = dQuoteRate;
								if (Prevprice != dQuoteRate) {
									OrdDetailSell.orderstat = (OrderStatus)PENDING;
                                    //OrdDetailSell.orderstat = (OrderStatus)REPLACED;

									ORDER_MOD_IN_TR (
									                FirstToken,
									            //    ntohl( OrdDetailSell.Qty * BLQ),
									                dQuoteRate,OrdDetailSell.side,
									                PFNumber
									);

                        /*FILE_LOG(logDEBUG) << "  Modify from Sell OrdDetailSell.orderstat  "  << OrdDetailSell.orderstat << " PhaseReverseCancel "
                        <<PhaseReverseCancel << " dFirstSellRate " <<  dFirstSellRate <<" dFirstBestSellRate " << dFirstBestSellRate <<
                        " Prevprice " << Prevprice << " OrdDetailSell.ReplaceCount "<<OrdDetailSell.ReplaceCount << " _ProdSellSpread " <<_ProdSellSpread
                        << " ReverseSpreadDiff " <<ReverseSpreadDiff << " Second leg BuyPrice "<< SecondFP.MINASK;*/
                                   // cout << "Sell Price Modfy " << OrdDetailSell.Price << endl<<endl;

									OrdDetailSell.ReplaceCount += 1;

									_OrderDetails[FirstToken][OrdDetailSell.side] = OrdDetailSell;
                      //          cout<<"step 2 "<<endl;
								}


                        }
						}
						else if ((OrdDetailSell.orderstat == (OrderStatus)OPEN || OrdDetailSell.orderstat == (OrderStatus)REPLACED) && (!PhaseReverseCancel || !ReverseSpreadPhase) ) { //Double check to cancel

							OrdDetailSell.orderstat = (OrderStatus)PENDING;
							ORDER_CANCEL_IN_TR ( OrdDetailSell.Token, OrdDetailSell.side,PFNumber);
							//	OrdDetailSell.orderstat = (OrderStatus)CANCEL;
							OrdDetailSell.ReplaceCount =0;
							OrdDetailSell.Price = 0;
							//	OrdDetailSell.TotalTraded-=1;

							_OrderDetails[FirstToken][OrdDetailSell.side] = OrdDetailSell;
                    //    cout<<"step 5 "<<endl;


						}



	//	=======================================================BuyFar SellNear============================

		//=================================================================BUY FAR SELL NEAR SECTION STARTS HERE =======================================



			OrderDetails OrdDetailBuy =_OrderDetails[FirstToken][_NFToken._OptTokens.Token1side] ;


	OrdDetailBuy.side = (BUYSELL)_NFToken._OptTokens.Token1side  ;
    OrdDetailBuy.SIDE2 = (BUYSELL)_NFToken._OptTokens.Token2side ;

//    int dFirstMonthBuyRate = (OrdDetailBuy.side == BUY ? (dFirstBestBuyRate -CreateSpreadDiff) : (dFirstBestSellRate + CreateSpreadDiff)) ;


 int dFirstMonthBuyRate =0;



        dFirstMonthBuyRate =(OrdDetailBuy.side == BUY ? (dFirstBestSellRate -CreateSpreadDiff) : (dFirstBestBuyRate + CreateSpreadDiff)) ;

        PhaseCreateCancel =  OrdDetailBuy.ReplaceCount  <= 100 ? true:false;

			if((OrdDetailBuy.orderstat == (OrderStatus)TRADE || OrdDetailBuy.orderstat == (OrderStatus)CANCEL ||
				OrdDetailBuy.orderstat == (OrderStatus)REJECTED || OrdDetailBuy.orderstat == (OrderStatus)NONE)
				&& CREATEDIFF != 0 && _CREATEMNQ > 0 && _CREATEMXQ > 0 && OrdDetailBuy.TotalTraded <( _CREATEMXQ * _CREATEMNQ) && OrdDetailBuy.TotalTraded >= 0
				&& dFirstBestSellRate > 0 && dFirstBestBuyRate > 0 && dFirstMonthBuyRate > 0)

				//dBidsDifference < dHitsDifference
            {		//	 (FAROPENBUYQTY ==0   && BFSNDIFF != 0 && dBidsDifference < dHitsDifference && _BFSNMNQ > 0 && _BFSNMXQ > 0 && FARTRADEDBUYQTY < _BFSNMXQ && FARTRADEDBUYQTY >= 0)



         //   cout << "Inside create dFirstMonthBuyRate " <<dFirstMonthBuyRate << endl;
				if (dFirstMonthBuyRate > 0 && dFirstMonthBuyRate!= abs( _ProdBuySpread) && dFirstMonthBuyRate!= abs(CREATEDIFF) )
				{
                    //cout <<"Sell 1 PF "<< _NFToken._OptTokens.PORTFOLIONAME <<" -- " << __TIMESTAMP__ << endl<<endl;

                    _NFToken._BuyTQty.CurTradedTK1=0;
                    _NFToken._BuyTQty.CurTradedTK2=0;
                    _NFToken._BuyTQty.CurTradedTK3=0;

                    _NFToken._BuyTQty = BidQtyInit(_NFToken,true);


                    int TradableQty = _NFToken._BuyTQty.MINTK1BIDQTY;


                    if(TradableQty > 0 && OrdDetailBuy.orderstat != (OrderStatus)PENDING)
                    {

                    cout <<"Sell 2 PF "<< _NFToken._OptTokens.PORTFOLIONAME <<" -- " << __TIMESTAMP__ << endl<<endl;

					OrdDetailBuy.orderstat = (OrderStatus)PENDING;
					//OrdDetailBuy.orderstat = (OrderStatus)OPEN;
					OrdDetailBuy.Token = FirstToken;
					OrdDetailBuy.OrderNumber = -1;
					OrdDetailBuy.ReplaceCount = 1;
					OrdDetailBuy.OrderType = (_orderType)LIMIT;
					OrdDetailBuy.Price = dFirstMonthBuyRate;
					OrdDetailBuy.Qty =  TradableQty;
					//OrdDetailBuy.TotalTraded += 1;
					OrdDetailBuy.side = (BUYSELL) _NFToken._OptTokens.Token1side ;
                    OrdDetailBuy.SecondLegTradeCounter=0;
                    OrdDetailBuy.FirstLegTradeCounter=0;
                    OrdDetailBuy.ThirdLegTradeCounter=0;
                    OrdDetailBuy.FourthLegTradeCounter=0;
				  _OrderDetails[FirstToken][OrdDetailBuy.side] = OrdDetailBuy;

//cout << " OrdDetailBuy.Qty " << OrdDetailBuy.Qty << " OrdDetailBuy.side " << OrdDetailBuy.side << " BUY " << BUY << " SELL "<< SELL<< endl;

					BOARD_LOT_IN_TRBuy (
						FirstToken,
						TradableQty *BLQ,
						OrdDetailBuy.Price,
						PFNumber,OrdDetailBuy.side
					);

                        cout << " Tradable Qty Buy " << TradableQty << " NetTraded "<< _NFToken._BuyTQty.NetTradedTK1 << "  Max " << _NFToken._BuyTQty.TK1MAX << " Left "<<(_NFToken._BuyTQty.TK1MAX - _NFToken._BuyTQty.NetTradedTK1)<<  endl<<endl;

					}
           // cout << "Buy Price New " << OrdDetailBuy.Price << endl<<endl;

            SymbolDictionary[PFNumber] = _NFToken;
					return;

				}

//cout<<"Data Recieved_4 .."<<endl;
			} else if
			( (OrdDetailBuy.orderstat == (OrderStatus)OPEN || OrdDetailBuy.orderstat == (OrderStatus)REPLACED) && PhaseCreateCancel && dFirstMonthBuyRate > 0 && CreateSpreadPhase )
            {

                    if(dFirstMonthBuyRate > 0)
                    {

					int dQuoteRate = dFirstMonthBuyRate;
					int Prevprice = 0;
					Prevprice = OrdDetailBuy.Price;
					OrdDetailBuy.Price = dQuoteRate;

					if (Prevprice != dFirstBestBuyRate && Prevprice != dQuoteRate) {

						OrdDetailBuy.orderstat = (OrderStatus)PENDING;
                        //OrdDetailBuy.orderstat = (OrderStatus)REPLACED;

						ORDER_MOD_IN_TR (
						                OrdDetailBuy.Token,
						         //       ntohl(OrdDetailBuy.Qty *BLQ),
						                		dQuoteRate,
						                		OrdDetailBuy.side ,
						                		PFNumber
						);

                        //FILE_LOG(logDEBUG) << "  Modify from Sell OrdDetailBuy.orderstat  "  << OrdDetailBuy.orderstat << " PhaseCreateCancel "
                        //<<PhaseCreateCancel << " dFirstMonthBuyRate " <<  dFirstMonthBuyRate <<" dFirstBestBuyRate " << dFirstBestBuyRate <<
                        //" Prevprice " << Prevprice << " OrdDetailBuy.ReplaceCount "<<OrdDetailBuy.ReplaceCount << " _ProdBuySpread " << _ProdBuySpread
                        //<< " CreateSpreadDiff " << CreateSpreadDiff <<" Second Leg Price " <<SecondFP.MAXBID;

						//	OrdDetailBuy.orderstat = (OrderStatus)REPLACED;
						OrdDetailBuy.ReplaceCount += 1;
//cout << "Buy Price Modify " << OrdDetailBuy.Price << endl<<endl;
						_OrderDetails[FirstToken][OrdDetailBuy.side] = OrdDetailBuy;


					}
                }
//cout<<"Data Recieved_5 .."<<endl;


			}
			 else if((OrdDetailBuy.orderstat == (OrderStatus)OPEN || OrdDetailBuy.orderstat == (OrderStatus)REPLACED) &&
				(!PhaseCreateCancel || !CreateSpreadPhase)) {

				OrdDetailBuy.orderstat = (OrderStatus)PENDING;

				ORDER_CANCEL_IN_TR (OrdDetailBuy.Token,
				OrdDetailBuy.side ,
				PFNumber
				);
				//	OrdDetailBuy.orderstat = (OrderStatus)CANCEL;
				OrdDetailBuy.ReplaceCount =0;
				OrdDetailBuy.Price = 0;
				//	OrdDetailBuy.TotalTraded-=1;

				_OrderDetails [FirstToken][OrdDetailBuy.side] = OrdDetailBuy;


			}


//cout<<"Data Recieved_end .."<<endl;

			//=================================================================BUY FAR SELL NEAR SECTION ENDS HERE =======================================


}



 void BOARD_LOT_IN_TRMKT(int TokenNo,OptOrderPacket _Pack, int LEG, int Qty, int PFNumber)  //-- 20000
		{// MS_OE_REQUEST_TR 110+26

          //  cout<<"Buy: "<<FarTokenNo<<" price  "<<htonl(price)<<" size "<<sizeof (_NMPACK [FarTokenNo].FARMONTHBUY) <<endl;
                    if(LEG>=2 && LEG <=3)
                    {
                    //cout<<"BOARD_LOT_IN_TRBuy  : 1 ";
                    MS_OE_REQUEST_TR obj;
                   // cout<<"BOARD_LOT_IN_TRBuy  : 2 ";
                    memset(&obj,0,136);
                    //cout<<"BOARD_LOT_IN_TRBuy  : 3 ";
                    if(LEG==2)
                    obj= _Pack.SecondToken;
                    else if(LEG==3)
                    obj= _Pack.ThirdToken;

                    obj.Volume = ntohl(Qty);

                    obj.DisclosedVolume=ntohl(Qty);

	 				obj.Price=ntohl(0);
	 				//cout<<"BOARD_LOT_IN_TRBuy  : 5 ";
	 				ProcessToEnqueue((char*)&obj,136);
	 				//cout<<"BOARD_LOT_IN_TRBuy  : 6 ";
                cout << "Market Order for First Token "<< TokenNo << " PF " << PFNumber<<" Qty to be modified " << Qty <<" Leg " << LEG << endl<< endl;
                }
		}

 void BOARD_LOT_IN_TRBuy(int FarTokenNo, int Qty, int price, int PFNumber, BUYSELL BS)  //-- 20000
		{// MS_OE_REQUEST_TR 110+26

          //  cout<<"Buy: "<<FarTokenNo<<" price "<<htonl(price)<<" size "<<sizeof (_NMPACK [FarTokenNo].FARMONTHBUY) <<endl;



                    //cout<<"BOARD_LOT_IN_TRBuy  : 1 ";
                    MS_OE_REQUEST_TR obj;
                   // cout<<"BOARD_LOT_IN_TRBuy  : 2 ";
                    memset(&obj,0,136);
                    //cout<<"BOARD_LOT_IN_TRBuy  : 3 ";

                    obj= _OptOrderPacket[PFNumber][BS].FirstToken;
                   // cout<<"BOARD_LOT_IN_TRBuy  : 4 ";
	 			//	obj.DisclosedVolume=obj.Volume=volume;

                   // cout << "Qty placed Buy " << htonl(obj.Volume) << endl;
                    obj.Volume = ntohl(Qty);
                    obj.DisclosedVolume=ntohl(Qty);
	 				obj.Price=ntohl(price);
	 				//cout<<"BOARD_LOT_IN_TRBuy  : 5 ";
	 				ProcessToEnqueue((char*)&obj,136);
	 				//cout<<"BOARD_LOT_IN_TRBuy  : 6 ";
cout << "New Order Buy Token "<< FarTokenNo << " PF " << PFNumber<<" SIDE "<< (short)BS<<" Qty to be modified " << Qty <<" Price "<<price<< endl<< endl;
		}


 void BOARD_LOT_IN_TRSell(int FarTokenNo,int Qty, int price, int PFNumber,BUYSELL BS)  //-- 20000
		{// MS_OE_REQUEST_TR 110+26

//cout<<"Sell: "<<FarTokenNo<<" price "<<htonl(price)<<" size "<<sizeof (_NMPACK [FarTokenNo].FARMONTHSELL) <<endl;

                   // cout<<"BOARD_LOT_IN_TRSell  : 1 " << endl;
                    MS_OE_REQUEST_TR obj;
                  // cout<<"BOARD_LOT_IN_TRSell  : 2 " << endl;
                    memset(&obj,0,136);
                   //cout<<"BOARD_LOT_IN_TRSell  : 3 " << endl;
                    obj=_OptOrderPacket[PFNumber][BS].FirstToken;
	 			//	obj.DisclosedVolume=obj.Volume=volume;
	 				//cout<<"BOARD_LOT_IN_TRSell  : 4 " << obj.Volume << endl;
	 				//cout << "Qty placed Sell" << htonl(obj.Volume) << endl;
	 				obj.Volume =ntohl(Qty);
	 				obj.DisclosedVolume = ntohl(Qty);
	 				obj.Price=ntohl(price);
	 				//cout<<"BOARD_LOT_IN_TRSell  : 5 " <<  obj.Price<< endl;
	 				ProcessToEnqueue((char*)&obj,136);
	 				//cout<<"BOARD_LOT_IN_TRSell  : 6 ";
	 				cout << "New Order Sell Token "<< FarTokenNo << " PF " << PFNumber<<" SIDE "<< (short)BS<<" Qty to be modified " << Qty <<" Price "<<price<< endl<< endl;
		}


 void ORDER_MOD_IN_TR(int TokenNo, int price,BUYSELL buySell,int PFNumber)// 20040
		{//MS_OM_REQUEST_TR 138+26=164

                if(TokenNo>0 &&(buySell==BUY || buySell ==SELL ))
                {
                    //cout<<"ORDER_MOD_IN_TR  : 1 ";
                    MS_OM_REQUEST_TR obj;
                  //  cout<<"ORDER_MOD_IN_TR  : 2 ";
                    memset(&obj,0,164);
                  //  cout<<"ORDER_MOD_IN_TR  : 3 ";
  	 				switch ((short)buySell) {
	 				case 1:
	 					obj=_OptOrderPacket[PFNumber][buySell].FirstTokenMod;
	 				//	cout<<"ORDER_MOD_IN_TR  : 4 ";

	 					break;
	 				case 2:
	 					obj=_OptOrderPacket[PFNumber][buySell].FirstTokenMod;
                      //  cout<<"ORDER_MOD_IN_TR  : 5 ";
	 					break;
	 				}
	 				char TransCodeBytes[0];
                    //cout<<"ORDER_MOD_IN_TR  : 6 ";
	 				obj.TransactionCode=ModificationCode;
	 			//	obj.DisclosedVolume=obj.Volume=volume;
	 				//cout<<"ORDER_MOD_IN_TR  : 7 ";
	 				obj.Price=ntohl(price);
	 				//cout<<"ORDER_MOD_IN_TR  : 8 ";

	 				cout << "Token "<< TokenNo << " PF " << PFNumber<<" SIDE "<< buySell<<" Qty to be modified " << htonl(obj.Volume) <<" Price "<<price<< endl<< endl;


                    ProcessToEnqueue((char*)&obj,164);
                  //  cout<<"ORDER_MOD_IN_TR  : 9 ";
                }
                else
                {
                    cout <<"TOken 0 found for PF " << PFNumber<<endl<<endl;
                }
		}


 void ORDER_CANCEL_IN_TR(int TokenNo, BUYSELL buySell, int PFNumber)  //-- 20070
		{// MS_OM_REQUEST_TR 138+26

        //cout<<"ORDER_CANCEL_IN_TR  : 1 ";
		MS_OM_REQUEST_TR obj;
		//cout<<"ORDER_CANCEL_IN_TR  : 2 ";
        memset(&obj,0,164);
        //cout<<"ORDER_CANCEL_IN_TR  : 3 ";
  	 				switch ((short)buySell) {
	 				case 1:
	 					obj=_OptOrderPacket[PFNumber][buySell].FirstTokenMod;
	 					//cout<<"ORDER_CANCEL_IN_TR  : 4 ";
	 					break;
	 				case 2:
	 					obj=_OptOrderPacket[PFNumber][buySell].FirstTokenMod;
                    //    cout<<"ORDER_CANCEL_IN_TR  : 5 ";
	 					break;
	 				}
	 			//	cout<<"ORDER_CANCEL_IN_TR  : 6 ";
	 				char TransCodeBytes[0];
	 				obj.TransactionCode=CancelCode;
	 				//cout<<"ORDER_CANCEL_IN_TR  : 7 ";
                    ProcessToEnqueue((char*)&obj,164);
                  //  cout<<"ORDER_CANCEL_IN_TR  : 8 ";
		}

 //==========================================================   OUT

bool OrderSide(int _TKN,int PFNumber , short _BS ,string CalledFrom )
{

        FOPAIRLEG2 _OptpairObj;
        memset(&_OptpairObj,0, sizeof(FOPAIRLEG2));
      _OptpairObj = SymbolDictionary[PFNumber]._OptTokens ;

        bool CreateReverse;

       // cout <<CalledFrom << " PFNumber " << PFNumber << endl;

        if( _OptpairObj.Token1 == _TKN)
        {
            CreateReverse = _OptpairObj.Token1side == _BS ? true : false;
            //cout << "First Token "<< CalledFrom << " " << _TKN << "Actual Side " << _OptpairObj.Token1side << " Traded Side " << _BS<< endl;
        }
        if(_OptpairObj.Token2 == _TKN)
        {
            CreateReverse = _OptpairObj.Token2side == _BS ? true : false;
            //cout << "Second Token "<< CalledFrom << " " << _TKN << "Actual Side " << _OptpairObj.Token2side << " Traded Side " << _BS<< endl;
        }

        if(_OptpairObj.Token3 == _TKN)
        {
            CreateReverse = _OptpairObj.Token3side == _BS ? true : false;
            //cout << "Third Token "<< CalledFrom << " " << _TKN << "Actual Side " << _OptpairObj.Token3side << " Traded Side " << _BS<< endl;
        }


        if( _OptpairObj.Token4 == _TKN)
        {
            CreateReverse = _OptpairObj.Token4side == _BS ? true : false;
           // cout << "Fourth Token "<< CalledFrom << " " << _TKN << "Actual Side " << _OptpairObj.Token4side << " Traded Side " << _BS<< endl;
        }

    return CreateReverse;

}



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
        short Side =  htonl(obj.Buy_SellIndicator);
       // cout<<"ORDER_CONFIRMATION_TR  : 6 ";


        //cout << "Order Confirmation for Token "<< _TKN <<" volume " << _Volume <<  endl<< endl;

        int PFNumber = Primeleg[_TKN].PortfolioName;


        //bool CreateReverse = OrderSide(_TKN,PFNumber,_BS,"ORDER_CONFIRMATION_TR");
  if(SymbolDictionary[PFNumber]._OptTokens.Token1==_TKN)
  {
            MS_OM_REQUEST_TR _obj;
            memset(&_obj,0,164);

            _obj = _OptOrderPacket[PFNumber][_BS].FirstTokenMod;

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

     _OptOrderPacket[PFNumber][_BS].FirstTokenMod=_obj;
//cout<<"ORDER_CONFIRMATION_TR  : 8 ";
       _OrderDetails[_TKN][_BS].orderstat = (OrderStatus)OPEN;
     	_OrderDetails[_TKN][_BS].OrderNumber = _OrderNo;
     		_OrderDetails[_TKN][_BS].Price = _price;
     		//cout<<"ORDER_CONFIRMATION_TR  : 9 ";
}
     /*   else if(SymbolDictionary[PFNumber]._OptTokens.Token2==_TKN)
        {


              MS_OM_REQUEST_TR _obj;
            memset(&_obj,0,164);

            _obj = _OptOrderPacket[PFNumber][CreateReverse].SecondTokenMod;

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



            _obj.Price=ntohl(0);
            _obj.TransactionCode=ModificationCode;
            _obj.Volume=ntohl(_Volume);
            _obj.DisclosedVolume=ntohl(_Volume);

            ProcessToEnqueue((char*)&_obj,164);
            _OptOrderPacket[PFNumber][CreateReverse].SecondTokenMod=_obj;


        }*/

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

		int PFNumber = Primeleg[_TKN].PortfolioName;


       // bool CreateReverse = OrderSide(_TKN,PFNumber,_BS,"ORDER_CXL_CONFIRMATION_TR");

		// cout<<"ORDER_CONFIRMATION_TR  : 3 ";

            _OrderDetails[_TKN][_BS].orderstat = (OrderStatus)CANCEL;
            //_OrderDetails[_TKN][_BS].TotalTraded -=1 ;
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

		int _Volume = htonl(obj.Volume);
		//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step8"<<endl;

       // cout << "Order MOD Confirmation for Token "<< _TKN <<" volume " << _Volume <<  endl<< endl;

        int PFNumber = Primeleg[_TKN].PortfolioName;


       // bool CreateReverse = OrderSide(_TKN,PFNumber,_BS,"ORDER_MOD_CONFIRMATION_TR");


			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step9"<<endl;
            MS_OM_REQUEST_TR _obj;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step10"<<endl;
            memset(&_obj,0,164);
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step11"<<endl;
             _obj = _OptOrderPacket[PFNumber][_BS].FirstTokenMod;
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

            _OptOrderPacket[PFNumber][_BS].FirstTokenMod=_obj;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step23"<<endl;

            _OrderDetails[_TKN][_BS].orderstat = (OrderStatus)REPLACED;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step24"<<endl;
     		_OrderDetails[_TKN][_BS].Price = _price;
			//cout<<"ORDER_MOD_CONFIRMATION_TR =>"<<"step25"<<endl;




	//	ORDER_CANCEL_IN_TR(_TKN,(BUYSELL)_BS);

	}


		void TRADE_CONFIRMATION_TR (char *buffer) //-- 20222
		{
           // cout <<"============================================Trade IN ========================"<<endl<<endl;
            MS_TRADE_CONFIRM_TR obj_Trade;
            memset(&obj_Trade,0,153);
            memcpy(&obj_Trade,buffer,153);

            int _TKN =htonl(obj_Trade.Token);
            int PFNumber = Primeleg[_TKN].PortfolioName;
            short _BS = htons(obj_Trade.Buy_SellIndicator);
            NFToken _NFDets = SymbolDictionary[PFNumber];

          //  cout<<" Trade Event PFNumber "<<PFNumber<< " Token " << _TKN  << " Qty " << Qty <<endl;

          if(_NFDets._OptTokens.Token1 ==_TKN)
          {
			//  cout <<"============================================Trade Event Start ========================"<<endl<<endl;


            int _Qty = htonl(obj_Trade.FillQuantity);

            int Leg2Qty;

            int Leg3Qty;

             int BLQ = _NFDets.BLQ;
            _Qty = _Qty > 0 ? _Qty / BLQ : 0;

            if(_BS==BUY)
            {
                _NFDets._BuyTQty.NetTradedTK1+=_Qty;
                _NFDets._BuyTQty.CurTradedTK1+=_Qty;

                _NFDets._BuyTQty= UpdateQty(_NFDets,true);

                Leg2Qty = _NFDets._BuyTQty.MINHITTK2;
                Leg3Qty = _NFDets._BuyTQty.MINHITTK3;

                _NFDets._BuyTQty.NetTradedTK2+= Leg2Qty;
                _NFDets._BuyTQty.CurTradedTK2+=Leg2Qty;

                _NFDets._BuyTQty.NetTradedTK3+=Leg3Qty;
                _NFDets._BuyTQty.CurTradedTK3+=Leg3Qty;
            }
            else if(_BS==SELL)
            {
                _NFDets._SellTQty.NetTradedTK1+=_Qty;
                _NFDets._SellTQty.CurTradedTK1+=_Qty;

                _NFDets._SellTQty= UpdateQty(_NFDets,false);

                Leg2Qty = _NFDets._SellTQty.MINHITTK2;
                Leg3Qty = _NFDets._SellTQty.MINHITTK3;

                _NFDets._SellTQty.NetTradedTK2+= Leg2Qty ;
                _NFDets._SellTQty.CurTradedTK2+=Leg2Qty;

                _NFDets._SellTQty.NetTradedTK3+=Leg3Qty;
                _NFDets._SellTQty.CurTradedTK3+=Leg3Qty;

            }


          //  int TradedQty = htonl(obj_Trade.VolumeFilledToday)/BLQ;

                OptOrderPacket _tmpOptOrderPacket = _NFDets._InnerPack[_BS];

                if(Leg2Qty > 0 )
                {
                    /* MS_OE_REQUEST_TR obj_Leg2;

                    memset(&obj_Leg2,0,sizeof(MS_OE_REQUEST_TR));

                    obj_Leg2=_tmpOptOrderPacket.SecondToken;

                    obj_Leg2.Volume =ntohl(Leg2Qty * BLQ);
                    obj_Leg2.DisclosedVolume =ntohl(Leg2Qty * BLQ);

	 				ProcessToEnqueue((char*)&obj_Leg2,sizeof(MS_OE_REQUEST_TR));
                    */

                    BOARD_LOT_IN_TRMKT(_TKN,_tmpOptOrderPacket,2,Leg2Qty * BLQ,PFNumber);

                    cout << " Leg2Qty " << Leg2Qty << endl<< endl;
                }
                if(Leg3Qty > 0 )
                {
                   /*  MS_OE_REQUEST_TR obj_Leg3;

                    memset(&obj_Leg3,0,sizeof(MS_OE_REQUEST_TR));

                    obj_Leg3=_tmpOptOrderPacket.ThirdToken;

                    obj_Leg3.Volume =ntohl(Leg3Qty * BLQ);
                    obj_Leg3.DisclosedVolume =ntohl(Leg3Qty * BLQ);

	 				ProcessToEnqueue((char*)&obj_Leg3,sizeof(MS_OE_REQUEST_TR));*/

	 				BOARD_LOT_IN_TRMKT(_TKN,_tmpOptOrderPacket,3,Leg3Qty * BLQ,PFNumber);


                    cout << " Leg3Qty " << Leg3Qty << endl<< endl;

                }


                SymbolDictionary[PFNumber] = _NFDets;

                OrderDetails _TrdOrderDetails;

                memset(&_TrdOrderDetails,0, sizeof(OrderDetails));

                _TrdOrderDetails=_OrderDetails[_TKN][_BS];

                int RemainingQty = htonl(obj_Trade.RemainingVolume);

                RemainingQty = RemainingQty > 0? RemainingQty/BLQ : 0;

                if(RemainingQty<=0)
                {
                    _TrdOrderDetails.TotalTraded =_BS==BUY ? _NFDets._BuyTQty.NetTradedTK1 : _NFDets._SellTQty.NetTradedTK1;

                    _TrdOrderDetails.orderstat = (OrderStatus)TRADE;

                    _OrderDetails[_TKN][_BS] = _TrdOrderDetails;
                    cout <<" Firstleg completed Order Status set to TRADE"<< endl<<endl;

                    cout << " ======================================================= "<< endl<<endl;

                    cout << " Total Traded Qty " << _TrdOrderDetails.TotalTraded << " Side " << _BS <<endl<<endl;

                    cout << " ======================================================= "<< endl<<endl;

                }
                //=IF(B8<B13,IF(B16>0,1,0),B16)




		  //cout<<"TRADE_CONFIRMATION_TR =>"<<"step7"<<endl;


			 //cout <<"============================================Trade Event End ========================"<<endl<<endl;
			// cout<<"TRADE_CONFIRMATION_TR =>"<<"step21"<<endl;
			}
			 else
			 {
			  cout<<"Trade Near  _TKN: "<<_TKN<<" _BS: "<<_BS<<endl<<endl;

			 // cout<<"TRADE_CONFIRMATION_TR =>"<<"step22"<<endl;
			  }
			 // cout<<"TRADE_CONFIRMATION_TR =>"<<"step23"<<endl;
            //cout <<"============================================Trade Out ========================"<<endl<<endl;
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

		int PFNumber = Primeleg[_TKN].PortfolioName;


       // bool CreateReverse = OrderSide(_TKN,PFNumber,_BS,"ORDER_CXL_REJ_OUT");


							//cout<<"ORDER_CXL_REJ_OUT =>"<<"step8"<<endl;
							if (_Error == 16273)

							_OrderDetails[_TKN][(BUYSELL)_BS].orderstat = (OrderStatus)CANCEL;

							else if(_OrderDetails[_TKN][_BS].orderstat != (OrderStatus)TRADE)
							_OrderDetails[_TKN][_BS].orderstat = (OrderStatus)REPLACED;



							//cout<<"ORDER_CXL_REJ_OUT =>"<<"step9"<<endl;
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
                int PFNumber = Primeleg[_TKN].PortfolioName;


              //  bool CreateReverse = OrderSide(_TKN,PFNumber,_BS,"ORDER_MOD_REJ_OUT");

				if (_Error == 16273)
				_OrderDetails[_TKN][_BS].orderstat = (OrderStatus)CANCEL;
				else if(_OrderDetails[_TKN][_BS].orderstat != (OrderStatus)TRADE)
				_OrderDetails[_TKN][_BS].orderstat = (OrderStatus)REPLACED;

				//cout<<"ORDER_MOD_REJ_OUT => "<< _TKN << " BS " << _BS << endl;
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
        int PFNumber = Primeleg[_TKN].PortfolioName;


       // bool CreateReverse = OrderSide(_TKN,PFNumber,_BS,"ORDER_ERROR_OUT");


		_OrderDetails[_TKN][(BUYSELL)_BS].orderstat = (OrderStatus)REJECTED;
		_OrderDetails[_TKN][(BUYSELL)_BS].TotalTraded -=1 ;
     //   cout<<"ORDER_ERROR_OUT BUY\n";

//cout<<"ORDER_ERROR_OUT =>"<<"step7"<<endl;
		}




  int Datasock;// = nn_socket(AF_SP, NN_SUB);
void SubscribeToken (int _token)
{
    nn_setsockopt(Datasock, NN_SUB, NN_SUB_SUBSCRIBE,&_token , 4);
}

void UnSubscribeToken (int _token)
{
    nn_setsockopt(Datasock, NN_SUB, NN_SUB_UNSUBSCRIBE,&_token , 4);
}

void Datasubscriber (void *data)
{
  const char* addr = "tcp://192.168.100.222:7070";     //For Seperate DataServer IP:Port
   //const char* addr = "inproc://DataSubPub";       //For Inbuit DataServer


cout << "Datasubscriber Start: "<<addr<<"  ClientIdAuto: "<<ClientIdAuto<<endl;

  Datasock = nn_socket(AF_SP, NN_SUB);
  assert(Datasock >= 0);
  int msg =111;
  assert(nn_setsockopt(Datasock, NN_SUB, NN_SUB_SUBSCRIBE,&msg , sizeof(msg)) >= 0);

/*
  msg =44381;
  assert(nn_setsockopt(Datasock, NN_SUB, NN_SUB_SUBSCRIBE,&msg , sizeof(msg)) >= 0);
  msg =44370;
  assert(nn_setsockopt(Datasock, NN_SUB, NN_SUB_SUBSCRIBE,&msg , sizeof(msg)) >= 0);
   msg =44293;
 assert(nn_setsockopt(Datasock, NN_SUB, NN_SUB_SUBSCRIBE,&msg , sizeof(msg)) >= 0);
   msg =44932;
  assert(nn_setsockopt(Datasock, NN_SUB, NN_SUB_SUBSCRIBE,&msg , sizeof(msg)) >= 0);
 */
  assert(nn_connect(Datasock, addr) >= 0);
//std::mutex lock;



  FinalPrice* buf;


  buf =(FinalPrice *) std::malloc(sizeof(FinalPrice));


  int fpSize= sizeof(FinalPrice);


  while (!IsExit) {

    int bytes = nn_recv(Datasock, buf,fpSize, 0);
    //cout<<" bytes :"<<bytes<<endl;

    if(bytes > 0)
    {

       /* _Datadict[buf->Token]=*buf;
        printf("\n");
        printf("received: %d %d %d %d byte %d size %d \n", buf->Token,buf->MAXBID , buf->MINASK , buf->LTP,bytes, fpSize);
        printf("received Dict : %d %d %d %d %d \n", _Datadict[buf->Token].Token,_Datadict[buf->Token].MAXBID , _Datadict[buf->Token].MINASK , _Datadict[buf->Token].LTP);
        printf("\n");
        */
     //   cout<< "Recv Token : "<< buf->Token<<endl;
        OnDataArrived(buf);

    }

    /*nn_freemsg(&buf);*/
    //memset(&buf, 0, 200);
  }
 cout << "Datasubscriber End" << endl;
 // delete(buf);
 //   pthread_mutex_destroy(&mutex2);
  }

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

 void Eventsubscriber (void *data)
    {

    //const char* addr = "tcp://127.0.0.1:7071";
 const char* addr = "ipc:///tmp/eventpubsub.ipc";//"inproc://eventpubsub";

    cout << "Eventsubscriber Start: "<<addr<<"  ClientIdAuto: "<<ClientIdAuto<<endl;

 int* msg1;
//cout << "Eventsubscriber Step:  1 "<<endl;
  int sock = nn_socket(AF_SP, NN_SUB);
  //cout << "Eventsubscriber Step:  2 "<<endl;
  assert(sock >= 0);
  //  *msg1=123;
  long lng=concat((short)(MessageType)eORDER,ClientIdAuto);
   nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));

//cout <<"Order"<< endl<< endl;
  lng=concat((short)(MessageType)eIOCPAIR,ClientIdAuto );
   nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));
//cout <<"eIOCPAIR"<< endl<< endl;

   lng=concat((short)(MessageType)eIOCPAIRDIFF,ClientIdAuto);
   nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));
//cout <<"eIOCPAIRDIFF"<< endl<< endl;
   lng=concat((short)(MessageType)eIOCPAIRUNSUB,ClientIdAuto);
   nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));
//cout <<"eIOCPAIRUNSUB"<< endl<< endl;

  assert(nn_connect(sock, addr) >= 0);

char _buffer[1024];
char buffer[1024];
 //   _buffer =(char *) std::malloc(1024);
	short TransCode;

 while (!IsExit)
 {

        //cout << "While Loop Eventsubscriber step 1"<< endl;
        memset(&_buffer,0,1024);
        //cout << "While Loop Eventsubscriber step 1_A"<< endl;
        int size = nn_recv(sock, _buffer,1024, 0);
        //cout << "While Loop Eventsubscriber step 2"<< endl;
        if(size<1)
        {
           // cout <<"Size recieved less than 1" << endl;
            continue;
        }
        //cout << "While Loop Eventsubscriber step 3"<< endl;
	 	  long suId;
            memcpy(&suId,_buffer,sizeof(suId));
       		suId=*(long*)_buffer;
        //cout << "While Loop Eventsubscriber step 4"<< endl;

       		//cout << "MsgType suID normal " << suId << endl;
       		//cout << "MsgType suID divided " << suId/1000000000000000 << endl;

            short MsgType =suId/1000000000000000;
         //   cout << "While Loop Eventsubscriber step 5"<< endl;
	 	// cout<< "->MsgType "<<MsgType<<" size: "<<size<<" suId "<<suId<<" MsgType "<<(char)MsgType<<" "<<MsgType<<endl;

	 	 memset(buffer,0,1024);
	 	 memcpy(buffer,_buffer+8,size-8);
	 	// cout << "While Loop Eventsubscriber step 6"<< endl;

	 	// cout << "Incoming Msg Type " << MsgType << " Actual Msg Type " <<  eFOPAIR << endl;
				switch ((MessageType)MsgType)//(MessageType)BitConverter.ToInt16(_IncomingData, 0))
								{
                                case (MessageType)eIOCPAIR:
								HandleOnIOCPairSubscription(buffer);
								break;

								case (MessageType)eIOCPAIRUNSUB:
                                HandleOnIOCPairUnSubscription(buffer);
                                break;

								case (MessageType)eIOCPAIRDIFF:
								HandleOnIOCPairDiff(buffer);
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
			//			 cout << "Eventsubscriber End" << endl;
//delete (_buffer);
		}


void CancelAllOrder()
	{

        cout<<"CancelAllOrder"<<endl;

        for(map<int,_innerOrderPack>::iterator it = _OrderDetails.begin(); it != _OrderDetails.end(); it++)
        {
            _innerOrderPack _tempin = it->second;
            CancelBuyOrder(_tempin[BUY].Token);
            CancelSellOrder(_tempin[BUY].Token);
        }

	 cout<<"Cancel All Order SuccessFully........."<<endl;
	}

void CancelBuyOrder(int Token)
{
    _innerOrderPack _tempin = _OrderDetails[Token] ;

    cout << " Innerpack found"<< endl<<endl;

    if(_tempin.find(BUY)!= _tempin.end())
    {

        OrderStatus _Stat = (OrderStatus)_tempin[BUY].orderstat;
        if(_tempin[BUY].OrderNumber!=0)
        if (_Stat == (OrderStatus)OPEN || _Stat == (OrderStatus)REPLACED || _Stat == (OrderStatus)PENDING)
        {
             ORDER_CANCEL_IN_TR (_tempin[BUY].Token, (BUYSELL)BUY,Primeleg[_tempin[BUY].Token].PortfolioName);
             cout <<"Buy Cancelation Send Order No: " << _tempin[BUY].OrderNumber<< " OrderStat " << (OrderStatus)_Stat<<endl<< endl;
        }

    }
}

void CancelSellOrder(int Token)
{
 _innerOrderPack _tempin = _OrderDetails[Token] ;

    cout << " Innerpack found"<< endl<<endl;

   if(_tempin.find(SELL)!= _tempin.end())
   {
      OrderStatus _Stat = (OrderStatus)_tempin[SELL].orderstat;
      if(_tempin[SELL].OrderNumber!=0)
      if (_Stat == (OrderStatus)OPEN || _Stat == (OrderStatus)REPLACED || _Stat == (OrderStatus)PENDING)
      {
          ORDER_CANCEL_IN_TR (_tempin[SELL].Token, (BUYSELL)SELL,Primeleg[_tempin[SELL].Token].PortfolioName);
          cout <<"Sell Cancelation Send Order No: " << _tempin[SELL].OrderNumber<< " OrderStat "<<(OrderStatus)_Stat<< endl<<endl;
      }

   }
}

	void Dispose()
	{
	cout<<"Dispose Of class Called"<<endl;
	 CancelAllOrder();
	 sleep(1);
	IsExit=true;
    cout<<"DisposeCPP End SuccessFully........."<<endl;
	}


    void start(void *params)
    {
        cout << " Start called"<< endl;
        this->params = params;
        pthread_create (&threadId, 0, runDatasubscriber, static_cast<void*>(this));
        cout << " runDatasubscriber called"<< endl;
        pthread_create (&threadId, 0, runEventsubscriber, static_cast<void*>(this));
        cout << " runEventsubscriber called"<< endl;
        cout << "start End" << endl;
    }

};
}
