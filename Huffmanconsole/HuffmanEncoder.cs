using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HuffmanCoding {

    public class HuffmanEncoder {

        private string _path;

        private const string EncrypedExtension = ".huffman";

        public HuffmanEncoder(string path) {
            if (!File.Exists(path)) {
                throw new ArgumentException();
            }

            _path = path;
        }


        public void Encode() {
            var map = CountChars();
            var tree = new HTree(map.OrderBy(pair => pair.Value).ThenByDescending(pair => pair.Key));
            var table = tree.Table;

            Encrypt(map, table);
        }

        private Dictionary<byte, int> CountChars() {
            var dictionary = new Dictionary<byte, int>();
            int size = 500;
            using (var stream = new BinaryReader(new FileStream(_path, FileMode.Open))) {
                for (long remaining = stream.BaseStream.Length; remaining > 0; remaining -= size) {
                    size = (int) Math.Min(size, remaining);
                    var buffer = new byte[size];
                    stream.Read(buffer, 0, size);

                    foreach (var c in buffer) {
                        if (!dictionary.ContainsKey(c))
                            dictionary.Add(c, 0);

                        dictionary[c]++;
                    }
                }
            }

            return dictionary;
        }

        private void Encrypt(Dictionary<byte, int> map, Dictionary<byte, BitSet> table) {
            var destination = _path + EncrypedExtension;
            var bitWriter = new BitWriter(_path, destination, table);
            bitWriter.WriteMap(map);

            int size = 500;
            using (var stream = new BinaryReader(new FileStream(_path, FileMode.Open))) {
                for (long remaining = stream.BaseStream.Length - stream.BaseStream.Position; remaining > 0;
                    remaining -= size) {
                    size = (int) Math.Min(size, remaining);
                    var buffer = new byte[size];
                    stream.Read(buffer, 0, size);
                    foreach (var c in buffer)
                        bitWriter.Write(c);
                }
               
            }
            
            bitWriter.TheEnd();
        }

    }

}