using System;

namespace Eertree
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = "eskee";
            var eertree = new EerTreeStructure.Eertree(str);
            Console.WriteLine(eertree);
        }
    }
}