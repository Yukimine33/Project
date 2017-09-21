using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bit
{
    class Program
    {
        static void WriteBit(string s, uint n)
        {
            Console.WriteLine(s + ":  " + Convert.ToString(n, 2));
        }

        // 0. 基本位运算操作符举例
        static void Test()
        {
            uint n;

            n = 1 << 4;
            // 将1的二进制数左移4位
            // 1的二进制：
            // 0001
            // 左移4位：
            // 10000
            WriteBit("1<<4", n); 
            // 输出结果为10000

            n = 0b11111111; //255的二进制表示
            WriteBit("n", n); //输出该二进制11111111

            n &= 0b10101010; //170的二进制表示
            WriteBit("&", n);
            // 11111111
            // &
            // 10101010 =
            // 10101010

            n &= 0b01010101; //85的二进制表示
            WriteBit("&", n);
            // 10101010
            // &
            // 01010101 =
            // 00000000 = 0

            n = 1 << 4;
            WriteBit("n", n); //10000

            n = n | 0b0011; //3的二进制表示
            WriteBit("|", n);
            // 10000
            // |
            // 00011 =
            // 10011

            n = 0b00001111; //15的二进制表示
            WriteBit("n", n);

            n = n ^ 0b1001; //9的二进制表示
            WriteBit("^", n);
            // 00001111
            // ^
            // 00001001 = 
            // 00000110

            n = n ^ 0b1001;
            WriteBit("^", n);
            // 0110
            // |
            // 1001 =
            // 1111

            n = 0b00001111;
            WriteBit("n", n);

            n = ~n;
            WriteBit("~", n);
            // 取反
            // 0000 0000 0000 0000 0000 0000 0000 1111
            // ~
            // 1111 1111 1111 1111 1111 1111 1111 0000
        }

        // 1. 判断是否是偶数
        static bool IsEven(uint n)
        {
            return (n & 1) == 0;
        }

        // 2. 利用Mask查看某一位
        static bool CheckBit(uint n, int i)
        {
            return (n & (1u << i)) != 0;
        }

        // 3. 设置某一个bit为1
         static uint SetBit(uint n, int i)
        {
            return n | (1u << i);
        }

        // 4. 设置某一个bit为0
        static uint ClearBit(uint n, int i)
        {
            uint mask = 0xffffffff;
            mask = mask ^ (1u << i);
            return n & mask;
        }


        // 5. 实际问题：IP普通形式与Uint的互转
        static string UInt2IP(uint ip)
        {
            string IP = "";
            uint n;

            n = ip >> 3*8;
            IP += n + ".";
            n = (ip >> 2*8) & 0x000000FF;
            IP += n + ".";
            n = ip >> 1*8 & 0x000000FF;
            IP += n + ".";
            n = ip & 0x000000FF;
            IP += n;

            return IP;
        }
        static uint IP2Uint(string IP)
        {
            string[] array_ip = IP.Split('.');
            uint ip = 0;
            for (int i=0; i<array_ip.Length; ++i)
            {
                uint n = uint.Parse(array_ip[i]);
                n <<= (3 - i) * 8;

                ip += n;        // 这里也可以写 ip |= n;
            }

            return ip;
        }


        // 有难度的问题：一个整数的二进制表示里包含多少个1
        static uint Count1(uint n)
        {
            uint count = 0;
            while (n > 0)
            {
                n &= (n - 1);
                count++;
            }
            return count;
        }

        static void Main(string[] args)
        {
            Console.WriteLine("-------基础例子开始----------");
            Test();
            Console.WriteLine("=======基础例子结束==========");

            uint n = 3; //二进制0b0011
            Console.WriteLine(n << 2); //二进制1100，输出为unit：12

            Console.WriteLine(IsEven(n));

            Console.WriteLine(Count1(65535)); //195
            //   1111 1111 1111 1111
            // & 1111 1111 1111 1110
            // = 1111 1111 1111 1110
            // & 1111 1111 1111 1101
            // = 1111 1111 1111 1100
            // & 1111 1111 1111 1011
            // = 1111 1111 1111 1000
            //   ………………
            // = 0000 0000 0000 0000
            // 共进行16次，则该数字1的个数为16

            Console.WriteLine(CheckBit(65536, 15));
            // 65536的二进制:
            // 0001 0000 0000 0000 0000
            // 1u << 15:
            // 0000 1000 0000 0000 0000
            // 65536 & (1u << 15) =
            // 0
            // return false

            Console.WriteLine(SetBit(8, 2));
            // 8的二进制:
            // 1000
            // 1u << 2:
            // 0100
            // 8 | (1u << 2) =
            // 1100 => 12

            Console.WriteLine(ClearBit(65536, 16));
            // 65536的二进制:
            // 0000 0000 0000 0001 0000 0000 0000 0000
            // mask的二进制:
            // 1111 1111 1111 1111 1111 1111 1111 1111
            // 1u << 16:
            // 0000 0000 0000 0001 0000 0000 0000 0000
            // mask ^ (1u << 16) = 
            // 1111 1111 1111 1110 1111 1111 1111 1111
            // 65536 & mask =
            // 0000 0000 0000 0000 0000 0000 0000 0000 => 0


            string IP = UInt2IP(0x7F80FFFe);
            // 0x7F80FFFe的二进制:
            // 0111 1111 1000 0000 1111 1111 1111 1110
            // 0x000000FF的二进制:
            // 1111 1111
            // ip >> 3*8:
            // 0111 1111 => 127
            // ip >> 2*8:
            // 0111 1111 1000 0000
            // (ip >> 2*8) & 0x000000FF =
            // 0000 0000 1000 0000 => 128
            // ip >> 1*8:
            // 0111 1111 1000 0000 1111 1111
            // (ip >> 1*8) & 0x000000FF = 
            // 1111 1111 => 255
            // ip & 0x000000FF = 
            // 1111 1110 => 254
            // IP = 127.128.255.254

            Console.WriteLine(IP);

            uint ip = IP2Uint(IP);
            // array_ip[0] = 127 => 0111 1111
            // n <<= (3 - 0) * 8:
            // 0111 1111 0000 0000 0000 0000 0000 0000
            // ip |= n:
            // 0111 1111 0000 0000 0000 0000 0000 0000
            // array_ip[1] = 128 => 1000 0000
            // n <<= (3 - 1) * 8:
            // 1000 0000 0000 0000 0000 0000
            // ip |= n:
            // 0111 1111 1000 0000 0000 0000 0000 0000
            // ………………
            // ip = 0111 1111 1000 0000 1111 1111 1111 1110

            Console.WriteLine("IP转换回uint: " + Convert.ToString(ip, 16));

            Console.WriteLine("{0}", 1 & 2);

            Console.ReadKey();
        }
    }
}
