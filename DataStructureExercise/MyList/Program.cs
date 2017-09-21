using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyList
{
    class MyList<T>
    {
        T[] myList;
        int count; //当前数组长度
        int capacity; //数组容量

        public MyList()
        {
            count = 0;
            capacity = 8;
            myList = new T[capacity];
        }

        /// <summary>
        /// 判断是否越界
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        bool isOver(int n)
        {
            if (n < 1 || n > count)
            {
                Console.WriteLine("越界错误");
                return true;
            }
            return false;
        }

        /// <summary>
        /// 列表扩容
        /// </summary>
        void ExtendList()
        {
            capacity *= 2;
            T[] temp = myList;
            myList = new T[capacity];
            temp.CopyTo(myList, 0);
        }

        /// <summary>
        /// 打印列表
        /// </summary>
        public void Print()
        {
            for(int i = 0;i < count;i++)
            {
                Console.WriteLine("{0}", myList[i]);
            }
        }

        /// <summary>
        /// 实现列表增加元素
        /// </summary>
        /// <param name="num"></param>
        public void Add(T num)
        {
            if(count == capacity)
            {
                ExtendList();
            }
            myList[count] = num;
            count += 1;
        }

        /// <summary>
        /// 移除列表某一位置元素
        /// </summary>
        /// <param name="num"></param>
        public void RemoveAt(int num)
        {
            if (isOver(num))
            {
                return;
            }

            count -= 1;
            for (int i = num - 1; i < count; i++)
            {
                myList[i] = myList[i + 1];
            }
        }

        /// <summary>
        /// 在第n项之前插入content
        /// </summary>
        /// <param name="num"></param>
        /// <param name="content"></param>
        public void Insert(int n,T content)
        {
            if (isOver(n))
            {
                return;
            }

            if (count == capacity)
            {
                ExtendList();
            }

            for (int i = count; i >= n; i--)
            {
                myList[i] = myList[i - 1];
            }
            myList[n - 1] = content;
            count += 1;
        }

        /// <summary>
        /// 设置列表第n项为m，要考虑越界的情况
        /// </summary>
        public void Set(int n,T m)
        {
            if (isOver(n))
            {
                return;
            }
            myList[n - 1] = m;
        }

        /// <summary>
        /// 得到列表第n项的值
        /// </summary>
        /// <param name="n"></param>
        public T Get(int n)
        {
            if(isOver(n))
            {
                return default(T);
            }
            return myList[n - 1];
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyList<string> l = new MyList<string>();

            l.Add("a");
            l.Add("b");
            l.Add("c");
            l.Add("d");
            l.Add("e");
            l.Add("f");
            l.Add("g");
            l.Add("h");
            //l.Add("i");
            //l.Add("j");
            Console.WriteLine("列表为：");
            l.Print();

            /*
            Console.WriteLine("请输入要删除列表中第几个元素：");
            int num1 = Convert.ToInt32(Console.ReadLine());
            l.RemoveAt(num1);
            Console.WriteLine("新列表为：");
            l.Print();

            Console.WriteLine("请输入要修改列表中第几个元素：");
            int num2 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("请输入要该元素的新值：");
            string content1 = Convert.ToString(Console.ReadLine());
            l.Set(num2, content1);//设置列表第n项为某个数，要考虑越界的情况
            Console.WriteLine("新列表为：");
            l.Print();
            */
            Console.WriteLine("请输入要在第几个元素前插入新值：");
            int num3 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("请输入要该元素的新值：");
            string content2 = Convert.ToString(Console.ReadLine());
            l.Insert(num3, content2);//设置列表第n项为某个数，要考虑越界的情况
            Console.WriteLine("新列表为：");
            l.Print();

            Console.WriteLine("请输入要得到第几个元素的值：");
            int num4 = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("第{0}个元素的值为：{1}",num4, l.Get(num4));

            Console.ReadKey();
        }
    }
}
