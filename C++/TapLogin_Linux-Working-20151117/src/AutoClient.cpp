#include "AutoClient.h"



#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/ini_parser.hpp>
#include <boost/lexical_cast.hpp>


using namespace boost::signals2;
using namespace boost::property_tree;



AutoClient::AutoClient()
{
    //ctor
}

AutoClient::~AutoClient()
{
    //dtor
}


void AutoClient::InItClass(map<int,ContractDetails> AllCimlist)
{
//cout << " Contract loaded successfully "<< AllCimlist.size() << endl<<endl;

       // ptree pt;
    //   boost::property_tree::ptree pt;
   //     boost::property_tree::ini_parser::read_ini("settings.ini", pt);

       // ini_parser::read_ini("settings.ini",pt);

        SECTION =23;// pt.get<std::string>("SECTION.ID");
        cout << " SECTION "<<SECTION<<endl<<endl;

        CONTRACTFILEPATH ="contract.txt";//pt.get<std::string>(SECTION +".CONTRACTFILEPATH");
        DATANANOPATH = "tcp://192.168.168.36:7070";//pt.get<std::string>(SECTION +".DATANANOPATH");

        BrokerId= 12468;//pt.get<int>(SECTION +".BROKERID");
        BranchId= 1;//  pt.get<int>(SECTION +".BRANCHID");
        UserId=32865;// pt.get<int>(SECTION +".USERID");

        IsExit=false;


    //cout << "SECTION : " << SECTION << " CONTRACTFILEPATH : " << CONTRACTFILEPATH << " DATANANOPATH : " << DATANANOPATH << " BrokerId : "
    //<< BrokerId << " BranchId : " << BranchId << " UserId : " << UserId << endl;

    cimlist.insert(AllCimlist.begin(),AllCimlist.end());

   // cimlist = AllCimlist;

    cout << " Contract loaded successfully "<< cimlist.size() << endl<<endl;
    //Contract_Filefun();

    _dataThread = new boost::thread(&AutoClient::Datasubscriber, this);

    producer_threads.add_thread(_dataThread);

    cout << "Thread Started for Datasubscriber"<< endl;

    _eventThread=new boost::thread(&AutoClient::Eventsubscriber, this);

    producer_threads.add_thread(_eventThread);

    cout << "Thread Started for Eventsubscriber"<< endl;
    //Eventsubscriber

	}

	 void AutoClient::PadRight(char *string, int padded_len, char *pad)
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

void AutoClient::toUpper(char* pArray, int arrayLength)
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


TokenPartnerDetails AutoClient::InitTokenDetails( int FirstLeg, int alternateLeg, int PortfolioName)
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
                struct ContractDetails cf ;//=new ContractDetails();
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


