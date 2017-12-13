using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Eertree.EerTreeStructure.EmptyStringPalindromeNode;

namespace Eertree.EerTreeStructure
{
    public class Eertree
    {
        private static char[] _storedString;

        //backing data structure for eertree nodes, an ArrayList for convenience, but it could also have been an array or an HashMap
        private readonly List<PalindromeNode> _tree = new List<PalindromeNode>();

        private int _currentLongestPalindromeSuffixNodeIndex;

        public Eertree(string str)
        {
            _storedString = str.ToCharArray();
            Build();
        }

        private void InitTree()
        {
            var imaginaryString = new ImaginaryStringPalindromeNode();
            _tree.Insert(imaginaryString.Index, imaginaryString);

            PalindromeNode emptyString = new EmptyStringPalindromeNode(imaginaryString);
            _tree.Insert(emptyString.Index, emptyString);

            _currentLongestPalindromeSuffixNodeIndex = emptyString.Index;
        }

        private string Build()
        {
            InitTree();
            for (var i = 0; i < _storedString.Length; i++)
            {
                AddLetter(i);
            }
            return _tree.ToString();
        }

        public bool AddLetter(int letterIndex)
        {
            var insertion = new Insertion(letterIndex);
            var longestPalindromePrefix = GetLongestPalindromePrefixForNextPalindromeNode(insertion);

            if (IsDuplicatePalindrome(insertion.Letter, longestPalindromePrefix))
            {
                //Disable tracking duplicate polynomes
                _currentLongestPalindromeSuffixNodeIndex =
                    longestPalindromePrefix.OutgoingNodes[insertion.Letter].Index;
                return false;
            }

            AddNewPalindromeNode(longestPalindromePrefix, insertion);
            return true;
        }


        private void AddNewPalindromeNode(PalindromeNode longestPalindromePrefix, Insertion insertion)
        {
            var nextNodeIndex = _tree.Count; //current max node index is tree.size() - 1
            //I create a PalindromeNode with a null suffix, I'll update that later
            var newNode = new PalindromeNode(nextNodeIndex);
            if (longestPalindromePrefix.IsImaginaryStringPalindromeNode)
            {
                //we are ignoring the imaginary prefix (centered at the position -1 of the string) and what comes before
                // the suffix (i.e. an occurrence of addedLetter at the left of the imaginary string,
                // and we just add a new palindrome of length 1 and content 'addedLetter'
                newNode.Label = insertion.Letter.ToString();
            }
            else
            {
                newNode.Label = insertion.Letter + longestPalindromePrefix.Label + insertion.Letter;
            }
            _tree.Add(newNode);

            //adding the edge connecting the palindrome prefix to the new PalindromeNode
            longestPalindromePrefix.OutgoingNodes.Add(insertion.Letter, newNode);

            //suffix reference to imaginary string is not allowed, change it to the empty string
            if (longestPalindromePrefix.IsImaginaryStringPalindromeNode)
            {
                newNode = _tree[INDEX_EMPTY_STRING];
            }
            else
            {
                var suffixForNewNode =
                    GetLongestPalindromeSuffixForNewNode(insertion, longestPalindromePrefix);
                newNode.LongestPalindromeSuffix = suffixForNewNode;
            }

            _currentLongestPalindromeSuffixNodeIndex = newNode.Index;
        }

        //if there is already an outgoing node from the longestPalindromeSuffix with the same letter, it means
        //there is already a duplicate of the palindrome we have just found.
        private bool IsDuplicatePalindrome(char letter, PalindromeNode longestPalindromeSuffixForNextPalindrome)
        {
            return longestPalindromeSuffixForNextPalindrome.OutgoingNodes.ContainsKey(letter);
        }

        //I start with the longest palindrome suffix of the previous PalindromeNode added to the eertree, then I go back
        // traversing the eertree using the PalindromeNode.getLongestPalindromeSuffix() references, until I find an appropriate node to use as a prefix
        // for my next PalindromeNode (in worst case I end up hitting the imaginary string)
        private PalindromeNode GetLongestPalindromePrefixForNextPalindromeNode(Insertion insertion)
        {
            var longestPalindromePrefix = _tree[_currentLongestPalindromeSuffixNodeIndex];
            while (isNecessaryToKeepTraversingTheSuffixChain(insertion, longestPalindromePrefix)) //todo: fckng NRE .cs
            {
                longestPalindromePrefix = longestPalindromePrefix.LongestPalindromeSuffix;
            }
            return longestPalindromePrefix;
        }

        //I start with the longest palindrome suffix of the palindrome prefix I have just used to compute the new PalindromeNode, then I
        //go back traversing the eertree using the PalindromeNode.getLongestPalindromeSuffix() references until I find an
        //appropriate palindrome suitable as a suffix for the new PalindromeNode
        private PalindromeNode GetLongestPalindromeSuffixForNewNode(Insertion insertion,
            PalindromeNode longestPalindromePrefix)
        {
            var suffixForNewNode = longestPalindromePrefix.LongestPalindromeSuffix;
            while (isNecessaryToKeepTraversingTheSuffixChain(insertion, suffixForNewNode))
            {
                suffixForNewNode = suffixForNewNode.LongestPalindromeSuffix;
            }
            suffixForNewNode = suffixForNewNode.OutgoingNodes[insertion.Letter];
            return suffixForNewNode;
        }

        private bool isNecessaryToKeepTraversingTheSuffixChain(Insertion insertion, PalindromeNode currentSuffix)
        {
            if (currentSuffix.IsImaginaryStringPalindromeNode)
            {
                return false;
            }
            //if B=suffixForNewNode is not imaginary, we need to check that inside the original string xBx is a palindrome
            var indexOfLetterPrecedingTheCurrentSuffix =
                insertion.LetterIndex - currentSuffix.GetPalindromeLength - 1;
            return (indexOfLetterPrecedingTheCurrentSuffix < 0 ||
                    insertion.Letter != _storedString[indexOfLetterPrecedingTheCurrentSuffix]);
        }

        class Insertion
        {
            public readonly char Letter;
            public readonly int LetterIndex;

            public Insertion(int letterIndex)
            {
                LetterIndex = letterIndex;
                Letter = _storedString[letterIndex];
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            var orderedTree = _tree.OrderBy(n => n.Index);
            
            foreach (var node in orderedTree)
            {
                var outIndexes = node.OutgoingNodes.Select(n => n.Value.Index);
                sb.AppendLine($"I: {node.Index}, Label: '{node.Label}', OutTo: {{ {string.Join(", ", outIndexes)} }}");
            }

            return sb.ToString();
        }
    }
}