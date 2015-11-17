#include "TapSignOn.h"

#include "md5.h"
#include "../PFMap.h"
#include "ClientHandler.h"

#include <boost/thread/mutex.hpp>
#include<boost/thread/condition_variable.hpp>

md5_byte_t check[16];

PFHashMap _PFHashMap;

ClientHandler _ClientHandler;

int LastSeq;

TapSignOn::TapSignOn()
{
    try
    {
        TapSignOn::PubSock = nn_socket(AF_SP,NN_PUB);

        nn_bind(TapSignOn::PubSock,"inproc://eventpubsub");

        memset(&pkt,0,sizeof(Packets));
    }
    catch(int e)
    {
        cout << " Error binding PublishSock from TapSignOn "<<endl<<endl;
    }
}

TapSignOn::~TapSignOn()
{
    //dtor
}

void TapSignOn::SendToExchange()
{

    while(true)
    {
       //sleep(1);
        //cout << "_FillData._Bidqueue.size() "<<_FillData._BidQueuesize()<<"\t"<< _Bidqueue.size() <<endl<<endl;
        boost::mutex::scoped_lock lock(the_outmutex);
        if(pkt.InvitationCount > 0 &&  ( _FillData._BidQueuesize()> 0 || _FillData._MktQueuesize()>0 ) )
        {
             PFHolder _out;

             if(_FillData._MktQueuesize()>0)
             {
               _out= _Mktqueue.try_pop();
             }
             else if(_FillData._BidQueuesize() >0 )
             {
                //popped=_FillData._GetBidData(_out);
                _out = _Bidqueue.try_pop();
             }

             if(_out.side==(short)BUY)
             {
                cout<< " Buy Order found"<<endl;
            }
             else if(_out.side==(short)SELL)
             {
                cout<< " Sell Order found"<<endl;
             }

                if(_out.PF<=0)
                {
                    cout << " PF not found in Out queue"<<endl;
                }

                if(_out.PF>-1)
                {

                switch(_out._size)
                {

                    case 136:
                    {



                        pkt.SeqNo++;

                        if(LastSeq+1 != pkt.SeqNo)
                        {
                            cout << " Seq Error LastSeq "<<LastSeq << " pkt.SeqNo " << pkt.SeqNo <<endl;
                        }
                        else
                        {
                            LastSeq = pkt.SeqNo;
                        }

                        cout<<"\n \n ****** SendToExchange 136 Length= "<<ntohs(_out._oetr.Length) <<" tcode "<<ntohs(_out._oetr.TransactionCode) <<" UserId= "<< ntohl(_out._oetr.UserId)<< " pkt.SeqNo "<<pkt.SeqNo<<" Side "<<ntohs(_out._oetr.Buy_SellIndicator)  << " ***** "<<endl<<endl;

                        short pktLen=_out._size;
                        //memcpy(&pktLen,_out.buff,out._size);

                        _out._oetr.SequenceNumber=ntohl(pkt.SeqNo);

                        _out._oetr.Open_Close='O';

                    // memcpy(_out.oetr.SequenceNumber,&pkt.SeqNo,4);
                        pkt.md5CheckSum((const unsigned char*)&_out._oetr+24,pktLen-24,check);
                        memcpy(_out._oetr.CheckSum,&check,16);



                        pkt.InvitationCount--;

                       // _PFHashMap.AddRecord(_out);

                        //Send to exchange




                        sock.send(&_out._oetr,pktLen);
                        cout<<"\n \n ****** Sent = "<<ntohs(_out._oetr.Length) <<" tcode "<<ntohs(_out._oetr.TransactionCode) <<" UserId= "<< ntohl(_out._oetr.UserId)<< " pkt.SeqNo "<<pkt.SeqNo<<" Side "<<ntohs(_out._oetr.Buy_SellIndicator)  << " ***** "<<endl<<endl;
                    }
                    break;
                    case 164:
                    {


                        pkt.SeqNo=pkt.SeqNo+1;
cout<<"\n\n ****before SendToExchange 164 Modify Length= "<<ntohs(_out._omtr.Length) <<" tcode "<<ntohs(_out._omtr.TransactionCode) <<" UserId= "<< ntohl(_out._omtr.UserId)<< " pkt.SeqNo "<<pkt.SeqNo <<" Side "<<ntohs(_out._omtr.Buy_SellIndicator)<< " ***"<<endl<<endl;
                        if(LastSeq+1 != pkt.SeqNo)
                        {
                            cout << " Seq Error "<<endl;
                        }
                        else
                        {
                            LastSeq = pkt.SeqNo;
                        }

                        short pktLen=_out._size;
                        //memcpy(&pktLen,_out.buff,out._size);

                        _out._omtr.SequenceNumber=ntohl(pkt.SeqNo);
                        _out._omtr.Open_Close ='O';
                    // memcpy(_out.oetr.SequenceNumber,&pkt.SeqNo,4);
                        pkt.md5CheckSum((const unsigned char*)&_out._omtr+24,pktLen-24,check);
                        memcpy(_out._omtr.CheckSum,&check,16);

                        pkt.InvitationCount--;

                        //_PFHashMap.AddRecord(_out);

                        //Send to exchange

                        sock.send(&_out._omtr,pktLen);
                        cout<<"\n\n ****Sent = "<<ntohs(_out._omtr.Length) <<" tcode "<<ntohs(_out._omtr.TransactionCode) <<" UserId= "<< ntohl(_out._omtr.UserId)<< " pkt.SeqNo "<<pkt.SeqNo <<" Side "<<ntohs(_out._omtr.Buy_SellIndicator)<< " ***"<<endl<<endl;
                    }
                    break;
                }


                }

        }
         lock.unlock();
        the_outcondition_variable.notify_one();
        //if(pkt.InvitationCount > 0 && )
    }
}

