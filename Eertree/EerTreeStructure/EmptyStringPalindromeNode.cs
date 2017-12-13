﻿/**
 * Empty String is a special node whose suffix is always the imaginary string, and whose index fixed
 */

namespace Eertree.EerTreeStructure
{
    public sealed class EmptyStringPalindromeNode : PalindromeNode
    {
        public static readonly int INDEX_EMPTY_STRING = 1;

        public EmptyStringPalindromeNode(ImaginaryStringPalindromeNode imaginaryStringPalindromeNode) : base(INDEX_EMPTY_STRING)
        {
            LongestPalindromeSuffix = imaginaryStringPalindromeNode;
            Label = string.Empty;
        }
    }
}