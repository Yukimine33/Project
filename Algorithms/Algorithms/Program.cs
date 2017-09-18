using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    class Program
    {
        /// <summary>
        /// 快速排序
        /// </summary>
        /// <param name="array"></param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        public static void QuickSort(int[] array, int low, int high)
        { 
            if(low >= high)
            {
                return;
            }

            int first = low;
            int last = high;
            int key = array[low];

            while(first < last)
            {
                while(first < last && array[last] >= key)
                {
                    last--;
                }
                array[first] = array[last];

                while(first < last && array[first] <= key)
                {
                    first++;
                }
                array[last] = array[first];
            }
            array[first] = key;

            QuickSort(array, low, first - 1);
            QuickSort(array, first + 1, high);
        }

        /// <summary>
        /// 直接插入排序
        /// </summary>
        /// <param name="array"></param>
        public static void InsertSort(int[] array)
        {
            for(int i = 1; i < array.Length; i++)
            {
                int temp = array[i];
                int j = i;
                while(j > 0 && array[j - 1] > temp)
                {
                    array[j] = array[j - 1];
                    j--;
                }
                array[j] = temp;
            }
        }

        /// <summary>
        /// 希尔排序
        /// </summary>
        /// <param name="array"></param>
        public static void ShellSort(int[] array)
        {
            int length = array.Length;
            int step = length / 2;

            int j;
            while(step >= 1)
            {
                for(int i = step; i < length; i++)
                {
                    int temp = array[i];
                    for(j = i - step; j >= 0 && temp < array[j]; j = j -step)
                    {
                        array[j + step] = array[j];
                    }

                    array[j + step] = temp;
                }

                step = step / 2;
            }
        }

        /// <summary>
        /// 简单选择排序
        /// </summary>
        /// <param name="array"></param>
        public static void SimpleSelectSort(int[] array)
        {
            int temp = 0;
            int t = 0;
            for(int i = 0; i < array.Length; i++)
            {
                t = i;
                for(int j = i + 1; j < array.Length; j++)
                {
                    if(array[t] > array[j])
                    {
                        t = j;
                    }
                }

                temp = array[i];
                array[i] = array[t];
                array[t] = temp;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine("-----QuickSort-----");
            int[] array1 = new int[] { 2, 3, 1, 4, 6, 5 };
            QuickSort(array1, 0, array1.Length - 1);
            PrintArray(array1);

            Console.WriteLine("-----InsertSort-----");
            int[] array2 = new int[] { 5, 6, 1, 2, 4, 3 };
            InsertSort(array2);
            PrintArray(array2);

            Console.WriteLine("-----ShellSort-----");
            int[] array3 = new int[] { 6, 4, 5, 3, 1, 2 };
            ShellSort(array3);
            PrintArray(array3);

            Console.WriteLine("-----SimpleSelectSort-----");
            int[] array4 = new int[] { 3, 1, 2, 6, 4, 5 };
            SimpleSelectSort(array4);
            PrintArray(array4);

            Console.ReadKey();
        }

        public static void PrintArray(int[] array)
        {
            for(int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i] + " ");
            }
            Console.WriteLine();
        }
    }
}
