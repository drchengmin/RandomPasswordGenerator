using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace RandomPasswordGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // Length = 64 = 2^6
            const string PasswordCharset = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz1234567890~@#$%&";

            const int PasswordLength = 8;

            var random = new RNGCryptoServiceProvider();
            byte[] data = new byte[(PasswordLength * 6 + 5) / 8];
            random.GetBytes(data);

            // Prints the raw random bytes.
            foreach (var b in data)
            {
                Console.WriteLine("{0:X}", b);
            }

            // 00000000 11111111 22222222
            // ++++++** ****(((( ((------

            // For the n-th 6-bit (n starts from 0), the offset of the first byte will be: int(n*6 / 8),
            // the second byte will be: int(((n+1)*6-1) / 8).
            // The total number of 6-bit
            int total6bit = data.Length * 8 / 6;
            for (int i = 0; i < total6bit; i++)
            {
                int index1 = i * 6 / 8;
                int index2 = ((i + 1) * 6 - 1) / 8;
                UInt16 b1 = (UInt16)data[index1];
                UInt16 b2 = (UInt16)data[index2];
                Console.WriteLine("b1={0:X}   b2={1:X}", b1, b2);
 
                int shift = i * 6 - index1 * 8;
                int index;
                if (index1 == index2)
                {
                    index = (b1 >> shift) & 0x3F;
                }
                else
                {
                    index = ((b1 | (b2 << 8)) >> shift) & 0x3F;
                }

                // Prints the index of each char in charset and the actual char.
                Console.WriteLine("{0:X} -- {1}", index, PasswordCharset[index]);
            }
        }
    }
}
