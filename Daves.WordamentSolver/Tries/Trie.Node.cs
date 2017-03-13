using System.Collections.Generic;

namespace Daves.WordamentSolver.Tries
{
    public partial class Trie
    {
        public class Node
        {
            protected internal Node(char value, int depth, IEqualityComparer<char> charEqualityComparer = null)
            {
                Value = value;
                Depth = depth;
                Children = new Dictionary<char, Node>(charEqualityComparer);
            }

            // Storing Value isn't necessary, it just helps me debug and think clearly about what's going on.
            public char Value { get; }
            public int Depth { get; }
            public bool IsAWordEnd { get; set; }
            protected internal Dictionary<char, Node> Children { get; }

            public override string ToString()
                => Depth == 0 ? "root"
                : $"value: {Value}, depth: {Depth}, children: {Children.Count}, {(IsAWordEnd ? "is a word end" : "not a word end")}";
        }
    }
}
