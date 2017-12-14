using System.Collections.Generic;

namespace Application.EertreeSecond
{
    public class Node
    {
        public Node (int length)
        {
            Length = length;
        }
        
        public readonly int Length;
        public readonly Node[] Edges = new Node[256];
        public Node SuffixLink;
        public string Label;
    }
}