using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class MyList<T>
{
    T[] mem;
    int capcity;
    int size;

    public int Count { get { return size; } }

    public MyList()
    {
        size = 0;
        capcity = 8;
        mem = new T[capcity];
    }

    public void Add(T n)
    {
        if (size == capcity)
        {
            capcity *= 2;
            T[] temp = mem;
            mem = new T[capcity];
            temp.CopyTo(mem, 0);
        }
        size += 1;
        mem[size] = n;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= size)
        {
            return;
        }
        Array.Copy(mem, index + 1, mem, index, size - index-1);
        size -= 1;
    }

    public T this[int index]
    {
        get
        {
            if (index > 0 && index < size)
            {
                return mem[index];
            }
            return default(T);
        }

        set
        {
            if (index > 0 && index < size)
            {
                mem[index] = value;
            }
        }
    }


}

class Program
{
    static void Main(string[] args)
    {
        MyList<int> l = new MyList<int>();

        l.Add(1);
        l.Add(2);
        l.Add(3);
        l.Add(4);
        l.Add(5);
        l.Add(6);
        l.Add(7);

        for (int i=0; i<l.Count; ++i)
        {
            Console.WriteLine(l[i]);
        }
        Console.WriteLine("--------------------");

        l[3] = 99;

        for (int i = 0; i < l.Count; ++i)
        {
            Console.WriteLine(l[i]);
        }
        Console.WriteLine("--------------------");

        l.RemoveAt(3);

        for (int i = 0; i < l.Count; ++i)
        {
            Console.WriteLine(l[i]);
        }
        Console.WriteLine("--------------------");

        Console.ReadKey();
    }
}

