using System;


namespace Library.Library
{
    public static class Crc16
    {
        static public byte[] ModRTU_CRC(byte[] buf, int len)
        {
            UInt16 crc = 0xFFFF;

            for (int pos = 0; pos < len; pos++)
            {
                crc ^= buf[pos];

                for (int i = 8; i != 0; i--)
                {
                    if ((crc & 0x0001) != 0)
                    {
                        crc >>= 1;
                        crc ^= 0xA001;
                    }
                    else
                        crc >>= 1;
                }
            }

            return BitConverter.GetBytes(crc);
        }




        static public bool CheckCrc(byte[] buffRecv)
        {
            byte[] crcResv = ModRTU_CRC(buffRecv, buffRecv.Length - 2);

            if (crcResv[0] == buffRecv[buffRecv.Length - 2] && crcResv[1] == buffRecv[buffRecv.Length - 1])
                return true;

            return false;
        }
    }
}
