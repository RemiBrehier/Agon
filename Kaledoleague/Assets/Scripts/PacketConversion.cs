﻿using System;

namespace Bluetooth.Helpers
{
    public static class PacketConversion
    {
        const int BITS_UINT12 = 12;
        const int BITS_UINT16 = 16;
        const int BITS_INT16 = 16;
        const int BITS_UINT24 = 24;

        public static UInt16 ToUInt12(string binary, int offset = 0)
        {
            UInt16 value = 0;
            for (int i = 0; i < BITS_UINT12; i++)
            {
                if (binary[i + offset] == '1')
                    value += Convert.ToUInt16(Math.Pow(2, (BITS_UINT12 - 1) - i));
            }
            return value;
        }

        public static UInt16 ToUInt16(string binary, int offset = 0)
        {
            UInt16 value = 0;
            for (int i = 0; i < BITS_UINT16; i++)
            {
                if (binary[i + offset] == '1')
                    value += Convert.ToUInt16(Math.Pow(2, (BITS_UINT16 - 1) - i));
            }
            return value;
        }

        public static UInt32 ToUInt24(string binary, int offset = 0)
        {
            UInt32 value = 0;
            for (int i = 0; i < BITS_UINT24; i++)
            {
                if (binary[i + offset] == '1')
                    value += Convert.ToUInt32(Math.Pow(2, (BITS_UINT24 - 1) - i));
            }
            return value;
        }

        public static Int16 ToInt16(string binary, int offset = 0)
        {
            Int16 value = 0;
            for (int i = 0; i < BITS_UINT16; i++)
            {
                if (binary[i + offset] == '1')
                    value += (Int16)(Math.Pow(2, (BITS_INT16 - 1) - i));
            }
            return value;
        }
    }
}
