using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Decompression
{
    class Program
    {
        static void Main(string[] args)
        {
            var sr = new StreamReader(args[0]);
            string lines = "";
            List<string[]> set = new List<string[]>();
            while (lines != "*****")
            {
                lines = sr.ReadLine();
                string[] a = lines.Split();
                set.Add(a);
            }

            byte[] sts = sr.ReadToEnd();
            
            sr.Close();            
            string binarystring = Convert.ToString(sts, 2).PadLeft(8, '0');
            Console.WriteLine(binarystring);

        }
    }
}