void TapSignOn::BindConnection()
{

    cout << " Thread 1 Started inside "<<endl;

    try
    {
		const string servAddress = "192.168.168.236";	// First arg: server address
		const unsigned short Port = 9602;
		sock.remoteAdd=servAddress;
        sock.remotePort= Port;
		sock.TcpInit();

		PacketFormat RecvPacketFormat;
		RecvPacket incomingPkt;

		int bytesReceived = 0;
		short recvPacketSize = 0;
		int counter = 0;
		short tCode = 0;

		for (;;)
		{

            boost::mutex::scoped_lock inlock(the_inmutex);

			bytesReceived = sock.recv(&RecvPacketFormat.Length,2);

			if (bytesReceived<0)
			{
				cerr << "Unable to read";
					//exit(1);
            }
				recvPacketSize = htons(RecvPacketFormat.Length);

				bytesReceived = sock.recv(&incomingPkt, recvPacketSize - 2);

				if (bytesReceived<0)
				{
					cerr << "Unable to read";
					//exit(1);
				}
				if (bytesReceived <= 0)
				{
					cout << "\nConnection Lost... press any key to continue ...";
					char ch;
					cin >> ch;
					exit(1);
				}

				memcpy(&tCode, incomingPkt.Data, 2);
				tCode = htons(tCode);
				cout << "\n\n %%%%%%%% bytesReceived= " << bytesReceived << " tCode= " << tCode<<" %%%%%%"<<endl<<endl;
				switch (tCode)
				{
				case 15000:
				{
							  pkt.InvitationCount += 1;
							  cout << "\n15000 Invitation Count Received";
				}
					break;
				case 2301:
				{
							 MS_SIGN_ON_OUT_2301 LoginConfirm;
							 memcpy(&LoginConfirm, incomingPkt.Data, sizeof(MS_SIGN_ON_OUT_2301));
							 cout << "\n\n2301  Received";
							 cout << "\n\nError Code= " << htons(LoginConfirm.Header.ErrorCode);

				}
					break;
				case 1601:
				{
							 cout << "\n\n1601  Received";

				}
					break;
				case 2321:
				{
							 cout << "\n\n2321 Received";

				}
					break;
                case 2231:
				{

				cout<<"\n Rejected section ===============";
                   // boost::mutex::scoped_lock lockhash(the_Hashmutex);
                        MESSAGE_HEADER _header;
                        memset(&_header,0,sizeof(MESSAGE_HEADER));
                        memcpy(&_header,incomingPkt.Data,sizeof( MESSAGE_HEADER ) );

                        cout <<"\n\n===========  2231 Received error code is " << htons(_header.ErrorCode);
                        short tcodee=htons(_header.TransactionCode);
                        short msglen=htons(_header.MessageLength);
                        short errorcode=htons(_header.ErrorCode);

                        cout<<"\n tcode=" <<htons(_header.TransactionCode);

                  //  lockhash.unlock();
                   //     the_Hashcondition_variable.notify_one();

				}
                break;

					case 20073: //Order_Confirmation_IN_TR
					case 20075:
					case 20074:
					{

                        boost::mutex::scoped_lock lockhash(the_Hashmutex);

                        MS_OE_RESPONSE_TR _OrderResp;
                        memset(&_OrderResp,0,sizeof(MS_OE_RESPONSE_TR));
                        memcpy(&_OrderResp,incomingPkt.Data,sizeof( MS_OE_RESPONSE_TR ) );


                        char *Data;//[1024];
                        Data = (char * )malloc(8 +sizeof( MS_OE_RESPONSE_TR ) );

                        //long isend= 6520101000171;

                        //memcpy(Data, &isend, 8);


                        PFHolder _PH;
                        memset(&_PH,0,sizeof(PFHolder));
                        switch(tCode)
                        {
                            case 20073:
                                _PH = GetPF(_OrderResp.TokenNo,_OrderResp.Price,_OrderResp.Volume,_OrderResp.Buy_SellIndicator,_OrderResp.OrderNumber);
                            break;
                            case 20074:
                            case 20075:
                                _PH =  _PFHashMap.GetRecordByOrder(_OrderResp.OrderNumber);
                                break;
                        }
                        cout << " Packet recieved in BindConnection 1 "<<endl<<endl;
                        if(_PH.PF==0)
                        {
                            cout << "Break  From PF ZERO"<<endl;
                        }
                        if(_PH.PF>0)
                        {

                            if(_PH.side==(short)BUY)
                            {
                                cout<< " Buy Order found"<<endl;
                            }
                            else if(_PH.side==(short)SELL)
                            {
                                cout<< " Sell Order found"<<endl;
                            }


                            memcpy(Data, &_PH.SubscriptionTag,8);

                           // cout << " Subscription tag TapSignOn "<< _PH.SubscriptionTag<<endl<<endl;

                            memcpy(Data + 8, &incomingPkt.Data,sizeof( MS_OE_RESPONSE_TR ) );


                            cout << " Packet recieved in BindConnection 2 PF "<< _PH.PF<<" CID "<< _PH.CID
                            <<" Side "<< ntohs(_OrderResp.Buy_SellIndicator) <<endl<<endl;


                            int ret = nn_send(TapSignOn::PubSock,Data,8+sizeof(MS_OE_RESPONSE_TR), 0);


                                ClientUpdateMsg _clnt;
                                memset(&_clnt,0,sizeof(ClientUpdateMsg));
                                _clnt.TransectionCode=(short)((MessageType)eORDER);
                                _clnt.ClintId= _PH.CID ;
                                _clnt.PF = _PH.PF;
                                memcpy(_clnt.buffer,&incomingPkt.Data,sizeof(MS_OE_RESPONSE_TR));
                            _ClientHandler.PushServerPacket(_clnt,sizeof(ClientUpdateMsg));
                       // _ClientHandler.PushOrderPacket(Data,8+sizeof(MS_OE_RESPONSE_TR));

                        free(Data);
                        }
                        //cout << " PF "<<_RetVal.PF << " CID "<< _RetVal.CID<<endl;
                        lockhash.unlock();
                        the_Hashcondition_variable.notify_one();
					}

					break;


				}

				counter++;

				switch (counter)
				{
				case 1:
				{
						  pkt.SeqNo = 1;
						  LastSeq = pkt.SeqNo;
						  MS_SIGN_ON_2300 Login = pkt.LoginPacket_2300(32865, "Aa@11111", "", "12468", 1, 93700, 0, "1234567", "DIVYA PORTFOLIO PVT LTD");
						  sock.send(&Login, sizeof(MS_SIGN_ON_2300));
						  cout << "\nLogin Packet Sent";

						  pkt.InvitationCount  -= 1;
				}
					break;

				case 2:
				{
                          sleep(1);
						  pkt.SeqNo = 2;
						  LastSeq = pkt.SeqNo;
						  MS_SYSTEM_INFO_REQ_1600 SystemInfo = pkt.SystemInfoRequestPacket_1600(32865);
						  sock.send(&SystemInfo, sizeof(MS_SYSTEM_INFO_REQ_1600));
						  cout << "\nSystemInfo Packet Sent";
						  pkt.InvitationCount -= 1;
				}
					break;

                case 3:
                {
                   /* sleep(10);
                    SignOutRequest_2320 TapLogOut = pkt.Fun_SignOut_2320(32865, pkt.SeqNo+1);
                    sock.send(&TapLogOut, sizeof(SignOutRequest_2320));
                    cout << "\nLogOut Packet Sent";
                    pkt.InvitationCount = pkt.InvitationCount - 1;
                    */
                }
                break;

				}

                      inlock.unlock();
                        the_incondition_variable.notify_one();
			}
			cout << "\nFor Logout Enter 5";
			int logout;
			cin >> logout;

			if (logout == 5)
			{
				SignOutRequest_2320 TapLogOut = pkt.Fun_SignOut_2320(32865, pkt.SeqNo+1);
				sock.send(&TapLogOut, sizeof(SignOutRequest_2320));
				cout << "\nLogOut Packet Sent";
				pkt.InvitationCount = pkt.InvitationCount - 1;
			}


		}
		catch (SocketException &e)
		{
			cerr << e.what() << endl;
			exit(1);
		}
}

