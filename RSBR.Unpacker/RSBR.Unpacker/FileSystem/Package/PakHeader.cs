using System;

namespace RSBR.Unpacker
{
    class PakHeader
    {
        public UInt32 dwMagic { get; set; } // 0x50414B31 (1KAP)
        public Int32 dwTotalFiles { get; set; }
        public UInt32 dwEntryTableOffset { get; set; }
    }
}
