using System;

namespace RSBR.Unpacker
{
    class PakEntry
    {
        public UInt32 dwHashName { get; set; }
        public UInt32 dwUnknown { get; set; }
        public Int32 dwDecompressedSize { get; set; }
        public Int32 dwCompressedSize { get; set; }
        public UInt32 dwOffset { get; set; }
    }
}
