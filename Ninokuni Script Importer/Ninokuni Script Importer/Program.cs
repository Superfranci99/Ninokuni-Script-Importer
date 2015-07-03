using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninokuni_Script_Importer.Font;

namespace Ninokuni_Script_Importer
{
    class Program
    {
        public static string pathExcel = @"D:\Francesco\Desktop\a.xlsx";
        public static string pathFont = @"D:\Francesco\Dropbox\Text translated\font\font12.NFTR";
        public static string pathXml   = @"D:\Francesco\Desktop\out.xml";

        static void Main(string[] args)
        {
            Converter converter = new Converter(pathExcel, pathXml, pathFont, 223);
            converter.StartProcess();
        }
    }
}
