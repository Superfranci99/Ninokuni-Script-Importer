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
        public static string pathFont = @"D:\Francesco\Documenti\NDS\Hacking\Laboratory\ninokuni\font\font8.NFTR";

        static void Main(string[] args)
        {
            //Converter converter = new Converter(pathExcel, @"D:\Francesco\Desktop\out.xml");
            //converter.StartProcess();
            NFTR font = new NFTR();
            Stream fs = new FileStream(pathFont, FileMode.Open);
            font.Read(fs);
        }
    }
}
