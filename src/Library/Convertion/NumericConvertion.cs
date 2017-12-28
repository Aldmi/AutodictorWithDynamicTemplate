using System;
using System.Linq;

namespace Library.Convertion
{
    public static class NumericConvertion
    {
        public static byte[] UshortArrayToByteArray(ushort[] input)
        {
            return input.SelectMany(BitConverter.GetBytes).ToArray();
        }


        public static ushort[] ByteArrayToUshortArray(byte[] input)
        {
            var output = new ushort[input.Length / 2];      
            Buffer.BlockCopy(input, 0, output, 0, input.Length);
            return output;
        }
    }
}