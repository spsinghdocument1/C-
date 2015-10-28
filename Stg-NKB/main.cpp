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

extern "C" void PassRefOfMethod( ProgressCallback progressCallback, long ClientId)
{

 map<long,AutoClientFoFo*>::iterator _stgRes = _StgMap.find(ClientId);
 if(_stgRes!=_StgMap.end())
 {
    cout << "Current ID is already mapped with a running stg " << ClientId << endl;
    return;
 }
else
{
    _StgMap[ClientId]= new AutoClientFoFo();
    cout << "Creating Strategy Instance for CleintID " << ClientId << endl;
}

    _StgMap[ClientId]->ClientIdAuto=ClientId;
    _StgMap[ClientId]->ProcessToEnqueue=progressCallback;
    _StgMap[ClientId]->InItClass();

    cout << " Sizeof FOPAIRDIFFLEG2 " <<sizeof(FOPAIRDIFFLEG2)<<endl<<endl;

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
