using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace HuffmanCoding {

    public class HNode : IComparable {

        public byte Data { get; set; }

        public int Count { get; set; }

        public HNode Parent { get; set; }

        public HNode Left { get; set; }

        public HNode Right { get; set; }

        public bool IsLeaf => Left == null && Right == null;


        public HNode(byte data, int count) {
            Data = data;
            Count = count;
        }


        public bool HasParent => Parent != null;


        public int CompareTo(object obj) {
            var other = obj as HNode;
            if (other == null)
                throw new ArgumentException("Comparing is available only to Nodes");

            return Count.CompareTo(other.Count);
        }
		/*
        public override string ToString() {
            return Data != char.MinValue ? $"[{Data}, {Count}]" : $" [{Count}] ";
        }*/
        

    }

    public class HTree {

        public HNode Root = null;

        private List<HNode> _nodeList = new List<HNode>();

        public Dictionary<byte, BitSet> Table {
            get {
				Dictionary<byte, BitSet> table = new Dictionary<byte, BitSet>();
                EncodedTable(Root, new BitSet(), table);
                return table;
            }
        }

        private void EncodedTable(HNode node, BitSet bitset, Dictionary<byte, BitSet> table) {
            if (node.IsLeaf) {
                table.Add(node.Data, bitset);
                return;
            }


            EncodedTable(node.Left, new BitSet(bitset) + 0, table);
            EncodedTable(node.Right, new BitSet(bitset) + 1, table);
        }

        public HTree(IOrderedEnumerable<KeyValuePair<byte, int>> dictionary) {
            foreach (KeyValuePair<byte, int> pair in dictionary) {
                _nodeList.Add(new HNode(pair.Key, pair.Value));
            }

            CreateTree();
        }

        private void CreateTree() {
            while (true) {
                var hasntParentList = _nodeList.Where(node => !node.HasParent).OrderBy(node => node.Count).ToList();
                if (hasntParentList.Count <= 1) {
                    Root = hasntParentList[0];
                    break;
                }

                Merge(hasntParentList[0], hasntParentList[1]);
            }
        }

        private void Merge(HNode one, HNode other) {
            var node = new HNode(byte.MinValue, one.Count + other.Count);

            one.Parent = node;

            other.Parent = node;

            node.Left = one;

            node.Right = other;
            _nodeList.Add(node);
        }

        internal void Pass(HNode node, StringBuilder builder) {
            if (node == null)
                return;

            builder.Append("(");
            Pass(node.Left, builder);
            builder.Append(node);
            Pass(node.Right, builder);
            builder.Append(")");
        }

        public override string ToString() {
            var builder = new StringBuilder();
            Pass(Root, builder);
            return builder.ToString();
        }

    }

}