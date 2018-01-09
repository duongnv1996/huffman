using HuffmanCoding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Huffmanconsole
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //var encoder = new HuffmanEncoder("C:\\Users\\DuongKK\\Downloads\\HuffmanCoding-master\\HuffmanCoding-master\\tests\\test.jpg");
            //encoder.Encode();
          //  var decoder = new HuffmanDecoder("C:\\Users\\DuongKK\\Downloads\\HuffmanCoding-master\\HuffmanCoding-master\\tests\\test.jpg.huffman");
           // decoder.Decode();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var encoder = new HuffmanEncoder(openFileDialog1.FileName);
                encoder.Encode();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                var decoder = new HuffmanDecoder(openFileDialog2.FileName);
                decoder.Decode();
            }
        }
    }
}
