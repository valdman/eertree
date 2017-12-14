using System;
using Application.EertreeSecond;

namespace Application
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = "eertree";
            var palindromic = new Eertree();
            var longestPalindrome = palindromic.LongestPalindromeSubstring(str);
            Console.WriteLine(longestPalindrome);
        }
    }
}