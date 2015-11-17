using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client.Spread
{
    class SpreadStructure
    {
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi, Pack = 2)]
    public struct PlaceSpreadOrder
    {
        public short TransactionCode;
        public int Price1;
        public double OrderNumber1;
        public int TotalVolRemaining1;
        public int Volume1;
        public int VolumeFilledToday1;

        public int TotalVolRemaining2;
        public int Volume2;
        public int VolumeFilledToday2;
        public int Price2;
        public short BuySell1;
        public short BuySell2;
        public int Token1;
        public int Token2;

        public CONTRACT_DESC SecInfo1;
        public CONTRACT_DESC SecInfo2;

    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi, Pack = 2)]
    public struct Message_Header
    {
        //Packet Length: 40 bytes
        public short TransactionCode;
        public int LogTime;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2)]
        public string AlphaChar;
        public int TraderId;
        public short ErrorCode;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 8)]
        public string Timestamp;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 8)]
        public string TimeStamp1;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 8)]
        public string TimeStamp2;
        public short MessageLength;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi, Pack = 2)]
    public struct CONTRACT_DESC
    {
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 6)]
        public string InstrumentName;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 10)]
        public string Symbol;
        public int ExpiryDate;
        public int StrikePrice;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2)]
        public string OptionType;
        public short CALevel;
    }

    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi, Pack = 2)]
    public struct SPD_LEG_INFO
    {
        public int Token2;
        public CONTRACT_DESC SecurityInfo2;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 5)]
        public string OpBrokerId2;
        public byte Fillerx2;
        public short OrderType2;
        public int BuySell2;
        public int DisclosedVol2;
        public int DisclosedVolRemaining2;
        public int TotalVolRemaining2;
        public int Volume2;
        public int VolumeFilledToday2;
        public int Price2;
        public int TriggerPrice2;
        public int MinFillAon2;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2)]
        public string OrderFlags2;
        public byte OpenClose2;
        public byte AddtnlOrderFlags1;
        public byte GiveupFlag2;
        public byte FillerY;
    }



    [System.Runtime.InteropServices.StructLayoutAttribute(System.Runtime.InteropServices.LayoutKind.Sequential, CharSet = System.Runtime.InteropServices.CharSet.Ansi, Pack = 2)]
    public struct MS_SPD_OE_REQUEST_2100
    {
        public Message_Header Header1;
        public byte ParticipantType1;
        public byte Filler1;
        public short CompetitorPeriod1;
        public short SolicitorPeriod1;
        public byte ModCxlBy1;
        public byte Filler9;
        public short ReasonCode1;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2)]
        public string StartAlpha1;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2)]
        public string EndAlpha1;
        public int Token1;
        public CONTRACT_DESC SecurityInfo1;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 5)]
        public string OpBrokerId1;
        public byte Fillerx1;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 3)]
        public string FillerOptions1;
        public byte Fillery1;
        public short OrderType1;
        public double OrderNumber1;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 10)]
        public string AccountNumber1;
        public short BookType1;
        public short BuySell1;
        public int DisclosedVol1;
        public int DisclosedVolRemaining1;
        public int TotalVolRemaining1;
        public int Volume1;
        public int VolumeFilledToday1;
        public int Price1;
        public int TriggerPrice1;
        public int GoodTillDate1;
        public int EntryDateTime1;
        public int MinFillAon1;
        public int LastModified1;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 2)]
        public string OrderFlags1;
        public short BranchId1;
        public int TraderId1;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 5)]
        public string BrokerId1;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 24)]
        public string cOrdFiller;
        public byte OpenClose1;
        [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst = 12)]
        public string Settlor1;
        public short ProClient1;
        public short SettlementPeriod1;
        public byte AddtnlOrderFlags1;
        public byte GiveupFlag1;

        /// filler1 : 1
        ///filler2 : 1
        ///filler3 : 1
        ///filler4 : 1
        ///filler5 : 1
        ///filler6 : 1
        ///filler7 : 1
        ///filler8 : 1
        ///filler9 : 1
        ///filler10 : 1
        ///filler11 : 1
        ///filler12 : 1
        ///filler13 : 1
        ///filler14 : 1
        ///filler15 : 1
        ///filler16 : 1
        public uint bitvector1;

        /// char
        public byte filler17;

        /// char
        public byte filler18;

        /// double
        public double NnfField;

        /// double
        public double MktReplay;

        /// int
        public int PriceDiff;

        /// SPD_LEG_INFO
        public SPD_LEG_INFO leg2;

        public uint filler1
        {
            get
            {
                return ((uint)((this.bitvector1 & 1u)));
            }
            set
            {
                this.bitvector1 = ((uint)((value | this.bitvector1)));
            }
        }

        public uint filler2
        {
            get
            {
                return ((uint)(((this.bitvector1 & 2u)
                            / 2)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 2)
                            | this.bitvector1)));
            }
        }

        public uint filler3
        {
            get
            {
                return ((uint)(((this.bitvector1 & 4u)
                            / 4)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 4)
                            | this.bitvector1)));
            }
        }

        public uint filler4
        {
            get
            {
                return ((uint)(((this.bitvector1 & 8u)
                            / 8)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 8)
                            | this.bitvector1)));
            }
        }

        public uint filler5
        {
            get
            {
                return ((uint)(((this.bitvector1 & 16u)
                            / 16)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 16)
                            | this.bitvector1)));
            }
        }

        public uint filler6
        {
            get
            {
                return ((uint)(((this.bitvector1 & 32u)
                            / 32)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 32)
                            | this.bitvector1)));
            }
        }

        public uint filler7
        {
            get
            {
                return ((uint)(((this.bitvector1 & 64u)
                            / 64)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 64)
                            | this.bitvector1)));
            }
        }

        public uint filler8
        {
            get
            {
                return ((uint)(((this.bitvector1 & 128u)
                            / 128)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 128)
                            | this.bitvector1)));
            }
        }

        public uint filler9
        {
            get
            {
                return ((uint)(((this.bitvector1 & 256u)
                            / 256)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 256)
                            | this.bitvector1)));
            }
        }

        public uint filler10
        {
            get
            {
                return ((uint)(((this.bitvector1 & 512u)
                            / 512)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 512)
                            | this.bitvector1)));
            }
        }

        public uint filler11
        {
            get
            {
                return ((uint)(((this.bitvector1 & 1024u)
                            / 1024)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 1024)
                            | this.bitvector1)));
            }
        }

        public uint filler12
        {
            get
            {
                return ((uint)(((this.bitvector1 & 2048u)
                            / 2048)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 2048)
                            | this.bitvector1)));
            }
        }

        public uint filler13
        {
            get
            {
                return ((uint)(((this.bitvector1 & 4096u)
                            / 4096)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 4096)
                            | this.bitvector1)));
            }
        }

        public uint filler14
        {
            get
            {
                return ((uint)(((this.bitvector1 & 8192u)
                            / 8192)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 8192)
                            | this.bitvector1)));
            }
        }

        public uint filler15
        {
            get
            {
                return ((uint)(((this.bitvector1 & 16384u)
                            / 16384)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 16384)
                            | this.bitvector1)));
            }
        }

        public uint filler16
        {
            get
            {
                return ((uint)(((this.bitvector1 & 32768u)
                            / 32768)));
            }
            set
            {
                this.bitvector1 = ((uint)(((value * 32768)
                            | this.bitvector1)));
            }
        }
    }

    /*
 
     struct Message_Header
    {
        //Packet Length: 40 bytes
        short TransactionCode;
        long LogTime;
        char AlphaChar [2];
        long TraderId;
        short ErrorCode;
        char Timestamp [8];
        char TimeStamp1 [8];
        char TimeStamp2 [8];
        short MessageLength ;
    };
    struct CONTRACT_DESC
    {
        char InstrumentName2[6];
        char Symbol[10];
        long ExpiryDate;
        long StrikePrice;
        char OptionType[2];
        short CALevel;
    };
    struct SPD_LEG_INFO
    {
        long	Token2; 
        struct CONTRACT_DESC SecurityInfo2;
        char OpBrokerId2[5];
        char Fillerx2;
        short OrderType2;
        long BuySell2;
        long DisclosedVol2;
        long DisclosedVolRemaining2;
        long TotalVolRemaining2;
        long Volume2;
        long VolumeFilledToday2;
        long  Price2;
        long TriggerPrice2;
        long MinFillAon2; 
        char OrderFlags2[2]; //CheckPoint
        char OpenClose2;
        char AddtnlOrderFlags1;
        char GiveupFlag2;
        char FillerY;
    };

    struct MS_SPD_OE_REQUEST_2100
    {
            //Packet Length: 404 bytes
            //Transaction Code: SP_BOARD_LOT_IN (2100) 
		
            Message_Header Header1;
            char ParticipantType1;
            char Filler1;
            short CompetitorPeriod1;
            short SolicitorPeriod1;
            char ModCxlBy1;
            char Filler9;
            short ReasonCode1;
            char StartAlpha1[2];
            char EndAlpha1[2];
            long Token1;
            CONTRACT_DESC SecurityInfo1;
            char OpBrokerId1[5];
            char Fillerx1;
            char FillerOptions1[3];
            char Fillery1;
            short OrderType1;
            double OrderNumber1;
            char AccountNumber1[10];
            short BookType1;
            short BuySell1;
            long DisclosedVol1;
            long DisclosedVolRemaining1;
            long TotalVolRemaining1;
            long Volume1;
            long VolumeFilledToday1;
            long Price1;
            long TriggerPrice1;
            long GoodTillDate1;
            long EntryDateTime1;
            long MinFillAon1;
            long LastModified1 ;
            char OrderFlags1[2];
            short BranchId1;
            long TraderId1;
            char BrokerId1 [5];
            char cOrdFiller [24];
            char OpenClose1;
            char Settlor1 [12];
            short ProClient1;
            short SettlementPeriod1;
            char AddtnlOrderFlags1;
            char GiveupFlag1;
            ushort filler1 :1;
            ushort filler2 :1;
            ushort filler3 :1;
            ushort filler4 :1;
            ushort filler5 :1;
            ushort filler6 :1;
            ushort filler7 :1;
            ushort filler8 :1;
            ushort filler9 :1;
            ushort filler10 :1;
            ushort filler11 :1;
            ushort filler12 :1;
            ushort filler13 :1;
            ushort filler14 :1;
            ushort filler15 :1;
            ushort filler16 :1;
            char filler17;
            char filler18;
            double NnfField;
            double MktReplay;
            long PriceDiff; 	
            SPD_LEG_INFO leg2;
    };
 
     */


}
