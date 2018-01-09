using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace HuffmanCoding {

    public class HuffmanDecoder {

        private string _path;

        public HuffmanDecoder(string path) {
            if (!File.Exists(path)) {
                throw new ArgumentException();
            }

            _path = path;
        }


        public void Decode() {
            var stream = new FileStream(_path, FileMode.Open, FileAccess.Read);
            var map = CreateCountMap(stream);
            
            var tree = new HTree(map.OrderBy(pair => pair.Value).ThenByDescending(pair => pair.Key));
            var table = tree.Table;
            var path = Path.GetFullPath(_path);
            var destination = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(path)) +
                              "_decoded" + Path.GetExtension(Path.GetFileNameWithoutExtension(path));
            
            int size = (int) (stream.Length - stream.Position);

            var outputStream = new BinaryWriter(new FileStream(destination, FileMode.Create, FileAccess.Write));

            StringBuilder builder = new StringBuilder();
            for (long remaining = stream.Length - stream.Position; remaining > 0; remaining -= size) {
                size = (int) Math.Min(size, remaining);
                var buffer = new byte[size];
                stream.Read(buffer, 0, size);
                HNode node;
                for (int i = 0; i < size; i++) { // idx in buffer in block
                    var bits = new BitSet(buffer[i]);
                    for (int j = 0; j < 8;) { // idx in byte
                        for (node = tree.Root; !node.IsLeaf;) {
                            if (j >= 8 && !node.IsLeaf) {
                                j = 0;
                                if (i + 1 >= size) {
                                    break;
                                }

                                bits = new BitSet(buffer[++i]);
                            }

                            if (bits[j] == 0)
                                node = node.Left;
                            else if (bits[j] == 1)
                                node = node.Right;
                            j++;
                        }

                        outputStream.Write(node.Data);
                        map[node.Data]--;
                        if (map[node.Data] == 0)
                            map.Remove(node.Data);
                        if (map.Count == 0) {
                            outputStream.Close();
                            return;
                        }
                    }
                }
            }

            outputStream.Close();
        }

        private Dictionary<byte, int> CreateCountMap(FileStream stream) {
            var byteArray = new byte[256 * 4];
            var map = new Dictionary<byte, int>();

            stream.Read(byteArray, 0, 256 * 4);

			for (int i = -1, sign = -1; i < byteArray.Length; i += (int) 4, sign++) {
                if ((i + 1) % 4 == 0 && i != -1) {
                    var value = BitConverter.ToInt32(byteArray, i - 3);
                    if (value != 0)
                        map.Add((byte) sign, value);
                }
            }

            return map;
        }

        public static string ToBinary(byte data) {
            var result = new StringBuilder();
            if (data == 0) {
                return 0.ToString();
            }

            while (data > 0) {
                result.Append(data % 2);
                data /= 2;
            }

            return new string(result.ToString().ToCharArray().Reverse().ToArray());
        }

    }

}