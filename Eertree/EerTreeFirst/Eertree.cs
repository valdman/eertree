using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.EerTreeFirst
{
    public class Eertree
    {
        private static char[] _storedString;

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
            var nextNodeIndex = _tree.Count; //todo: change null-suffix to smth
            
            var newNode = new PalindromeNode(nextNodeIndex);
            if (longestPalindromePrefix.IsImaginaryStringPalindromeNode)
            {
                newNode.Label = insertion.Letter.ToString();
            }
            else
            {
                newNode.Label = insertion.Letter + longestPalindromePrefix.Label + insertion.Letter;
            }
            _tree.Add(newNode);

            longestPalindromePrefix.OutgoingNodes.Add(insertion.Letter, newNode);

            if (longestPalindromePrefix.IsImaginaryStringPalindromeNode)
            {
                newNode = _tree[EmptyStringPalindromeNode.INDEX_EMPTY_STRING];
            }
            else
            {
                var suffixForNewNode =
                    GetLongestPalindromeSuffixForNewNode(insertion, longestPalindromePrefix);
                newNode.LongestPalindromeSuffix = suffixForNewNode;
            }

            _currentLongestPalindromeSuffixNodeIndex = newNode.Index;
        }

        private bool IsDuplicatePalindrome(char letter, PalindromeNode longestPalindromeSuffixForNextPalindrome)
        {
            return longestPalindromeSuffixForNextPalindrome.OutgoingNodes.ContainsKey(letter);
        }

        private PalindromeNode GetLongestPalindromePrefixForNextPalindromeNode(Insertion insertion)
        {
            var longestPalindromePrefix = _tree[_currentLongestPalindromeSuffixNodeIndex];
            while (isNecessaryToKeepTraversingTheSuffixChain(insertion, longestPalindromePrefix)) //todo: fckng NRE .cs
            {
                longestPalindromePrefix = longestPalindromePrefix.LongestPalindromeSuffix;
            }
            return longestPalindromePrefix;
        }

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