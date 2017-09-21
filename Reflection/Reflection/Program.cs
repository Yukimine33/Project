using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using static System.Console;

namespace Reflection
{
    class Program
    {
        private static StringBuilder OutputText = new StringBuilder();

        static void Main(string[] args)
        {
            Type t = typeof(int);

            AnalyzeType(t);
            WriteLine($"Analysis of type {t.Name}");
            WriteLine(OutputText.ToString());

            WriteLine($"Char: {sizeof(char)}"); //2
            WriteLine($"Int: {sizeof(int)}"); //4
            WriteLine($"Short: {sizeof(short)}"); //2
            WriteLine($"Float: {sizeof(float)}"); //4
            WriteLine($"Long: {sizeof(long)}"); //8

            WriteLine("----------");

            unsafe
            { WriteLine($"Size: {sizeof(Test)}"); }
            

            ReadKey();
        }

        static void AnalyzeType(Type t)
        {
            TypeInfo typeInfo = t.GetTypeInfo();
            AddToOutput($"Type name: {t.Name}");
            AddToOutput($"Full name: {t.FullName}");
            AddToOutput($"Namespace: {t.Namespace}");

            Type tBase = t.BaseType;

            if(tBase != null)
            {
                AddToOutput($"Base Type: {tBase.Name}");
            }

            AddToOutput("\nPublic members:");

            foreach(MemberInfo nextMumber in t.GetMembers())
            {
                AddToOutput($"{nextMumber.DeclaringType} {nextMumber.Name}");
            }
        }

        static void AddToOutput(string text)
        {
            OutputText.Append("\n" + text);
        }

        public struct Test
        {
            public int a;
            public char b;
            public short c;
            public float d;
            public char e;
            public int f;
        }
    }
}
