using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyChain
{
    public class Node
    {
        public Node next = null;
        public object data = null;

        public Node InsertNode()
        {
            if (next == null)
            {
                next = new Node();
                return next;
            }
            Node temp = next;
            next = new Node();
            next.next = temp;
            return next;
        }

        public bool DelNext()
        {
            if (next == null)
            {
                return false;
            }
            next = next.next;
            return true;
        }
    }

    class Program
    {
        static void PrintChain(Node head)
        {
            Node cur = head;
            while (cur != null)
            {
                Console.WriteLine(cur.data);
                cur = cur.next;
            }
            Console.WriteLine("-----------------");
        }

        static void Main(string[] args)
        {
            Node head = new Node();
            head.data = "Head";

            Node cur = head;
            for (int i=0; i<10; ++i)
            {
                Node node = cur.InsertNode();
                node.data = "Hello " + i;
                cur = node;
            }

            PrintChain(head);

            Node node1 = head.next.next.next.next;
            Node node2 = node1.InsertNode();
            node2.data = 666;

            PrintChain(head);

            Console.ReadKey();
        }
    }
}
