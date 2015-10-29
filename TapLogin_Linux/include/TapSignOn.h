#ifndef TAPSIGNON_H
#define TAPSIGNON_H
#include "Socket.h"
#include "../Packets.h"
#include <boost/thread/thread.hpp>


class TapSignOn
{
    public:
        TapSignOn();
       void BindConnection();
        void SendToExchange();
        virtual ~TapSignOn();
        void Init();
        TCPSocket sock;
        Packets pkt;
        boost::thread_group producer_threads;
        boost::thread* _inDataThread;
        boost::thread* _outDataThread;
    protected:
    private:
};


static class ClientConnection
{

    TapSignOn _obj;


public:


  void BindConnection1()
    {
 //producer_threads.add_thread(new boost::thread(&TapSignOn::BindConnection));
       // _obj.BindConnection();
 //_eventThread=new boost::thread(&TapSignOn::BindConnection, this);


        //_obj.SendToExchange();
    }


    void SendOrder(char* buffer)
    {
      //_obj.publisher(Data);
      //  printf("\n Data Sent \n");
    }

    void RecieveOrder()
    {

    }
}SockConn;
#endif // TAPSIGNON_H
