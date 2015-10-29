#ifndef CLIENTHANDLER_H
#define CLIENTHANDLER_H

#include <boost/signals2.hpp>
#include <boost/bind.hpp>
#include <boost/thread/thread.hpp>
#include <boost/timer/timer.hpp>

#include <nn.h>
#include <pubsub.h>
#include <reqrep.h>

#include "Enums.h"
#include "../2LStg/AutoClientFoFo.h"

//#include <string.h>
using namespace AutoClient;
using namespace boost;
using namespace boost::signals2;
using namespace Enums;
using namespace boost::timer;
using namespace std;


class ClientHandler
{
    public:
        ClientHandler();
        virtual ~ClientHandler();


    protected:
        boost::signals2::signal<void(char *)> OnLogin;
        boost::signals2::signal<void(char *)> OnHeartBeat;
        boost::signals2::signal<void(char *)> OnOrder;
        void OrderEventHandler(char *);
        void HeartBeatEventHandler(char *);
        void LoginEventHandler(char *);
        void Login(char * obj);
        boost::thread* _EventThread;
        void RecieveDataAsServer();
        void PushServerPacket(ClientUpdateMsg buff,int _size);
        int ReplySock;
        int PubSock;
        bool loadCimList;

        map<long,AutoClientFoFo*> _StgMap;
    private:

};

#endif // CLIENTHANDLER_H
