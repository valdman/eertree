using System;
using Eertree.EertreeSecond;

namespace Eertree
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = Console.ReadLine();
            var palindromic = new PalindromicTree();
            Console.WriteLine(palindromic.LongestPalindromeSubstring(str));
        }
    }
}