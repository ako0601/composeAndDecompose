using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compression
{
    class Program
    {
        public static string[] resultbit = new string[128];

        static void Main(string[] args)
        {
            Console.WriteLine("Compression Processing...");
            var t1 = Stopwatch.StartNew();
            string filename = args[0];
            var sr = new StreamReader(filename);
            List<string> sss = new List<string>();
            List<string> header = new List<string>();
            List<char> wordsaved = new List<char>();
            StringBuilder gibberish = new StringBuilder();

            int[] count = new int[128];
            string lines;

            while (!sr.EndOfStream)
            {
                lines = sr.ReadToEnd();
                sss.Add(lines);
            }
            sr.Close();


            foreach (string i in sss)
            {
                foreach (char j in i)
                {
                    count[j]++;
                    wordsaved.Add(j);
                }
            }

            string zzz = "";
            bytalizing(Huffman(count), zzz);
            StringBuilder sb = new StringBuilder();
            int total = 0;

            for (int i = 0; i < resultbit.Length; i++)
            {
                if (resultbit[i] != null)
                {
                    if ((char)i == ' ')
                    {
                        sb.Append(resultbit[i] + " " + "space" + "\n");
                    }
                    else if ((char)i == '\n')
                    {
                        sb.Append(resultbit[i] + " " + "newline" + "\n");
                    }
                    else if ((char)i == '\r')
                    {
                        sb.Append(resultbit[i] + " " + "return" + "\n");
                    }
                    else if ((char)i == '\t')
                    {
                        sb.Append(resultbit[i] + " " + "tab" + "\n");
                    }
                    else
                    {
                        sb.Append(resultbit[i] + " " + (char)i + "\n");

                    }
                    total += resultbit[i].Length * count[i];
                }
            }

            foreach (char i in wordsaved)
            {
                gibberish.Append(resultbit[i]);
            }            

            sb.Append("*****\n");
            sb.Append(total.ToString() + "\n");

            while (gibberish.Length % 8 != 0)
            {
                gibberish.Append("0");
            }

            byte[] bytei = new byte[(int)Math.Ceiling((double)gibberish.Length / 8)];
            string gib=gibberish.ToString();
            for (int i = 0; i < bytei.Length; i++)
            {
                string temp = gib.Substring(i * 8, 8);
                bytei[i] = Convert.ToByte(temp, 2);
            }

            string newfile = filename.Substring(0, filename.Length - 4);
            FileStream fs = new FileStream(newfile+".zip301", FileMode.Create, FileAccess.Write);
            BufferedStream bs = new BufferedStream(fs);
            byte[] bibi = new byte[sb.Length];
            for (int i = 0; i < bibi.Length; i++)
            {
                bibi[i] = (byte)sb[i];
            }

            bs.Write(bibi, 0, bibi.Length);
            bs.Write(bytei, 0, bytei.Length);
            bs.Close();
            fs.Close();
            t1.Stop();            
            Console.WriteLine("Compression Finished!\nZIP301 file is " + total + " bit");
            Console.WriteLine("Takes "+((float)t1.ElapsedMilliseconds/1000)+"s");
        }

        public static Node Huffman(int[] c)
        {
            List<Node> node = new List<Node>();
            int n = 0;

            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] != 0)
                {
                    node.Add(new Node((char)i, c[i]));
                    n++;
                }
            }

            node.Sort(delegate (Node a, Node b)
            {
                if (a.frequency > b.frequency) return 1;
                else if (a.frequency < b.frequency) return -1;
                return 0;
            });


            for (int i = 1; i < n; i++)
            {
                Node z = new Node();
                z.L = node[0];
                node.RemoveAt(0);
                Node x = z.L;
                z.R = node[0];
                node.RemoveAt(0);
                Node y = z.R;
                z.frequency = x.frequency + y.frequency;
                node.Add(z);
                node.Sort(delegate (Node a, Node b)
                {
                    if (a.frequency > b.frequency) return 1;
                    else if (a.frequency < b.frequency) return -1;
                    return 0;
                });
            }
            return node[0];
        }

        public static void bytalizing(Node root, string n)
        {
            if (root.L == null && root.R == null)
            {
                resultbit[root.character] = n;
            }

            if (root.L != null)
            {
                bytalizing(root.L, n + "0");
            }

            if (root.R != null)
            {
                bytalizing(root.R, n + "1");
            }
        }
    }

    class Node
    {
        public char character;
        public int frequency;
        public Node L, R;

        public Node()
        {
            L = null;
            R = null;
        }

        public Node(char a, int b)
        {
            character = a;
            frequency = b;
            L = null;
            R = null;
        }
    }
}
