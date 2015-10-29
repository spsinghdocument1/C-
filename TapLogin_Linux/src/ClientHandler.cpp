#include "ClientHandler.h"

//http://www.boost.org/doc/libs/1_59_0/doc/html/lockfree/examples.html#lockfree.examples.waitfree_single_producer_single_consumer_queue
//http://www.bogotobogo.com/cplusplus/Boost/boost_AsynchIO_asio_tcpip_socket_server_client_timer_bind_handler_multithreading_synchronizing_network_D.php
//http://www.boost.org/doc/libs/1_35_0/doc/html/boost_asio/tutorial/tutdaytime3.html


void ClientHandler::HeartBeatEventHandler(char* buffer)
{

}
void ClientHandler::LoginEventHandler(char* buffer)
{

}
void ClientHandler::OrderEventHandler(char* buffer)
{

}
ClientHandler::ClientHandler()
{
    //ctor

    OnLogin.connect(bind(&ClientHandler::LoginEventHandler,this,"abcd"));
    OnHeartBeat.connect(bind(&ClientHandler::HeartBeatEventHandler,this,"ass"));
    OnOrder.connect(bind(&ClientHandler::OrderEventHandler,this,"asd"));

    try
    {
        ClientHandler::ReplySock = nn_socket(AF_SP, NN_REP);

        nn_bind(ClientHandler::ReplySock, "tcp://192.168.168.36:8013");
    }
    catch(int e)
    {
        cout << " Error binding ReplySock"<<endl<<endl;
    }

    try
    {
        ClientHandler::PubSock = nn_socket(AF_SP,NN_PUB);

        nn_bind(ClientHandler::PubSock,"tcp://192.168.168.36:8012");
    }
    catch(int e)
    {
        cout << " Error binding PublishSock"<<endl<<endl;
    }


       // cout << "PassRefOfMethod called " << endl;
    if(!loadCimList)
    {
        _ldContract.Contract_Filefun("contract.txt");
        loadCimList= true;
    }

    _EventThread = new boost::thread(&ClientHandler::RecieveDataAsServer, this);




}

ClientHandler::~ClientHandler()
{
    //dtor


}

void ClientHandler::Login(char* obj)
{
    C_SignIn _loginpack;
    memset(&_loginpack,0,sizeof(C_SignIn));
    memcpy(&_loginpack,obj,sizeof(C_SignIn));

    cout<< " Login Method raised from event "<<endl<<endl;
    cout <<" ============================== "<<endl<<endl;
    cout << " StgType\t" << _loginpack.StgType<<endl<<endl;
    cout <<" ============================== "<<endl<<endl;

    switch(_loginpack.TransectionCode)
    {
        case 2320:
        {
            cout <<"Client Logout requested"<<endl<<endl;
            map<long,AutoClientFoFo*>::iterator _stgRes = _StgMap.find(_loginpack.ClintId);
            if(_stgRes!=_StgMap.end())
            {
                _stgRes->second->Dispose();
                _StgMap.erase(_stgRes->first);
                cout << "Strategy Instance found for CleintID " << _loginpack.ClintId << " Dispose called for the same " << endl;
            }
            else
            {
                cout << "Strategy Instance not found for ClientID " << _loginpack.ClintId << " you must dispose it to run new instance for the same id"<< endl;
            }
            break;
        }
        default:
        {
             map<long,AutoClientFoFo*>::iterator _stgRes = _StgMap.find(_loginpack.ClintId);

            if(_stgRes!=_StgMap.end())
            {
                cout << "Current ID is already mapped with a running stg " << (long)_loginpack.ClintId << endl;
                return;
            }
            else
            {
                _StgMap[_loginpack.ClintId]= new AutoClientFoFo();
                cout << "Creating Strategy Instance for CleintID " << _loginpack.ClintId << endl;
            }

            _StgMap[_loginpack.ClintId]->ClientIdAuto=_loginpack.ClintId;
           // _StgMap[_loginpack.ClintId]->InItClass(_ldContract.cimlist);

            ClientUpdateMsg _cmsg;
            memset(&_cmsg,0,sizeof(ClientUpdateMsg));
            _cmsg.TransectionCode =(short)MessageType(eLOGIN);
            _cmsg.ClintId =_loginpack.ClintId;

            _cmsg._csign.TransectionCode = _loginpack.TransectionCode;
             _cmsg._csign.Status=(short)LogInStatus(LogIn);

            ClientHandler::PushServerPacket(_cmsg,sizeof(ClientUpdateMsg));

            break;
        }

    }

}

