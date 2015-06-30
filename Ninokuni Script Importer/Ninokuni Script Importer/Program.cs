using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ninokuni_Script_Importer
{
    class Program
    {
        public static string path = @"D:\Francesco\Desktop\a.xlsx";

        static void Main(string[] args)
        {
            Converter converter = new Converter(path, @"D:\Francesco\Desktop\out.xml");
            converter.StartProcess();
        }
    }
}
