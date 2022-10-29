using System;
using System.IO;

namespace RSBR.Unpacker
{
    class PakCipher
    {
        private static Boolean bInitialized = false;

        private static Int32 mt_state;
        private static UInt32[] mt_magic = { 0x0, 0x9908b0df };
        private static UInt32[] mt_table = new UInt32[624];

        private static UInt32[] m_Key = new UInt32[773];

        private static void mt_init_genrand(UInt32 dwSeed)
        {
            mt_table[0] = dwSeed & 0xffffffffU;

            for (mt_state = 1; mt_state < 624; mt_state++)
            {
                mt_table[mt_state] = (UInt32)(1812433253U * (mt_table[mt_state - 1] ^ (mt_table[mt_state - 1] >> 30)) + mt_state);
                mt_table[mt_state] &= 0xffffffffU;
            }
        }

        private static UInt32 mt_genrand_int32()
        {
            UInt32 mt_result;

            if (mt_state >= 624)
            {
                Int32 j = 0;

                if (mt_state == 624 + 1)
                    mt_init_genrand(5489U);

                for (j = 0; j < 624 - 397; j++)
                {
                    mt_result = (mt_table[j] & 0x80000000U) | (mt_table[j + 1] & 0x7fffffffU);
                    mt_table[j] = mt_table[j + 397] ^ (mt_result >> 1) ^ mt_magic[mt_result & 0x1U];
                }

                for (; j < 624 - 1; j++)
                {
                    mt_result = (mt_table[j] & 0x80000000U) | (mt_table[j + 1] & 0x7fffffffU);
                    mt_table[j] = mt_table[j + (397 - 624)] ^ (mt_result >> 1) ^ mt_magic[mt_result & 0x1U];
                }

                mt_result = (mt_table[624 - 1] & 0x80000000U) | (mt_table[0] & 0x7fffffffU);
                mt_table[624 - 1] = mt_table[397 - 1] ^ (mt_result >> 1) ^ mt_magic[mt_result & 0x1U];

                mt_state = 0;
            }

            mt_result = mt_table[mt_state++];

            mt_result ^= (mt_result >> 11);
            mt_result ^= (mt_result << 7) & 0x9d2c5680U;
            mt_result ^= (mt_result << 15) & 0xefc60000U;
            mt_result ^= (mt_result >> 18);

            return mt_result;
        }

        public static void iMakeKey(UInt32 dwSeed)
        {
            mt_init_genrand(dwSeed);

            for (Int32 i = 0; i < 773; i++)
            {
                m_Key[i] = mt_genrand_int32();
            }

            bInitialized = true;
        }

        public static Byte[] iDecryptData(Byte[] lpBuffer, UInt32 dwOffset, UInt32 dwSeed = 0xBAFF1ED)
        {
            if (!bInitialized)
            {
                iMakeKey(dwSeed);
            }

            for (Int32 i = 0; i < lpBuffer.Length; i++)
            {
                lpBuffer[i] ^= (Byte)(m_Key[dwOffset++ % 773]);
            }

            return lpBuffer;
        }
    }
}
