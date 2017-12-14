using System.Collections.Generic;
using System.Linq;

namespace Application.EerTreeFirst
{
    public class PalindromeNode
    {
        public virtual string Label { get; set; }
        public int Index { get; set; }
        public virtual PalindromeNode LongestPalindromeSuffix { get; set; }
        
        //Outgoing nodes representing the palindromes xAx that can be obtained adding a letter x to the current PalindromeNode A
        //(If the current PalindromeNode

        public Dictionary<char, PalindromeNode> OutgoingNodes { get; set; } = new Dictionary<char, PalindromeNode>();

        protected internal PalindromeNode(int index)
        {
            Index = index;
        }

        public virtual bool IsImaginaryStringPalindromeNode => false;

        public virtual int GetPalindromeLength => Label.Length;


        /**
         * Not required by the algorithm, it is just for outputting the Eertree in a human-friendly fashion
         */

        public override string ToString()
        {
            var outgoingNodesCompressedFormat = string.Join(", ",
                OutgoingNodes.Values
                    .Select(node => node.Label));
            return string.Format(
                "PalindromeNode[Label='{0}', Length={1}, OutgoingNodes=[{2}], LongestPalindromeSuffix={3}\n",
                Label, GetPalindromeLength, outgoingNodesCompressedFormat, LongestPalindromeSuffix.Label);
        }
    }
}