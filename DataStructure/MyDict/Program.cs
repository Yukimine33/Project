using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 注：字典代码只是示范，与现实情况出入很大
public class MyDict<K, V>
{
    public V[] mem;
    public int capcity;
    public int size;

    public int Count { get { return size; } }

    public MyDict()
    {
        size = 0;
        capcity = 8;
        mem = new V[capcity];
    }

    public void Add(K k, V v)
    {
        int h = Math.Abs(k.GetHashCode() % 1024);
        if (h >= capcity)
        {
            capcity = h + 1;
            var temp = mem;
            mem = new V[capcity];
            temp.CopyTo(mem, 0);
        }
        /*
        if (value != dv)
        {
            // 实际的字典要考虑碰撞问题，也就是不同的对象有同样的Hash值
            return;
        }
        */
        size += 1;
        mem[h] = v;
    }

    public V this[K k]
    {
        get
        {
            int h = Math.Abs(k.GetHashCode() % 1024);
            if (h<capcity)
            {
                return mem[h];
            }
            return default(V);
        }
    }
}

class Program
{
    static void MyDictInfo<K,V>(MyDict<K,V> d)
    {
        Console.WriteLine("-------------");
        Console.WriteLine(d.Count);
        for (int i=0; i<d.capcity; ++i)
        {
            if (d.mem[i] == null)
            {
                continue;
            }
            Console.Write(d.mem[i]);
            Console.Write(" ");
        }
        Console.WriteLine("=============");
    }

    static void Main(string[] args)
    {
        MyDict<string, int> d = new MyDict<string, int>();
        d.Add("hello", 666);
        d.Add("A", 1);
        d.Add("B", 2);
        d.Add("C", 3);

        MyDictInfo<string, int>(d);
        Console.ReadKey();
    }
}

