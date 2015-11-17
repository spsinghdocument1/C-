#ifndef STRATEGYCLASS_H
#define STRATEGYCLASS_H
#include "Enums.h"



using namespace Enums;

class StrategyClass
{
    public:


        StrategyClass();
        virtual ~StrategyClass();
        NearMonthPacket _NMPack;
    void OnSubscriptionEventHandler(strFOPAIR* _FOpairObj);
    void OnUnSubscriptionEventHandler(strFOPAIR* _FOpairObj);
    void OnDataEventHandler(FinalPrice _fp);
    void OnDifferenceEventHandler(FOPAIRDIFF* _INpairDiff);
    void BOARD_LOT_IN_TRSell(int FarTokenNo,int qty, int price, PFHolder _PF);
    void BOARD_LOT_IN_TRBuy(int FarTokenNo,int qty, int price, PFHolder _PF);

    void ORDER_MOD_IN_TR(int TokenNo, int price,BUYSELL buySell);
    void ORDER_CANCEL_IN_TR(int TokenNo, BUYSELL buySell) ;
int ClientID;

    PFHolder _pfBuy;
PFHolder _pfSell;

OrderStatus BuyFarStatus;
    OrderStatus SellFarStatus;

bool CancelBuyOrder;
bool CancelSellOrder;
int BLQ;

void ORDER_CONFIRMATION_TR (char *buffer) ;
void ORDER_CXL_CONFIRMATION_TR (char *buffer) ;
void ORDER_MOD_CONFIRMATION_TR (char *buffer) ;
void TRADE_CONFIRMATION_TR (char *buffer) ;

 void ORDER_CXL_REJ_OUT (char *buffer);

 void ORDER_MOD_REJ_OUT (char *buffer);
 void ORDER_ERROR_OUT (char *buffer);



    protected:
    FinalPrice FarFP;
    FinalPrice NearFP;

    TokenPartnerDetails FarDetails;
    TokenPartnerDetails NearDetails;

    short BuyTradeCounter;
    short SellTradeCounter;
    short PFNumber;

    int _BNSFMNQ ;
	int _BFSNMNQ ;
	int _BNSFMXQ ;
	int _BFSNMXQ ;
	int BFSNDIFF ;
	int BNSFDIFF ;


    short  CancelCode;
	short  ModificationCode;

     long long concat(long long x, long long y);



    private:


StructHash _shBuy;
StructHash _shSell;


};

#endif // STRATEGYCLASS_H
