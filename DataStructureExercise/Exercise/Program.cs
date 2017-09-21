using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exercise
{
    class Program
    {
        public static int FibonacciSequence(int n)
        {
            if(n == 1 || n == 2)
            {
                return 1;
            }

            return FibonacciSequence(n - 1) + FibonacciSequence(n - 2);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("请输入要查询数列中第几个数：");
            int i = Convert.ToInt32(Console.ReadLine());

            if(i < 1)
            {
                Console.WriteLine("请输入大于等于1的数字");
            }
            else
            {
                Console.WriteLine("第{0}个数字为： {1}", i, FibonacciSequence(i));
            }
        }
    }
}
