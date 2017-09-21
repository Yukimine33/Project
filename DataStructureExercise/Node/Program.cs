using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeExercise
{
    class Node
    {
        public Node head;
        public Node next;
        public int value;
        public Node(int _value)
        {
            value = _value;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Node head = new Node(1);
            Node node2 = AddNode(head, 2);
            Node node3 = AddNode(node2, 3);
            Node node4 = AddNode(node3, 4);
            Node node5 = AddNode(node4, 5);

            DeleteNode(node5);

            InsertNode(head, 10);

            PrintNode(head);
            
            Console.ReadKey();
        }

        /// <summary>
        /// 打印链表
        /// </summary>
        /// <param name="n"></param>
        static void PrintNode(Node head)
        {
            var temp = head;
            while (temp != null)
            {
                Console.Write(temp.value + " ");
                temp = temp.next;
            }
        }

        /// <summary>
        /// 增加节点至链表最后
        /// </summary>
        /// <param name="head"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        static Node AddNode(Node head, int newValue)
        {
            var temp = head;
            Node newNode = new Node(newValue);
            while(temp.next != null)
            {
                temp = temp.next;
            }
            temp.next = newNode;
            newNode.head = temp;
            newNode.next = null;
            return newNode;
        }

        /// <summary>
        /// 在目标节点之后插入新节点
        /// </summary>
        /// <param name="targetNode"></param>
        static void InsertNode(Node targetNode, int newValue)
        {
            var temp = targetNode.next;
            Node newNode = new Node(newValue);
            targetNode.next = newNode;
            newNode.next = temp;
        }

        /// <summary>
        /// 删除链表中的节点（删除不了第一个节点）
        /// </summary>
        /// <param name="targetNode"></param>
        static void DeleteNode(Node targetNode)
        {
            if (targetNode.next != null)
            {
                targetNode.head.next = targetNode.next;
            }
            else
            {
                targetNode.head.next = null;
            }
        }
    }
}
