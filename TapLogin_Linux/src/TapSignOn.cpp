#include "TapSignOn.h"
#include <iostream>           // For cerr and cout
#include <cstdlib>            // For atoi()
#include "Socket.h"
#include "../Packets.h"
#include "md5.h"
#include "structure.h"
#include<string.h>
#include <netinet/in.h>



#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/ini_parser.hpp>


void TapSignOn::SendToExchange()
{
    while(true)
    {
        //if(pkt.InvitationCount > 0 && )
    }
}

void TapSignOn::BindConnection()
{

    cout << " Thread 1 Started inside "<<endl;
try
    {
		const string servAddress = "192.168.100.253";	// First arg: server address
		const unsigned short Port = 9601;
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
				cout << "\n\nbytesReceived= " << bytesReceived << " tCode= " << tCode;
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

				}

				counter++;

				switch (counter)
				{
				case 1:
				{
						  pkt.SeqNo = 1;
						  MS_SIGN_ON_2300 Login = pkt.LoginPacket_2300(32865, "Zz@77777", "", "12468", 1, 93700, 0, "1234567", "DIVYA PORTFOLIO PVT LTD");
						  sock.send(&Login, sizeof(MS_SIGN_ON_2300));
						  cout << "\nLogin Packet Sent";

						  pkt.InvitationCount  -= 1;
				}
					break;

				case 2:
				{
                          sleep(1);
						  pkt.SeqNo = 2;
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

void TapSignOn::Init()
{

    cout << " Thread 1 Started "<<endl;
    _inDataThread = new boost::thread(&TapSignOn::BindConnection, this);
    producer_threads.add_thread(_inDataThread);

    cout << " Thread 2 Started "<<endl;
    _outDataThread = new boost::thread(&TapSignOn::SendToExchange, this);
    producer_threads.add_thread(_outDataThread);


}

TapSignOn::TapSignOn()
{
}

TapSignOn::~TapSignOn()
{
    //dtor
}