//char *
MS_OE_REQUEST_TR AutoClient::ReturnNearPack(int Token,BUYSELL BS,int Qty,int FMBlq =0)
{

    MS_OE_REQUEST_TR obj2;

//cout << " Contract loaded successfully "<< cimlist.size() << endl<<endl;
cout << " Contract loaded successfully "<< cimlist.size() << endl<<endl;
    //for (vector<Contract_File>::iterator it2 = cimlist.begin(); it2 != cimlist.end(); it2++)
    if(cimlist.find(Token)!=cimlist.end())
		{
            strncpy(obj2.InstrumentName,cimlist[Token].InstrumentName.c_str(),sizeof(obj2.InstrumentName));
			//strncpy(obj2.InstrumentName,cimlist[Token].InstrumentName,sizeof(obj2.InstrumentName));
			//obj2.InstrumentName=it2->InstrumentName;
			strncpy(obj2.Symbol,cimlist[Token].Symbol.c_str(),sizeof(obj2.Symbol));
			//strncpy(obj2.Symbol,cimlist[Token].Symbol,sizeof(obj2.Symbol));
			//obj2.Symbol=it2->Symbol;
			obj2.ExpiryDate=cimlist[Token].ExpiryDate;
			obj2.StrikePrice=cimlist[Token].StrikePrice;
			//strncpy(obj2.OptionType,cimlist[Token].OptionType,sizeof(obj2.OptionType));

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


            cout << " cimlist[Token].InstrumentName " <<cimlist[Token].InstrumentName <<  " cimlist[Token].Symbol "<<cimlist[Token].Symbol << "  cimlist[Token].OptionType "
            << cimlist[Token].OptionType<<endl<<endl;


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
				double dbl=ClientIdAlgo;
				htond(dbl) ;
				obj.nnffield =dbl;

				obj.Length =ntohs(sizeof(MS_OE_REQUEST_TR));
			//	obj.SequenceNumber =ntohs(0);
				obj.MsgCount =ntohs(1);

            cout<<"\nIn msOerequesttr PktLen= " <<ntohs(obj.Length) <<"  tcode= "<<ntohs(obj.TransactionCode)<<endl;

            //cout<<"--->Token. :"<< Token <<" BS :"<<BS<<" InstrumentName :"<<obj2.InstrumentName<<" obj2.TokenNo :"<<obj2.TokenNo<<endl;
                return obj;
		}



 void AutoClient::htond (double &x)
{
  int *Double_Overlay;
  int Holding_Buffer;
  Double_Overlay = (int *) &x;
  Holding_Buffer = Double_Overlay [0];
  Double_Overlay [0] = htonl (Double_Overlay [1]);
  Double_Overlay [1] = htonl (Holding_Buffer);
}



 MS_OM_REQUEST_TR AutoClient::ReturnModificationPack(int Token,BUYSELL BS)
		{


             //****we use dll for below line..................
		    //var _contract = CSV_Class.cimlist.Where (a => a.Token == Token).ToList ();
                MS_OM_REQUEST_TR obj2;
        //for (vector<Contract_File>::iterator it2 = cimlist.begin(); it2 != cimlist.end(); it2++)
        if(cimlist.find(Token)!=cimlist.end())

		{
            strncpy(obj2.InstrumentName,cimlist[Token].InstrumentName.c_str(),sizeof(obj2.InstrumentName));
			//strncpy(obj2.InstrumentName,cimlist[Token].InstrumentName,sizeof(obj2.InstrumentName));
			//obj2.InstrumentName=it2->InstrumentName;
			//strncpy(obj2.Symbol,cimlist[Token].Symbol,sizeof(obj2.Symbol));
			strncpy(obj2.Symbol,cimlist[Token].Symbol.c_str(),sizeof(obj2.Symbol));
			//obj2.Symbol=it2->Symbol;
			obj2.ExpiryDate=cimlist[Token].ExpiryDate;
			obj2.StrikePrice=cimlist[Token].StrikePrice;
			//strncpy(obj2.OptionType,cimlist[Token].OptionType,sizeof(obj2.OptionType));

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

                double dbl=ClientIdAlgo;
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




  void AutoClient::HandleOnFOPairSubscription (strFOPAIR _FOpairObj)
{
     //strFOPAIR _FOpairObj;

	 //memcpy(&_FOpairObj,buffer,sizeof(_FOpairObj));

cout << "PFNumber " << _FOpairObj.PORTFOLIONAME  <<" Token Far "<< _FOpairObj.TokenFar << " Token Near "<< _FOpairObj.TokenNear<<endl<<endl;
if(_FOpairObj.PORTFOLIONAME<=0)
return;

cout << " _StgClass "<< _StgClass.size()<<endl<<endl;

    if(_StgClass.find(_FOpairObj.PORTFOLIONAME)== _StgClass.end())
    {

    TokenPartnerDetails _TokenDets = InitTokenDetails(_FOpairObj.TokenFar,_FOpairObj.TokenNear,_FOpairObj.PORTFOLIONAME);
    cout<< " New class PairSubs %%%%%%%%%%%%%%%%%%%%%%% "<<endl<<endl;
        StrategyClass _stg;
       _StgClass[_FOpairObj.PORTFOLIONAME] =_stg;
       _StgClass[_FOpairObj.PORTFOLIONAME].ClientID = ClientIdAuto;
       _StgClass[_FOpairObj.PORTFOLIONAME].BLQ=_TokenDets.CF.BoardLotQuantity ;
      // cout<< " New class PairSubs ########################## "<<endl<<endl;

_DataPack[_FOpairObj.TokenFar].connect(bind(&StrategyClass::OnDataEventHandler,&_StgClass[_FOpairObj.PORTFOLIONAME],_1));
_DataPack[_FOpairObj.TokenNear].connect(bind(&StrategyClass::OnDataEventHandler,&_StgClass[_FOpairObj.PORTFOLIONAME],_1));

      // FinalPrice _myfp;
       //_myfp.Token =12345;

        //_DataPack[_FOpairObj.TokenFar].connect(bind(&StrategyClass::OnDataEventHandler,&_stg,new FinalPrice() ));
        //cout << " Data Handler bound for FarToken "<< _FOpairObj.TokenFar<<endl<<endl;
       // _myfp.MAXBID =123;
        //_myfp.MINASK = 567;

       // _DataPack[_FOpairObj.TokenFar](&_myfp);

      // _DataPack[_FOpairObj.TokenNear].connect(bind(&StrategyClass::OnDataEventHandler,&_stg,new FinalPrice()));
       //cout << " Data Handler bound for NearToken "<< _FOpairObj.TokenFar<<endl<<endl;
    }

        _StgClass[_FOpairObj.PORTFOLIONAME].OnSubscriptionEventHandler(&_FOpairObj);

            SymbolDictionary[_FOpairObj.TokenNear].FARTOKEN = _FOpairObj.TokenFar;
			SymbolDictionary[_FOpairObj.TokenNear].NEARTOKEN = _FOpairObj.TokenNear;
			SymbolDictionary[_FOpairObj.TokenNear].PFNUMBER = _FOpairObj.PORTFOLIONAME ;
			//SymbolDictionary[_FOpairObj.TokenNear].BLQ= Primeleg[_FOpairObj.TokenFar].CF.BoardLotQuantity;

			SymbolDictionary[_FOpairObj.TokenFar].FARTOKEN = _FOpairObj.TokenFar;
			SymbolDictionary[_FOpairObj.TokenFar].NEARTOKEN = _FOpairObj.TokenNear ;
			SymbolDictionary[_FOpairObj.TokenFar].PFNUMBER = _FOpairObj.PORTFOLIONAME;
			//SymbolDictionary[_FOpairObj.TokenFar].BLQ= Primeleg[_FOpairObj.TokenFar].CF.BoardLotQuantity;

//  Packet
	//if (_NMPACK.find(_FOpairObj.TokenFar) ==_NMPACK.end())
	if(_StgClass[_FOpairObj.PORTFOLIONAME]._NMPack.FARMONTHBUY.TransactionCode != ntohs((short)20000))
	{
        memset(&_NMPACK[_FOpairObj.TokenFar],0,sizeof(MS_OE_REQUEST_TR));

		memcpy(&_NMPACK [_FOpairObj.TokenFar].NEARMONTHBUYMARKET,&ReturnNearPack(_FOpairObj.TokenNear, BUY, 1,SymbolDictionary[_FOpairObj.TokenFar].BLQ),sizeof(MS_OE_REQUEST_TR));
        memcpy(&_NMPACK [_FOpairObj.TokenFar].NEARMONTHSELLMARKET,&ReturnNearPack(_FOpairObj.TokenNear, SELL, 1,SymbolDictionary[_FOpairObj.TokenFar].BLQ),sizeof(MS_OE_REQUEST_TR));


		memcpy(&_NMPACK [_FOpairObj.TokenFar].FARMONTHBUY,&ReturnNearPack(_FOpairObj.TokenFar, BUY, 1),sizeof(MS_OE_REQUEST_TR));
		memcpy(&_NMPACK [_FOpairObj.TokenFar].FARMONTHSELL,&ReturnNearPack(_FOpairObj.TokenFar, SELL, 1),sizeof(MS_OE_REQUEST_TR));

        memcpy(&_NMPACK [_FOpairObj.TokenFar].FARMONTHMODBUY,&ReturnModificationPack(_FOpairObj.TokenFar,BUY),sizeof(MS_OM_REQUEST_TR));
        memcpy(&_NMPACK [_FOpairObj.TokenFar].FARMONTHMODSELL,&ReturnModificationPack(_FOpairObj.TokenFar, SELL),sizeof(MS_OM_REQUEST_TR));


        memset(&_NMPACK[_FOpairObj.TokenNear],0,sizeof(MS_OE_REQUEST_TR));

        _StgClass[_FOpairObj.PORTFOLIONAME]._NMPack = _NMPACK[_FOpairObj.TokenFar];

         cout<<"\n1 Length= "<<ntohs(_StgClass[_FOpairObj.PORTFOLIONAME]._NMPack.FARMONTHBUY.Length) <<" tcode "<<ntohs(_StgClass[_FOpairObj.PORTFOLIONAME]._NMPack.FARMONTHBUY.TransactionCode)<<endl;
         cout<<"\n2 Length= "<<ntohs(_NMPACK[_FOpairObj.TokenFar].FARMONTHBUY.Length) <<" tcode "<<ntohs(_NMPACK[_FOpairObj.TokenFar].FARMONTHBUY.TransactionCode)<<endl;



	}

	_FOPAIRDIFF [_FOpairObj.PORTFOLIONAME].BNSFMNQ=0;
	_FOPAIRDIFF [_FOpairObj.PORTFOLIONAME].BFSNMNQ=0;
	_FOPAIRDIFF [_FOpairObj.PORTFOLIONAME].BNSFMXQ=0;
	_FOPAIRDIFF [_FOpairObj.PORTFOLIONAME].BFSNMXQ=0;




        SubscribeTokenOnFalse(_FOpairObj.TokenFar);
        SubscribeTokenOnFalse(_FOpairObj.TokenNear);


      //  SubscribePFOnFalse(_FOpairObj.PORTFOLIONAME,_StgClass[_FOpairObj.PORTFOLIONAME]._pfBuy.SubscriptionTag);

}

 void AutoClient::HandleOnFOPairUnSubscription (strFOPAIR _FOpairObj)
		{

        if(_StgClass.find(_FOpairObj.PORTFOLIONAME)== _StgClass.end())
        {
            StrategyClass _stg;
            _StgClass[_FOpairObj.PORTFOLIONAME] =_stg;

        }
        _StgClass[_FOpairObj.PORTFOLIONAME].OnUnSubscriptionEventHandler(&_FOpairObj);

        //SubscribePFOnFalse(_FOpairObj.PORTFOLIONAME,_StgClass[_FOpairObj.PORTFOLIONAME]._pfBuy.SubscriptionTag);
		}


void AutoClient::HandleOnFOPairDiff (FOPAIRDIFF _INpairDiff)
	{


        if(_StgClass.find(_INpairDiff.PORTFOLIONAME)== _StgClass.end())
        {
            StrategyClass _stg;
            _StgClass[_INpairDiff.PORTFOLIONAME] =_stg;

            _DataPack[_INpairDiff.TokenFar].connect(bind(&StrategyClass::OnDataEventHandler,&_StgClass[_INpairDiff.PORTFOLIONAME],_1));
            _DataPack[_INpairDiff.TokenNear].connect(bind(&StrategyClass::OnDataEventHandler,&_StgClass[_INpairDiff.PORTFOLIONAME],_1));

       //_DataPack[_INpairDiff.TokenFar].connect(bind(&StrategyClass::OnDataEventHandler,&_StgClass[_INpairDiff.PORTFOLIONAME],_1));

       cout << " Data Handler bound for FarToken "<< _INpairDiff.TokenFar<<endl<<endl;

      // _DataPack[_INpairDiff.TokenNear].connect(bind(&StrategyClass::OnDataEventHandler,&_StgClass[_INpairDiff.PORTFOLIONAME],_1));

       cout << " Data Handler bound for NearToken "<< _INpairDiff.TokenNear<<endl<<endl;
    }
    _StgClass[_INpairDiff.PORTFOLIONAME].OnDifferenceEventHandler(&_INpairDiff);

}


void AutoClient::Datasubscriber ()
{
    const char* addr = DATANANOPATH.c_str();

    cout << "Datasubscriber Start: "<<addr<<"  ClientIdAuto: "<<ClientIdAuto<<endl;

  Datasock = nn_socket(AF_SP, NN_SUB);
  assert(Datasock >= 0);
  int msg =111;

  // int setBufSize= 100 * 1024 * 1024 ;

   // nn_setsockopt(Datasock,NN_SOL_SOCKET,NN_RCVBUF,&setBufSize,sizeof(setBufSize));


  assert(nn_setsockopt(Datasock, NN_SUB, NN_SUB_SUBSCRIBE,&msg , sizeof(msg)) >= 0);

  dataconnectid =nn_connect(Datasock, addr);

  FinalPrice* buf;

  buf =(FinalPrice *) std::malloc(sizeof(FinalPrice));

  int fpSize= sizeof(FinalPrice);

  while (!IsExit)
  {

    int bytes = nn_recv(Datasock, buf,fpSize, 0);
    if(IsExit)
    break;
    if(bytes > 0)
    {


       // OnDataArrived(buf);
       /*
      FinalPrice _fp;
       memset(&_fp,0,sizeof(FinalPrice));
      // memcpy(&_fp,buf,sizeof(FinalPrice));

        _fp.Token =44932;
        _fp.MAXBID= 44930;
        _fp.MINASK =44935;
*/
      //  int PFNum = SymbolDictionary[buf->Token].PFNUMBER;

       // cout << "Data arrived @ parent class Token "<< buf->Token <<" PFNUM "<< PFNum<<endl<< endl;

     //  _DataPack[_fp.Token](&_fp);
        if(_DataPack.find(buf->Token)!=_DataPack.end())
        _DataPack[buf->Token](*buf);



//sleep(2);
     //_StgClass[PFNum].OnDataEventHandler(&_fp);
//sleep(1);
    }

    /*nn_freemsg(&buf);*/
    //memset(&buf, 0, 200);
  }
 cout << "Datasubscriber End" << endl;

 DataThreadExitted = true;
 // delete(buf);
 //   pthread_mutex_destroy(&mutex2);
  }
void AutoClient::SubscribeTokenOnFalse(int Token)
{
            if(_SubStatus.find(Token)==_SubStatus.end())
            {
                _SubStatus[Token]= false;
            }

            if(!_SubStatus[Token])
			{
			//_udpObj.Subscribe = _FOpairObj.TokenFar;
                _SubStatus[Token]=true;
               // _dataHolder.InsertRecord(Token);


                SubscribeToken(Token);
                cout << "Symbol " << Token << " subscribed successfully "<< endl;
			}

}


 void AutoClient::SubscribePF (SubscriptionTagDets Key)
 {

 //  long lng=6520101000171;
        //nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));

     nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&Key , sizeof(Key));

 }

 void AutoClient::SubscribePF (long Key)
 {

   //long lng=6520101000171;
        //nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));

     nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&Key , sizeof(Key));

 }
 void AutoClient::UnSubscribePF (long Key)
 {
    nn_setsockopt(sock, NN_SUB,NN_SUB_UNSUBSCRIBE,&Key , sizeof(Key));
 }

 void AutoClient::SubscribePFOnFalse(short _PF,long Key)
 {

    if(_SubStatus.find(_PF)==_SubStatus.end())
            {
                _SubStatus[_PF]= false;
            }

            if(!_SubStatus[_PF])
			{
			//_udpObj.Subscribe = _FOpairObj.TokenFar;
                _SubStatus[_PF]=true;
               // _dataHolder.InsertRecord(Token);

                SubscribePF(Key);
                cout << "Portfolio " << _PF << " subscribed successfully Key " << Key<< endl;
			}
 }

 void AutoClient::UnSubscribePFOnTrue(int _PF,long SubKey)
{
if(_SubStatus.find(_PF)!=_SubStatus.end())
     {
        if(_SubStatus[_PF])
        {
			//_udpObj.Subscribe = _FOpairObj.TokenFar;
            _SubStatus[_PF]=false;
            UnSubscribePF(SubKey);
            cout << "Portfolio " << _PF << " UnSubscribed successfully "<< endl;

          // _dataHolder.CleanRecord(Token);
        }
    }
}

void AutoClient::UnSubscribeTokenOnTrue(int Token)
{
     if(_SubStatus.find(Token)!=_SubStatus.end())
     {
        if(_SubStatus[Token])
        {
			//_udpObj.Subscribe = _FOpairObj.TokenFar;
            _SubStatus[Token]=false;
            UnSubscribeToken(Token);
            cout << "Symbol " << Token << " UnSubscribed successfully "<< endl;

          // _dataHolder.CleanRecord(Token);
        }
    }
}




  int Datasock;// = nn_socket(AF_SP, NN_SUB);
  int sock ;
void AutoClient::SubscribeToken (int _token)
{
    nn_setsockopt(Datasock, NN_SUB, NN_SUB_SUBSCRIBE,&_token , 4);
}
void AutoClient::UnSubscribeToken (int _token)
{
    nn_setsockopt(Datasock, NN_SUB, NN_SUB_UNSUBSCRIBE,&_token , 4);
}


long long AutoClient::concat(long long x, long long y)
{
    long long temp = y;
    while (y != 0) {
        x *= 10;
        y /= 10;
    }
    return x + temp;
}

void AutoClient::Eventsubscriber ()
    {

        const char* addr = "inproc://eventpubsub";

        cout << "Eventsubscriber Start: "<<addr<<"  ClientIdAuto: "<<ClientIdAuto<<endl;

       // int setBufSize= 32 * 1024 * 1024 ;

        int sock = nn_socket(AF_SP, NN_SUB);

       // nn_setsockopt(sock,NN_SOL_SOCKET,NN_RCVBUF,&setBufSize,sizeof(setBufSize));

        assert(sock >= 0);

        eventconnectid =nn_connect(sock, addr);


        // long lng=6520101000171;
       // nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lng , sizeof(lng));



        long lng=concat((short)(MessageType)eORDER,ClientIdAuto);
        long lngpf ;//= concat(lng,(short)1);

        for(int ipf = 1 ; ipf<20;ipf++)
        {
            lngpf = concat(lng,(short)ipf);
            nn_setsockopt(sock, NN_SUB,NN_SUB_SUBSCRIBE,&lngpf , sizeof(lngpf));
        }


      //  cout << " Subscription tag AutoClient "<< lng<<endl<<endl;

        char _buffer[1024];
        char buffer[1024];

        short TransCode;

        while (!IsExit)
        {

                memset(&_buffer,0,1024);

                int _size = nn_recv(sock, _buffer,1024, 0);
                if(IsExit)
                    break;

                cout <<" Size of data recieved "<< _size<< "Auto ID "<< AutoCountID << endl<<endl;

                if(_size<1)
                {
                    continue;
                }

                long suId;

                suId=*(long*)_buffer;

                string SubVal =boost::lexical_cast<string>(suId);

                short MsgType = stoi(SubVal.substr(0,2) )  ;
                int CID =stoi( SubVal.substr(2,10));
                short PFNum =stoi(  SubVal.substr(12) );

                cout << " MsgType "<<MsgType << " CID "<<CID << " PFNum "<<PFNum<<" Auto ID "<< AutoCountID <<endl<<endl;

                if(_StgClass.find(PFNum)!=_StgClass.end())
                {
                    cout << "Loc 1 ID Count " <<AutoCountID <<endl;
                    memset(buffer,0,1024);
                    cout << "Loc 2 ID Count " <<AutoCountID <<endl;
                    memcpy(buffer,_buffer+8,_size-8);
                    cout << "Loc 3 ID Count " <<AutoCountID <<endl;
				switch ((MessageType)MsgType)
				{
                    case (MessageType)eORDER:
                        TransCode=0;
                        cout << "Loc 4 ID Count " <<AutoCountID <<endl;
                        TransCode=*(short*)buffer;

                        cout << "  ((((((((((((((((((   Transcode from AutoClient "<< htons(TransCode) <<" ID Count "<< AutoCountID <<" ))))))))))))))))))"<<endl<<endl;

                        switch (htons(TransCode))
                        {
                            case 20073:
                                _StgClass[PFNum].ORDER_CONFIRMATION_TR (buffer);
                                break;
                            case 20075:
                                _StgClass[PFNum].ORDER_CXL_CONFIRMATION_TR (buffer);
                                break;
                            case 20074:
                                _StgClass[PFNum].ORDER_MOD_CONFIRMATION_TR (buffer);
                                break;
                            case 20222:
                                _StgClass[PFNum].TRADE_CONFIRMATION_TR (buffer);
                                break;
                            case 2072:
                                _StgClass[PFNum].ORDER_CXL_REJ_OUT (buffer);
                                break;
                            case 2042:
                                _StgClass[PFNum].ORDER_MOD_REJ_OUT (buffer);
                                break;
                            case 2231:
                                _StgClass[PFNum].ORDER_ERROR_OUT (buffer);
                                break;
                        }


									default:
									break;

                }///switch end
            }///IF_STGclass end()

                cout << " while loop last line Auto ID "<< AutoCountID <<endl;
						}

						EventThreadExitted = true;


		}


void AutoClient::CancelAllOrder()
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


void AutoClient::StopAllOrder()
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

       void AutoClient::Dispose()
	{

        IsExit=true;

        sleep(1);



        long lng=concat((short)(MessageType)eORDER,ClientIdAuto);
        long lngpf ;//= concat(lng,(short)1);
		for(int ipf = 1 ; ipf<20;ipf++)
        {
            lngpf = concat(lng,(short)ipf);
            nn_setsockopt(sock, NN_SUB,NN_SUB_UNSUBSCRIBE,&lngpf , sizeof(lngpf));
        }

        cout << "Shutting Down Datasocket Con iD "<< dataconnectid<< endl;
        nn_shutdown(Datasock,0);

        cout << "Shutting Down Tradesocket Con ID " << eventconnectid << endl;
        nn_shutdown(sock,0);

        sock = NULL;
        Datasock = NULL;
        cout << "Calling StopAllOrder/Token Unsubscription "<<endl;
        StopAllOrder();

        cout<<"Looking for open orders"<<endl;
        CancelAllOrder();


        //cout << " Waiting for 30 secs to see if any open order traded before cancelling" << endl<<endl;
       // boost::this_thread::sleep( boost::posix_time::seconds(30) );
       //sleep(30);
        cout << " Time over. Time to release used resources"<<endl<<endl;
        cout << "Unsubscribing Data for Tokens and clearing SymbolDictionary" << endl;
         while (!SymbolDictionary.empty())
        {
            int TokenNear=0;
            int TokenFar =0;
            int PFNumber =0;

            TokenNear = SymbolDictionary.begin()->second.NEARTOKEN;
            TokenFar = SymbolDictionary.begin()->second.FARTOKEN;
            PFNumber = SymbolDictionary.begin()->second.PFNUMBER;

            cout << " Unsubscribing DataEvent for Token Near " << TokenNear << " Token Far " << TokenFar<< endl;


            /*UnSubscribeTokenOnTrue(TokenFar);
            UnSubscribeTokenOnTrue(TokenNear);*/
            if(_StgClass.find(PFNumber)!=_StgClass.end())
            {
                _StgClass.erase(_StgClass.find(PFNumber));
            }

            if(_DataPack.find(TokenFar)!= _DataPack.end())
            {
                _DataPack.erase(_DataPack.find(TokenFar));
            }
            if(_DataPack.find(TokenNear)!= _DataPack.end())
            {
                _DataPack.erase(_DataPack.find(TokenNear));
            }
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
       // _dataHolder.ClearAllRecords();


        cout << "Clearing _FOPAIRDIFF" << endl;
        while (!_FOPAIRDIFF.empty())
        {
            _FOPAIRDIFF.erase(_FOPAIRDIFF.begin());
        }


        sleep(1);



        if(producer_threads.is_thread_in(_dataThread))
        {
            try
            {
            cout << "Stopping running Data thread "<< producer_threads.is_thread_in(_dataThread)  << endl;
            producer_threads.remove_thread(_dataThread);
            cout << "Data Thread released from group"<< endl;


           // if(!DataThreadExitted)
          //    _dataThread->~thread();

             _dataThread = NULL;
            DataThreadExitted = false;

             cout << "Data Thread disposed "<< endl;
             }
             catch(int exp)
             {
                cout << " Error disposing Data Thread. Possible reason : Already Disposed"<<endl;
             }
        }

        if(producer_threads.is_thread_in(_eventThread))
        {
            try
            {
            cout << "Stopping running Event thread "<< producer_threads.is_thread_in(_eventThread)  << endl;
            producer_threads.remove_thread(_eventThread);
             cout << "Event Thread released from group"<< endl;

         //    _eventThread->detach();
          //  if(!EventThreadExitted)
          //   _eventThread->~thread();


             _eventThread = NULL;

             EventThreadExitted = false;

              cout << "Data Thread disposed "<< endl;
              }
              catch(int exp)
             {
                cout << " Error disposing Event Thread. Possible reason : Already Disposed"<<endl;
             }

        }




        cout<<"Dispose Of class Called "<<endl;

        cout<<"DisposeCPP End SuccessFully........."<<endl;
	}



void AutoClient::CancelBuyOrder(int Token)
{
    if (_OrderDetailsBuy [Token].orderstat == (OrderStatus)OPEN || _OrderDetailsBuy [Token].orderstat == (OrderStatus)REPLACED || _OrderDetailsBuy [Token].orderstat == (OrderStatus)PENDING)
    {
          //  ORDER_CANCEL_IN_TR ( Token, (BUYSELL)BUY);
		cout <<"Buy Cancelation Send Order No: " << _OrderDetailsBuy [Token].OrderNumber<<endl;
	}
}

void AutoClient::CancelSellOrder(int Token)
{
    if (_OrderDetailsSell [Token].orderstat == (OrderStatus)OPEN || _OrderDetailsSell[Token].orderstat == (OrderStatus)REPLACED || _OrderDetailsSell [Token].orderstat == (OrderStatus)PENDING)
    {
       // ORDER_CANCEL_IN_TR (Token, (BUYSELL)SELL);
		cout <<"Sell Cancelation Send Order No: " << _OrderDetailsSell [Token].OrderNumber<<endl;
	}
}


