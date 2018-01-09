using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using System.Text;

namespace HuffmanCoding {

    public class BitSet : IEnumerable<byte> {

        public List<byte> Data;

        public int Count => Data.Count;

        public byte this[int key] {
			get {
				return Data[key];
			}
			set {
				value = Data[key];
			}
        }

        public BitSet() {
            Data = new List<byte>();
        }

        public BitSet(BitSet bitSet) {
            Data = new List<byte>(bitSet.Data);
        }

        public int ToInt() {
            int value = 0;
            return Convert.ToInt32(ToString());
        }

        public BitSet(List<byte> data) {
            Data = new List<byte>(data);
        }

        public BitSet(byte data) {
            Data = new List<byte>();
            for (int idx = 0; idx < 8; idx++) {
                byte sign = (byte) ((data >> 7 - idx) & 1);
                Data.Add(sign);
            }
        }

        public void Add(byte data) {
            Data.Add(data);
        }


        public void DropLast() {
            if (Data.Count > 0)
                Data.RemoveAt(Data.Count - 1);
        }

        public static BitSet operator +(BitSet one, BitSet other) {
            var bitset = new BitSet(one.Data);
            foreach (var b in other.Data) {
                bitset.Add(b);
            }

            return bitset;
        }

        public static BitSet operator +(BitSet one, byte other) {
            var bitset = new BitSet(one.Data);

            bitset.Add(other);

            return bitset;
        }

        public static bool operator ==(BitSet one, BitSet other) {
            return one.Data.Equals(other.Data);
        }

        public static bool operator !=(BitSet one, BitSet other) {
            return !(one == other);
        }


        public IEnumerator<byte> GetEnumerator() {
            for (byte i = 0; i < Data.Count; ++i)
                yield return Data[i];
        }

        public override string ToString() {
            var builder = new StringBuilder();
            foreach (byte b in Data) {
                builder.Append(b);
            }

            return builder.ToString();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

    }

}