void ClientHandler::PushServerPacket(ClientUpdateMsg buff,int _size)
{
   /* char shortBuffer[5];
    char cidBuffer[10];
    char _tempBuffer[1024];

     sprintf(cidBuffer,"%ld",CID);
    sprintf(shortBuffer,"%d",MT);

    memcpy(_tempBuffer,shortBuffer,2);
    memcpy(_tempBuffer+2,cidBuffer,8);
    memcpy(_tempBuffer+10,buff,sizeof(C_SignIn));
    */


    int ret = nn_send(ClientHandler::PubSock,(void*)&buff,_size, 0);
    if(ret)
    cout << "Login Confirmation sent"<<"\tret\t"<<ret<<endl;

    else
    cout << "Login Confirmation failed"<<endl;

}

void ClientHandler::RecieveDataAsServer()
{
    cout << " ClientHandler started"<<endl<<endl;

    try
    {

        char buffer[1024];
        char _buffCut[1024];

        while(true)
        {
            memset(&buffer,0,1024);

	        int _size = nn_recv(ReplySock, buffer,1024, 0);

	        if(_size<1)
            {
				cout << "Some error occured in eventubsub"<<endl<<endl;
			        continue;
            }
            InHeader _header;
            memset(&_header,0,sizeof(InHeader));
            memcpy(&_header,buffer,sizeof(InHeader));


            memset(_buffCut,0,1024);
            memcpy(_buffCut,buffer+10,_size-10);

            switch((MessageType) _header.TransectionCode )
            {
                case eLOGIN:
                            //OnLogin(_buffCut);

            ClientHandler::Login(_buffCut);
                    break;
                case eCANCELALL:
                case eSTOP_ALL:
                case eFOPAIR:
                case eFOPAIRDIFF:
                case eFOPAIRUNSUB:
                case eDelete:
                    {
                     C_SignIn _loginpack;
                    memset(&_loginpack,0,sizeof(C_SignIn));
                    memcpy(&_loginpack,_buffCut,sizeof(C_SignIn));
                    cout<<"step1 :_header.ClintId\t"<<_header.ClintId<<"_buffCut.ClintId"<<_loginpack.ClintId<<endl;
                        map<long,AutoClientFoFo*>::iterator _stgRes = _StgMap.find(_loginpack.ClintId);

                        if(_stgRes==_StgMap.end())
                        {
                            cout << "Event recieved for running stg " << _loginpack.ClintId << endl;



                        switch((MessageType) _header.TransectionCode)
                        {
                            case eFOPAIR:
                                {
                                    strFOPAIR _FOpairObj;
                                    memset(&_FOpairObj,0,sizeof(strFOPAIR));
                                    memcpy(&_FOpairObj,_buffCut,sizeof(strFOPAIR));
                                    _StgMap[_header.ClintId]->HandleOnFOPairSubscription(_FOpairObj);
                                    break;
                                }
                            case eFOPAIRDIFF:
                            {
                            cout<<"step4"<<endl;
                                    FOPAIRDIFF _INpairDiff;
                                    memset(&_INpairDiff,0,sizeof(FOPAIRDIFF));
                                    memcpy(&_INpairDiff,_buffCut,sizeof(FOPAIRDIFF));
                                    _StgMap[_header.ClintId]->HandleOnFOPairDiff(_INpairDiff);
                                    break;

                            }
                        }
                        }
                        else
                        {
                        cout<<"client Id not found in map<long,AutoClientFoFo*>::iterator _stgRes\n";
                        }

                    }
                    break;
            }



        }
    }
    catch(int e)
    {

    }

}
