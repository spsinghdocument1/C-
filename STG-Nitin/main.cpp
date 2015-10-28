// The functions contained in this file are pretty dummy
// and are included only as a placeholder. Nevertheless,
// they *will* get included in the shared library if you
// don't remove them :)
//
// Obviously, you 'll have to write yourself the super-duper
// functions to include in the resulting library...
// Also, it's not necessary to write every function in this file.
// Feel free to add more files in this project. They will be
// included in the resulting library.

#include <map>
#include "AutoClientFoFo.h"
//#include <stdio.h>
using namespace AutoClient;

 //AutoClientFoFo obj;
 map<long,AutoClientFoFo*> _StgMap;
bool loadCimList;

extern "C" void PassRefOfMethod( ProgressCallback progressCallback, long ClientId)
{
   // cout << "PassRefOfMethod called " << endl;
if(!loadCimList)
{
   _ldContract.Contract_Filefun("contract.txt");
   loadCimList= true;
}

 map<long,AutoClientFoFo*>::iterator _stgRes = _StgMap.find(ClientId);
 //cout << "Map Creation called " << endl;
 if(_stgRes!=_StgMap.end())
 {

    cout << "Current ID is already mapped with a running stg " << ClientId << endl;
    return;
 }
else
{
   // cout << "new client called " << endl;
    _StgMap[ClientId]= new AutoClientFoFo();
    cout << "Creating Strategy Instance for CleintID " << ClientId << endl;
}

//int i;
//cout<<"->doub FOPAIRDIFF: "<<sizeof(FOPAIRDIFF)<<endl;
    _StgMap[ClientId]->ClientIdAuto=ClientId;
    _StgMap[ClientId]->ProcessToEnqueue=progressCallback;
    _StgMap[ClientId]->InItClass(_ldContract.cimlist);



/*
    void *params;
    obj.Eventsubscriber(params);
    int TokenFar= 44932;
    int TokenNear=44293;

    strFOPAIR Fopair;
    Fopair.PORTFOLIONAME=1;
    Fopair.TokenNear= TokenNear ;
    Fopair.TokenFar=TokenFar;
    char PairBuff[sizeof(strFOPAIR)];
    memcpy(PairBuff,&Fopair,sizeof(strFOPAIR));
    obj.HandleOnFOPairSubscription(PairBuff);

*/

  //  1828795

  //  obj.ReturnNearPack(TokenFar,(BUYSELL)BUY,1);

  //  cout<<" MS_OE_REQUEST_TR: "<<sizeof(MS_OE_REQUEST_TR)<<endl;
  //  cout<<" MS_OM_REQUEST_TR: "<<sizeof(MS_OM_REQUEST_TR)<<endl;
  //  cout<<" MS_TRADE_CONFIRM_TR: "<<sizeof(MS_TRADE_CONFIRM_TR)<<endl;
 //   cout<<" MS_OE_RESPONSE_TR: "<<sizeof(MS_OE_RESPONSE_TR)<<endl;
 //   cout<<" MS_OE_REQUEST: "<<sizeof(MS_OE_REQUEST)<<endl;

 //   cout<<"--->>>>  obj.ClientIdAuto: "<<obj.ClientIdAuto <<endl;
//int i;
//cin>>i;

 /*    char abc[]="Hello Durga--**-";
obj.test();

        // do the work...

        if (obj.ProcessToEnqueue)
        {
            // send progress update
            obj.ProcessToEnqueue(( char*)abc);
        }

printf(" Progress call check");

*/
}


extern "C" void DisposeCPP(long ClientId)
{
    map<long,AutoClientFoFo*>::iterator _stgRes = _StgMap.find(ClientId);
    if(_stgRes!=_StgMap.end())
    {
        _stgRes->second->Dispose();
        _StgMap.erase(_stgRes->first);
        cout << "Strategy Instance found for CleintID " << ClientId << " Dispose called for the same " << endl;
    }
    else
    {
        cout << "Strategy Instance not found for ClientID " << ClientId << " you must dispose it to run new instance for the same id"<< endl;
    }
}

extern "C" void StartData(short NumStream)
{
    cout << "You have choosen " << NumStream << " to run.."<<endl<< endl;


}


