﻿namespace Application.EerTreeFirst
{
    public class ImaginaryStringPalindromeNode : PalindromeNode
    {
        private static readonly int INDEX_IMAGINARY_STRING = 0;

        public ImaginaryStringPalindromeNode() : base(INDEX_IMAGINARY_STRING) {}

        public override string Label => "-1";

        public override PalindromeNode LongestPalindromeSuffix => this;

        public override int GetPalindromeLength => -1;

        public override bool IsImaginaryStringPalindromeNode => true;
    }
}