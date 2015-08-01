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
        public static string pathFont12 = @"D:\Francesco\Dropbox\Ninokuni\File Translated\Font\font12.NFTR";
        public static string pathFont10 = @"D:\Francesco\Dropbox\Ninokuni\File Translated\Font\font10.NFTR";
        public static int scriptPixel = 223;
        public static int diarioPixel = 210;
        public static int itemPixel = 140;
        public static int obiettivoPixel = 217;

        static void Main(string[] args)
        {
            // Font 12
            NFTR font12 = new NFTR();
            font12.Read(new FileStream(pathFont12, FileMode.Open));

            // Font 10
            NFTR font10 = new NFTR();
            font10.Read(new FileStream(pathFont10, FileMode.Open));

            

            // Script
            ConvertFolder(@"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Excel\Script\",
                @"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Xml\Script\",
                font12, 223);

            // Subtitles
            ConvertFolder(@"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Excel\Subtitles\",
                @"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Xml\Subtitles\",
                font12, 223);

            // Overlays
            ConvertFolder(@"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Excel\Overlays\",
                @"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Xml\Overlays\",
                font12, 223);

            // Demo
            ConvertFolder(@"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Excel\Demo\",
                @"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Xml\Demo\",
                font12, 223);

            // Events \ MainQuest
            ConvertFile(@"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Excel\Events\MainQuest.xlsx",
                @"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Xml\Events\",
                font10, 217);

            // Events \ Scenario
            ConvertFile(@"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Excel\Events\Scenario.xlsx",
                @"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Xml\Events\",
                font10, 256);

            // Events \ System
            ConvertFile(@"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Excel\Events\System.xlsx",
                @"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Xml\Events\",
                font10, 223);

            // Events \ SubQuests
            ConvertFolder(@"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Excel\Events\Subquests\",
                @"D:\Francesco\Dropbox\Ninokuni\File Translated\Text Xml\Events\Subquests\",
                font10, 223);



        }


        public static void ConvertFolder(string folderIn, string folderOut, NFTR font, int pixel)
        {
            string[] files = Directory.GetFiles(folderIn, ".", SearchOption.AllDirectories);

            foreach (string file in files)
                ConvertFile(file, folderOut, font, pixel);
        }

        public static void ConvertFile(string file, string folderOut, NFTR font, int pixel)
        {
            if (!file.Contains(".xlsx"))
                return;
            if (file.Contains("ba001t"))
                return;
            GoogleToXML converter = new GoogleToXML(new FileStream(file, FileMode.Open), font, pixel, folderOut);
            converter.FromSpreadsheet();
        }



    }
}
