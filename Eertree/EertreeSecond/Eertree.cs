namespace Application.EertreeSecond
{
    public class Eertree
    {
        private readonly Node _fictive = new Node(-1);
        private readonly Node _empty = new Node(0);

        public Eertree()
        {
            _empty.SuffixLink = _fictive;
            _fictive.SuffixLink = _fictive;
        }

        public string LongestPalindromeSubstring(string s)
        {
            var longestPalindromeEnd = -1;
            var length = 0; // by default, the longest palindromic substring is the empty string
            
            var T = _empty;  // T is the largest proper palindrome suffix for the current prefix
            
            for (var i = 0; i < s.Length; i++)
            {
                var c = s[i];

                var current = T;
                while (current != _fictive)
                {
                    var index = i - current.Length - 1;
                    if (index >= 0 && s[index] == c)
                    {
                        break;
                    }

                    current = current.SuffixLink;
                }
                Node newNode;
                if (current == _fictive)
                {
                    newNode = new Node(1);
                    _fictive.Edges[s[i]] = newNode;
                }
                else
                {
                    newNode = new Node(current.Length + 2);
                    current.Edges[s[i]] = newNode;
                }
                // now find the largest proper palindrome suffix for the new node palindrome

                if (current == _fictive)
                {
                    newNode.SuffixLink = _empty;
                }
                else
                {
                    current = current.SuffixLink;
                    while (current != _fictive)
                    {
                        var index = i - current.Length - 1;
                        if (index >= 0 && s[index] == c)
                        {
                            break;
                        }
                        current = current.SuffixLink;
                    }
                    newNode.SuffixLink = current.Edges[c];
                }
                T = newNode;
                if (T.Length > length)
                {
                    length = T.Length;
                    longestPalindromeEnd = i;
                }
            }

            var longestPalindrome = "";
            if (longestPalindromeEnd != -1)
            {
                longestPalindrome = s.Substring(longestPalindromeEnd - length + 1, length);
            }

            return longestPalindrome;
        }
    }
}