PFHolder TapSignOn::GetPF(int Token,int Price, int Qty, short side,double OrderNumber)
{


    StructHash _TrdHash;
    memset(&_TrdHash,0,sizeof(StructHash));

    _TrdHash.Token1= htonl(Token);
    _TrdHash.Price1 = htonl(Price);
    _TrdHash.Qty1 = htonl(Qty);
    _TrdHash.side1 = htons(side);

cout << "Token "<< _TrdHash.Token1<< " Price "<<_TrdHash.Price1 << " Qty "<< _TrdHash.Qty1<<" Side "<< _TrdHash.side1<< " OrderNumber "<<OrderNumber<<endl<<endl;


    long HashKey = MyHash<StructHash>()(_TrdHash);
     PFHolder _RetVal;


        memset(&_RetVal ,0 ,sizeof(PFHolder));
        _RetVal =  _PFHashMap.GetRecord(HashKey);
        _RetVal.OrderNo = OrderNumber;
        _RetVal.HashKey=LastSeq;
        _PFHashMap.AddRecord(_RetVal,true);

    return _RetVal;

}

void TapSignOn::Init()
{

    cout << " Thread 1 Started "<<endl;
    _inDataThread = new boost::thread(&TapSignOn::BindConnection, this);
    producer_threads.add_thread(_inDataThread);

    cout << " Thread 2 Started "<<endl;
    _outDataThread = new boost::thread(&TapSignOn::SendToExchange, this);
    producer_threads.add_thread(_outDataThread);


}


