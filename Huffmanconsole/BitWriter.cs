using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace HuffmanCoding {

    public class BitWriter {

        private BinaryWriter writer;

        private Dictionary<byte, BitSet> _table;

        private byte buffer = 0;

        private int bufferSize = 0;

        public BitWriter(string source, string destination, Dictionary<byte, BitSet> table) {
            writer = new BinaryWriter(new FileStream(destination, FileMode.Create));
            _table = table;
        }

        public void WriteMap(Dictionary<byte, int> map) {
            var mapArray = new int[256];
            foreach (var pair in map) {
                mapArray[pair.Key] = pair.Value;
            }
            foreach (var value in mapArray) {
                writer.Write(value);
            }
        }

        public void Write(byte sign) {
            Write(_table[sign]);
        }

        private void Write(BitSet coded) {
            foreach (byte sign in coded) {
                buffer = (byte) (buffer | sign << 7 - bufferSize);
                bufferSize++;
                if (bufferSize == 8) {
                    writer.Write(buffer);
                    buffer = 0;
                    bufferSize = 0;
                }
            }
        }

        public void TheEnd() {
            if (bufferSize != 0) {
                writer.Write(buffer);
            }
        }

    }

}